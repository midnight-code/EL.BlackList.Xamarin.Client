using EL.BlackList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.Views
{
    public partial class AddFeedBackPage : ContentPage, IQueryAttributable
    {
        UserBase users = new UserBase();
        int _driverID = 0;
        int _taxiPoolID = 0;
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            _driverID = Convert.ToInt32(HttpUtility.UrlDecode(query["driverid"]));
            _taxiPoolID = Convert.ToInt32(HttpUtility.UrlDecode(query["taxipoolid"]));
        }
        public AddFeedBackPage()
        {
            InitializeComponent();
            AddFeedBack();
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            FeedBackModel feedBack = new FeedBackModel()
            {
                CityID = users.CityID,
                DateADD = DateTime.Now,
                DriverID = _driverID,
                TaxiPoolID = _taxiPoolID,
                Subjest = editMessage.Text,
                UserID = users.UserID,
                City = null,
                ID = 0,
                TaxiPool = null
            };

            string token = "Bearer " + users.Token;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(feedBack), Encoding.UTF8, "application/json");
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                using (var response = await httpClient.PostAsJsonAsync($"https://api.itiss.ru/api/FeedBack", feedBack))
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        string contents = await response.Content.ReadAsStringAsync();
                        await Shell.Current.DisplayAlert("Внимание!", "Запись добавлена успешно!", "OK");
                        await Shell.Current.GoToAsync("..");
                    }
                }
            }
        }

        private async void AddFeedBack()
        {
            users = await App.BlackListDB.GetUserBaseAsync();
            string token = "Bearer " + users.Token;
            lblDate.Text = DateTime.Now.ToShortDateString();
            lblCity.Text = users.CityName;
            lblTaxiPool.Text = users.NameCompPublic;
            editMessage.Placeholder = "Опешите возникшие проблемы в подробностях...";
            editMessage.PlaceholderColor = Color.Olive;
        }
    }
}