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
								.Select(slot => new SessionGroup(
																slot.Key,
																slot.OrderBy(session => session.start).ThenBy(t => t.title)));

				var groupList = grouped.ToList();

				Sessions = groupList;
			}
			else
			{
				Sessions = new List<SessionGroup>();
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
								.Select(slot => new SessionGroup(
																slot.Key,
																slot.OrderBy(session => session.start).ThenBy(t => t.title)));

				var groupList = grouped.ToList();

				Sessions = groupList;
			}
			else
			{
				Sessions = new List<SessionGroup>();
			}
		}

		public string name { get; set; }
		public string slug { get; set; }

		public IEnumerable<SessionGroup> Sessions { get; set; }

		public List<SessionGroup> SessionsByTime
		{
			get
			{
				var grouped = _sessions
												.OrderBy(x => x.start)
												.GroupBy(session => session.start.ToString("ddd, h:mm tt"))
												.Select(slot => new SessionGroup(
																				slot.Key,
																				slot.OrderBy(session => session.start).ThenBy(t => t.title)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}

		public List<SessionGroup> SessionsByTitle
		{
			get
			{
				var grouped = _sessions
								.OrderBy(x => x.title)
								.GroupBy(session => FirstCharacter(session.title))
								.Select(slot => new SessionGroup(
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

		public List<SessionGroup> SessionsByTag
		{
			get
			{
				var grouped = _sessions
								.OrderBy(x => x.tagNames)
								.GroupBy(session => FirstCharacter(session.tagNames))
								.Select(slot => new SessionGroup(
								slot.Key,
								slot.OrderBy(session => session.tagNames)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}

		public List<SessionGroup> SessionsByRoom
		{
			get
			{
				var grouped = _sessions
								.OrderBy(x => x.room)
								.GroupBy(session => session.room)
								.Select(slot => new SessionGroup(
											slot.Key,
											slot.OrderBy(session => session.room)));

				var groupList = grouped.ToList();
				return groupList;
			}
		}
	}

	public class SessionGroup : List<ConferenceSessionListDto>
	{
		public string Key { get; set; }

		public SessionGroup(string key, IEnumerable<ConferenceSessionListDto> items)
			: base(items)
		{
			Key = key;
		}
	}

}