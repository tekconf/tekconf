using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace TekConf.UI.Api.Tests.Unit
{
		using Moq;

		using Should;

		using TinyMessenger;

		using global::TekConf.Common.Entities;

	[TestFixture]
		public class ConferenceEntityTests
		{
				private IFixture _fixture;

				[TestFixtureSetUp]
				public void TestInitialize()
				{
						_fixture = new Fixture()
								.Customize(new MultipleCustomization())
								.Customize(new AutoMoqCustomization());
				}

				[Test]
				public void Publish_should_set_to_live()
				{
						var hub = new Mock<ITinyMessengerHub>().Object;
						var repository = new Mock<IRepository<ConferenceEntity>>().Object;
						var conference = new ConferenceEntity(hub, repository);

						conference.isLive.ShouldBeFalse();
						conference.Publish();
						conference.isLive.ShouldBeTrue();
				}

				[Test]
				public void Publish_should_update_date_published()
				{
						var hub = new Mock<ITinyMessengerHub>().Object;
						var repository = new Mock<IRepository<ConferenceEntity>>().Object;
						var conference = new ConferenceEntity(hub, repository);

						conference.datePublished.ShouldEqual(default(DateTime));
						conference.Publish();
						conference.datePublished.ShouldNotEqual(default(DateTime));
				}
		}
}
