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
	public partial class AddDriverPage : ContentPage
	{
		public AddDriverPage ()
		{
			InitializeComponent ();
			this.BindingContext = new AddDriverViewModel();
		}

		private void dateBithday_DateSelected(object sender, DateChangedEventArgs e)
		{
			EntrBirthday.Text=e.NewDate.ToString();
		}
	}
}