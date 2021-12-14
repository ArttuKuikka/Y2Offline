using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Y2Offline.ViewModels;
using Y2Offline.Views;

namespace Y2Offline
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
