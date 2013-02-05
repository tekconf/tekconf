using Microsoft.AspNet.SignalR;

namespace TekConf.UI.Api
{
	public class EventsHub : Hub
	{
		public void Send(string name, string message)
		{
			// Call the broadcastMessage method to update clients.
			Clients.All.broadcastMessage(name, message);
		}
	}
}