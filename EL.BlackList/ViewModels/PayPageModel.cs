using EL.BlackList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Xamarin.Forms;

namespace EL.BlackList.ViewModels
{
    class PayPageModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public PayPageModel()
        {
            Title = "Home";
            SaveCommand = new Command(Button_Clicked);
        }

        private async void Button_Clicked(object sender)
        {
            UserBase users = await App.BlackListDB.GetUserBaseAsync();
            using (HttpClient httpClient = new HttpClient())
            {
                ResetToken resetToken = new ResetToken() { userName = $"{users.LoginUser}" };
                using (var response = await httpClient.PostAsJsonAsync($"https://api.itiss.ru/api/Authenticate/reset-password-token", resetToken))
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        string contents = await response.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<TokenResetModel>(contents);
                        users.Token = token.token;
                        users.DateEnd = DateTime.Now.AddDays(30);
                        await App.BlackListDB.UpdateUserBaseAsync(users);
                        await Shell.Current.DisplayAlert("Внимание!", "Запись изменина успешно!", "OK");
                    }
                }
            }
        }
    }
}
