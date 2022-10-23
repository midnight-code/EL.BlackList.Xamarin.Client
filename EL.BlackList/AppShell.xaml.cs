using EL.BlackList.ViewModels;
using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EL.BlackList
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            this.BindingContext = new AppShellViewModel();

            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(PinPage), typeof(PinPage));
            Routing.RegisterRoute(nameof(DriverDetailsPage), typeof(DriverDetailsPage));
            Routing.RegisterRoute(nameof(FeedBackDetailsPage), typeof(FeedBackDetailsPage));
            //Routing.RegisterRoute(nameof(AddDriverPage), typeof(AddDriverPage));
            Routing.RegisterRoute(nameof(AddFeedBackPage), typeof(AddFeedBackPage));
            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
        }

    }
}
