using System;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Models;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public class ConferencesListViewDto : MvxViewModel
	{
		private readonly IMvxFileStore _fileStore;

		public ConferencesListViewDto(FullConferenceDto fullConference, IMvxFileStore fileStore)
		{
			_fileStore = fileStore;

			if (fullConference != null)
			{
				slug = fullConference.slug;
				name = fullConference.name;
				start = fullConference.start;
				DateRange = fullConference.DateRange;
				FormattedAddress = fullConference.FormattedAddress;
				imageUrl = fullConference.imageUrl;
				ImageBytes = fullConference.ImageBytes;
			}
		}

		public string name { get; set; }
		public string DateRange { get; set; }
		public string slug { get; set; }
		public string FormattedAddress { get; set; }
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
				ImageService.GetImageAsync(_fileStore, null, _imageUrl, GetImageSuccess, GetImageError);
			}
		}

	}
}