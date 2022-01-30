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
using System.IO;
using Newtonsoft.Json.Linq;

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
            

            string videoid = string.Empty;

            try
            {
                videoid = getvideoidxamarin(url);
            }
            catch (Exception ex)
            {
                invalidurl = true;
                new AlertDialog.Builder(this)
                         .SetTitle("Error")
                         .SetMessage("Invalid url " + url)

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

                    Services.YTVid videoDetails = null;
                    using (var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                    {
                        ApiKey = GetApiKey(),
                    }))
                    {
                        var searchRequest = youtubeService.Videos.List("snippet");
                        searchRequest.Id = videoid;
                        var searchResponse = await searchRequest.ExecuteAsync();

                        var youTubeVideo = searchResponse.Items.FirstOrDefault();
                        if (youTubeVideo != null)
                        {
                            videoDetails = new Services.YTVid()
                            {
                               Id = youTubeVideo.Id,
                               Description = youTubeVideo.Snippet.Description,
                               Title = youTubeVideo.Snippet.Title,
                               Author = youTubeVideo.Snippet.ChannelTitle,
                               PublishedAt = (DateTime)youTubeVideo.Snippet.PublishedAt
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

        string getvideoidxamarin(string url)
        {
            if (url == null) { throw new ArgumentNullException("url"); }

            var viewParam = url;

            if (url.Contains("watch?v="))
            {
                viewParam = url.Substring(url.LastIndexOf("watch?v=") + 8);
                char[] charArray = viewParam.ToCharArray();
                Array.Reverse(charArray);
                var tempstr = new string(charArray);
                tempstr = tempstr.Remove(0, tempstr.Length - 11);
                char[] charArray2 = tempstr.ToCharArray();
                Array.Reverse(charArray2);
                viewParam = new string(charArray2);

            }
            else if (url.Contains("youtube.com/v/") || url.Contains("youtu.be/"))
            {
                viewParam = url.Substring(url.LastIndexOf("/") + 1);
                if (viewParam.Length > 10)
                {
                    char[] charArray = viewParam.ToCharArray();
                    Array.Reverse(charArray);
                    var tempstr = new string(charArray);
                    tempstr = tempstr.Remove(0, tempstr.Length - 11);
                    char[] charArray2 = tempstr.ToCharArray();
                    Array.Reverse(charArray2);
                    viewParam = new string(charArray2);
                }

            }
            else
            {
                throw new Exception("Can't parse the url");
            }






            if (viewParam == null || viewParam == "") { throw new Exception("Can't parse the url"); }

            return viewParam;
        }

        public string GetApiKey()
        {
            string filePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyVideos);
            string mypath = Path.Combine(filePath, "settings.json");

            if (File.Exists(mypath))
            {
                //if settings have been generated


                JObject jsonObject = JObject.Parse(File.ReadAllText(mypath));

                return (string)jsonObject["apikey"];

            }
            else
            {
                return "";
            }
        }
    }
}