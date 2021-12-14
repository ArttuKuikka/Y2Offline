using System.ComponentModel;
using Xamarin.Forms;
using Y2Offline.ViewModels;

namespace Y2Offline.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}