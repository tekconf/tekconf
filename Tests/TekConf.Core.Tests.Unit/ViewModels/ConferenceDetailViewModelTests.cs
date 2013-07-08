//using System.Linq;
//using Cirrious.MvvmCross.Plugins.Messenger;
//using Moq;
//using NUnit.Framework;
//using Should;
//using TekConf.Core.Interfaces;
//using TekConf.Core.Messages;
//using TekConf.Core.Repositories;
//using TekConf.Core.Services;
//using TekConf.Core.ViewModels;

//namespace TekConf.Core.Tests.Unit.ViewModels
//{
//	using System.Collections.Generic;

//	using Ploeh.AutoFixture;
//	using System;
//	using TekConf.RemoteData.Dtos.v1;

//	[TestFixture]
//	public class ConferenceDetailViewModelTests : TestBase
//	{
//		private IFixture _fixture;

//		[TestFixtureSetUp]
//		public void Initialize()
//		{
//			_fixture = new Fixture();
//			base.Setup();
//		}

//		[Test]
//		public void Should_get_view_model()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			vm.ShouldNotBeNull();
//		}

//		[Test]
//		public void Should_get_conference_from_cache()
//		{
//			var slug = _fixture.Create<string>();

//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var conference = _fixture.Create<ConferenceDetailViewDto>();
//			localConferencesRepository.Setup(x => x.Get(slug)).Returns(conference);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);
//			vm.Init(slug);

//			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Once());
//			remoteDataService.Verify(x => x.GetConferenceDetail(slug, false, It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Never());

//		}

//		[Test]
//		public void Should_get_conference_from_remote()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);
//			var slug = _fixture.Create<string>();
//			vm.Init(slug);

//			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Once());
//			remoteDataService.Verify(x => x.GetConferenceDetail(slug, false, It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Once());
//		}

//		[Test]
//		public void Should_publish_exception_message_when_exception_getting_conference_from_remote()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			remoteDataService.Setup(
//				x =>
//					x.GetConferenceDetail(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Action<ConferenceDetailViewDto>>(),
//						It.IsAny<Action<Exception>>())).Throws(new Exception());

//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);
//			var slug = _fixture.Create<string>();
//			vm.Init(slug);

//			messenger.Verify(x => x.Publish(It.IsAny<ConferenceDetailExceptionMessage>()), Times.Once());
//			vm.IsLoading.ShouldEqual(false);
//		}

//		[Test]
//		public void Should_get_conference_from_remote_when_refreshing()
//		{
//			var slug = _fixture.Create<string>();

//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var conference = _fixture.Create<ConferenceDetailViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferenceDetail(slug)).Returns(conference);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);
//			vm.Init(slug);
//			vm.IsLoading = false;
//			vm.Refresh(slug);
//			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Once());
//			remoteDataService.Verify(x => x.GetConferenceDetail(slug, true, It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Once());
//		}

//		[Test]
//		public void Should_not_get_conference_from_cache_if_loading()
//		{
//			var slug = _fixture.Create<string>();

//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var conference = _fixture.Create<ConferenceDetailViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferenceDetail(slug)).Returns(conference);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				IsLoading = true
//			};
//			vm.Init(slug);

//			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Never());

//		}

//		[Test]
//		public void Should_not_get_conference_from_remote_if_loading()
//		{
//			var slug = _fixture.Create<string>();

//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var conference = _fixture.Create<ConferenceDetailViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferenceDetail(slug)).Returns(conference);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				IsLoading = true
//			};
//			vm.Init(slug);

//			remoteDataService.Verify(x => x.GetConferenceDetail(slug, It.IsAny<bool>(), It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Never());

//		}

//		[Test]
//		public void Should_update_conference()
//		{
//			Assert.Fail();
//		}

//		[Test]
//		public void Should_notify_when_authentication_changes()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			int propertyChangedCount = 0;
//			vm.PropertyChanged += (sender, args) =>
//			{
//				if (args.PropertyName == "IsAuthenticated")
//				{
//					propertyChangedCount++;
//				}
//			};

//			vm.IsAuthenticated = true;
//			propertyChangedCount.ShouldEqual(1);

//		}

//		[Test]
//		public void Should_notify_when_page_title_changes()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			int propertyChangedCount = 0;
//			vm.PropertyChanged += (sender, args) =>
//			{
//				if (args.PropertyName == "PageTitle")
//				{
//					propertyChangedCount++;
//				}
//			};

//			vm.PageTitle = _fixture.Create<string>();
//			propertyChangedCount.ShouldEqual(1);

//		}

