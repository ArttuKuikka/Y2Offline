﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Y2Offline.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        public Player()
        {
            InitializeComponent();

            DependencyService.Get<istatu>
        }
    }

    
}