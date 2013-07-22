using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Entities;
using TekConf.Core.Models;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class ConferencesListViewDto : MvxViewModel
	{
		private readonly IMvxFileStore _fileStore;

		public ConferencesListViewDto(ConferenceEntity entity, IMvxFileStore fileStore)
		{
			try
			{
				_fileStore = fileStore ?? Mvx.Resolve<IMvxFileStore>();
			}
			catch (Exception)
			{
			}

			if (entity != null)
			{
				slug = entity.Slug;
				name = entity.Name;
				start = entity.Start;
				DateRange = entity.DateRange();
				FormattedAddress = entity.FormattedAddress();
				FormattedCity = entity.FormattedCity();
				imageUrl = entity.ImageUrl;
				//ImageBytes = entity.ImageBytes;
			}
		}


		public ConferencesListViewDto(FullConferenceDto fullConference, IMvxFileStore fileStore)
		{
			try
			{
				_fileStore = fileStore ?? Mvx.Resolve<IMvxFileStore>();
			}
			catch (Exception)
			{
			}

			if (fullConference != null)
			{
				slug = fullConference.slug;
				name = fullConference.name;
				start = fullConference.start;
				DateRange = fullConference.DateRange;
				FormattedAddress = fullConference.FormattedAddress;
				FormattedCity = fullConference.FormattedCity;
				imageUrl = fullConference.imageUrl;
				//ImageBytes = fullConference.ImageBytes;
			}
		}

		public string name { get; set; }
		public string DateRange { get; set; }
		public string slug { get; set; }
		public string FormattedAddress { get; set; }
		public string FormattedCity { get; set; }
		public DateTime start { get; set; }

		private string _imageUrl;

		private byte[] _imageBytes;
		public byte[] ImageBytes
		{
			get
			{
				return _imageBytes;
			}
			set
			{
				_imageBytes = value;
				RaisePropertyChanged(() => ImageBytes);
			}
		}

		private void GetImageError(Exception exception)
		{

		}

		private void GetImageSuccess(byte[] image)
		{
			InvokeOnMainThread(() =>
				ImageBytes = image
				);
		}

		public string imageUrl
		{
			get { return _imageUrl; }
			set
			{
				_imageUrl = value;
				if (_fileStore != null)
					ImageService.GetImageAsync(_fileStore, null, _imageUrl, GetImageSuccess, GetImageError);
			}
		}

	}
}