using EL.BlackList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.UserControl
{
	public partial class PaymentView : ContentView
	{
		public PaymentView ()
		{
			InitializeComponent ();
			this.BindingContext = new PayPageModel();
		}
	}
}