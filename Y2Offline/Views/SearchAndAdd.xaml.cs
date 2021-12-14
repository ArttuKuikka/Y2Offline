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
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var yt = new Services.YTSearch();
                List<Services.YTVid> result = await yt.Run(SearchBox.Text);
                foreach(var video in result)
                {
                    
                    
                    StackLayout stackLayout = new StackLayout();
                    stackLayout.Orientation = StackOrientation.Horizontal;

                    Image image = new Image();
                    image.WidthRequest = 60;
                    image.HeightRequest = 20;
                    
                    image.Source = ImageSource.FromStream(() => {
                        video.Thumbnail.Flush();
                        video.Thumbnail.Position = 0;
                        return video.Thumbnail; });


                    stackLayout.Children.Add(image);

                    StackLayout stackLayout1 = new StackLayout();

                    Label label = new Label();
                    label.FontSize = 16;
                    label.VerticalOptions = LayoutOptions.CenterAndExpand;
                    label.Text = video.Title;

                    stackLayout1.Children.Add(label);

                    Label label1 = new Label();
                    label1.FontSize = 12;
                    label1.VerticalOptions = LayoutOptions.CenterAndExpand;
                    label1.Text = video.Author;

                    stackLayout1.Children.Add(label1);

                    stackLayout.Children.Add(stackLayout1);

                    Button button = new Button();
                    
                    button.Clicked += async (sender2, args) => await Navigation.PushAsync(new VideoInfo(video.Id));
                    button.HeightRequest = 4;
                    
                    button.Text = "Download";
                    stackLayout.Children.Add(button);

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