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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddDriverPage : ContentPage
	{
		public AddDriverPage ()
		{
			InitializeComponent ();
			this.BindingContext = new AddDriverViewModel();
		}
	}
}