using System;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;

public class EventsHub : Hub
{
	public void Send(string name, string message)
	{
		// Call the broadcastMessage method to update clients.
		Clients.All.broadcastMessage(name, message);
	}
}