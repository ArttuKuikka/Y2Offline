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
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            //notch.Text = Settings1.Default.notchint.ToString();
        }

        private void notch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Settings1.Default.notchint = Int32.Parse(notch.Text); //joskus int check
        }
    }
}