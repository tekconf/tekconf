using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Core.Messages
{


	public class SettingsIsOauthUserRegisteredExceptionMessage : ConnectionExceptionMessage
	{
		public SettingsIsOauthUserRegisteredExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class SessionDetailExceptionMessage : ConnectionExceptionMessage
	{
		public SessionDetailExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class CreateOAuthUserExceptionMessage : ConnectionExceptionMessage
	{
		public CreateOAuthUserExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class ConferenceSessionsExceptionMessage : ConnectionExceptionMessage
	{
		public ConferenceSessionsExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class ConferenceDetailExceptionMessage : ConnectionExceptionMessage
	{
		public ConferenceDetailExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class ConferencesListFavoritesExceptionMessage : ConnectionExceptionMessage
	{
		public ConferencesListFavoritesExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class ConferencesListAllExceptionMessage : ConnectionExceptionMessage
	{
		public ConferencesListAllExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class ConnectionExceptionMessage : ExceptionMessage
	{
		public ConnectionExceptionMessage(object sender, Exception exception) : base(sender, exception) { }
	}

	public class ExceptionMessage : MvxMessage
	{
		public ExceptionMessage(object sender, Exception exception)
			: base(sender)
		{
			ExceptionObject = exception;
		}

		public Exception ExceptionObject { get; private set; }
	}
}