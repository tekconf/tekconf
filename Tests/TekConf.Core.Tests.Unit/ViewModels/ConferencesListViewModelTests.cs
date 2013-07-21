//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Threading;
//using Cirrious.CrossCore.Core;
//using Cirrious.MvvmCross.Plugins.File;
//using Cirrious.MvvmCross.Plugins.Messenger;
//using Cirrious.MvvmCross.Test.Mocks.Dispatchers;
//using Moq;
//using NUnit.Framework;
//using Ploeh.AutoFixture;
//using Should;
//using TekConf.Core.Interfaces;
//using TekConf.Core.Repositories;
//using TekConf.Core.Services;
//using TekConf.Core.ViewModels;

//namespace TekConf.Core.Tests.Unit.ViewModels
//{
//	using System.Linq;

//	using Cirrious.MvvmCross.Views;

//	[TestFixture]
//	public class ConferencesListViewModelTests : TestBase
//	{
//		private IFixture _fixture;
//		[TestFixtureSetUp]
//		public void Initialize()
//		{
//			_fixture = new Fixture();
//			base.Setup();
//		}

//		[Test]
//		public void Should_not_be_null()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);

//			vm.ShouldNotBeNull();
//		}

//		//CONFERENCES
//		[Test]
//		public void Should_get_remote_conferences()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Init("");

//			remoteDataService.Verify(x => x.GetConferences(
//				It.IsAny<bool>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Once());

//		}

//		[Test]
//		public void Should_check_local_conferences_cache_before_calling_remote()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Init("");

//			localConferencesRepository.Verify(x => x.GetConferencesListView(), Times.Once());
//		}

//		[Test]
//		public void Should_display_conferences_if_local_cache()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Conferences.ShouldBeNull();
//			vm.Init("");
//			vm.Conferences.ShouldNotBeNull();
//			vm.Conferences.ShouldEqual(conferences);
//		}

//		[Test]
//		public void Should_raise_property_changed_when_conferences_loaded()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Conferences.ShouldBeNull();
//			int propertyChangedCount = 0;
//			vm.PropertyChanged += (sender, args) =>
//			{
//				if (args.PropertyName == "Conferences")
//				{
//					propertyChangedCount++;
//				}
//			};
//			vm.Init("");
//			Thread.Sleep(20);
//			vm.Conferences.ShouldNotBeNull();
//			vm.Conferences.ShouldEqual(conferences);
//			propertyChangedCount.ShouldEqual(1);
//		}


//		[Test]
//		public void Should_not_call_remote_if_local_conferences_cache()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Conferences.ShouldBeNull();
//			vm.Init("");
//			remoteDataService.Verify(x => x.GetConferences(
//				It.IsAny<bool>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Never());
//		}

//		//SCHEDULE

//		[Test]
//		public void Should_get_remote_favorites()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			authentication.Setup(x => x.IsAuthenticated).Returns(true);
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Init("");

//			remoteDataService.Verify(x => x.GetSchedules(
//				It.IsAny<string>(),
//				It.IsAny<bool>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Once());
//		}

//		[Test]
//		public void Should_check_local_schedule_cache_before_calling_remote()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			authentication.Setup(x => x.IsAuthenticated).Returns(true);
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Init("");

//			localScheduleRepository.Verify(x => x.GetConferencesList(), Times.Once());
//		}

//		[Test]
//		public void Should_display_schedule_if_local_cache()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var favorites = _fixture.CreateMany<ConferencesListViewDto>();
//			localScheduleRepository.Setup(x => x.GetConferencesList()).Returns(favorites);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			authentication.Setup(x => x.IsAuthenticated).Returns(true);
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Favorites.ShouldBeNull();
//			vm.Init("");
//			vm.Favorites.ShouldNotBeNull();
//			vm.Favorites.ShouldEqual(favorites);
//		}

//		[Test]
//		public void Should_not_call_remote_if_local_schedule_cache()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Conferences.ShouldBeNull();
//			vm.Init("");
//			remoteDataService.Verify(x => x.GetSchedules(
//				It.IsAny<string>(),
//				It.IsAny<bool>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Never());
//		}

//		[Test]
//		public void Should_not_get_conferences_when_loading()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);

//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.IsLoadingConferences = true;
//			vm.Init("");

//			remoteDataService.Verify(x => x.GetConferences(
//				It.IsAny<bool>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Never());

//			localConferencesRepository.Verify(x => x.GetConferencesListView(), Times.Never());
//		}

//		[Test]
//		public void Should_not_get_favorites_when_loading()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);

//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.IsLoadingFavorites = true;
//			vm.Init("");

//			remoteDataService.Verify(x => x.GetSchedules(
//				It.IsAny<string>(),
//				It.IsAny<bool>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Never());

//			localScheduleRepository.Verify(x => x.GetConferencesList(), Times.Never());
//		}

//		[Test]
//		public void Should_get_remote_conferences_when_refreshing_even_when_cache()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Init("");
//			vm.Refresh();
//			remoteDataService.Verify(x => x.GetConferences(
//				It.IsAny<bool>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<bool?>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<string>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<double?>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Once());
//		}

//		[Test]
//		public void Should_get_remote_favorites_when_refreshing_even_when_cache()
//		{
//			var fileStore = new Mock<IMvxFileStore>();
//			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
//			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
//			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
//			localScheduleRepository.Setup(x => x.GetConferencesList()).Returns(conferences);

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			authentication.Setup(x => x.IsAuthenticated).Returns(true);
		
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			vm.Init("");
//			vm.IsLoadingFavorites = false;
//			vm.Refresh();
//			remoteDataService.Verify(x => x.GetSchedules(
//				It.IsAny<string>(),
//				It.IsAny<bool>(),
//				It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
//				It.IsAny<Action<Exception>>()
//				), Times.Once());
//		}

//		[Test]
//		public void Should_navigate_to_settings_view_model()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			MockDispatcher.Requests.Count.ShouldEqual(0);
			
//			vm.ShowSettingsCommand.Execute(null);
//			MockDispatcher.Requests.Count.ShouldEqual(1);
//			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(SettingsViewModel));
//		}

//		[Test]
//		public void Should_navigate_to_conferences_search_view_model()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			MockDispatcher.Requests.Count.ShouldEqual(0);

//			vm.ShowSearchCommand.Execute(null);
//			MockDispatcher.Requests.Count.ShouldEqual(1);
//			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(ConferencesSearchViewModel));
//		}

//		[Test]
//		public void Should_navigate_to_conference_detail_view_model()
//		{
//			var remoteDataService = new Mock<IRemoteDataService>();
//			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
//			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

//			var analytics = new Mock<IAnalytics>();
//			var authentication = new Mock<IAuthentication>();
//			var messenger = new Mock<IMvxMessenger>();

//			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
//			MockDispatcher.Requests.Count.ShouldEqual(0);

//			vm.ShowDetailCommand.Execute(null);
//			MockDispatcher.Requests.Count.ShouldEqual(1);
//			MockDispatcher.Requests.First().ViewModelType.ShouldEqual(typeof(ConferenceDetailViewModel));
//		}
//	}
//}