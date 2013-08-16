using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net.Http;
using HttpClientPortable.Core;
using System.Threading.Tasks;

namespace HttpClientPortable.Touch
{
	public partial class HttpClientPortable_TouchViewController : UIViewController
	{
		public HttpClientPortable_TouchViewController () : base ("HttpClientPortable_TouchViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Console.WriteLine ("Doing stuff");

			// Perform any additional setup after loading the view, typically from a nib.
			var task = MyClass.DoStuff ();

			Console.WriteLine ("Started doing stuff");

			task.ContinueWith ((Task<string> t) => {
				Console.WriteLine(t.Result);
			});

		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

