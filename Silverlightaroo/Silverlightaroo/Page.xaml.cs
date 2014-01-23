using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Documents;

namespace Silverlightaroo
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The search results
        /// </summary>
        protected List<SearchResult> searchResultsList;

        /// <summary>
        /// Start async request for JSON
        /// http://msdn.microsoft.com/en-us/library/cc197953(VS.95).aspx
        /// 
        /// Loading animation from 
        /// http://blog.buttonchrome.co.uk/post/Silverlight-Loadinge-Spin-Icon-in-XAML.aspx
        /// </summary>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string host = Application.Current.Host.Source.Host;
            try
            {
                searchResultsList = new List<SearchResult>(); resultList.ItemsSource = searchResultsList; // blank out previous
                if (Application.Current.Host.Source.Port != 80)
                {
                    host = host + ":" + Application.Current.Host.Source.Port;
                }
                Uri serviceUri = new Uri("http://" + host + "/SearchJson.aspx?searchfor=" + query.Text,UriKind.Absolute);
                WebClient downloader = new WebClient();
                downloader.OpenReadCompleted += new OpenReadCompletedEventHandler(downloader_OpenReadCompleted);
                // loading animation thanks to http://blog.buttonchrome.co.uk/post/Silverlight-Loadinge-Spin-Icon-in-XAML.aspx
                downloader.OpenReadCompleted += (s, e1) => Spinner.Visibility = Visibility.Collapsed;
                Spinner.Visibility = Visibility.Visible;
                downloader.OpenReadAsync(serviceUri);
            }
            catch (Exception ex)
            {
                searchResultsList.Add(new SearchResult { name = "error on " + host, description = ex.Message, size = "-1", url = "#" });
                resultList.ItemsSource = searchResultsList;
            }
        }
        /// <summary>
        /// Filter the resultlist by 'images' or 'NOT images'
        /// </summary>
        List<SearchResult> Filter(List<SearchResult> list)
        {
            if (searchAllRadio.IsChecked.GetValueOrDefault())
            {
                return list;
            }
            else if (searchImageRadio.IsChecked.GetValueOrDefault())
            {
                var i = (from result in list
                         where result.url.EndsWith("jpg")
                         select result).ToList();
                return i;
            }
            else if (searchWebRadio.IsChecked.GetValueOrDefault())
            {
                var i = (from result in list
                         where !result.url.EndsWith("jpg")
                         select result).ToList();
                return i;
            }
            return list;
        }
        /// <summary>
        /// Receive JSON stream, parse into objects and bind to ListBox
        /// http://msdn.microsoft.com/en-us/library/cc197957(VS.95).aspx
        /// </summary>
        void downloader_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                using (Stream responseStream = e.Result)
                {
                    try
                    {
                        JsonArray resultStream = (JsonArray)JsonArray.Load(responseStream);
                        var results = from result in resultStream
                                      select result;
                        List<SearchResult> list = new List<SearchResult>();
                        foreach (JsonObject r in results)
                        {
                            var result = new SearchResult
                            {
                                name = r["name"]
                                , description = r["description"]
                                , url = r["url"]
                                , size = r["size"]
                                , date = r["date"]
                                , tags = r["tags"]
                                , extension = r["extension"]
                            };
                            list.Add(result);
                        }
                        searchResultsList = list;
                        resultList.ItemsSource = Filter(searchResultsList);
                    }
                    catch (Exception ex)
                    {
                        searchResultsList = new List<SearchResult>();
                        searchResultsList.Add(new SearchResult { name = "error", description = ex.Message, size = "", url = "#" });
                        resultList.ItemsSource = searchResultsList;
                    }
                    //HACK: couldn't get this working... not sure why
                    //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchResponse));
                    //SearchResponse sr = (SearchResponse)serializer.ReadObject(responseStream);
                    //resultList.DataContext = sr;
                }
            }
        }

        /// <summary>
        /// Filter without re-querying
        /// </summary>
        private void searchRadio_Click(object sender, RoutedEventArgs e)
        {
            if (searchResultsList != null && searchResultsList.Count > 0)
            {
             //   var source = e.OriginalSource;
                resultList.ItemsSource = Filter(searchResultsList);
            }
        }
    }
    /// <summary>
    /// SearchResult class to display results
    /// </summary>
    public class SearchResult
    {
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string tags { get; set; }
        public string size { get; set; }
        public string date { get; set; }
        public string rank { get; set; }
        public string gps { get; set; }
        public string extension { get; set; }
        /// <summary>For display only</summary>
        public string summary { get { return String.Format("{0} - {1} - {2}", url, size, date); } }
        
        /// <summary>For display only - doesn't work though</summary>
        public TextBlock descriptionBlock { get {
            string xamlString = @"<TextBlock xmlns='http://schemas.microsoft.com/client/2007'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>"; //Foo<Run FontWeight=\""Bold\"">Bar</Run></TextBlock>";

            string d = description.Replace("<b>", @"<Run FontWeight=""Bold"">");
            d = d.Replace("</b>", @"</Run>");

            d = xamlString + d + "</TextBlock>";

            TextBlock t = XamlReader.Load(d) as TextBlock;
            return t;
        } }
    }
}
