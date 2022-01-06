using Newtonsoft.Json.Linq;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Y2Offline.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Downloads : ContentPage
    {

        private Services.QueueHandler QueueHandler { get; set; }
        public Downloads()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("Download all", "download_icon.png", async () => await DownloadAll()));

            ToolbarItems.Add(new ToolbarItem("Refress", "refresh.png", async () => MakeGUI()));

            QueueHandler = new Services.QueueHandler();

           
            MakeGUI();
        }

        public void MakeGUI()
        {
            MainStack.Children.Clear();
            foreach (var item in QueueHandler.GetQueue())
            {
                var Title = (string)item["Title"];
                var Author = (string)item["Author"];
                var Id = (string)item["Id"];
                var PublishedAt = (DateTime)item["PublishedAt"];
                var State = (int)item["State"];
                var Resolution = (string)item["Resolution"];


                Frame frame = new Frame();
                frame.BackgroundColor = Color.FromHex("#434545");

                StackLayout stackLayout = new StackLayout();
                stackLayout.Orientation = StackOrientation.Horizontal;


                StackLayout stackLayout1 = new StackLayout();

                Label label = new Label();
                label.FontSize = 16;
                label.VerticalOptions = LayoutOptions.CenterAndExpand;

                if (Title == "Error getting info")
                {
                    label.TextColor = Color.Red;
                }
                else
                {
                    label.TextColor = Color.Black;
                }

                label.Text = Title;


                stackLayout1.Children.Add(label);

                Label label1 = new Label();
                label1.FontSize = 12;
                label1.VerticalOptions = LayoutOptions.CenterAndExpand;
                label1.Text = Author + "  " + PublishedAt;
                label1.TextColor = Color.Black;

                stackLayout1.Children.Add(label1);



                stackLayout.Children.Add(stackLayout1);



                ActivityIndicator activityIndicator = new ActivityIndicator();
                

                stackLayout.Children.Add((ActivityIndicator)activityIndicator);




                ImageButton button1 = new ImageButton();
                button1.Source = "download_icon.png";
                button1.Clicked += async (sender2, args) =>
                {
                    using (var cancellationTokenSource = new CancellationTokenSource())
                    {
                        var buttonfilename = button1.Source.ToString();
                        if (buttonfilename == "File: download_icon.png")
                        {
                            await DownloadSingleVideo(button1, activityIndicator, item);
                        }
                        else
                        {
                            cancellationTokenSource.Cancel();
                            cancellationTokenSource.Dispose();
                            activityIndicator.IsVisible = false;
                            activityIndicator.IsEnabled = false;
                            activityIndicator.IsRunning = false;

                            button1.Source = "download_icon.png";
                            // delete folder
                        }
                    }

                        
                };
                button1.HeightRequest = 10;
                button1.WidthRequest = 50;
                button1.BackgroundColor = Color.FromHex("#3b403f");
                button1.BorderColor = Color.FromHex("#3b403f");
                button1.HorizontalOptions = LayoutOptions.End;
                stackLayout.Children.Add(button1);






                frame.Content = stackLayout;
                frame.BorderColor = Color.Black;
                frame.Margin = new Thickness(2);
                MainStack.Children.Add(frame);

            }


            
        }

        async Task DownloadSingleVideo(ImageButton thisbutton, ActivityIndicator activityIndicator1, JToken videojson)
        {
            thisbutton.Source = "close.png";

            activityIndicator1.IsVisible = true;
            activityIndicator1.IsEnabled = true;
            activityIndicator1.IsRunning = true;

            var Title = (string)videojson["Title"];
            var Author = (string)videojson["Author"];
            var Id = (string)videojson["Id"];
            var PublishedAt = (DateTime)videojson["PublishedAt"];
            var State = (int)videojson["State"];
            var Resolution = (string)videojson["Resolution"];

            var Type = "Mp4";

            if(Resolution == "MP3")
            {
                Resolution = "128";
                Type = "Mp3";
            }



            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            string folderpath = Path.Combine(filePath, Id);

            var tmpdir = System.IO.Path.GetTempPath();

            var dir = Path.Combine(tmpdir, Id + ".jpg");

            Directory.CreateDirectory(folderpath);

            var succsesfuldownload = true;

            var videoname = Path.Combine(folderpath, Id + "." + Type);



            try
            {
                await Y2Sharp.Youtube.Video.GetInfo(Id);
                
                var y2video = new Y2Sharp.Youtube.Video();

                await y2video.DownloadAsync(videoname, Type, Resolution);
                await DownloadSpeed(videoname, Resolution);
            }
            catch (Exception)
            {
                
                succsesfuldownload = false;
            }


            if (succsesfuldownload)
            {
                string infofilecontent = "Title=" + Title + ";" + "Author=" + Author + ";" + "Type=" + Type + ";" + "Published=" + PublishedAt.ToString("dd/MM/yyyy HH:mm") + ";";

                var infofilepath = Path.Combine(folderpath, Id + ".txt");

                File.WriteAllText(infofilepath, infofilecontent);

                try
                {
                    File.Copy(dir, folderpath, true);
                }
                catch (Exception) { }

                QueueHandler.RemoveFromQueue(Id);

            }
            else
            {
                //delete just created dir
                IFolder ifolder = FileSystem.Current.LocalStorage;
                IFolder file = await ifolder.GetFolderAsync(folderpath);
                await file.DeleteAsync();

                DisplayAlert("Error", "Error while downloading file, try again later", "OK");
            }

            activityIndicator1.IsVisible = false;
            activityIndicator1.IsEnabled = false;
            activityIndicator1.IsRunning = false;

            
            MakeGUI();

            

        }


        async Task DownloadAll()
        {
            
        }


        async Task DownloadSpeed(string path, string resolution)
        {

        }
        

       
    }
}