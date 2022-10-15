using EL.BlackList.Models;
using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        public string ImagName { get; set; }


        public Command AddCommand { get; }
        public Command AddAvatar { get; }
        public FileResult file { get; set; }


        public AddDriverViewModel()
        {
            Title = "Home";
            AddCommand = new Command(OnAddDriverClicked);
            AddAvatar= new Command(OnAddAvatarClicked);
            ImagName = (file == null) ? "user.png" : file.FullPath;
        }
        private async void OnAddDriverClicked(object obj)
        {
            if (string.IsNullOrWhiteSpace(AddFirstName) || string.IsNullOrWhiteSpace(AddLastName) || string.IsNullOrEmpty(Birthday))
                await Shell.Current.DisplayAlert("Error", "Все поля отмеченные * должны быть заполненны.", "OK");
            else
            {
                if (Birthday == null)
                {
                    Birthday = DateTime.Now.ToString();
                }
                UserBase users = await App.BlackListDB.GetUserBaseAsync();
                string token = "Bearer " + users.Token;

                int idFile = Task.Run(()=> UploadFile(file, token)).Result;
                driver = new DriverModel()
                {
                    FirstName = AddFirstName,
                    LastName = AddLastName,
                    SecondName = AddSecondName,
                    DataRogden = Convert.ToDateTime(Birthday),
                    BlackList = true,
                    INN = AddINN,
                    PassportID = (idFile > 0) ? idFile : 1,
                    TaxiPoolID = users.TaxiPoolID,
                    Avatar = 1,
                    DriverLicenseID = 1,
                    FeedBacks = null,
                    ID = 0
                };
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    using (var response = await httpClient.PostAsJsonAsync($"https://api.itiss.ru/api/Driver/adddriver", driver))
                    {
                        var str = response.RequestMessage.Headers;
                        if (((int)response.StatusCode) == 200)
                        {
                            string contents = await response.Content.ReadAsStringAsync();
                            int id= Convert.ToInt32(contents);
                            await Shell.Current.DisplayAlert("Completed successfully", "Driver save success! ", "OK");
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

        private async void OnAddAvatarClicked(object obj)
        {
            file = await MediaPicker.PickPhotoAsync();

            if (file == null)
                return;
            else
                ImagName = file.FullPath;
            
        }

        private async Task<int> UploadFile(FileResult fileResult, string token)
        {
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                //Load the file and set the file's Content-Type header
                var fileStreamContent = new StreamContent(File.OpenRead(fileResult.FullPath));
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(fileResult.ContentType);

                multipartFormContent.Add(fileStreamContent, name: "file", fileName: fileResult.FileName);

                //Send it
                var response = await httpClient.PostAsync("https://api.itiss.ru/uploadedocument", multipartFormContent);
                response.EnsureSuccessStatusCode();
                var r = await response.Content.ReadFromJsonAsync<int>();
                int id = r;
                if (response.IsSuccessStatusCode)
                    return r;
                else
                    return 0;
            }
        }
    }
}
