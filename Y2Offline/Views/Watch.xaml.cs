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
    public partial class Watch : ContentPage
    {
        public Watch()
        {
            InitializeComponent();

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var filesList = Directory.GetFiles(filePath);
            foreach (var file in filesList)
            {
                var filename = Path.GetFileName(file);


                StackLayout stackLayout = new StackLayout();
                stackLayout.Orientation = StackOrientation.Horizontal;

                Image image = new Image();
                image.WidthRequest = 60;
                image.HeightRequest = 20;

                //image.Source = video.Thumbnail;


                stackLayout.Children.Add(image);

                StackLayout stackLayout1 = new StackLayout();

                Label label = new Label();
                label.FontSize = 16;
                label.VerticalOptions = LayoutOptions.CenterAndExpand;
                label.Text = filename;
                label.TextColor = Color.Black;

                stackLayout1.Children.Add(label);

                Label label1 = new Label();
                label1.FontSize = 12;
                label1.VerticalOptions = LayoutOptions.CenterAndExpand;
                label1.Text = "author";
                label1.TextColor = Color.Black;

                stackLayout1.Children.Add(label1);

                stackLayout.Children.Add(stackLayout1);

                Button button = new Button();

                button.Clicked += async (sender2, args) => await Navigation.PushModalAsync(new Player(file));
                button.HeightRequest = 4;

                button.Text = "Watch";
                stackLayout.Children.Add(button);

                MainStack.Children.Add(stackLayout);
            }


           
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new Player());
        }
    }
}