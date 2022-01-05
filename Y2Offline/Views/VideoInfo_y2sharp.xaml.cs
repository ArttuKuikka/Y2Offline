using Newtonsoft.Json.Linq;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Xamarin.Forms.Xaml;



namespace Y2Offline.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoInfo_y2sharp : ContentPage
    {
        public Y2Sharp.Youtube.Video y2video { get; set; }
        public string y2id { get; set; }

        public Services.YTVidDetails ytvid { get; set; }
        public VideoInfo_y2sharp(Services.YTVidDetails VidDetails)
        {
            y2video = new Y2Sharp.Youtube.Video();

            ytvid = VidDetails;

            y2id = y2video.Url.Replace("https://www.youtube.com/watch?v=", string.Empty);
            InitializeComponent();

            var tmpdir = System.IO.Path.GetTempPath();

            var dir = Path.Combine(tmpdir, y2id + ".jpg");

            if (GetShowThumbnails())
            {
                //lataa thumnnail
                using (WebClient webClient = new WebClient())
                {
                    try
                    {


                        webClient.DownloadFile("https://i.ytimg.com/vi/" + y2id + "/0.jpg", dir);
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Error", "Error while downloading thumbnail", "OK");
                    }
                }
            }
            else
            {
                dir = null;
            }

            if (y2id == null) { DisplayAlert("Error", "error getting video info", "OK"); return; }
            

            

            VideoTitle.Text = VidDetails.Title;
            ChannelName.Text = VidDetails.ChannelTitle;
            ThumbnailImage.Source = dir;
            PublishedAtLabel.Text = VidDetails.PublicationDate.ToString("dd/MM/yyyy HH:mm");
            Descriptionlabel.Text = VidDetails.Description;



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



                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

                var path = Path.Combine(filePath, y2video.Title);



                try
                {
                    await Download("128", "mp3");

                    button2.Text = "Downloaded";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error downloading", ex.ToString(), "OK");
                }

            };

            Button addquebutton2 = new Button();
            addquebutton2.Text = "Add to queue";
            addquebutton2.VerticalOptions = LayoutOptions.End;
            addquebutton2.HorizontalOptions = LayoutOptions.End;

            addquebutton2.Clicked += async (sender2, args) =>
            {
                addquebutton2.IsEnabled = false;

                var video = new Services.YTVid
                {
                    Title = ytvid.Title,
                    Author = ytvid.ChannelTitle,
                    Id = y2id,
                    PublishedAt = ytvid.PublicationDate,
                    State = 0,
                    Resolution = "MP3"
                };

                var que = new Services.QueueHandler();

                if (que.IsAlreadyInQueue(ytvid.VideoId))
                {
                    await DisplayAlert("Error", "this video is already in the queue", "OK");

                }
                else
                {
                    que.AddToQueue(video);

                    addquebutton2.Text = "Added";
                }

                
            };


            



            stackLayout2.Children.Add(label2);
            stackLayout2.Children.Add(button2);
            stackLayout2.Children.Add(addquebutton2);

            frame2.Content = stackLayout2;
            frame2.BorderColor = Color.Black;
            frame2.Margin = new Thickness(5);

            DownloadOptions.Children.Add(frame2);
            //mp3 manual adding end


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
                
              
                var res = vid.Replace("p", string.Empty);

                try
                {
                    await Download(res, "mp4");

                    button.Text = "Downloaded";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error downloading", ex.ToString(), "OK");

                    button.Text = "ERROR";
                    button.BackgroundColor = Color.Red;
                }
                
            };

                Button addquebutton = new Button();
                addquebutton.Text = "Add to queue";
                addquebutton.VerticalOptions = LayoutOptions.End;
                addquebutton.HorizontalOptions = LayoutOptions.End;

                addquebutton.Clicked += async (sender2, args) =>
                {
                    addquebutton.IsEnabled = false;

                    var video = new Services.YTVid
                    {
                        Title = ytvid.Title,
                        Author = ytvid.ChannelTitle,
                        Id = y2id,
                        PublishedAt = ytvid.PublicationDate,
                        State = 0,
                        Resolution = vid.Replace("p", string.Empty)
                };

                    var que = new Services.QueueHandler();

                    if (que.IsAlreadyInQueue(ytvid.VideoId))
                    {
                        await DisplayAlert("Error", "this video is already in the queue", "OK");

                    }
                    else
                    {
                        que.AddToQueue(video);

                        addquebutton2.Text = "Added";
                    }

                };

                stackLayout.Children.Add(label);
            stackLayout.Children.Add(button);
                stackLayout.Children.Add(addquebutton);

                frame.Content = stackLayout;
            frame.BorderColor = Color.Black;
            frame.Margin = new Thickness(5);

            DownloadOptions.Children.Add(frame);
            }
     
        }

        public async Task Download(string quality, string type)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            string folderpath = Path.Combine(filePath, y2id);

            var tmpdir = System.IO.Path.GetTempPath();

            var dir = Path.Combine(tmpdir, y2id + ".jpg");

            Directory.CreateDirectory(folderpath);

            var succsesfuldownload = true;

            var videoname = Path.Combine(folderpath, y2id + "." + type);

            try
            {
                await y2video.DownloadAsync(videoname, type, quality);
            }
            catch(Exception)
            {
                succsesfuldownload = false;
            }


            if (succsesfuldownload)
            {
                string infofilecontent = "Title=" + y2video.Title + ";" + "Author=" + ytvid.ChannelTitle + ";" + "Type=" + type + ";" + "Published=" + ytvid.PublicationDate.ToString("dd/MM/yyyy HH:mm") + ";";

                

                var infofilepath = Path.Combine(folderpath, y2id + ".txt");

                File.WriteAllText(infofilepath, infofilecontent);

                try
                {
                    File.Copy(dir, folderpath, true);
                }
                catch (Exception) { }
            }
            else
            {
                //delete just created dir
                IFolder ifolder = FileSystem.Current.LocalStorage;
                IFolder file = await ifolder.GetFolderAsync(folderpath);
                await file.DeleteAsync();

                DisplayAlert("Error", "Error while downloading file", "OK");
            }
            

           

            

        }

        public bool GetShowThumbnails()
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            string mypath = Path.Combine(filePath, "settings.json");

            if (File.Exists(mypath))
            {
                //if settings have been generated


                JObject jsonObject = JObject.Parse(File.ReadAllText(mypath));

                return (bool)jsonObject["showthumbnails"];

            }
            else
            {
                return true;
            }
        }

        
    }
}