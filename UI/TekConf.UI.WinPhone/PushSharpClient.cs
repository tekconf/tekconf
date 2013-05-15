using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Notification;
using TekConf.Core.Models;

namespace TekConf.UI.WinPhone
{
	public class PushSharpClient
	{
		public void RegisterForToast()
		{
			// Holds the push channel that is created or found.

			// The name of our push channel.
			const string channelName = "TekConf.NotificationChannel.Toast";
			//const string channelName = "http://api.tekconf.com/push/wp/register";
			// Try to find the push channel.
			var pushChannel = HttpNotificationChannel.Find(channelName);

			// If the channel was not found, then create a new connection to the push service.
			if (pushChannel == null)
			{
				pushChannel = new HttpNotificationChannel(channelName);

				// Register for all the events before attempting to open the channel.
				pushChannel.ChannelUriUpdated +=
					delegate(object sender, NotificationChannelUriEventArgs e)
					{
						PushService.PostWindowsPhonePushNotificationAsync("RobGibbens", e.ChannelUri.AbsoluteUri, false,
							isSuccessful => { },
							(ex) => { }
							);
						Debug.WriteLine("PushChannel URI Updated: " + e.ChannelUri.ToString());
					};
				pushChannel.ErrorOccurred +=
					delegate(object sender, NotificationChannelErrorEventArgs e)
					{
						Debug.WriteLine("PushChannel Error: " + e.ErrorType.ToString() + " -> " + e.ErrorCode + " -> " + e.Message +
							" -> " + e.ErrorAdditionalData);
					};

				// Register for this notification only if you need to receive the notifications while your application is running.
				pushChannel.ShellToastNotificationReceived += (sender, e) =>
				{
					Deployment.Current.Dispatcher.BeginInvoke(() =>
					{
						if (e != null && e.Collection != null && e.Collection.Count == 3)
							MessageBox.Show(e.Collection.Skip(1).Single().Value);
					});

				};

				pushChannel.Open();
			}
			else
			{
				// The channel was already open, so just register for all the events.
				pushChannel.ChannelUriUpdated +=
					delegate(object sender, NotificationChannelUriEventArgs e)
					{
						PushService.PostWindowsPhonePushNotificationAsync("RobGibbens", e.ChannelUri.AbsoluteUri, false,
							isSuccessful => { },
							(ex) => { }
							);
						Debug.WriteLine("PushChannel URI Updated: " + e.ChannelUri.ToString());
					};
				pushChannel.ErrorOccurred +=
					delegate(object sender, NotificationChannelErrorEventArgs e)
					{
						Debug.WriteLine("PushChannel Error: " + e.ErrorType.ToString() + " -> " + e.ErrorCode + " -> " + e.Message +
							" -> " + e.ErrorAdditionalData);
					};

				// Bind this new channel for toast events.
				if (pushChannel.IsShellToastBound)
					Console.WriteLine("Already Bound to Toast");
				else
					pushChannel.BindToShellToast();

				if (pushChannel.IsShellTileBound)
					Console.WriteLine("Already Bound to Tile");
				else
					pushChannel.BindToShellTile();

				//// Register for this notification only if you need to receive the notifications while your application is running.
				pushChannel.ShellToastNotificationReceived += (sender, e) =>
				{
					Deployment.Current.Dispatcher.BeginInvoke(() =>
					{
						if (e != null && e.Collection != null && e.Collection.Count == 3)
							MessageBox.Show(e.Collection.ToArray()[1].Value);
					});
				};
			}

			// Bind this new channel for toast events.
			if (pushChannel.IsShellToastBound)
				Console.WriteLine("Already Bound to Toast");
			else
				pushChannel.BindToShellToast();

			if (pushChannel.IsShellTileBound)
				Console.WriteLine("Already Bound to Tile");
			else
				pushChannel.BindToShellTile();

			// Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
			if (pushChannel.ChannelUri != null)
				Debug.WriteLine(pushChannel.ChannelUri.ToString());
		}

		//public void RegisterForTile()
		//{
		//	// Holds the push channel that is created or found.

