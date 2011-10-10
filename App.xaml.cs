using Expression.Blend.SampleData.SampleDataSource;
using System;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Collections.Generic;

namespace WikipediaQueryToolXaml
{
    partial class App
    {
        // TODO: Create a data model appropriate for your problem domain to replace the sample data
        private static SampleDataSource _sampleData;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            //ShowSplit(null);
            ShowCollection();
            Window.Current.Activate();
        }

        public static void ShowCollection()
        {
            var page = new CollectionPage();
            //if (_sampleData == null) _sampleData = new SampleDataSource(page.BaseUri);
            //page.Items = _sampleData.GroupedCollections.Select((obj) => (Object)obj);
            Window.Current.Content = page;
        }

        public static void ShowSplit(int selectedIndex, List<Netflix.Catalog.v2.Title> movies)
        {
            var page = new SplitPage();
            //if (_sampleData == null) _sampleData = new SampleDataSource(page.BaseUri);
            //if (collection == null) collection = _sampleData.GroupedCollections.First();
            page.Items = movies;
            page.Context = movies[selectedIndex];
            page.SelectedIndex = selectedIndex;
            Window.Current.Content = page;
        }
    }
}
