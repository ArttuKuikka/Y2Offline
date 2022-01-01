using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json.Linq;

namespace Y2Offline.Services
{
    internal class YTSearch
    {
        
        
        
        [STAThread]
        static void Main(string[] args)
        {
           

           

           
        }

        public async Task<List<Services.YTVid>> Run(string searchterm)
        {
            var downloadthumbnails = GetShowThumbnails();
            
            if(searchterm == null) { throw new Exception("searchterm was null"); }
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDSf8QFsOOdjTMwOpa1408Beo1vskmNPkI",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = searchterm; // Replace with your search term.

            var searchlimit = GetSearchLimit();

            searchListRequest.MaxResults = Convert.ToInt64(searchlimit);

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            
            List<Services.YTVid> vidlist = new List<Services.YTVid>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        //videos.Add(searchResult.Snippet.Title + ", " + searchResult.Id.VideoId + ", " + searchResult.Snippet.ChannelTitle);
                        Services.YTVid vid = new Services.YTVid();
                        vid.Title = searchResult.Snippet.Title;
                        vid.Author = searchResult.Snippet.ChannelTitle;
                        vid.Id = searchResult.Id.VideoId;
                        

                        if (downloadthumbnails)
                        {
                            //Get thumbnail
                            byte[] imageBytes = new WebClient().DownloadData("https://i.ytimg.com/vi/" + searchResult.Id.VideoId + "/0.jpg");
                            MemoryStream ms = new MemoryStream(imageBytes);


                            vid.Thumbnail = ImageSource.FromStream(() => {
                                ms.Flush();
                                ms.Position = 0;
                                return ms;
                            });
                        }
                        else
                        {
                            vid.Thumbnail = null;
                        }


                        vidlist.Add(vid);

                        
                        break;

                    

                    
                }
            }

            return vidlist;
            
            
        }

        public double GetSearchLimit()
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string mypath = Path.Combine(filePath, "settings.json");

            if (File.Exists(mypath))
            {
                //if settings have been generated


                JObject jsonObject = JObject.Parse(File.ReadAllText(mypath));

                return (Double)jsonObject["searchlimit"];

            }
            else
            {
                return 15;
            }
        }

        public bool GetShowThumbnails()
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
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
