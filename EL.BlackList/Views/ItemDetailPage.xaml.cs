using EL.BlackList.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace EL.BlackList.Views
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