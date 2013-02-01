using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Web.ViewModels
{
	public class SpeakersViewModel
	{
		public List<ConferencesDto> OpenConferences { get; set; }
		public List<PresentationDto> Presentations { get; set; }

		public List<ConferencesDto> MyConferences { get; set; }
	}
}