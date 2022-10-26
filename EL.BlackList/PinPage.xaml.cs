using EL.BlackList.Models;
using EL.BlackList.UserControl;
using EL.BlackList.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList
{
    public partial class PinPage : ContentPage
    {/// <summary>
    /// Нужно убедиться, что срок лицензии неистёк, пользователь был авторизован и получил действующий токен.
    /// Иначе отправить на страницу оплаты или авторизации.
    /// </summary>
        public PinPage()
        {
            InitializeComponent();
            ppp();
        }

        private async void ppp()
        {
            UserBase users = await App.BlackListDB.GetUserBaseAsync();
            if (users.PinCode == 0 || users.PinCode > 9999)
            {
                NewPinCode.IsVisible = true;
                PinCode.IsVisible = false;
            }
            else
            {
                NewPinCode.IsVisible = false;
                PinCode.IsVisible = true;
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            int pincod = 1234;
            UserBase users = await App.BlackListDB.GetUserBaseAsync();
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            if (users.PinCode == 0)
            {
                users.PinCode = pincod;
                await App.BlackListDB.UpdateUserBaseAsync(users);
                await Shell.Current.DisplayAlert("", "Pin code succes", "OK");
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                if (pincod == users.PinCode)
                {
                    await Shell.Current.DisplayAlert("", "Pin code succes", "OK");
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                    
                else
                    await Shell.Current.DisplayAlert("Error", "Error pin code", "OK");
            }
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UserBase users = await App.BlackListDB.GetUserBaseAsync();
            if (users == null && users.DateEnd < DateTime.Now && string.IsNullOrWhiteSpace(users.UserID))
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}