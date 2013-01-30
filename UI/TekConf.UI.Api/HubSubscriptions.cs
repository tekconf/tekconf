using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNet.SignalR;
using SendGridMail;
using SendGridMail.Transport;
using TinyMessenger;

namespace TekConf.UI.Api
{
	public interface IEmailSender
	{
		void Send(string message);
	}
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Send(string message)
		{
			var myMessage = SendGrid.GetInstance();

			myMessage.From = new MailAddress("robgibbens@arteksoftware.com");

			var recipients = new List<string>
				{
						@"Rob Gibbens <robgibbens@gmail.com.com>",
						@"Rob Gibbens <robgibbens@arteksoftware.com>",
				};

			myMessage.AddTo(recipients);

			myMessage.Subject = "Testing the SendGrid Library";

			myMessage.Html = string.Format("<p>{0}</p>", message);
			myMessage.Text = string.Format("{0}", message);

		
			// Create credentials, specifying your user name and password.
			var credentials = new NetworkCredential("azure_4c325a45cc209c2f4b523188604da156@azure.com", "a3gdm7bn");

			// Create an SMTP transport for sending email.
			var transportSmtp = SMTP.GetInstance(credentials);

			// Send the email.
			transportSmtp.Deliver(myMessage);
		}
	}
	public class HubSubscriptions
	{
		private readonly ITinyMessengerHub _hub;
		private readonly IRepository<SessionRoomChangedMessage> _sessionRoomChangedRepository;
		private readonly IRepository<ConferenceLocationChangedMessage> _conferenceLocationChangedRepository;
		private readonly IRepository<ConferenceEndDateChangedMessage> _conferenceEndDateChangedRepository;
		private readonly IRepository<ConferencePublishedMessage> _conferencePublishedRepository;
		private readonly IRepository<ConferenceUpdatedMessage> _conferenceUpdatedRepository;
		private readonly IRepository<ConferenceStartDateChangedMessage> _conferenceStartDateChangedRepository;
		private readonly IRepository<SessionAddedMessage> _sessionAddedRepository;
		private readonly IRepository<SessionRemovedMessage> _sessionRemovedRepository;
		private readonly IRepository<SpeakerAddedMessage> _speakerAddedRepository;
		private readonly IRepository<SpeakerRemovedMessage> _speakerRemovedRepository;
		private readonly IRepository<ConferenceCreatedMessage> _conferenceCreatedRepository;
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;

		public HubSubscriptions(ITinyMessengerHub hub,
																IRepository<SessionRoomChangedMessage> sessionRoomChangedRepository,
																IRepository<ConferenceLocationChangedMessage> conferenceLocationChangedRepository,
																IRepository<ConferenceEndDateChangedMessage> conferenceEndDateChangedRepository,
																IRepository<ConferencePublishedMessage> conferencePublishedRepository,
																IRepository<ConferenceUpdatedMessage> conferenceUpdatedRepository,
																IRepository<ConferenceStartDateChangedMessage> conferenceStartDateChangedRepository,
																IRepository<SessionAddedMessage> sessionAddedRepository,
																IRepository<SessionRemovedMessage> sessionRemovedRepository,
																IRepository<SpeakerAddedMessage> speakerAddedRepository,
																IRepository<SpeakerRemovedMessage> speakerRemovedRepository,
																IRepository<ConferenceCreatedMessage> conferenceCreatedRepository,
																IEmailSender emailSender,
																IConfiguration configuration
														)
		{
			_hub = hub;

			_sessionRoomChangedRepository = sessionRoomChangedRepository;
			_sessionAddedRepository = sessionAddedRepository;
			_conferencePublishedRepository = conferencePublishedRepository;

			_conferenceLocationChangedRepository = conferenceLocationChangedRepository;
			_conferenceEndDateChangedRepository = conferenceEndDateChangedRepository;
			_conferenceUpdatedRepository = conferenceUpdatedRepository;
			_conferenceStartDateChangedRepository = conferenceStartDateChangedRepository;
			_sessionRemovedRepository = sessionRemovedRepository;
			_speakerAddedRepository = speakerAddedRepository;
			_speakerRemovedRepository = speakerRemovedRepository;
			_conferenceCreatedRepository = conferenceCreatedRepository;
			_emailSender = emailSender;
			_configuration = configuration;

			Subscribe();
		}

		private void Subscribe()
		{
			SubscribeToSessionRoomChangedEvent();
			SubscribeToSessionAdded();
			SubscribeToConferencePublished();
			SubscribeToConferenceLocationChanged();
			SubscribeToConferenceEndDateChanged();
			SubscribeToConferenceUpdated();
			SubscribeToConferenceStartDateChanged();
			SubscribeToSessionRemoved();
			SubscribeToSpeakerAdded();
			SubscribeToSpeakerRemoved();
			SubscribeToConferenceCreated();
		}

		private void SubscribeToSpeakerRemoved()
		{
			_hub.Subscribe<SpeakerRemovedMessage>((@event) =>
			{
				_speakerRemovedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.SpeakerName + " has been removed from " + @event.SessionTitle;
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToSpeakerAdded()
		{
			_hub.Subscribe<SpeakerAddedMessage>((@event) =>
			{
				_speakerAddedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.SpeakerName + " has been added to " + @event.SessionTitle;
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToSessionRemoved()
		{
			_hub.Subscribe<SessionRemovedMessage>((@event) =>
			{
				_sessionRemovedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.SessionTitle + " has been removed from " + @event.ConferenceName;
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceStartDateChanged()
		{
			_hub.Subscribe<ConferenceStartDateChangedMessage>((@event) =>
			{
				_conferenceStartDateChangedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " start date has changed from "
																+ (@event.OldValue.HasValue ? @event.OldValue.Value.ToString() : "(not set)")
																+ " to "
																+ (@event.NewValue.HasValue ? @event.NewValue.Value.ToString() : "(not set)");
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceUpdated()
		{
			_hub.Subscribe<ConferenceUpdatedMessage>((@event) =>
			{
				_conferenceUpdatedRepository.Save(@event);

				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " has been updated.";
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceCreated()
		{
			_hub.Subscribe<ConferenceCreatedMessage>((@event) =>
			{
				_conferenceCreatedRepository.Save(@event);

				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " has been created.";
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceEndDateChanged()
		{
			_hub.Subscribe<ConferenceEndDateChangedMessage>((@event) =>
			{
				_conferenceEndDateChangedRepository.Save(@event);
				//TODO: Only send to people that have this conference saved to their schedule
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " end date has changed from "
																+ (@event.OldValue.HasValue ? @event.OldValue.Value.ToString() : "(not set)")
																+ " to "
																+ (@event.NewValue.HasValue ? @event.NewValue.Value.ToString() : "(not set)");

				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceLocationChanged()
		{
			_hub.Subscribe<ConferenceLocationChangedMessage>((@event) =>
			{
				_conferenceLocationChangedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " location has changed from "
																+ (!string.IsNullOrWhiteSpace(@event.OldValue) ? @event.OldValue : "(not set)")
																+ " to "
																+ (!string.IsNullOrWhiteSpace(@event.NewValue) ? @event.NewValue : "(not set)");
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToSessionRoomChangedEvent()
		{
			_hub.Subscribe<SessionRoomChangedMessage>((@event) =>
							{
								_sessionRoomChangedRepository.Save(@event);
								var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
								var message = @event.SessionTitle + " room has changed from "
																				+ (!string.IsNullOrWhiteSpace(@event.OldValue) ? @event.OldValue : "(not set)")
																				+ " to "
																				+ (!string.IsNullOrWhiteSpace(@event.NewValue) ? @event.NewValue : "(not set)");
								context.Clients.All.broadcastMessage(message);

								_emailSender.Send(message);
			});
		}

		private void SubscribeToSessionAdded()
		{
			_hub.Subscribe<SessionAddedMessage>((@event) =>
								{
									_sessionAddedRepository.Save(@event);

									var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
									var message = @event.SessionTitle + " has been added ";
									context.Clients.All.broadcastMessage(message);

									_emailSender.Send(message);
								});
		}

		private void SubscribeToConferencePublished()
		{
			_hub.Subscribe<ConferencePublishedMessage>((@event) =>
							{
								// Add to community megaphone
								// Add to rss feed
								// Tweet
								_conferencePublishedRepository.Save(@event);
								var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
								var message = @event.ConferenceName + " has been published. ";
								context.Clients.All.broadcastMessage(message);

								_emailSender.Send(message);
							});

		}
	}
}
