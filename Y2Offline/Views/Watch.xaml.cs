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

            

            var folderlist = Directory.GetDirectories(filePath);
            foreach (var folder in folderlist)
            {
                

                var foldername = folder.Replace(filePath, string.Empty);
                foldername = foldername.Replace("/", string.Empty);

                var Infotextapth = Path.Combine(folder, foldername + ".txt");

                var infotext = File.ReadAllText(Infotextapth);


                var Title = GetBetween(infotext, "Title=", ";");
                var Author = GetBetween(infotext, "Author=", ";");
                var Type = GetBetween(infotext, "Type=", ";");
                var videopath = folder.Replace(filePath, string.Empty);
                videopath = videopath.Replace("/", string.Empty);

                videopath = Path.Combine(folder, videopath + "." + Type);



                //create ui
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
                label.Text = Title;
                label.TextColor = Color.Black;

                stackLayout1.Children.Add(label);

                Label label1 = new Label();
                label1.FontSize = 12;
                label1.VerticalOptions = LayoutOptions.CenterAndExpand;
                label1.Text = Author;
                label1.TextColor = Color.Black;

                stackLayout1.Children.Add(label1);

                stackLayout.Children.Add(stackLayout1);

                Button button = new Button();

                button.Clicked += async (sender2, args) => await Navigation.PushModalAsync(new Player(videopath));
                button.HeightRequest = 4;

                button.Text = "Watch";
                stackLayout.Children.Add(button);

                MainStack.Children.Add(stackLayout);






            }


           
        }

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }


    }
}