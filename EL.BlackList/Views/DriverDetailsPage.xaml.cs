using EL.BlackList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.Views
{
    [QueryProperty(nameof(driverModels), nameof(driverModels))]
    public partial class DriverDetailsPage : ContentPage
    {
        UserBase users=new UserBase();
        private int _driverid = 0;
        public string driverModels 
        {
            set
            {
                GetResult(value);
            }
        }
        public DriverDetailsPage()
        {
            InitializeComponent();
        }
        public async void GetResult(string content)
        {
            var rateInfo = JsonConvert.DeserializeObject<List<DriverModel>>(content);
            DriverModel driver = new DriverModel();
            _driverid = rateInfo[0].ID;
            users = await App.BlackListDB.GetUserBaseAsync();
            string token = "Bearer " + users.Token;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                using (var response = await httpClient.GetAsync($"https://api.itiss.ru/getdriverbyid/{rateInfo[0].ID}"))
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        string contents = await response.Content.ReadAsStringAsync();
                        driver = JsonConvert.DeserializeObject<DriverModel>(contents);
                    }
                }
            }
            
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(51), Encoding.UTF8, "application/json");
                using (var response = httpClient.PostAsync($"https://api.itiss.ru/DownloadDocument/{driver.Avatar}", content1).Result)
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        //var ms = await response.Content.ReadAsStreamAsync();
                        var dyt = await response.Content.ReadAsByteArrayAsync();
                        //ms.Seek(0, SeekOrigin.Begin);
                        imgAvtar.Source = ImageSource.FromStream(() => new MemoryStream(dyt));
                    }
                }
            }
            lstFeedBack.ItemsSource = driver.FeedBacks;
            lblBithdey.Text = driver.DataRogden.ToShortDateString();
            BindingContext = driver;
        }

        private async void onItemSelected(Object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as FeedBackModel;
            await Shell.Current.GoToAsync($"{nameof(FeedBackDetailsPage)}?{nameof(FeedBackDetailsPage.FedBack)}={selected.ID}");
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(AddFeedBackPage)}?driverid={_driverid}&taxipoolid={users.TaxiPoolID}");
        }
    }
}