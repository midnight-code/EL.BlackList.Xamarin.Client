using EL.BlackList.Models;
using EL.BlackList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            Payment();
            InitializeComponent();
            this.BindingContext = new HomeViewModel();
        }
        private async void Payment()
        {

            PaymentUserModel model = await App.PaymentUserDB.GetPaymentUserByUserId();
            if (model.DatePayment.AddDays(model.PeriodPayment) < DateTime.Now)
            {
                PayModel.IsVisible= true;
                SerchModel.IsVisible= false;
            }
            else
            {
                PayModel.IsVisible = false;
                SerchModel.IsVisible = true;
            }
        }
    }
}