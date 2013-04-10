//using System;
//using Moq;
//using NUnit.Framework;
//using Ploeh.AutoFixture;
//using TinyMessenger;

//namespace TekConf.Common.Entities.Tests.Unit
//{
//	[TestFixture]
//	public class Conference_Entity
//	{
//		private IFixture _fixture;
//		private Mock<ITinyMessengerHub> _mockHub;
//		private Mock<IRepository<ConferenceEntity>> _mockRepository;
//		private ConferenceEntity _conference;

//		[TestFixtureSetUp]
//		public void Setup()
//		{
//			_fixture = new Fixture();
//			_mockHub = new Mock<ITinyMessengerHub>();
//			_mockRepository = new Mock<IRepository<ConferenceEntity>>();
//			_conference = new ConferenceEntity(_mockHub.Object, _mockRepository.Object);

//		}

//		//[Test]
//		//public void should_publish_event_when_saved()
//		//{
//		//	_conference.Save();

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<ConferenceUpdatedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}

//		//[Test]
//		//public void should_publish_event_when_session_added()
//		//{
//		//	var session = new SessionEntity();
//		//	_conference.AddSession(session);

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<SessionAddedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}

//		//[Test]
//		//public void should_publish_event_when_session_removed()
//		//{
//		//	var session = new SessionEntity();
//		//	_conference.RemoveSession(session);

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<SessionRemovedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}

//		//[Test]
//		//public void should_publish_event_when_conference_is_edited()
//		//{
//		//	Assert.Fail();
//		//}

//		//[Test]
//		//public void should_publish_event_when_conference_location_is_changed()
//		//{
//		//	_conference.location = _fixture.CreateAnonymous<string>();

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<ConferenceLocationChangedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		}

//		//[Test]
//		//public void should_publish_event_when_conference_start_date_is_changed()
//		//{
//		//	_conference.start = _fixture.CreateAnonymous<DateTime>();

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<ConferenceStartDateChangedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}

//		//[Test]
//		//public void should_publish_event_when_conference_end_date_is_changed()
//		//{
//		//	_conference.end = _fixture.CreateAnonymous<DateTime>();

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<ConferenceEndDateChangedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}


//		//[Test]
//		//public void should_publish_event_when_conference_removed()
//		//{
//		//	Assert.Fail();
//		//}
//	}

//	[TestFixture]
//	public class Session_Entity
//	{
//		private IFixture _fixture;
//		private Mock<ITinyMessengerHub> _mockHub;
//		private Mock<IRepository<ConferenceEntity>> _mockRepository;
//		private ConferenceEntity _conference;

//		[TestFixtureSetUp]
//		public void Setup()
//		{
//			_fixture = new Fixture();
//			_mockHub = new Mock<ITinyMessengerHub>();
//			_mockRepository = new Mock<IRepository<ConferenceEntity>>();
//			_conference = new ConferenceEntity(_mockHub.Object, _mockRepository.Object);
//		}

//		//[Test]
//		//public void should_publish_event_when_session_is_edited()
//		//{
//		//	Assert.Fail();
//		//}

//		//[Test]
//		//public void should_publish_event_when_session_time_is_changed()
//		//{
//		//	Assert.Fail();
//		//}

//		//[Test]
//		//public void should_publish_event_when_session_room_is_changed()
//		//{
//		//	var session = new SessionEntity() { slug = _fixture.CreateAnonymous<string>() };
//		//	_conference.slug = _fixture.CreateAnonymous<string>();
//		//	_conference.AddSession(session);
//		//	session.room = _fixture.CreateAnonymous<string>();

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<SessionRoomChangedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}

//		//[Test]
//		//public void should_publish_event_when_speaker_is_added()
//		//{
//		//	var session = new SessionEntity();
//		//	_conference.AddSession(session);

//		//	var speaker = new SpeakerEntity();
//		//	_conference.AddSpeakerToSession(session.slug, speaker);

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<SpeakerAddedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}

//		//[Test]
//		//public void should_publish_event_when_speaker_is_removed()
//		//{
//		//	var session = new SessionEntity();
//		//	_conference.AddSession(session);

//		//	var speaker = new SpeakerEntity();
//		//	_conference.RemoveSpeakerFromSession(session.slug, speaker);

//		//	_mockHub.Verify(x => x.Publish(It.IsAny<SpeakerRemovedMessage>()), Times.Once());
//		//	_mockHub.VerifyAll();
//		//}
//	}
//}
