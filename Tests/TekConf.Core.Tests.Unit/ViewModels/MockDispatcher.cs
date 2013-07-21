namespace TekConf.Core.Tests.Unit.ViewModels
{
	using System;
	using System.Collections.Generic;

	using Cirrious.CrossCore.Core;
	using Cirrious.MvvmCross.ViewModels;
	using Cirrious.MvvmCross.Views;

	public class MockDispatcher : MvxMainThreadDispatcher, IMvxViewDispatcher
	{
		public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();

		public bool ChangePresentation(MvxPresentationHint hint)
		{
			throw new NotImplementedException();
		}

		public bool ShowViewModel(MvxViewModelRequest request)
		{
			this.Requests.Add(request);
			return true;
		}

		public bool RequestMainThreadAction(Action action)
		{
			action();
			return true;
		}
	}
}