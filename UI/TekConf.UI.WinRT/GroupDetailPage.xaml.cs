using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace TekConf.UI.WinRT
{
	/// <summary>
	/// A page that displays an overview of a single group, including a preview of the items
	/// within the group.
	/// </summary>
	public sealed partial class GroupDetailPage : TekConf.UI.WinRT.Common.LayoutAwarePage
	{
		public GroupDetailPage()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="navigationParameter">The parameter value passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
		/// </param>
		/// <param name="pageState">A dictionary of state preserved by this page during an earlier
		/// session.  This will be null the first time a page is visited.</param>
		protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
		{
			// TODO: Create an appropriate data model for your problem domain to replace the sample data
			//var group = SampleDataSource.GetGroup((String)navigationParameter);

			var slug = (String)navigationParameter;

			var url = "http://localhost:25825/";
			var repository = new RemoteDataRepository(url);
			var conference = await repository.GetConference(slug);

			this.DefaultViewModel["Group"] = conference;
			this.DefaultViewModel["Items"] = conference.sessions;
		}

		/// <summary>
		/// Invoked when an item is clicked.
		/// </summary>
		/// <param name="sender">The GridView (or ListView when the application is snapped)
		/// displaying the item clicked.</param>
		/// <param name="e">Event data that describes the item clicked.</param>
		void ItemView_ItemClick(object sender, ItemClickEventArgs e)
		{
			// Navigate to the appropriate destination page, configuring the new page
			// by passing required information as a navigation parameter
			var fullSessionDto = ((FullSessionDto)e.ClickedItem);
			this.Frame.Navigate(typeof(SessionDetailPage), fullSessionDto);
		}


	}
}
