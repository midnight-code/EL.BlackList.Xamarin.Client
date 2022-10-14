using EL.BlackList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            start();
        }
        
        public async void start()
        {
            UserBase users = await App.BlackListDB.GetUserBaseAsync();
            if (users == null)
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
            else if (users.DateEnd < DateTime.Now)
            {
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(users.UserID))
                {
                    await Shell.Current.GoToAsync($"{nameof(PinPage)}");
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
                }
            }
                
        }
        
    }
}