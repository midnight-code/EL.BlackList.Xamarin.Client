using EL.BlackList.Models;
using EL.BlackList.Services;
using EL.BlackList.UserControl;
using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EL.BlackList.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ErrorSubjest { get; set; }
        public Command LoginCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                UserBase userBase= await LoginRepository.Login(UserName, Password);
                if (userBase!=null)
                {
                    await Shell.Current.GoToAsync($"{nameof(PinPage)}");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Error login on password", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Error login on password is not null", "OK");
            }
        }
    }
}
