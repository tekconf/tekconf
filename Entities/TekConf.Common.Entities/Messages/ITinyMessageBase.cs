using System;
using TinyMessenger;

namespace TekConf.Common.Entities
{
	public interface ITinyMessageBase : ITinyMessage
	{
		Guid Id { get; set; }
		object Sender { get; set; }
		DateTime EventDate { get; set; }
	}
}