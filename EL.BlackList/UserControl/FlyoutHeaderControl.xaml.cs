using EL.BlackList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.UserControl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutHeaderControl : ContentView
    {
        public FlyoutHeaderControl()
        {
            InitializeComponent();

            ReadUser();
        }

        private async void ReadUser()
        {
            UserBase users = await App.BlackListDB.GetUserBaseAsync();
            PaymentUserModel model = await App.PaymentUserDB.GetPaymentUserByUserId();
            if (!string.IsNullOrWhiteSpace(users.UserID) && !string.IsNullOrWhiteSpace(users.FirstName))
            {
                lblUserName.Text = $"{users.FirstName} {users.LastName.ElementAt(0)}. {users.SecondName.ElementAt(0)}.  ";
                lblEmail.Text = users.NameCompPublic;
            }
            if (model != null)
            {
                lblPayment.Text = $"Дата следующего платежа: {model.DatePayment.AddDays(model.PeriodPayment).ToShortDateString()}";
                
            }
            else
            {
                lblPayment.TextColor = Color.Red;
                lblPayment.Text = $"Вы просрочили оплату, функционал ограничен.";
            }
        }
    }
}