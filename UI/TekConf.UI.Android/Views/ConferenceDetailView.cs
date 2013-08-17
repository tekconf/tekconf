using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using TekConf.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace TekConf.UI.Android.Views
{
	using Cirrious.MvvmCross.Droid.Views;

	using global::Android.App;
	using global::Android.OS;

	[Activity(Label = "Detail")]
	public class ConferenceDetailView : MvxActivity
	{
		private MvxImageView _image;
		private MvxAndroidBindingContext _bindingContext;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			//SetContentView(Resource.Layout.ConferenceDetailView);

//			var mainLayoutParams = new RelativeLayout.LayoutParams (
//				ViewGroup.LayoutParams.FillParent,
//				ViewGroup.LayoutParams.FillParent);
//
//			var mainLayout = new LinearLayout (this);
//			mainLayout.LayoutParameters = mainLayoutParams;
//			mainLayout.Orientation = Orientation.Vertical;
//			mainLayout.SetPadding (5, 5, 5, 5);
//
//			mainLayout.SetBackgroundColor (Color.White);
//
//			var imageLayoutParams = new RelativeLayout.LayoutParams (
//				ViewGroup.LayoutParams.FillParent,
//				ViewGroup.LayoutParams.FillParent);
//			var imageLinearLayout = new LinearLayout (this);
//			imageLinearLayout.Orientation = Orientation.Horizontal;
//			imageLinearLayout.SetGravity (GravityFlags.CenterHorizontal);
//			imageLinearLayout.LayoutParameters = imageLayoutParams;
//
//			_image = new MvxImageView (this);
//			imageLinearLayout.AddView (_image);
//
//
//			mainLayout.AddView (imageLinearLayout);
//
//			SetContentView (mainLayout);
		}

		protected override void OnViewModelSet()
		{
			var mainLayoutParams = new RelativeLayout.LayoutParams (
				ViewGroup.LayoutParams.FillParent,
				ViewGroup.LayoutParams.FillParent);

			var scrollView = new ScrollView (this);
			scrollView.LayoutParameters = mainLayoutParams;
			scrollView.SetBackgroundColor (Color.White);

			var mainLayout = new LinearLayout (this);
			mainLayout.LayoutParameters = mainLayoutParams;
			mainLayout.Orientation = Orientation.Vertical;
			mainLayout.SetPadding (5, 5, 5, 5);
			mainLayout.SetBackgroundColor (Color.White);

			var imageLayoutParams = new RelativeLayout.LayoutParams (
				ViewGroup.LayoutParams.FillParent,
				ViewGroup.LayoutParams.FillParent);
			var imageLinearLayout = new LinearLayout (this);
			imageLinearLayout.Orientation = Orientation.Horizontal;
			imageLinearLayout.SetGravity (GravityFlags.CenterHorizontal);
			imageLinearLayout.LayoutParameters = imageLayoutParams;

			_image = new MvxImageView (this);
			imageLinearLayout.AddView (_image);

			var name = new MyTextView (this);
			name.TextSize = 20f;
			name.SetPadding (3, 3, 3, 3);
			name.SetTextColor (Color.Black);
			mainLayout.AddView (name);

			var dateRange = new MyTextView (this);
			dateRange.TextSize = 18f;
			dateRange.SetPadding (3, 3, 3, 3);
			dateRange.SetTextColor (Color.Black);
			mainLayout.AddView (dateRange);

			var formattedCity = new MyTextView (this);
			formattedCity.TextSize = 18f;
			formattedCity.SetPadding (3, 3, 3, 3);
			formattedCity.SetTextColor (Color.Black);
			mainLayout.AddView (formattedCity);

			var description = new MyTextView (this);
			description.TextSize = 14f;
			description.SetPadding (3, 3, 3, 3);
			description.SetTextColor (Color.Black);
			mainLayout.AddView (description);



			mainLayout.AddView (imageLinearLayout);

			scrollView.AddView (mainLayout);

			SetContentView (scrollView);


			var set = this.CreateBindingSet<ConferenceDetailView, ConferenceDetailViewModel>();
			//set.Bind(_image).To(vm => vm.Conference.imageUrl);
			set.Bind (name).To (vm => vm.Conference.name);
			set.Bind (dateRange).To (vm => vm.Conference.DateRange);
			set.Bind (formattedCity).To (vm => vm.Conference.FormattedCity);
			set.Bind (description).To (vm => vm.Conference.description);
			set.Apply();


		}
	}

	public class MyTextView : TextView
	{
		Context _context;
		public MyTextView (Context context) : base(context)
		{
			_context = context;
			Init ();
		}

		private void Init()
		{
			Typeface tf = Typeface.CreateFromAsset(_context.Assets, "font/OpenSans-Light.ttf");
			this.Typeface = tf;
		}
	}
}