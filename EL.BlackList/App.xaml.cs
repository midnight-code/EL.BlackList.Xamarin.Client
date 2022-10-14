using EL.BlackList.Data;
using EL.BlackList.Models;
using EL.BlackList.Services;
using EL.BlackList.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList
{
    public partial class App : Application
    {
        public static string TokenUser;

        public const string UriApi = "https://api.itiss.ru/";
        static BlackListDB blackListDB;
        static PaymentUserDB paymentUserDB;
        public static BlackListDB BlackListDB
        {
            get
            {
                if (blackListDB == null)
                {
                    blackListDB = new BlackListDB(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "NotesDataBase.db3"
                        ));
                }
                return blackListDB;
            }
        }
        public static PaymentUserDB PaymentUserDB
        {
            get
            {
                if (paymentUserDB == null)
                {
                    paymentUserDB = new PaymentUserDB(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "NotesDataBase.db3"));
                }
                return paymentUserDB;
            }
        }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<LoginServices>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
