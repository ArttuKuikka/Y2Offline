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
using Android.Widget;
using Android.Views;

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
            //loading animation
            LoadApplication(new App(2, null));

            
            bool invalidurl = false;
            
            var url = Intent.GetStringExtra(Android.Content.Intent.ExtraText);
            url = url + ";";

            string videoid = string.Empty;

            if (url.Contains("youtu.be/"))
            {
                
                videoid = GetBetween(url, "youtu.be/", ";");
            }
            else if (url.Contains("m.youtube.com/watch?v=") && url.Length > 47)
            {
                videoid = GetBetween(url, "m.youtube.com/watch?v=", "&");
            }
            else if (url.Contains("m.youtube.com/watch?v=") && url.Length < 47)
            {
                videoid = GetBetween(url, "m.youtube.com/watch?v=", ";");
            }
            else if (url.Contains("www.youtube.com/watch?v=") && url.Length > 44)
            {
                videoid = GetBetween(url, "www.youtube.com/watch?v=", "&");
            }
            else if (url.Contains("www.youtube.com/watch?v=") && url.Length < 44)
            {
                videoid = GetBetween(url, "www.youtube.com/watch?v=", ";");
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
               invalidurl = true;
                new AlertDialog.Builder(this)
                         .SetTitle("Error")
                         .SetMessage("Invalid url")
                         
                         .SetPositiveButton("OK", (dialog, whichButton) =>
                         {
                         
                    FinishAndRemoveTask();
                             FinishAffinity();
                         })
                         .Show();
            }

            if(videoid == null) { invalidurl = true; }


            if (!invalidurl)
            {
                try
                {
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
                                ChannelTitle = youTubeVideo.Snippet.ChannelTitle,
                                PublicationDate = youTubeVideo.Snippet.PublishedAt
                            };
                        }

                    }


                    LoadApplication(new App(1, videoDetails));
                }
                catch(Exception ex)
                {
                    new AlertDialog.Builder(this)
                         .SetTitle("Error")
                         .SetMessage(ex.Message)

                         .SetPositiveButton("OK", (dialog, whichButton) =>
                         {

                             FinishAndRemoveTask();
                             FinishAffinity();
                         })
                         .Show();
                }
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