using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Y2Offline.Services;
using Y2Offline.Views;

namespace Y2Offline
{
    public partial class App : Application
    {

        public App(int state, Services.YTVid details)
        {
            InitializeComponent();


            if(state == 0)
            {
                MainPage = new AppShell();
            }
            else if(state == 1)
            {
                MainPage = new VideoInfo(details);
            }
            else if(state == 2)
            {
                MainPage = new LoadingPage();
            }
            
            
            
        }

        protected override void OnStart()
        {
            base.OnStart();
            

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        
    }
}
