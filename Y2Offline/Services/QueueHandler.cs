using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Y2Offline.Services
{
    class QueueHandler
    {

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

        string DownloadsFilePath { get; set; }

        public QueueHandler()
        {
            DownloadsFilePath = Path.Combine(filePath, "Downloads.json");

                if (!File.Exists(DownloadsFilePath))
                {
                File.WriteAllText(DownloadsFilePath, "[]");
                }
          
        }
        public void AddToQueue(Services.YTVid video)
        {
            var myData = new
            {
                Title = video.Title.Replace(" ", "%20"),
                Author = video.Author.Replace(" ", "%20"),
                Id = video.Id,
                PublishedAt = video.PublishedAt,
                State = 0,
                Resolution = video.Resolution
            };
            string jsonData = JsonConvert.SerializeObject(myData);


            string file = File.ReadAllText(DownloadsFilePath);
            if(file == "[]")
            {
                file = file.Replace("]", "");

                file = file + jsonData;

                file = file + "]";

                File.WriteAllText(DownloadsFilePath, file);
            }
            else
            {
                file = file.Replace("]", "");

                file = file + ",";

                file = file + jsonData;

                file = file + "]";

                File.WriteAllText(DownloadsFilePath, file);
            }

            

            


        }

        public void RemoveFromQueue(string videoid)
        {
            JArray jsonObject = JArray.Parse(File.ReadAllText(DownloadsFilePath));

            string file = File.ReadAllText(DownloadsFilePath).ToString();

            foreach(var item in jsonObject)
            {
                if (item.ToString().Contains(videoid))
                {
                    var newitem = item.ToString();

                    newitem = newitem.Replace("{{", "{");
                    newitem = newitem.Replace("}}", "}");
                    newitem = newitem.Replace(System.Environment.NewLine, String.Empty);
                    newitem = newitem.Replace(" ", string.Empty);

                    if(file.Contains(newitem + ","))
                    {
                        newitem = newitem + ",";
                    }
                    

                    var readyfile = file.Replace(newitem, string.Empty);
                    File.WriteAllText(DownloadsFilePath, readyfile);
                }  
            }
        }

        public bool IsAlreadyInQueue(string videoid)
        {
            string file = File.ReadAllText(DownloadsFilePath).ToString();

            if (file.Contains(videoid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public JArray GetQueue() 
        {
            return JArray.Parse(File.ReadAllText(DownloadsFilePath).Replace("%20", " "));
        }

        public void ResetQueue()
        {
            File.Delete(DownloadsFilePath);
        }
    }
}
