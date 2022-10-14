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
	public partial class SerchDriverView : ContentView
	{
		public SerchDriverView ()
		{
			InitializeComponent ();
            this.BindingContext = new SerchDriverModelView();
            EntrBirthday.Text = dateBithday.Date.ToShortDateString();

        }
        private void dateBithday_DateSelected(object sender, DateChangedEventArgs e)
        {
            EntrBirthday.Text = dateBithday.Date.ToShortDateString();
        }
    }
}