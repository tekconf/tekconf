using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Core.Repositories;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Messages
{
	public class FavoriteConferencesUpdatedMessage : MvxMessage
	{
		public IEnumerable<ConferencesListViewDto> Conferences { get; set; }

		public FavoriteConferencesUpdatedMessage(object sender, IEnumerable<ConferencesListViewDto> conferences)
			: base(sender)
		{
			Conferences = conferences;
		}
	}
}