using EL.BlackList.Models;
using EL.BlackList.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.Views
{
	public partial class PaymentPage : ContentPage
	{
		public PaymentPage ()
		{
			InitializeComponent ();
            this.BindingContext = new PaymentPageViewModel();
        }

		
	}
}