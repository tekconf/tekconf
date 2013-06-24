using System;
using System.Collections.Generic;
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
	using Cirrious.CrossCore.Core;
	using Cirrious.MvvmCross.Plugins.File;
	using Cirrious.MvvmCross.Test.Mocks.Dispatchers;

	using Ploeh.AutoFixture;

	[TestFixture]
	public class ConferencesListViewModelTests : TestBase
	{
		private IFixture _fixture;
		[TestFixtureSetUp]
		public void Initialize()
		{
			_fixture = new Fixture();
			base.Setup();
		}

		[Test]
		public void ConferencesList_should_not_be_null()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);

			vm.ShouldNotBeNull();
		}

		//CONFERENCES
		[Test]
		public void ConferencesList_should_get_conferences_from_remote_data()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Init("");

			remoteDataService.Verify(x => x.GetConferences(
						It.IsAny<bool>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<bool?>(),
						It.IsAny<bool?>(),
						It.IsAny<bool?>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<double?>(),
						It.IsAny<double?>(),
						It.IsAny<double?>(),
						It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
						It.IsAny<Action<Exception>>()
				), Times.Once());
			vm.ShouldNotBeNull();
		}

		[Test]
		public void ConferencesList_should_check_local_conferences_cache_before_calling_remote()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Init("");

			localConferencesRepository.Verify(x => x.GetConferencesListView(), Times.Once());
		}

		[Test]
		public void ConferencesList_should_display_conferences_if_local_cache()
		{
			var dispatcher = new InlineMockMainThreadDispatcher();
			Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
			var fileStore = new Mock<IMvxFileStore>();
			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Conferences.ShouldBeNull();
			vm.Init("");
			vm.Conferences.ShouldNotBeNull();
			vm.Conferences.ShouldEqual(conferences);
		}

		[Test]
		public void ConferencesList_should_not_call_remote_if_local_conferences_cache()
		{
			var dispatcher = new InlineMockMainThreadDispatcher();
			Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
			var fileStore = new Mock<IMvxFileStore>();
			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Conferences.ShouldBeNull();
			vm.Init("");
			remoteDataService.Verify(x => x.GetConferences(
						It.IsAny<bool>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<bool?>(),
						It.IsAny<bool?>(),
						It.IsAny<bool?>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<string>(),
						It.IsAny<double?>(),
						It.IsAny<double?>(),
						It.IsAny<double?>(),
						It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
						It.IsAny<Action<Exception>>()
				), Times.Never());
		}

		//SCHEDULE

		[Test]
		public void ConferencesList_should_get_schedule_from_remote_data()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Init("");

			remoteDataService.Verify(x => x.GetSchedules(
						It.IsAny<string>(),
						It.IsAny<bool>(),
						It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
						It.IsAny<Action<Exception>>()
				), Times.Once());
			vm.ShouldNotBeNull();
		}

		[Test]
		public void ConferencesList_should_check_local_schedule_cache_before_calling_remote()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Init("");

			localScheduleRepository.Verify(x => x.GetConferencesList(), Times.Once());
		}

		[Test]
		public void ConferencesList_should_display_schedule_if_local_cache()
		{
			var dispatcher = new InlineMockMainThreadDispatcher();
			Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
			var fileStore = new Mock<IMvxFileStore>();
			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

			var favorites = _fixture.CreateMany<ConferencesListViewDto>();
			localScheduleRepository.Setup(x => x.GetConferencesList()).Returns(favorites);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Favorites.ShouldBeNull();
			vm.Init("");
			vm.Favorites.ShouldNotBeNull();
			vm.Favorites.ShouldEqual(favorites);
		}

		[Test]
		public void ConferencesList_should_not_call_remote_if_local_schedule_cache()
		{
			var dispatcher = new InlineMockMainThreadDispatcher();
			Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
			var fileStore = new Mock<IMvxFileStore>();
			Ioc.RegisterSingleton(typeof(IMvxFileStore), fileStore.Object);
			_fixture.Register<IMvxFileStore>(() => fileStore.Object);
			var remoteDataService = new Mock<IRemoteDataService>();
			var localConferencesRepository = new Mock<ILocalConferencesRepository>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();

			var conferences = _fixture.CreateMany<ConferencesListViewDto>();
			localConferencesRepository.Setup(x => x.GetConferencesListView()).Returns(conferences);
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, localConferencesRepository.Object, localScheduleRepository.Object, analytics.Object, authentication.Object, messenger.Object);
			vm.Conferences.ShouldBeNull();
			vm.Init("");
			remoteDataService.Verify(x => x.GetSchedules(
						It.IsAny<string>(),
						It.IsAny<bool>(),
						It.IsAny<Action<IEnumerable<ConferencesListViewDto>>>(),
						It.IsAny<Action<Exception>>()
				), Times.Never());
		}

	}
}