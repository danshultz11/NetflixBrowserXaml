using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Web.Script.Serialization;
using WikipediaQueryToolXaml.Data;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using System.Linq;
using Windows.UI.Xaml.Input;

namespace WikipediaQueryToolXaml
{
    public sealed partial class CollectionPage
    {
        string netflixApiAddress = "http://odata.netflix.com/Catalog/Titles?$filter=substringof('{0}', Name)&$format=json&$top=50&$orderby=Rating";
        //string netflixApiSuffix = "', Name)&$format=json&$top=50&$orderby=Rating";
        
        public CollectionPage()
        {
            InitializeComponent();
            BackButton.IsEnabled = false;
        }

        void ItemView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Construct the appropriate destination page and set its context appropriately
            //var selectedItem = (sender as Selector).SelectedIndex as IGroupInfo;
            App.ShowSplit((sender as Selector).SelectedIndex, Movies);
        }

        private IEnumerable<Object> _items;
        public IEnumerable<Object> Items
        {
            get
            {
                return this._items;
            }

            set
            {
                this._items = value;
                CollectionViewSource.Source = value;
            }
        }

        List<Netflix.Catalog.v2.Title> Movies { get; set; }

        // View state management for switching among Full, Fill, Snapped, and Portrait states

        private DisplayPropertiesEventHandler _displayHandler;
        private TypedEventHandler<ApplicationLayout, ApplicationLayoutChangedEventArgs> _layoutHandler;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_displayHandler == null)
            {
                _displayHandler = Page_OrientationChanged;
                _layoutHandler = Page_LayoutChanged;
            }
            DisplayProperties.OrientationChanged += _displayHandler;
            ApplicationLayout.GetForCurrentView().LayoutChanged += _layoutHandler;
            SetCurrentViewState(this);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            DisplayProperties.OrientationChanged -= _displayHandler;
            ApplicationLayout.GetForCurrentView().LayoutChanged -= _layoutHandler;
        }

        private void Page_LayoutChanged(object sender, ApplicationLayoutChangedEventArgs e)
        {
            SetCurrentViewState(this);
        }

        private void Page_OrientationChanged(object sender)
        {
            SetCurrentViewState(this);
        }

        private void SetCurrentViewState(Control viewStateAwareControl)
        {
            VisualStateManager.GoToState(viewStateAwareControl, this.GetViewState(), false);
        }

        private String GetViewState()
        {
            var orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Portrait ||
                orientation == DisplayOrientations.PortraitFlipped) return "Portrait";
            var layout = ApplicationLayout.Value;
            if (layout == ApplicationLayoutState.Filled) return "Fill";
            if (layout == ApplicationLayoutState.Snapped) return "Snapped";
            return "Full";
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FindMovies();
        }

        private void txtSearch_KeyDown(object sender, Windows.UI.Xaml.Input.KeyEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                FindMovies();
        }

        private void FindMovies()
        {
            var n = new Netflix.Catalog.v2.NetflixCatalog(new Uri("http://odata.netflix.com/Catalog/"));
            var titles = n.Titles.Where(x => x.Name.Contains(txtSearch.Text));
            Movies = titles.ToList();
            Items = titles;
        }

        //private async void GetData()
        //{
        //    var search = string.Format(netflixApiAddress, System.Web.HttpUtility.UrlEncode(txtSearch.Text));
        //    var client = new HttpClient { MaxResponseContentBufferSize = 10000000 };
        //    var data = client.GetAsync(search);
        //    var response = await data;
        //    var d = new JavaScriptSerializer().Deserialize <ICollection<Movie>>(response.Content.ReadAsString());
        //    if (d != null)
        //    {

        //    }
        //}
    }
}
