using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Y2Offline.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchAndAdd : ContentPage
    {
        
        
        public SearchAndAdd()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("Downloads", "download_icon.png", async () =>
            {
                await Navigation.PushAsync(new Downloads());

            }));
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {

            


            try
            {
                vidview.Children.Clear();
                
                ActivityIndicator activityIndicator = new ActivityIndicator();
                activityIndicator.IsVisible = true;
                activityIndicator.IsEnabled = true;
                activityIndicator.IsRunning = true;
                //{ IsRunning = true; IsEnabled = true; IsVisible = true; };
                mainlayout.Children.Add(activityIndicator);

                var yt = new Services.YTSearch();
                List<Services.YTVid> result = await yt.Run(SearchBox.Text);

                activityIndicator.IsRunning = false;
                

                foreach(var video in result)
                {
                    
                    
                    StackLayout stackLayout = new StackLayout();
                    stackLayout.Orientation = StackOrientation.Horizontal;

                    Image image = new Image();
                    image.WidthRequest = 60;
                    image.HeightRequest = 20;

                    image.Source = video.Thumbnail;


                    stackLayout.Children.Add(image);

                    StackLayout stackLayout1 = new StackLayout();

                    Label label = new Label();
                    label.FontSize = 16;
                    label.VerticalOptions = LayoutOptions.CenterAndExpand;
                    label.Text = video.Title;
                    label.TextColor = Color.Black;

                    stackLayout1.Children.Add(label);

                    Label label1 = new Label();
                    label1.FontSize = 12;
                    label1.VerticalOptions = LayoutOptions.CenterAndExpand;
                    label1.Text = video.Author;
                    label1.TextColor = Color.Black;

                    stackLayout1.Children.Add(label1);

                    stackLayout.Children.Add(stackLayout1);


                    var tgr = new TapGestureRecognizer();
                    tgr.Tapped += async (sender2, args) =>
                    {
                        
                        
                        ActivityIndicator activityIndicator2 = new ActivityIndicator();

                        stackLayout.Children.Add(activityIndicator2);

                        activityIndicator2.HorizontalOptions = LayoutOptions.Center;
                        activityIndicator2.VerticalOptions = LayoutOptions.Center;
                        activityIndicator2.IsVisible = true;
                        activityIndicator2.IsEnabled = true;
                        activityIndicator2.IsRunning = true;

                        await Y2Sharp.Youtube.Video.GetInfo(video.Id);

                        activityIndicator2.IsEnabled = false;
                        activityIndicator2.IsRunning = false;
                        stackLayout.Children.Remove(activityIndicator2);
                        

                        await Navigation.PushAsync(new VideoInfo(video));
                    };
                    stackLayout.GestureRecognizers.Add(tgr);

                    vidview.Children.Add(stackLayout);

                }
            }
            catch (AggregateException ex)
            {
                foreach (var exc in ex.InnerExceptions)
                {
                    await DisplayAlert("Error", exc.Message, "OK");
                }
            }
            

        }

        



    }
}