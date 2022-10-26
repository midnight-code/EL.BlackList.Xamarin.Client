using EL.BlackList.Models;
using EL.BlackList.ViewModels;
using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    UserBase users = await App.BlackListDB.GetUserBaseAsync();
        //    if (users != null && users.DateEnd > DateTime.Now && !string.IsNullOrWhiteSpace(users.UserID))
        //        await Shell.Current.GoToAsync($"{nameof(PinPage)}");
        //}
    }
}