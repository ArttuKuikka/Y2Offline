using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class SettingsPage : ContentPage
    {
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        

        public SettingsPage()
        {
           
            InitializeComponent();


            try
            {
                Directory.CreateDirectory(filePath);
            }
            catch(Exception ex)
            {
                DisplayAlert("Error while creating app folder", ex.Message, "OK");
            }

            string mypath = Path.Combine(filePath, "settings.json");

            if (!File.Exists(mypath))
            {
                var myData = new
                {
                    searchlimit = "15",
                    showthumbnails = "true",
                    apikey = ""
                };

                string jsonData = JsonConvert.SerializeObject(myData);

                try
                {
                    File.WriteAllText(mypath, jsonData);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error while writing to file", ex.Message, "OK");
                }
            }

            try
            {
                JObject jsonObject = JObject.Parse(File.ReadAllText(mypath));


                int searchlimit = (int)jsonObject["searchlimit"];
                bool showthumbnails = (bool)jsonObject["showthumbnails"];
                string apikey = (string)jsonObject["apikey"];


                searchlimitslider.Value = searchlimit;
                LoadThumbnailsSwitch.IsToggled = showthumbnails;
                ApiKeyEntry.Text = apikey;
                ApiKeyEntry.IsPassword = true;
            }
            catch(Exception ex)
            {
                DisplayAlert("Error while reading data", ex.Message, "OK");
            }

        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            string mypath = Path.Combine(filePath, "settings.json");

           

            var myData = new
            {
                searchlimit = Math.Round(searchlimitslider.Value).ToString(),
                showthumbnails = LoadThumbnailsSwitch.IsToggled.ToString(),
                apikey = ApiKeyEntry.Text
                };

                string jsonData = JsonConvert.SerializeObject(myData);

            

            try
            {
                File.WriteAllText(mypath, jsonData);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error while writing to file", ex.Message, "OK");
                
            }
            
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            searchlimitslider.Value = 15;

            LoadThumbnailsSwitch.IsToggled = true;

            
        }

        private void searchlimitslider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var noobslider = Math.Round(searchlimitslider.Value);
            
            SliderValueLabel.Text = noobslider.ToString();
        }

        private void ShowApiCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(ShowApiCheckbox.IsChecked == true)
            {
                ApiKeyEntry.IsPassword = false;

            }
            else
            {
                
                ApiKeyEntry.IsPassword = true;
            }
        }
    }
}