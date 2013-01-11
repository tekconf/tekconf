using System;
using TinyMessenger;

namespace TekConf.UI.Api
{
	public interface ITinyMessageBase : ITinyMessage
	{
		Guid Id { get; set; }
		object Sender { get; set; }
		DateTime EventDate { get; set; }
	}
}