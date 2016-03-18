using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TekConf.Core.Annotations;
using TekConf.Core.Entities;
using TekConf.Core.Models;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class SessionDetailDto : INotifyPropertyChanged
	{
		public SessionDetailDto(SessionEntity entity)
		{
			_speakers = new ObservableCollection<SpeakerDetailViewDto>();
			if (entity != null)
			{
				slug = entity.Slug;
				title = entity.Title;
				startDescription = entity.StartDescription();
				room = entity.Room;
				description = entity.Description;
				isAddedToSchedule = entity.IsAddedToSchedule;
				//TODO speakers = entity.Speakers.Select(x => new SpeakerDetailViewDto(x)).ToList();
			}
		}

		public SessionDetailDto(FullSessionDto fullSession)
		{
			_speakers = new ObservableCollection<SpeakerDetailViewDto>();

			if (fullSession != null)
			{
				slug = fullSession.slug;
				title = fullSession.title;
				startDescription = fullSession.startDescription;
				room = fullSession.room;
				description = fullSession.description;
				isAddedToSchedule = fullSession.isAddedToSchedule.HasValue && fullSession.isAddedToSchedule.Value;
				speakers = fullSession.speakers.Select(x => new SpeakerDetailViewDto(x)).ToList().ToObservableCollection();
			}
		}

		public string slug { get; set; }
		public string title { get; set; }
		public string startDescription { get; set; }
		public string room { get; set; }
		public string description { get; set; }

		private ObservableCollection<SpeakerDetailViewDto> _speakers;
		public ObservableCollection<SpeakerDetailViewDto> speakers
		{
			get
			{
				return _speakers;
			}
			set
			{
				_speakers = value;
			}
		}

		public bool isAddedToSchedule { get; set; }

		public void AddSpeaker(SpeakerDetailViewDto speaker)
		{
			_speakers.Add(speaker);
			OnPropertyChanged("speakers");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}