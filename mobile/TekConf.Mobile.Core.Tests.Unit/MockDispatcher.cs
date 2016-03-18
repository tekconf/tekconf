using System;
using MvvmCross.Platform.Core;
using MvvmCross.Core.Views;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;

public class MockDispatcher
	: MvxMainThreadDispatcher
	  , IMvxViewDispatcher
{
	public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest> ();
	public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint> ();

	public bool RequestMainThreadAction (Action action)
	{
		action ();
		return true;
	}

	public bool ShowViewModel (MvxViewModelRequest request)
	{
		Requests.Add (request);
		return true;
	}

	public bool ChangePresentation (MvxPresentationHint hint)
	{
		Hints.Add (hint);
		return true;
	}
}
