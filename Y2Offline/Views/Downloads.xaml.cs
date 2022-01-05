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

                Image image = new Image();
                image.WidthRequest = 60;
                image.HeightRequest = 20;

                //image.Source = thumbnailpath;


                stackLayout.Children.Add(image);

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



                var tgr = new TapGestureRecognizer();
                tgr.Tapped += async (sender2, args) => await DisplayAlert("painoit", "painoit tätä", "OK");
                stackLayout.GestureRecognizers.Add(tgr);

               

                


                frame.Content = stackLayout;
                frame.BorderColor = Color.Black;
                frame.Margin = new Thickness(2);
                MainStack.Children.Add(frame);

            }


            
        }


        async Task DownloadAll()
        {
            
        }

        

       
    }
}