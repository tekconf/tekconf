using System;
using TinyMessenger;

namespace TekConf.UI.Api
{
	public class ConferenceSavedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
	}
}