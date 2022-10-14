using EL.BlackList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.Views
{
    [QueryProperty(nameof(FedBack), nameof(FedBack))]
    public partial class FeedBackDetailsPage : ContentPage
    {
        public int FedBack
        {
            set
            {
                GetFedBack(value);
            }
        }

        UserBase users = new UserBase();
        FeedBackModel feedBack = new FeedBackModel();
        public FeedBackDetailsPage()
        {
            InitializeComponent();
        }
        
        private async void GetFedBack(int fedBack)
        {
            users = await App.BlackListDB.GetUserBaseAsync();
            feedBack = new FeedBackModel();
            string token = "Bearer " + users.Token;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                using (var response = await httpClient.GetAsync($"https://api.itiss.ru/feedbackbyid/{fedBack}"))
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        string contents = await response.Content.ReadAsStringAsync();
                        feedBack = JsonConvert.DeserializeObject<FeedBackModel>(contents);
                    }
                }
            }

            lblDate.Text = feedBack.DateADD.ToShortDateString();
            lblCity.Text = feedBack.City.Name;
            lblTaxiPool.Text = feedBack.TaxiPool.Name;
            lblMessage.Text = feedBack.Subjest;
            editMessage.Text = feedBack.Subjest;
            if (feedBack.UserID == users.UserID)
            {
                btnEdite.IsVisible = true;
                editMessage.IsVisible= true;
                lblMessage.IsVisible = false;
            }
            else
            {
                btnEdite.IsVisible = false;
                editMessage.IsVisible = false;
                lblMessage.IsVisible = true;
            }
        }

        private async void btnEdite_Clicked(object sender, EventArgs e)
        {
            feedBack.Subjest= editMessage.Text;
            string token = "Bearer " + users.Token;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(feedBack), Encoding.UTF8, "application/json");
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                using (var response = await httpClient.PutAsJsonAsync($"https://api.itiss.ru/api/FeedBack", feedBack))
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        string contents = await response.Content.ReadAsStringAsync();
                        await Shell.Current.DisplayAlert("Внимание!", "Запись изменина успешно!", "OK");
                    }
                }
            }
        }
    }
}