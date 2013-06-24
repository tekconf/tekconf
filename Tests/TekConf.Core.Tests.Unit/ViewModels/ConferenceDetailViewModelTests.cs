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
	using Ploeh.AutoFixture;

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
		public void Should_not_get_conference_if_loading()
		{
			Assert.Fail();
		}

		[Test]
		public void Should_update_conference()
		{
			Assert.Fail();
		}
	}
}