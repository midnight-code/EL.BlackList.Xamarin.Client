using EL.BlackList.Models;
using EL.BlackList.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EL.BlackList.Services
{
    public class LoginServices : ILoginRepository
    {
        public async Task<UserBase> Login(string username, string password)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    string token = string.Empty;
                    UserInfo userinfo = new UserInfo();
                    userinfo.UserName = username;
                    userinfo.Password = password;
                    var client = new HttpClient();
                    //string url = "https://api.itiss.ru/api/Authenticate/login";
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(userinfo), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(@"https://api.itiss.ru/api/Authenticate/login", content))
                        {
                            if (((int)response.StatusCode) == 200)
                            {
                                token = await response.Content.ReadAsStringAsync();
                                TokenModel tokenModel = JsonConvert.DeserializeObject<TokenModel>(token);
                                var user = Task.Run(() => GetUserReg(tokenModel, username)).Result;
                                var pay = Task.Run(() => GetPaymentUser(tokenModel)).Result;
                                if(pay!=true)
                                {
                                    PaymentUserModel paymentUser = new PaymentUserModel() 
                                    {
                                        Contributed=0,
                                        DatePayment=DateTime.Now.AddDays(-10),
                                        PeriodPayment=0,
                                        UserID=tokenModel.users 
                                    };
                                    await App.PaymentUserDB.UpdatePaymentUserAsync(paymentUser);
                                }
                                return await Task.FromResult(user);
                            }
                            else
                                return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


        private async Task<UserBase> GetUserReg(TokenModel tokenModel, string loguser)
        {
            UserRegClass userRegClass = new UserRegClass();
            UserBase user = new UserBase();
            string _token = "Bearer " + tokenModel.token;
            using (var httpClient1 = new HttpClient())
            {
                httpClient1.DefaultRequestHeaders.Add("Authorization", _token);
                using (var respo = await httpClient1.GetAsync($"https://api.itiss.ru/userreg/{tokenModel.users}"))
                {
                    if (((int)respo.StatusCode) == 200)
                    {
                        string cont = await respo.Content.ReadAsStringAsync();
                        userRegClass = JsonConvert.DeserializeObject<UserRegClass>(cont);

                        user.UserID = tokenModel.users;
                        user.Token = tokenModel.token;
                        user.FirstName = userRegClass.FirstName;
                        user.LastName = userRegClass.LastName;
                        user.SecondName = userRegClass.SecondName;
                        user.PhoneNumber = userRegClass.PhoneNumber;
                        user.PhoneNumberPublic = userRegClass.PhoneNumberPublic;
                        user.NameCompPublic = userRegClass.NameCompPublic;
                        user.DateStart = DateTime.Now;
                        user.DateEnd = Convert.ToDateTime(tokenModel.expiration);
                        user.CityID = userRegClass.CityID;
                        user.CityName = userRegClass.CityName;
                        user.TaxiPoolID = userRegClass.TaxiPoolID;
                        user.LoginUser = loguser;
                        App.BlackListDB.DroupTable();
                        await App.BlackListDB.UpdateUserBaseAsync(user);

                    }
                }
            }
            return await Task.Run(() => user);
        }

        private async Task<bool> GetPaymentUser(TokenModel tokenModel)
        {
            PaymentUserModel paymentUser = new PaymentUserModel();
            string _token = "Bearer " + tokenModel.token;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", _token);
                using (var respo = await httpClient.GetAsync($"https://api.itiss.ru/api/PaymentUser/pay-user-userid/{tokenModel.users}"))
                {
                    if (((int)respo.StatusCode) == 200)
                    {
                        string cont = await respo.Content.ReadAsStringAsync();
                        paymentUser = JsonConvert.DeserializeObject<PaymentUserModel>(cont);
                        await App.PaymentUserDB.UpdatePaymentUserAsync(paymentUser);
                        return true;
                    }
                    else
                        return false;
                }
            }
        } 
    }
}
