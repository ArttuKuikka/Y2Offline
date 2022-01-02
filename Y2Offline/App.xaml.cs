using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Y2Offline.Services;
using Y2Offline.Views;

namespace Y2Offline
{
    public partial class App : Application
    {

        public App(int state, Services.YTVidDetails details)
        {
            InitializeComponent();


            if(state == 0)
            {
                MainPage = new AppShell();
            }
            else if(state == 1)
            {
                MainPage = new VideoInfo_y2sharp(details);
            }
            else if(state == 2)
            {
                MainPage = new LoadingPage();
            }
            
            
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
