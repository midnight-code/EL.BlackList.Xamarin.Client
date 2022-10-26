using EL.BlackList.Models;
using EL.BlackList.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EL.BlackList.ViewModels
{
    public class SerchDriverModelView : BaseViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string Birthday { get; set; }
        public bool imgload { get; set; } = false;
        public Command SerchCommand { get; }
        public SerchDriverModelView()
        {
            Title = "Home";
            SerchCommand = new Command(OnSerchClicked);
        }
        private async void OnSerchClicked(object obj)
        {
            if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                await Shell.Current.DisplayAlert("Error", "Driver not found! " + Birthday, "OK");
            else
            {
                imgload = true;
                UserBase users = await App.BlackListDB.GetUserBaseAsync();
                string token = "Bearer " + users.Token;
                string named = $"{FirstName} {LastName} {SecondName}";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    using (var response = await httpClient.GetAsync($"https://api.itiss.ru/getdriver/{named}"))
                    {
                        var str = response.RequestMessage.Headers;
                        if (((int)response.StatusCode) == 200)
                        {
                            string contents = await response.Content.ReadAsStringAsync();
                            //var rateInfo = JsonConvert.DeserializeObject<List<DriverModel>>(contents);
                            await Shell.Current.GoToAsync($"{nameof(DriverDetailsPage)}?{nameof(DriverDetailsPage.driverModels)}={contents}");
                        }
                        else
                        {
                            await Task.Run(() => SaveToken(users));
                            await Shell.Current.DisplayAlert("Error", "Driver not found! " + response.RequestMessage.Headers, "OK");
                        }

                    }
                }
            }
        }

        private async void SaveToken(UserBase users)
        {

            //var t = JsonConvert.DeserializeObject<TokenModel>(users.Token);
            TokenClass tokenClass = new TokenClass() { ID = 0, DateAdd = users.DateEnd, Token = users.Token };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(tokenClass), Encoding.UTF8, "application/json");
            using (HttpClient httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("Authorization", token);
                using (var response = await httpClient.PostAsJsonAsync($"https://api.itiss.ru/api/TokenClass", tokenClass))
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        string contents = await response.Content.ReadAsStringAsync();
                        //await Shell.Current.DisplayAlert("Внимание!", "Запись изменина успешно!", "OK");
                    }
                }
            }
        }
    }
}
