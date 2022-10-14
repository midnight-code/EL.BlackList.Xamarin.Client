using EL.BlackList.Models;
using EL.BlackList.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
//using System.Text.Json;
using Xamarin.Forms;

namespace EL.BlackList.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public bool PayVisible { get; set; }
        public bool SerchVisible { get; set; }
        public HomeViewModel()
        {
            GetTest();
        }

        private async void GetTest()
        {
            var httpclient = new HttpClient();
            string url = $"https://24f.torov-lab.ru/api/Otdel/otdel";
            httpclient.BaseAddress = new Uri(url);
            HttpResponseMessage message = await httpclient.GetAsync("");
            if (message.IsSuccessStatusCode)
            {
                string contents = await message.Content.ReadAsStringAsync();
                //otdel = JsonConvert.DeserializeObject<List<OtdelModel>>(contents);
            }
        }
    }
}
