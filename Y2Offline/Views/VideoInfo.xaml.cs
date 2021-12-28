﻿using System;
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
            
            button.Clicked += async (sender2, args) =>
            {
                button.IsEnabled = false;
                button.Text = "Downloading...";
                
                await Y2Sharp.Youtube.Video.GetInfo(video.Id);

                var y2video = new Y2Sharp.Youtube.Video();

                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                var path = Path.Combine(filePath, video.Title);

                await y2video.DownloadAsync(path + ".mp3");

                button.Text = "Downloaded";
                
            };
            


            stackLayout.Children.Add(label);
            stackLayout.Children.Add(button);

            frame.Content = stackLayout;
            frame.BorderColor = Color.Black;
            frame.Margin = new Thickness(5);

            DownloadOptions.Children.Add(frame);



            
           

           
        }

        
    }
}