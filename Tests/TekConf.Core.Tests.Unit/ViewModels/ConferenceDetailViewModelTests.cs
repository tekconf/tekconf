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

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
				analytics.Object,
				authentication.Object,
				messenger.Object,
				localScheduleRepository.Object);

			vm.ShouldNotBeNull();
		}

		[Test]
		public void Should_get_conference_from_cache()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_get_conference_from_remote()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_get_conference_from_remote_when_refreshing()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_not_get_conference_from_cache_if_loading()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_not_get_conference_from_remote_if_loading()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_update_conference()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_navigate_to_settings()
		{
			Assert.Fail();
		}

		[Test]
		public void Add_to_favorites_should_add_to_local_cache()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var localScheduleRepository = new Mock<ILocalScheduleRepository>();
			
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();
			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x=> x.isAddedToSchedule, false).Create();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
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

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();
			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
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

			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			authentication.Setup(x => x.IsAuthenticated).Returns(true);
			var messenger = new Mock<IMvxMessenger>();
			var conference = _fixture.Build<ConferenceDetailViewDto>().With(x => x.isAddedToSchedule, false).Create();
			var vm = new ConferenceDetailViewModel(remoteDataService.Object,
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