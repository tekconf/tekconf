using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Core.ViewModels
{
	public class ExceptionMessage : MvxMessage
	{
		public ExceptionMessage(object sender, Exception exception) : base(sender)
		{
			ExceptionObject = exception;
		}

		public Exception ExceptionObject { get; private set; }
	}
}