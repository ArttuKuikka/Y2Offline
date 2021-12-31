using System;
using System.Collections.Generic;
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
        public SettingsPage()
        {
            InitializeComponent();

            searchlimitslider.Value = Settings.Default.SearchResultLimit;

            LoadThumbnailsSwitch.IsToggled = Settings.Default.DownloadThumbnails;

        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            Settings.Default.SearchResultLimit = searchlimitslider.Value;
            Settings.Default.DownloadThumbnails = LoadThumbnailsSwitch.IsToggled;
            
            Settings.Default.Save();
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            searchlimitslider.Value = 15;

            LoadThumbnailsSwitch.IsToggled = true;

            
        }

        private void searchlimitslider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SliderValueLabel.Text = searchlimitslider.Value.ToString();
        }
    }
}