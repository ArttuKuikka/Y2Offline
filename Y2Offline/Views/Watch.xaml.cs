using PCLStorage;
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

            ToolbarItems.Add(new ToolbarItem("refresh", "refresh.png", () =>
            {
                
                MainStack.Children.Clear();
                MainWatch();
            }));

            MainWatch();
        }

        public void MainWatch()
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var folderlist = Directory.GetDirectories(filePath);
            foreach (var folder in folderlist)
            {

                //get info for each video
                var foldername = folder.Replace(filePath, string.Empty);
                foldername = foldername.Replace("/", string.Empty);

                var Infotextapth = Path.Combine(folder, foldername + ".txt");

                var Title = "Error getting info";
                var Author = foldername;
                var Type = "mp4";

                if (File.Exists(Infotextapth))
                {
                    var infotext = File.ReadAllText(Infotextapth);

                    Title = GetBetween(infotext, "Title=", ";");
                    Author = GetBetween(infotext, "Author=", ";");
                    Type = GetBetween(infotext, "Type=", ";");
                }
                else
                {
                    DisplayAlert("Error", "Error getting info for " + foldername, "OK");

                }

                var videopath = Path.Combine(folder, foldername + "." + Type);

                var thumbnailpath = Path.Combine(folder, foldername + ".jpg"); 




                //create ui

                Frame frame = new Frame();
                frame.BackgroundColor = Color.FromHex("#434545");

                StackLayout stackLayout = new StackLayout();
                stackLayout.Orientation = StackOrientation.Horizontal;

                Image image = new Image();
                image.WidthRequest = 60;
                image.HeightRequest = 20;

                image.Source = thumbnailpath;


                stackLayout.Children.Add(image);

                StackLayout stackLayout1 = new StackLayout();

                Label label = new Label();
                label.FontSize = 16;
                label.VerticalOptions = LayoutOptions.CenterAndExpand;

                if(Title == "Error getting info")
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
                label1.Text = Author;
                label1.TextColor = Color.Black;

                stackLayout1.Children.Add(label1);

                stackLayout.Children.Add(stackLayout1);

              

                var tgr = new TapGestureRecognizer();
                tgr.Tapped += async (sender2, args) => await Navigation.PushModalAsync(new Player(videopath));
                stackLayout.GestureRecognizers.Add(tgr);

                var sgr = new SwipeGestureRecognizer();
                sgr.Direction = SwipeDirection.Left;
                sgr.Swiped += async (sender2, args) =>
                {
                    //are you sure dialagod
                    bool answer = await DisplayAlert("Are you sure you want to delete this video", Title, "Yes", "No");

                    if (answer)
                    {
                        MainStack.Children.Remove(stackLayout);
                        

                        try
                        {
                            IFolder ifolder = FileSystem.Current.LocalStorage;
                            IFolder file = await ifolder.GetFolderAsync(folder);
                            await file.DeleteAsync();
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", "Error while deleting file, you might have swiped too fast", "OK");
                        }

                        MainWatch();
                    }
                    else
                    {
                        return;
                    }


                    

                };

                stackLayout.GestureRecognizers.Add(sgr);


                frame.Content = stackLayout;
                frame.BorderColor = Color.Black;
                frame.Margin = new Thickness(2);
                MainStack.Children.Add(frame);


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