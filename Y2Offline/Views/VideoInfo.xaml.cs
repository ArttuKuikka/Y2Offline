using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Xamarin.Forms.Xaml;



namespace Y2Offline.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoInfo : ContentPage
    {
        public Y2Sharp.Youtube.Video y2video { get; set; }
        public VideoInfo(Services.YTVid video)
        {
            InitializeComponent();

            if(video == null) { DisplayAlert("Error", "error getting video info", "OK"); return; }
            

            y2video = new Y2Sharp.Youtube.Video();

            VideoTitle.Text = video.Title;
            ChannelName.Text = video.Author;
            ThumbnailImage.Source = video.Thumbnail;


            //mp3 manual adding

            Frame frame2 = new Frame();

            StackLayout stackLayout2 = new StackLayout();
            stackLayout2.Orientation = StackOrientation.Horizontal;
            stackLayout2.Margin = new Thickness(5, 0, 0, 0);
            frame2.BackgroundColor = Color.FromHex("#434545");



            Label label2 = new Label();
            label2.FontSize = 25;
            label2.VerticalTextAlignment = TextAlignment.Center;
            label2.TextColor = Color.Black;
            label2.Text = "MP3";

            Button button2 = new Button();
            button2.Text = "Download";
            button2.VerticalOptions = LayoutOptions.End;
            button2.HorizontalOptions = LayoutOptions.End;

            button2.Clicked += async (sender2, args) =>
            {
                button2.IsEnabled = false;
                button2.Text = "Downloading...";



                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                var path = Path.Combine(filePath, video.Title);



                try
                {
                    await Download("128", "mp3", video);

                    button2.Text = "Downloaded";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error downloading", ex.ToString(), "OK");
                }

            };



            stackLayout2.Children.Add(label2);
            stackLayout2.Children.Add(button2);

            frame2.Content = stackLayout2;
            frame2.BorderColor = Color.Black;
            frame2.Margin = new Thickness(5);

            DownloadOptions.Children.Add(frame2);
            //mp3 manual adding


            foreach (var vid in y2video.Resolutions)
            {


                Frame frame = new Frame();

            StackLayout stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Horizontal;
            stackLayout.Margin = new Thickness(5, 0, 0, 0);
            frame.BackgroundColor = Color.FromHex("#434545");



            Label label = new Label();
            label.FontSize = 25;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.TextColor = Color.Black;
            label.Text = vid;

            Button button = new Button();
            button.Text = "Download";
            button.VerticalOptions = LayoutOptions.End;
            button.HorizontalOptions = LayoutOptions.End;
            
            button.Clicked += async (sender2, args) =>
            {
                button.IsEnabled = false;
                button.Text = "Downloading...";
                
                

                //string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                //var path = Path.Combine(filePath, video.Title);

                var res = vid.Replace("p", string.Empty);

                try
                {
                    await Download(res, "mp4", video);

                    button.Text = "Downloaded";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error downloading", ex.ToString(), "OK");
                }
                
            };
            


            stackLayout.Children.Add(label);
            stackLayout.Children.Add(button);

            frame.Content = stackLayout;
            frame.BorderColor = Color.Black;
            frame.Margin = new Thickness(5);

            DownloadOptions.Children.Add(frame);
            }
     
        }

        public async Task Download(string quality, string type, Services.YTVid video)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string folderpath = Path.Combine(filePath, video.Id);

            Directory.CreateDirectory(folderpath);

            var videoname = Path.Combine(folderpath, video.Id + "." + type);

            await y2video.DownloadAsync(videoname, type, quality);

            

            string infofilecontent = "Title=" + video.Title + ";" + "Author=" + video.Author + ";" + "Type="+ type + ";";

            var infofilepath = Path.Combine(folderpath, video.Id + ".txt");  

            File.WriteAllText(infofilepath, infofilecontent);

            

        }

        

        
    }
}