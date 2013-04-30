//using NUnit.Framework;
//using Ploeh.AutoFixture;
//using Ploeh.AutoFixture.AutoMoq;

//namespace TekConf.UI.Api.Tests.Unit
//{
//	[TestFixture]
//	public class ConferencesCountServiceTest
//	{
//		private IFixture _fixture;

//		[TestFixtureSetUp]
//		public void TestInitialize()
//		{
//			_fixture = new Fixture()
//					.Customize(new MultipleCustomization())
//					.Customize(new AutoMoqCustomization());
//		}

//		//[Test]
//		//public void Count_should_return_correct_value_for_no_filter()
//		//{
//		//	ITinyMessengerHub hub = new Mock<ITinyMessengerHub>().Object;
//		//	var conferencesRepostioryMock = new Mock<IConferenceRepository>();

//		//	var conferenceEntityLive = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-1))
//		//			.With(x => x.end, DateTime.Now.AddDays(1))
//		//		.CreateAnonymous();
//		//	conferenceEntityLive.Publish();

//		//	var conferenceEntityNotPublished = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-1))
//		//			.With(x => x.end, DateTime.Now.AddDays(1))
//		//		.CreateAnonymous();

//		//	var conferenceEntityPublishedButOld = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-100))
//		//			.With(x => x.end, DateTime.Now.AddDays(-99))
//		//		.CreateAnonymous();
//		//	conferenceEntityPublishedButOld.Publish();

//		//	var conferences = new List<ConferenceEntity>()
//		//									{
//		//										conferenceEntityLive,
//		//										conferenceEntityNotPublished,
//		//										conferenceEntityPublishedButOld
//		//									};

//		//	conferencesRepostioryMock.Setup(x => x.AsQueryable()).Returns(conferences.AsQueryable());
//		//	var configuration = new Mock<IEntityConfiguration>().Object;

//		//	var geolocationRepositoryMock = new Mock<IRepository<GeoLocationEntity>>();
//		//	var service = new ConferencesService(hub, conferencesRepostioryMock.Object, geolocationRepositoryMock.Object, configuration);

//		//	var request = new ConferencesCount();
//		//	var count = service.Get(request);
//		//	count.ShouldEqual(1);
//		//}


//		//[Test]
//		//public void Count_should_return_correct_value_for_filter()
//		//{
//		//	var searchTerm = _fixture.Create<string>();
//		//	ITinyMessengerHub hub = new Mock<ITinyMessengerHub>().Object;
//		//	var conferencesRepositoryMock = new Mock<IConferenceRepository>();

//		//	var conferenceEntityLive = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-1))
//		//			.With(x => x.end, DateTime.Now.AddDays(1))
//		//			.With(x => x.name, searchTerm)
//		//		.CreateAnonymous();
//		//	conferenceEntityLive.Publish();

//		//	var conferenceEntityNotPublished = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-1))
//		//			.With(x => x.end, DateTime.Now.AddDays(1))
//		//		.CreateAnonymous();

//		//	var conferenceEntityPublishedButOld = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-100))
//		//			.With(x => x.end, DateTime.Now.AddDays(-99))
//		//		.CreateAnonymous();
//		//	conferenceEntityPublishedButOld.Publish();

//		//	var conferences = new List<ConferenceEntity>()
//		//									{
//		//										conferenceEntityLive,
//		//										conferenceEntityNotPublished,
//		//										conferenceEntityPublishedButOld
//		//									};

//		//	conferencesRepositoryMock.Setup(x => x.AsQueryable()).Returns(conferences.AsQueryable());
//		//	var configuration = new Mock<IEntityConfiguration>().Object;

//		//	var geolocationRepositoryMock = new Mock<IRepository<GeoLocationEntity>>();

//		//	var service = new ConferencesService(hub, conferencesRepositoryMock.Object, geolocationRepositoryMock.Object, configuration);

//		//	var request = new ConferencesCount()
//		//					{
//		//						searchTerm = searchTerm
//		//					};
//		//	var count = service.Get(request);
//		//	count.ShouldEqual(1);
//		//}

//		//[Test]
//		//public void Count_should_return_correct_value_for_past_conferences_filter()
//		//{
//		//	ITinyMessengerHub hub = new Mock<ITinyMessengerHub>().Object;
//		//	var conferencesRepositoryMock = new Mock<IConferenceRepository>();

//		//	var conferenceEntityLive = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-1))
//		//			.With(x => x.end, DateTime.Now.AddDays(1))
//		//		.CreateAnonymous();
//		//	conferenceEntityLive.Publish();

//		//	var conferenceEntityNotPublished = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-1))
//		//			.With(x => x.end, DateTime.Now.AddDays(1))
//		//		.CreateAnonymous();

//		//	var conferenceEntityPublishedButOld = _fixture.Build<ConferenceEntity>()
//		//			.With(x => x.start, DateTime.Now.AddDays(-100))
//		//			.With(x => x.end, DateTime.Now.AddDays(-99))
//		//		.CreateAnonymous();
//		//	conferenceEntityPublishedButOld.Publish();

//		//	var conferences = new List<ConferenceEntity>()
//		//									{
//		//										conferenceEntityLive,
//		//										conferenceEntityNotPublished,
//		//										conferenceEntityPublishedButOld
//		//									};

//		//	conferencesRepositoryMock.Setup(x => x.AsQueryable()).Returns(conferences.AsQueryable());
//		//	var configuration = new Mock<IEntityConfiguration>().Object;
//		//	var geolocationRepositoryMock = new Mock<IRepository<GeoLocationEntity>>();

//		//	var service = new ConferencesService(hub, conferencesRepositoryMock.Object, geolocationRepositoryMock.Object, configuration);

//		//	var request = new ConferencesCount()
//		//	{
//		//		showPastConferences = true
//		//	};


//		//	var count = service.Get(request);
//		//	count.ShouldEqual(2);
//		//}
//	}
//}
