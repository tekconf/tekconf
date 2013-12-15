using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.Web.App_Start;
using TekConf.Web.ViewModels;

namespace TekConf.Web.Controllers
{
    using AutoMapper;
    using System.Text.RegularExpressions;
    using TekConf.Common.Entities;
    using TekConf.RemoteData.v1;

	public class SpeakersController : Controller
	{
		private readonly IRemoteDataRepository _remoteDataRepository;
        private readonly IConferenceRepository _conferenceRepository;

		public SpeakersController(IRemoteDataRepository remoteDataRepository,
            IConferenceRepository conferenceRepository)
		{
			_remoteDataRepository = remoteDataRepository;
            _conferenceRepository = conferenceRepository;
		}

		[CompressFilter]
		public async Task<ActionResult> Index()
		{
			//var openCallsConferencesTask = _repository.GetConferencesWithOpenCalls();
			var openCallsConferencesTask = _remoteDataRepository.GetConferencesAsync(showOnlyOpenCalls: true);
			List<PresentationDto> presentations;
			List<FullConferenceDto> myConferences;

			if (Request.IsAuthenticated)
			{
				var userName = User.Identity.Name;
				var presentationsTask = _remoteDataRepository.GetPresentations(userName);
				var conferencesTask = _remoteDataRepository.GetSchedules(userName);

				await Task.WhenAll(openCallsConferencesTask, presentationsTask, conferencesTask);

				presentations = presentationsTask.Result;
				myConferences = conferencesTask.Result;
			}
			else
			{
				await Task.WhenAll(openCallsConferencesTask);

				presentations = new List<PresentationDto>();
				myConferences = new List<FullConferenceDto>();
			}
			

			var openCallConferences = openCallsConferencesTask.Result == null ? new List<FullConferenceDto>() : openCallsConferencesTask.Result.ToList();

            IList<FullConferenceDto> newestConferences = null;
            var getNewestConferencesTask = Task.Factory.StartNew(() =>
            {
                var nconferences = _conferenceRepository.GetNewestConferences();

                newestConferences = Mapper.Map<List<FullConferenceDto>>(nconferences);
            });
            await getNewestConferencesTask;
            ViewBag.NewestConferences = newestConferences;

			var vm = new SpeakersViewModel()
			{
				OpenConferences = openCallConferences.OrderBy(x => x.callForSpeakersCloses).ToList(),
				Presentations = presentations,
				MyConferences = myConferences,
			};

			return View(vm);
		}

	}

	public static class Extensions
	{
        public static List<string> SplitOn(this string initial, int MaxCharacters)
        {

            List<string> lines = new List<string>();

            if (string.IsNullOrEmpty(initial) == false)
            {
                string targetGroup = "Line";
                string theRegex = string.Format(@"(?<{0}>.{{1,{1}}})(?:\W|$)", targetGroup, MaxCharacters);
                //string theRegex = string.Format(@"(?.{{1,{1}}})(?:[^\S]+|$)", targetGroup, MaxCharacters);

                MatchCollection matches = Regex.Matches(initial, theRegex, RegexOptions.IgnoreCase
                                                                          | RegexOptions.Multiline
                                                                          | RegexOptions.ExplicitCapture
                                                                          | RegexOptions.CultureInvariant
                                                                          | RegexOptions.Compiled);
                if (matches != null)
                    if (matches.Count > 0)
                        foreach (Match m in matches)
                            lines.Add(m.Groups[targetGroup].Value);
            }

            return lines;
        }

		public static void Shuffle<T>(this IList<T> list)
		{
			Thread.Sleep(10);
			Random rng = new Random(DateTime.Now.Millisecond);
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}
