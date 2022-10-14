using EL.BlackList.Models;
using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EL.BlackList.ViewModels
{
    public class AddDriverViewModel : BaseViewModel
    {
        DriverModel driver;
        public string AddFirstName { get; set; }
        public string AddLastName { get; set; }
        public string AddSecondName { get; set; }
        public string Birthday { get; set; }
        public int AddINN { get; set; }
        public Command AddCommand { get; }

        public AddDriverViewModel()
        {
            Title = "Home";
            AddCommand = new Command(OnAddDriverClicked);
        }
        private async void OnAddDriverClicked(object obj)
        {
            if (string.IsNullOrWhiteSpace(AddFirstName) && string.IsNullOrWhiteSpace(AddLastName))
                await Shell.Current.DisplayAlert("Error", "Все поля отмеченные * должны быть заполненны.", "OK");
            else
            {
                UserBase users = await App.BlackListDB.GetUserBaseAsync();
                driver = new DriverModel()
                {
                    FirstName = AddFirstName,
                    LastName = AddLastName,
                    SecondName = AddSecondName,
                    DataRogden = Convert.ToDateTime(Birthday),
                    BlackList = true,
                    INN = AddINN,
                    PassportID = 1,
                    TaxiPoolID = users.TaxiPoolID,
                    Avatar = 1,
                    DriverLicenseID=1,
                    FeedBacks = null,
                    ID = 0
                };
                string token = "Bearer " + users.Token;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    using (var response = await httpClient.PostAsJsonAsync($"https://api.itiss.ru/api/FeedBack", driver))
                    {
                        var str = response.RequestMessage.Headers;
                        if (((int)response.StatusCode) == 200)
                        {
                            string contents = await response.Content.ReadAsStringAsync();
                            //var rateInfo = JsonConvert.DeserializeObject<List<DriverModel>>(contents);
                            //await Shell.Current.GoToAsync($"{nameof(DriverDetailsPage)}?{nameof(DriverDetailsPage.driverModels)}={contents}");
                        }
                        else
                        {
                            //await Task.Run(() => SaveToken(users));
                            //await Shell.Current.DisplayAlert("Error", "Driver not found! " + response.RequestMessage.Headers, "OK");
                        }

                    }
                }
            }
        }
    }
}
