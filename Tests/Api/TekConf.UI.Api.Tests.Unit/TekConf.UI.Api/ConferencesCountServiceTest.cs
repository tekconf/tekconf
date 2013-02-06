using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TekConf.UI.Api.Services.v1;
using TinyMessenger;
using Should;
using TekConf.UI.Api.Services.Requests.v1;
using Ploeh.AutoFixture.AutoMoq;

namespace TekConf.UI.Api.Tests.Unit
{

		[TestFixture]
		public class ConferencesCountServiceTest
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
				public void Count_should_return_correct_value_for_no_filter()
				{
						ITinyMessengerHub hub = new Mock<ITinyMessengerHub>().Object;
						var repositoryMock = new Mock<IRepository<ConferenceEntity>>();

						var conferenceEntityLive = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-1))
								.With(x => x.end, DateTime.Now.AddDays(1))
							.CreateAnonymous();
						conferenceEntityLive.Publish();

						var conferenceEntityNotPublished = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-1))
								.With(x => x.end, DateTime.Now.AddDays(1))
							.CreateAnonymous();

						var conferenceEntityPublishedButOld = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-100))
								.With(x => x.end, DateTime.Now.AddDays(-99))
							.CreateAnonymous();
						conferenceEntityPublishedButOld.Publish();

						var conferences = new List<ConferenceEntity>()
											{
												conferenceEntityLive,
												conferenceEntityNotPublished,
												conferenceEntityPublishedButOld
											};

						repositoryMock.Setup(x => x.AsQueryable()).Returns(conferences.AsQueryable());
						var configuration = new Mock<IConfiguration>().Object;

						var service = new ConferencesService(hub, repositoryMock.Object, configuration);

						var request = new ConferencesCount();
						var count = service.Get(request);
						count.ShouldEqual(1);
				}


				[Test]
				public void Count_should_return_correct_value_for_filter()
				{
					var searchTerm = _fixture.CreateAnonymous<string>();
						ITinyMessengerHub hub = new Mock<ITinyMessengerHub>().Object;
						var repositoryMock = new Mock<IRepository<ConferenceEntity>>();

						var conferenceEntityLive = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-1))
								.With(x => x.end, DateTime.Now.AddDays(1))
								.With(x => x.name, searchTerm)
							.CreateAnonymous();
						conferenceEntityLive.Publish();

						var conferenceEntityNotPublished = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-1))
								.With(x => x.end, DateTime.Now.AddDays(1))
							.CreateAnonymous();

						var conferenceEntityPublishedButOld = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-100))
								.With(x => x.end, DateTime.Now.AddDays(-99))
							.CreateAnonymous();
						conferenceEntityPublishedButOld.Publish();

						var conferences = new List<ConferenceEntity>()
											{
												conferenceEntityLive,
												conferenceEntityNotPublished,
												conferenceEntityPublishedButOld
											};

						repositoryMock.Setup(x => x.AsQueryable()).Returns(conferences.AsQueryable());
						var configuration = new Mock<IConfiguration>().Object;

						var service = new ConferencesService(hub, repositoryMock.Object, configuration);

						var request = new ConferencesCount()
										{
												searchTerm = searchTerm
										};
						var count = service.Get(request);
						count.ShouldEqual(1);
				}

				[Test]
				public void Count_should_return_correct_value_for_past_conferences_filter()
				{
						ITinyMessengerHub hub = new Mock<ITinyMessengerHub>().Object;
						var repositoryMock = new Mock<IRepository<ConferenceEntity>>();

						var conferenceEntityLive = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-1))
								.With(x => x.end, DateTime.Now.AddDays(1))
							.CreateAnonymous();
						conferenceEntityLive.Publish();

						var conferenceEntityNotPublished = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-1))
								.With(x => x.end, DateTime.Now.AddDays(1))
							.CreateAnonymous();

						var conferenceEntityPublishedButOld = _fixture.Build<ConferenceEntity>()
								.With(x => x.start, DateTime.Now.AddDays(-100))
								.With(x => x.end, DateTime.Now.AddDays(-99))
							.CreateAnonymous();
						conferenceEntityPublishedButOld.Publish();

						var conferences = new List<ConferenceEntity>()
											{
												conferenceEntityLive,
												conferenceEntityNotPublished,
												conferenceEntityPublishedButOld
											};

						repositoryMock.Setup(x => x.AsQueryable()).Returns(conferences.AsQueryable());
						var configuration = new Mock<IConfiguration>().Object;

						var service = new ConferencesService(hub, repositoryMock.Object, configuration);

						var request = new ConferencesCount()
						{
								showPastConferences = true
						};


						var count = service.Get(request);
						count.ShouldEqual(2);
				}
		}
}