//		[Test]
//		public void Should_include_facebook_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, connectItem)
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("facebookUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_homepageUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, connectItem)
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("homepageUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_lanyrdUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, connectItem)
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("lanyrdUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_meetupUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, connectItem)
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("meetupUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_googlePlusUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, connectItem)
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("googlePlusUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_vimeoUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, connectItem)
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("vimeoUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_youtubeUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, connectItem)
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("youtubeUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_githubUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, connectItem)
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("githubUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_linkedInUrl_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, connectItem)
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("linkedInUrl");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_twitterHashTag_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, connectItem)
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("twitterHashTag");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_include_twitterName_in_connectItems()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();

//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, connectItem)
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.ConnectItems.Count.ShouldEqual(1);
//			vm.ConnectItems.First().Name.ShouldEqual("twitterName");
//			vm.ConnectItems.First().Value.ShouldEqual(connectItem);
//		}

//		[Test]
//		public void Should_have_sessions()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.hasSessions, true).Create();
//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasSessions.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_facebook()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, connectItem)
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_homepage()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, connectItem)
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_lanyrd()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, connectItem)
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_meetup()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, connectItem)
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_googlePlus()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, connectItem)
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_vimeo()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, connectItem)
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_youtube()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, connectItem)
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_github()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, connectItem)
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_linkedIn()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, connectItem)
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_twitterHashTag()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, connectItem)
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_have_connect_items_if_twitter_name()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, connectItem)
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeTrue();
//		}

//		[Test]
//		public void Should_not_have_connect_items_if_null_conference()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//			};
//			vm.HasConnectItems.ShouldBeFalse();
//		}

//		[Test]
//		public void Should_not_have_connect_items_if_no_items()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
//			var connectItem = _fixture.Create<string>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>()
//				.With(x => x.facebookUrl, "")
//				.With(x => x.homepageUrl, "")
//				.With(x => x.lanyrdUrl, "")
//				.With(x => x.meetupUrl, "")
//				.With(x => x.googlePlusUrl, "")
//				.With(x => x.vimeoUrl, "")
//				.With(x => x.youtubeUrl, "")
//				.With(x => x.githubUrl, "")
//				.With(x => x.linkedInUrl, "")
//				.With(x => x.twitterHashTag, "")
//				.With(x => x.twitterName, "")
//				.Create();


//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object)
//			{
//				Conference = conference
//			};
//			vm.HasConnectItems.ShouldBeFalse();
//		}

//		[Test]
//		public void Should_navigate_to_settings()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();
			
//			MockDispatcher.Requests.Clear();
//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			MockDispatcher.Requests.Count.ShouldEqual(0);

//			vm.ShowSettingsCommand.Execute(null);
//			MockDispatcher.Requests.Count.ShouldEqual(1);
//			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(SettingsViewModel));
//		}

//		[Test]
//		public void Should_navigate_to_conference_sessions()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			MockDispatcher.Requests.Clear();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			MockDispatcher.Requests.Count.ShouldEqual(0);

//			vm.ShowSessionsCommand.Execute(null);
//			MockDispatcher.Requests.Count.ShouldEqual(1);
//			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(ConferenceSessionsViewModel));
//		}

//		[Test]
//		public void Should_navigate_to_session_detail()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			MockDispatcher.Requests.Clear();

//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			MockDispatcher.Requests.Count.ShouldEqual(0);

//			vm.ShowSessionDetailCommand.Execute(null);
//			MockDispatcher.Requests.Count.ShouldEqual(1);
//			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(SessionDetailViewModel));
//		}

//		[Test]
//		public void Add_to_favorites_should_add_to_local_cache()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			authentication.Setup(x => x.IsAuthenticated).Returns(true);
//			var messenger = new Mock<IMvxMessenger>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			vm.Conference = conference;
//			vm.AddFavoriteCommand.Execute(null);
//			localScheduleRepository.Verify(x => x.SaveSchedule(It.IsAny<ScheduleDto>()), Times.Once());
//		}

//		[Test]
//		public void Add_to_favorites_should_add_to_remote_data()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			authentication.Setup(x => x.IsAuthenticated).Returns(true);
//			var messenger = new Mock<IMvxMessenger>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			vm.Conference = conference;
//			vm.AddFavoriteCommand.Execute(null);
//			remoteDataService.Verify(x => x.AddToSchedule(
//																		It.IsAny<string>(),
//																		It.IsAny<string>(),
//																		It.IsAny<Action<ScheduleDto>>(),
//																		It.IsAny<Action<Exception>>()), Times.Once());
//		}

//		[Test]
//		public void Add_to_favorites_should_publish_message()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			authentication.Setup(x => x.IsAuthenticated).Returns(true);
//			var messenger = new Mock<IMvxMessenger>();
//			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
//			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
//				localConferencesRepository.Object,
//				analytics.Object,
//				authentication.Object,
//				messenger.Object,
//				localScheduleRepository.Object);

//			vm.Conference = conference;
//			vm.AddFavoriteCommand.Execute(null);


//			messenger.Verify(x => x.Publish(It.IsAny<FavoriteConferencesUpdatedMessage>()), Times.Once());
//		}
//	}
//}