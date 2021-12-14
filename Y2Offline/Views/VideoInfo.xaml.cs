using System;
using System.Collections.Generic;
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
        public VideoInfo(Services.YTVid video)
        {
            InitializeComponent();
            VideoTitle.Text = video.Title;
            ChannelName.Text = video.Author;
            ThumbnailImage.Source = video.Thumbnail;

            Frame frame = new Frame();

            StackLayout stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Horizontal;
            stackLayout.Margin = new Thickness(5, 0, 0, 0);
            frame.BackgroundColor = Color.FromHex("#434545");



            Label label = new Label();
            label.FontSize = 25;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.TextColor = Color.Black;
            label.Text = "1080P";

            Button button = new Button();
            button.Text = "Download";
            button.VerticalOptions = LayoutOptions.End;
            button.HorizontalOptions = LayoutOptions.End;
            button.Margin = new Thickness(120, 0, 0, 0);
            

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(button);

            frame.Content = stackLayout;
            frame.BorderColor = Color.Black;
            frame.Margin = new Thickness(5);
            
            DownloadOptions.Children.Add(frame);
            DownloadOptions.Children.Add(frame);
            DownloadOptions.Children.Add(frame);
            
            //var video = new Y2Sharp.Youtube.Video(videoid); //tässkin rikki
        }
    }
}