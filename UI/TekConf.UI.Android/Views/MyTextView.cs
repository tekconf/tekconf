using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using TekConf.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using System.Collections.Generic;
using System;
using Android.Util;

namespace TekConf.UI.Android.Views
{
	using Cirrious.MvvmCross.Droid.Views;

	using global::Android.App;
	using global::Android.OS;

	public class MyTextView : TextView
	{
		Context _context;
		public MyTextView (Context context, IAttributeSet attrs) : base(context, attrs)
		{
			_context = context;
			Init ();
		}
		public MyTextView (Context context) : base(context)
		{
			_context = context;
			Init ();
		}

		private void Init()
		{
			Typeface tf = Typeface.CreateFromAsset(_context.Assets, "font/OpenSans-Light.ttf");
			this.Typeface = tf;
			this.SetTextColor (Color.Black);
		}
	}
}