		//	// The name of our push channel.
		//	const string channelName = "TekConf.NotificationChannel.Tile";
		//	//const string channelName = "http://api.tekconf.com/push/wp/register";
		//	// Try to find the push channel.
		//	var pushChannel = HttpNotificationChannel.Find(channelName);

		//	// If the channel was not found, then create a new connection to the push service.
		//	if (pushChannel == null)
		//	{
		//		pushChannel = new HttpNotificationChannel(channelName);

		//		// Register for all the events before attempting to open the channel.
		//		pushChannel.ChannelUriUpdated +=
		//			delegate(object sender, NotificationChannelUriEventArgs e)
		//			{
		//				PushService.PostWindowsPhonePushNotificationAsync("RobGibbens", e.ChannelUri.AbsoluteUri, false,
		//					isSuccessful => { },
		//					(ex) => { }
		//					);
		//				Debug.WriteLine("PushChannel URI Updated: " + e.ChannelUri.ToString());
		//			};
		//		pushChannel.ErrorOccurred +=
		//			delegate(object sender, NotificationChannelErrorEventArgs e)
		//			{
		//				Debug.WriteLine("PushChannel Error: " + e.ErrorType.ToString() + " -> " + e.ErrorCode + " -> " + e.Message +
		//					" -> " + e.ErrorAdditionalData);
		//			};

		//		// Register for this notification only if you need to receive the notifications while your application is running.
		//		pushChannel.ShellToastNotificationReceived += (sender, e) =>
		//		{
		//			Deployment.Current.Dispatcher.BeginInvoke(() =>
		//			{
		//				if (e != null && e.Collection != null && e.Collection.Count == 3)
		//					MessageBox.Show(e.Collection.Skip(1).Single().Value);
		//			});

		//		};

		//		pushChannel.Open();
		//	}
		//	else
		//	{
		//		// The channel was already open, so just register for all the events.
		//		pushChannel.ChannelUriUpdated +=
		//			delegate(object sender, NotificationChannelUriEventArgs e)
		//			{
		//				PushService.PostWindowsPhonePushNotificationAsync("RobGibbens", e.ChannelUri.AbsoluteUri, false,
		//					isSuccessful => { },
		//					(ex) => { }
		//					);
		//				Debug.WriteLine("PushChannel URI Updated: " + e.ChannelUri.ToString());
		//			};
		//		pushChannel.ErrorOccurred +=
		//			delegate(object sender, NotificationChannelErrorEventArgs e)
		//			{
		//				Debug.WriteLine("PushChannel Error: " + e.ErrorType.ToString() + " -> " + e.ErrorCode + " -> " + e.Message +
		//					" -> " + e.ErrorAdditionalData);
		//			};

		//		// Bind this new channel for toast events.
		//		if (pushChannel.IsShellToastBound)
		//			Console.WriteLine("Already Bound to Toast");
		//		else
		//			pushChannel.BindToShellToast();

		//		if (pushChannel.IsShellTileBound)
		//			Console.WriteLine("Already Bound to Tile");
		//		else
		//			pushChannel.BindToShellTile();

		//		//// Register for this notification only if you need to receive the notifications while your application is running.
		//		pushChannel.ShellToastNotificationReceived += (sender, e) =>
		//		{
		//			Deployment.Current.Dispatcher.BeginInvoke(() =>
		//			{
		//				if (e != null && e.Collection != null && e.Collection.Count == 3)
		//					MessageBox.Show(e.Collection.ToArray()[1].Value);
		//			});
		//		};
		//	}

		//	// Bind this new channel for toast events.
		//	if (pushChannel.IsShellToastBound)
		//		Console.WriteLine("Already Bound to Toast");
		//	else
		//		pushChannel.BindToShellToast();

		//	if (pushChannel.IsShellTileBound)
		//		Console.WriteLine("Already Bound to Tile");
		//	else
		//		pushChannel.BindToShellTile();

		//	// Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
		//	if (pushChannel.ChannelUri != null)
		//		Debug.WriteLine(pushChannel.ChannelUri.ToString());
		//}



	}
}