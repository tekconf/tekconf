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
using Android.App;

namespace TekConf.UI.Android
{

	public class DroidMessageBox : IMessageBox
	{
		public void Show(string message)
		{
			AlertDialog alertMessage = new AlertDialog.Builder(Setup.CurrentActivityContext).Create();

			alertMessage.SetTitle("");

			alertMessage.SetMessage(message);

			alertMessage.Show();
		}
	}
	
}