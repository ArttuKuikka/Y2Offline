using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Y2Offline.Views;
using Xamarin.Forms;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using System.Linq;

namespace Y2Offline.Droid
{
    [Activity(Label = "Y2Offline", Name = "com.ArttuKuikka.y2offline.MainActivity", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(0, null));


            if (Intent.Action.Equals(Intent.Action) &&
            Intent.Type != null &&
            "text/plain".Equals(Intent.Type))
            {
                handleSendUrl();
                
                
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void handleSendUrl()
        {
            
            //add loading animation here
            
            
            var url = Intent.GetStringExtra(Android.Content.Intent.ExtraText);
            url = url + ";";

            string videoid = string.Empty;

            if (url.Contains("youtu.be/"))
            {
                
                videoid = GetBetween(url, "youtu.be/", ";");
            }
            else if (url.Contains("youtube.com/watch?v=") && url.Length > 44)
            {
                videoid = GetBetween(url, "youtube.com/watch?v=", "&");
            }
            else if (url.Contains("youtube.com/watch?v=") && url.Length < 44)
            {
                videoid = GetBetween(url, "youtube.com/watch?v=", ";");
            }
            else if (url.Contains("youtube.com/v/") && url.Length < 38)
            {
                videoid = GetBetween(url, "youtube.com/v/", ";");
            }
            else if (url.Contains("youtube.com/v/") && url.Length > 38)
            {
                videoid = GetBetween(url, "youtube.com/v/", "?");
            }
            else if (url.Contains("youtube.com/v/") && url.Length > 38)
            {
                videoid = GetBetween(url, "youtube.com/v/", "&");
            }
            else
            {
                //error
            }


            await Y2Sharp.Youtube.Video.GetInfo(videoid);

            Services.YTVidDetails videoDetails = null;
            using (var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDSf8QFsOOdjTMwOpa1408Beo1vskmNPkI",
            }))
            {
                var searchRequest = youtubeService.Videos.List("snippet");
                searchRequest.Id = videoid;
                var searchResponse = await searchRequest.ExecuteAsync();

                var youTubeVideo = searchResponse.Items.FirstOrDefault();
                if (youTubeVideo != null)
                {
                    videoDetails = new Services.YTVidDetails()
                    {
                        VideoId = youTubeVideo.Id,
                        Description = youTubeVideo.Snippet.Description,
                        Title = youTubeVideo.Snippet.Title,
                        ChannelTitle = youTubeVideo.Snippet.ChannelTitle
                    };
                }

            }


                LoadApplication(new App(1, videoDetails));

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