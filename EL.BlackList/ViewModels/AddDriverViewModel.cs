using EL.BlackList.Models;
using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EL.BlackList.ViewModels
{
    public class AddDriverViewModel : INotifyPropertyChanged
    {
        
        string imgpatch = "";
        string driverlicense1;
        string driverlicense2;
        DriverModel driver;
        bool _isTaskRunning = false;

        public event PropertyChangedEventHandler PropertyChanged;

        
        public string AddFirstName { get; set; }
        public string AddLastName { get; set; }
        public string AddSecondName { get; set; }
        public string Birthday { get; set; }
        public int AddINN { get; set; }

        public bool isTaskRunning 
        {
            get { return _isTaskRunning; }
            set
            {
                if(_isTaskRunning != value)
                {
                    _isTaskRunning = value;
                    OnPropertyChanged("isTaskRunning");
                }
            } 
        }

        public string ImagName 
        {
            get { return imgpatch; }
            set
            {
                if(imgpatch!= value)
                {
                    imgpatch = value;
                    SetNewImage(value);
                    OnPropertyChanged("ImagName");
                }
            }
        }
        public string ImagDriverLicense1
        {
            get { return driverlicense1; }
            set
            {
                if (driverlicense1 != value)
                {
                    driverlicense1 = value;
                    OnPropertyChanged("ImagDriverLicense1");
                }
            }
        }

        public string ImagDriverLicense2
        {
            get { return driverlicense2; }
            set
            {
                if (driverlicense2 != value)
                {
                    driverlicense2 = value;
                    OnPropertyChanged("ImagDriverLicense2");
                }
            }
        }



        public DateTime MaxiDate { get; set; }
        public DateTime EndDate { get; set; }


        public Command AddCommand { get; }
        public Command AddAvatar { get; }
        public Command AddDriverLicenc1 { get; }
        public Command AddDriverLicenc2 { get; }
        public FileResult[] file { get; set; } 


        public AddDriverViewModel()
        {
            AddCommand = new Command(OnAddDriverClicked);
            AddAvatar= new Command(OnAddAvatarClicked);
            AddDriverLicenc1 = new Command(OnAddDriverLicense1Clicked);
            AddDriverLicenc2 = new Command(OnAddDriverLicense2Clicked);
            file = new FileResult[3];
            isTaskRunning =false;
            AddFirstName = String.Empty;
            AddLastName = String.Empty;
            AddSecondName = String.Empty;
            ImagName = (file[0] == null) ? "user.png" : file[0].FullPath;

            EndDate= DateTime.Now.AddYears(-65);
            MaxiDate = DateTime.Now.AddYears(-3);
        }
        private async void OnAddDriverClicked(object obj)
        {

            isTaskRunning = !isTaskRunning;


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
                    Avatar = (file[0] != null) ? Task.Run(() => UploadFile(file[0], token)).Result : 1,
                    DriverLicenseID = 1,//реализовать запись в базу фото лицензий
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
                            driver.ID = id;
                            List<DriverModel> list = new List<DriverModel>();
                            list.Add(driver);
                            string ret = JsonSerializer.Serialize<List<DriverModel>>(list);
                            await Shell.Current.DisplayAlert("Completed successfully", "Driver save success! ", "OK");
                            await Shell.Current.GoToAsync($"{nameof(DriverDetailsPage)}?{nameof(DriverDetailsPage.driverModels)}={ret}");
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
            file[0] = await MediaPicker.PickPhotoAsync();

            if (file == null)
                return;
            else
            {
                ImagName = file[0].FullPath;
                
            }
            
        }
        private async void OnAddDriverLicense1Clicked(object obj)
        {
            file[1] = await MediaPicker.PickPhotoAsync();

            if (file == null)
                return;
            else
            {
                driverlicense1 = file[1].FullPath;

            }

        }
        private async void OnAddDriverLicense2Clicked(object obj)
        {
            file[2] = await MediaPicker.PickPhotoAsync();

            if (file == null)
                return;
            else
            {
                driverlicense2 = file[2].FullPath;

            }

        }
        protected void SetNewImage(string value)
        {
            ImagName = value;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
