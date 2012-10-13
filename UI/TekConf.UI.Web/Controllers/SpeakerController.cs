using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;

namespace TekConf.UI.Web.Controllers
{
  public class SpeakerController : AsyncController
  {
    public void IndexAsync(string conferenceSlug, string sessionSlug)
    {
      var remoteData = new RemoteDataRepository();
      AsyncManager.OutstandingOperations.Increment();
      remoteData.GetSessionSpeakers(conferenceSlug, sessionSlug, sessions =>
      {
        AsyncManager.Parameters["sessions"] = sessions;
        AsyncManager.OutstandingOperations.Decrement();
      });
    }

    public ActionResult IndexCompleted(List<SpeakersDto> speakers)
    {
      return View(speakers);
    }


    public void DetailAsync(string conferenceSlug, string sessionSlug, string speakerSlug)
    {
        var remoteData = new RemoteDataRepository();
        AsyncManager.OutstandingOperations.Increment(2);

        remoteData.GetSpeaker(conferenceSlug, speakerSlug, speaker =>
        {
            AsyncManager.Parameters["speaker"] = speaker;
            AsyncManager.OutstandingOperations.Decrement();
        });

        remoteData.GetFullConference(conferenceSlug, conference =>
        {
            AsyncManager.Parameters["conference"] = conference;
            AsyncManager.OutstandingOperations.Decrement();
        });
    }

    public ActionResult DetailCompleted(FullSpeakerDto speaker, FullConferenceDto conference)
    {
        if (speaker == null || conference == null)
        {
            return RedirectToAction("NotFound", "Error");
        }

        var conferenceDto = new ConferencesDto()
                                {
                                    description = conference.description,
                                    end = conference.end,
                                    imageUrl = conference.imageUrl,
                                    location = conference.location,
                                    name = conference.name,
                                    slug = conference.slug,
                                    start = conference.start
                                };

        var sessions = from s in conference.sessions
                       from sp in s.speakers
                       where sp.slug == speaker.slug
                       select new SessionsDto()
                                  {
                                      conferenceSlug = conference.slug,
                                      description = s.description,
                                      difficulty = s.difficulty,
                                      end = s.end,
                                      links = s.links,
                                      prerequisites = s.prerequisites,
                                      room = s.room,
                                      sessionType = s.sessionType,
                                      slug = s.slug,
                                      start = s.start,
                                      subjects = s.subjects,
                                      tags = s.tags,
                                      title = s.title,
                                      twitterHashTag = s.twitterHashTag,
                                  };

        var profileImage = new GravatarImage();
        
        var profileImageUrl = profileImage.GetURL("robgibbens@gmail.com", 100, "pg"); //TODO
        speaker.profileImageUrl = profileImageUrl;

        var viewModel = new SpeakerDetailViewModel()
                     {
                         Conference = conferenceDto,
                         Speaker = speaker,
                         Sessions = sessions.ToList(),
                     };

        return View(viewModel);
    }
  }
}
