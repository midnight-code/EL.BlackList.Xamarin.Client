using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EL.BlackList.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        public Command logOutCommand { get; }
        public AppShellViewModel()
        {
            logOutCommand = new Command(LogOut);
        }

        async void LogOut()
        {
            if (Preferences.ContainsKey(nameof(App.TokenUser)))
            {
                Preferences.Remove(nameof(App.TokenUser));
            }
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}
