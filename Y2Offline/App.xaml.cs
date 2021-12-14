using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Y2Offline.Services;
using Y2Offline.Views;

namespace Y2Offline
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
