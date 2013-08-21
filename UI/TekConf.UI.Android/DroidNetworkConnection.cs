using System;
using System.Diagnostics;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Models;
using TekConf.Core.Repositories;
using TekConf.Core.Services;

namespace TekConf.UI.Android
{

	public class DroidNetworkConnection : INetworkConnection
	{
		public bool IsNetworkConnected()
		{
			return true; //TODO
		}

		public string NetworkDownMessage
		{
			get
			{
				return "Could not connect to remote server. Please check your network connection and try again.";			
			}
		}
	}
	
}