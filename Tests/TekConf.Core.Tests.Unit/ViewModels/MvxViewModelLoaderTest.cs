// MvxViewModelLoaderTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.Test.Mocks.TestViewModels;
using Cirrious.MvvmCross.ViewModels;
using Moq;
using NUnit.Framework;
using Should;
using TekConf.Core.Interfaces;
using TekConf.Core.Repositories;
using TekConf.Core.Services;
using TekConf.Core.ViewModels;

namespace Cirrious.MvvmCross.Test.ViewModels
{
	[TestFixture]
	public class MvxViewModelLoaderTest : TestBase
	{
		[TestFixtureSetUp]
		public void Initialize()
		{
			base.Setup();
		}

		[Test]
		public void ConferencesList_should_not_be_null()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, analytics.Object, authentication.Object, messenger.Object);

			vm.ShouldNotBeNull();
		}

		[Test]
		public void ConferencesList_should_get_conferences_from_remote_data()
		{
			var remoteDataService = new Mock<IRemoteDataService>();
	
			var analytics = new Mock<IAnalytics>();
			var authentication = new Mock<IAuthentication>();
			var messenger = new Mock<IMvxMessenger>();

			var vm = new ConferencesListViewModel(remoteDataService.Object, analytics.Object, authentication.Object, messenger.Object);
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
		public void ConferencesList_should_check_local_cache_before_calling_remote()
		{
			Assert.Fail();
		}

		//[Test]
		//public void Test_NormalViewModel()
		//{
		//	ClearAll();

		//	IMvxViewModel outViewModel = new Test2ViewModel();

		//	var mockLocator = new Mock<IMvxViewModelLocator>();
		//	mockLocator.Setup(
		//			m => m.TryLoad(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>(), out outViewModel))
		//						 .Returns(() => true);

		//	var mockCollection = new Moq.Mock<IMvxViewModelLocatorCollection>();
		//	mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
		//								.Returns(() => mockLocator.Object);

		//	Ioc.RegisterSingleton(mockCollection.Object);

		//	var parameters = new Dictionary<string, string> { { "foo", "bar" } };
		//	var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null,
		//																												MvxRequestedBy.UserAction);
		//	var state = new MvxBundle();
		//	var loader = new MvxViewModelLoader();
		//	var viewModel = loader.LoadViewModel(request, state);

		//	Assert.AreSame(outViewModel, viewModel);
		//	Assert.AreEqual(MvxRequestedBy.UserAction, viewModel.RequestedBy);
		//}

		//[Test]
		//public void Test_LoaderForNull()
		//{
		//	ClearAll();

		//	var request = new MvxViewModelRequest<MvxNullViewModel>(null, null, MvxRequestedBy.UserAction);
		//	var state = new MvxBundle();
		//	var loader = new MvxViewModelLoader();
		//	var viewModel = loader.LoadViewModel(request, state);

		//	Assert.IsInstanceOf<MvxNullViewModel>(viewModel);
		//}

		//[Test]
		//[ExpectedException(typeof(MvxException))]
		//public void Test_FailedViewModel()
		//{
		//	ClearAll();

		//	IMvxViewModel outViewModel = null;

		//	var mockLocator = new Mock<IMvxViewModelLocator>();
		//	mockLocator.Setup(
		//			m => m.TryLoad(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>(), out outViewModel))
		//						 .Returns(() => false);

		//	var mockCollection = new Moq.Mock<IMvxViewModelLocatorCollection>();
		//	mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
		//								.Returns(() => mockLocator.Object);

		//	Ioc.RegisterSingleton(mockCollection.Object);

		//	var parameters = new Dictionary<string, string> { { "foo", "bar" } };
		//	var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null,
		//																												MvxRequestedBy.UserAction);
		//	var state = new MvxBundle();
		//	var loader = new MvxViewModelLoader();
		//	var viewModel = loader.LoadViewModel(request, state);

		//	Assert.Fail("We should never reach this line");
		//}

		//[Test]
		//[ExpectedException(typeof(MvxException))]
		//public void Test_FailedViewModelLocatorCollection()
		//{
		//	ClearAll();

		//	var mockCollection = new Moq.Mock<IMvxViewModelLocatorCollection>();
		//	mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
		//								.Returns(() => null);

		//	Ioc.RegisterSingleton(mockCollection.Object);

		//	var parameters = new Dictionary<string, string> { { "foo", "bar" } };
		//	var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null,
		//																												MvxRequestedBy.UserAction);
		//	var state = new MvxBundle();
		//	var loader = new MvxViewModelLoader();
		//	var viewModel = loader.LoadViewModel(request, state);

		//	Assert.Fail("We should never reach this line");
		//}
	}
}