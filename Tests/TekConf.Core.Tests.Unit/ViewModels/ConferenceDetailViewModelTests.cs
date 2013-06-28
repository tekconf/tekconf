using System.Linq;
using Cirrious.MvvmCross.Plugins.Messenger;
using Moq;
using NUnit.Framework;
using Should;
using TekConf.Core.Interfaces;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.Core.ViewModels;

namespace TekConf.Core.Tests.Unit.ViewModels
{
	using System.Collections.Generic;

	using Ploeh.AutoFixture;
	using System;
	using TekConf.RemoteData.Dtos.v1;

	[TestFixture]
	public class ConferenceDetailViewModelTests : TestBase
	{
		private IFixture _fixture;

		[TestFixtureSetUp]
		public void Initialize()
		{
			_fixture = new Fixture();
			base.Setup();
		}

		[Test]
		public void Should_get_view_model()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			vm.ShouldNotBeNull();
		}

		[Test]
		public void Should_get_conference_from_cache()
		{
			var slug = _fixture.Create<string>();

			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var conference = _fixture.Create<ConferenceDetailViewDto>();
			localConferencesRepository.Setup(x => x.GetConferenceDetail(slug)).Returns(conference);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);
			vm.Init(slug);

			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Once());
			remoteDataService.Verify(x => x.GetConferenceDetail(slug, false, It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Never());

		}

		[Test]
		public void Should_get_conference_from_remote()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);
			var slug = _fixture.Create<string>();
			vm.Init(slug);

			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Once());
			remoteDataService.Verify(x => x.GetConferenceDetail(slug, false, It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Once());
		}

		[Test]
		public void Should_get_conference_from_remote_when_refreshing()
		{
			var slug = _fixture.Create<string>();

			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var conference = _fixture.Create<ConferenceDetailViewDto>();
			localConferencesRepository.Setup(x => x.GetConferenceDetail(slug)).Returns(conference);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);
			vm.Init(slug);
			vm.IsLoading = false;
			vm.Refresh(slug);
			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Once());
			remoteDataService.Verify(x => x.GetConferenceDetail(slug, true, It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Once());
		}

		[Test]
		public void Should_not_get_conference_from_cache_if_loading()
		{
			var slug = _fixture.Create<string>();

			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var conference = _fixture.Create<ConferenceDetailViewDto>();
			localConferencesRepository.Setup(x => x.GetConferenceDetail(slug)).Returns(conference);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				IsLoading = true
			};
			vm.Init(slug);

			localConferencesRepository.Verify(x => x.GetConferenceDetail(slug), Times.Never());

		}

		[Test]
		public void Should_not_get_conference_from_remote_if_loading()
		{
			var slug = _fixture.Create<string>();

			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var conference = _fixture.Create<ConferenceDetailViewDto>();
			localConferencesRepository.Setup(x => x.GetConferenceDetail(slug)).Returns(conference);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				IsLoading = true
			};
			vm.Init(slug);

			remoteDataService.Verify(x => x.GetConferenceDetail(slug, It.IsAny<bool>(), It.IsAny<Action<ConferenceDetailViewDto>>(), It.IsAny<Action<Exception>>()), Times.Never());

		}

		[Test]
		public void Should_update_conference()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_have_sessions()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.hasSessions, true).Create();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasSessions.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_facebook()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, connectItem)
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_homepage()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, connectItem)
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_lanyrd()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, connectItem)
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_meetup()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, connectItem)
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_googlePlus()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, connectItem)
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_vimeo()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, connectItem)
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_youtube()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, connectItem)
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_github()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, connectItem)
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_linkedIn()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, connectItem)
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_twitterHashTag()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, connectItem)
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_have_connect_items_if_twitter_name()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, connectItem)
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeTrue();
		}

		[Test]
		public void Should_not_have_connect_items_if_null_conference()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
			};
			vm.HasConnectItems.ShouldBeFalse();
		}

		[Test]
		public void Should_not_have_connect_items_if_no_items()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			var connectItem = _fixture.Create<string>();
			var conference = _fixture.Build<ConferenceDetailViewDto>()
				.With(x => x.facebookUrl, "")
				.With(x => x.homepageUrl, "")
				.With(x => x.lanyrdUrl, "")
				.With(x => x.meetupUrl, "")
				.With(x => x.googlePlusUrl, "")
				.With(x => x.vimeoUrl, "")
				.With(x => x.youtubeUrl, "")
				.With(x => x.githubUrl, "")
				.With(x => x.linkedInUrl, "")
				.With(x => x.twitterHashTag, "")
				.With(x => x.twitterName, "")
				.Create();


			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object)
			{
				Conference = conference
			};
			vm.HasConnectItems.ShouldBeFalse();
		}

		[Test]
		public void Should_navigate_to_settings()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();
			
			MockDispatcher.Requests.Clear();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			MockDispatcher.Requests.Count.ShouldEqual(0);

			vm.ShowSettingsCommand.Execute(null);
			MockDispatcher.Requests.Count.ShouldEqual(1);
			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(SettingsViewModel));
		}

		[Test]
		public void Should_navigate_to_conference_sessions()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			MockDispatcher.Requests.Clear();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			MockDispatcher.Requests.Count.ShouldEqual(0);

			vm.ShowSessionsCommand.Execute(null);
			MockDispatcher.Requests.Count.ShouldEqual(1);
			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(ConferenceSessionsViewModel));
		}

		[Test]
		public void Should_navigate_to_session_detail()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			MockDispatcher.Requests.Clear();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			MockDispatcher.Requests.Count.ShouldEqual(0);

			vm.ShowSessionDetailCommand.Execute(null);
			MockDispatcher.Requests.Count.ShouldEqual(1);
			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(SessionDetailViewModel));
		}

		[Test]
		public void Add_to_favorites_should_add_to_local_cache()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();
			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			vm.Conference = conference;
			vm.AddFavoriteCommand.Execute(null);
			localScheduleRepository.Verify(x => x.SaveSchedule(It.IsAny<ScheduleDto>()), Times.Once());
		}

		[Test]
		public void Add_to_favorites_should_add_to_remote_data()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();
			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			vm.Conference = conference;
			vm.AddFavoriteCommand.Execute(null);
			remoteDataService.Verify(x => x.AddToSchedule(
																		It.IsAny<string>(),
																		It.IsAny<string>(),
																		It.IsAny<Action<ScheduleDto>>(),
																		It.IsAny<Action<Exception>>()), Times.Once());
		}

		[Test]
		public void Add_to_favorites_should_publish_message()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();
			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				localConferencesRepository.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			vm.Conference = conference;
			vm.AddFavoriteCommand.Execute(null);


			messenger.Verify(x => x.Publish(It.IsAny<FavoriteConferencesUpdatedMessage>()), Times.Once());
		}
	}
}