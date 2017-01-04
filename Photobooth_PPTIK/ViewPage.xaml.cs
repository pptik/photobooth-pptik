using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using RestSharp;
using Photobooth_PPTIK.Pages;

namespace Photobooth_PPTIK
{
    public partial class ViewPage : Page
    {

        string refreshAUTH = "https://graph.facebook.com/v2.2/130393034064943/accounts?access_token=EAAEabZBMG9bMBAIZAhk5oz2h15KN8XOdNutOZBY4HtOdvA0vlHMIKWv53UGY2CqWxpwVYCm2ojIbCe4PtyubQhUOoVkLvCBavZCoeGxLcoL0041J3rFKOs6PDZB0ZCPgt68BHKRqqtefbkzGqG5DWENNGoH97hFKZBPwYX4VDJldwZDZD&";
        public string serverAPI = "http://127.0.0.1";
        public string reqStatus = "photoboothapi/userbooth/status";
        public string APIpostFoto = "photoboothapi/userbooth/postfoto";

        string fullpath = "";
        private string path;
        
        public ViewPage(string p)
        {
            this.path = p;
            this.InitializeComponent();
            shareBtn.Focus();

            var uriSource1 = new Uri(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "photobootbsg.jpg"), UriKind.Relative);
            result.Source = new BitmapImage(uriSource1);

            BitmapImage photo = new BitmapImage();
            photo.BeginInit();
            photo.UriSource = new Uri(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), this.path));
            photo.EndInit();

            result.Source = photo;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (NavigationService.CanGoBack)
            //{
            //    NavigationService.GoBack();
            //}

            var page = new CapturePage();
            this.NavigationService.Navigate(page);
        }

        private void shareBtn_Click(object sender, RoutedEventArgs e)
        {

            //go to result page
            var page = new ShareEmail(this.path);
            this.NavigationService.Navigate(page);

            ////step 1 get full path of image

            ////fullpath = System.IO.Path.GetFullPath("1.png");

            //Console.WriteLine(this.path);
            //var client = new RestClient(serverAPI);
            //// client.Authenticator = new HttpBasicAuthenticator(username, password);

            //var request = new RestRequest(APIpostFoto, Method.POST);
            //request.AddParameter("path", this.path); // adds to POST or URL querystring based on Method
            //                                         //request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

            //// easily add HTTP Headers
            ////request.AddHeader("header", "value");

            //// add files to upload (works with compatible verbs)
            ////request.AddFile(path);

            //// execute the request
            //// IRestResponse response = client.Execute(request);
            ////var content = response.Content; // raw content as string
            //try
            //{

            //    IRestResponse<Response1> response2 = client.Execute<Response1>(request);

            //    var status = response2.Data.status;
            //    var msg = response2.Data.msg;
            //    if (status)
            //    {
            //        Console.WriteLine(msg);
            //        refreshauth();
            //    }
            //    else
            //    {
            //        Console.WriteLine(msg);
            //        refreshauth();
            //    }
            //}
            //catch (Exception)
            //{

            //    MessageBox.Show("Please Check Internet Connection");
            //}
        }
        private void refreshauth()
        {
            var client = new RestClient(refreshAUTH);
            var request = new RestRequest();

            IRestResponse<Response2> response2 = client.Execute<Response2>(request);


        }
    }
}
