using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TekConf.Core.Entities;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	using System;

	using Cirrious.MvvmCross.ViewModels;

	public class ConferenceSessionsListViewDto
	{

		private IEnumerable<ConferenceSessionListDto> _sessions; 
		public ConferenceSessionsListViewDto(IEnumerable<SessionEntity> sessions)
		{
			if (sessions != null)
			{
				_sessions = sessions.Select(x => new ConferenceSessionListDto(x));

				var grouped = _sessions
								.OrderBy(x => x.start)
								.GroupBy(session => session.start.ToString("ddd, h:mm tt"))
								.Select(slot => new ConferenceSessionGroup(
																slot.Key,
																slot.OrderBy(session => session.start).ThenBy(t => t.title)));

				var groupList = grouped.ToList();

				Sessions = groupList;
			}
			else
			{
				Sessions = new List<ConferenceSessionGroup>();
			}
		}

		public ConferenceSessionsListViewDto(IEnumerable<FullSessionDto> sessions)
		{
			if (sessions != null)
			{
				_sessions = sessions.Select(x => new ConferenceSessionListDto(x));

				var grouped = _sessions
								.OrderBy(x => x.start)
								.GroupBy(session => session.start.ToString("ddd, h:mm tt"))
								.Select(slot => new ConferenceSessionGroup(
																slot.Key,
																slot.OrderBy(session => session.start).ThenBy(t => t.title)));

				var groupList = grouped.ToList();

				Sessions = groupList;
			}
			else
			{
				Sessions = new List<ConferenceSessionGroup>();
			}
		}

		public string name { get; set; }
		public string slug { get; set; }

		public IEnumerable<ConferenceSessionGroup> Sessions { get; set; }

		public List<ConferenceSessionGroup> SessionsByTime
		{
			get
			{
				var grouped = _sessions
												.OrderBy(x => x.start)
												.GroupBy(session => session.start.ToString("ddd, h:mm tt"))
												.Select(slot => new ConferenceSessionGroup(
																				slot.Key,
																				slot.OrderBy(session => session.start).ThenBy(t => t.title)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}

		public List<ConferenceSessionGroup> SessionsByTitle
		{
			get
			{
				var grouped = _sessions
								.OrderBy(x => x.title)
								.GroupBy(session => FirstCharacter(session.title))
								.Select(slot => new ConferenceSessionGroup(
																slot.Key,
																slot.OrderBy(session => session.title)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}

		private string FirstCharacter(string value)
		{
			string firstCharacter = "";
			if (!string.IsNullOrWhiteSpace(value) && value.Length >= 1)
			{
				firstCharacter = value.Substring(0, 1);
			}
			return firstCharacter;
		}

		public List<ConferenceSessionGroup> SessionsByTag
		{
			get
			{
				var grouped = _sessions
								.OrderBy(x => x.tagNames)
								.GroupBy(session => FirstCharacter(session.tagNames))
								.Select(slot => new ConferenceSessionGroup(
								slot.Key,
								slot.OrderBy(session => session.tagNames)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}

		public List<ConferenceSessionGroup> SessionsByRoom
		{
			get
			{
				var grouped = _sessions
								.OrderBy(x => x.room)
								.GroupBy(session => session.room)
								.Select(slot => new ConferenceSessionGroup(
											slot.Key,
											slot.OrderBy(session => session.room)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}
	}
}