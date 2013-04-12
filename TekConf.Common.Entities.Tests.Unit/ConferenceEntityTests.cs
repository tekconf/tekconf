using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Should;
using TekConf.Common.Entities;
using TinyMessenger;

[TestFixture]
public class ConferenceEntityTests
{
	private IFixture _fixture;

	[TestFixtureSetUp]
	public void Setup()
	{
		_fixture = new Fixture().Customize(new AutoMoqCustomization());
	}

	[Test]
	public void Should_save_to_db()
	{
		var repository = new Mock<IConferenceRepository>();
		var hub = new Mock<ITinyMessengerHub>();
		var entity = new ConferenceEntity(hub.Object, repository.Object);
		entity.Save();

		repository.Verify(x => x.Save(entity));
	}

	[Test]
	public void Should_publish_conference_created_message()
	{
		var repository = new Mock<IConferenceRepository>();
		var hub = new Mock<ITinyMessengerHub>();
		var entity = new ConferenceEntity(hub.Object, repository.Object);
		entity.Save();

		var message = new ConferenceCreatedMessage()
		{
			ConferenceName = entity.name,
			ConferenceSlug = entity.slug
		};

		hub.Verify(x => x.Publish(message));
	}

	[Test]
	public void Should_publish_conference_updated_message()
	{
		var repository = new Mock<IConferenceRepository>();
		var hub = new Mock<ITinyMessengerHub>();
		var entity = new ConferenceEntity(hub.Object, repository.Object)
		{
			 _id = Guid.NewGuid()
		};
		entity.Save();

		var message = new ConferenceUpdatedMessage()
		{
			ConferenceName = entity.name,
			ConferenceSlug = entity.slug
		};

		hub.Verify(x => x.Publish(message));
	}

	[Test]
	public void Should_generate_slug_on_save()
	{
		var repository = new Mock<IConferenceRepository>();
		var hub = new Mock<ITinyMessengerHub>();
		var name = "ConFerence name with spaces  ";
		var entity = new ConferenceEntity(hub.Object, repository.Object)
		{
			name = name
		};

		entity.Save();
		entity.slug.ShouldEqual(name.Trim().ToLower().Replace(" ", "-"));
	}

	[Test]
	public void Should_set_date_on_save()
	{
		var repository = new Mock<IConferenceRepository>();
		var hub = new Mock<ITinyMessengerHub>();
		var entity = new ConferenceEntity(hub.Object, repository.Object);

		entity.Save();
		entity.dateAdded.Date.ShouldEqual(DateTime.Now.Date);
	}

	[Test]
	public void Should_set_isSaved_on_save()
	{
		var repository = new Mock<IConferenceRepository>();
		var hub = new Mock<ITinyMessengerHub>();
		var entity = new ConferenceEntity(hub.Object, repository.Object);

		entity.isSaved.ShouldBeFalse();
		entity.Save();
		entity.isSaved.ShouldBeTrue();
	}

	[Test]
	public void Should_add_session_to_list()
	{
		var repository = new Mock<IConferenceRepository>();
		var hub = new Mock<ITinyMessengerHub>();
		var entity = new ConferenceEntity(hub.Object, repository.Object);

		entity.isSaved.ShouldBeFalse();
		entity.Save();
		entity.isSaved.ShouldBeTrue();
		entity.sessions.ShouldNotBeNull();
		entity.sessions.Count().ShouldEqual(0);
		entity.AddSession(new SessionEntity());
		entity.sessions.Count().ShouldEqual(1);
		entity.Save();
		var message = new SessionAddedMessage();
		hub.Verify(x => x.Publish(message));
	}
}