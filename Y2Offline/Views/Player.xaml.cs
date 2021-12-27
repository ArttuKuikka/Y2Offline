﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using LibVLCSharp.Shared;

namespace Y2Offline.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        public Player()
        {
            InitializeComponent();

            DependencyService.Get<IStatusBar>().HideStatusBar();

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            MP.HeightRequest = mainDisplayInfo.Height / mainDisplayInfo.Density;
            MP.WidthRequest = mainDisplayInfo.Width / mainDisplayInfo.Density;

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Core.Initialize();


            var _libvlc = new LibVLC();
            var _mediaplayer = new MediaPlayer(_libvlc)
            {
                Media = new Media(_libvlc, new Uri("https://arttukuikka.fi/otv.mp4"))//media
            };

            MP.MediaPlayer = _mediaplayer;

            _mediaplayer.EncounteredError += mp_error;

            _mediaplayer.Play();
        }

        private async void mp_error(object sender, EventArgs e)
        {
            await DisplayAlert("Error", "Error", "ok");
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            base.OnBackButtonPressed();
            DependencyService.Get<IStatusBar>().ShowStatusBar();
        }
    }

    
}