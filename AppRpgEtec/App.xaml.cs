﻿using AppRpgEtec.Views.Usuarios;

namespace AppRpgEtec
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new AppShell();
            MainPage = new NavigationPage(new LoginView());
        }
    }
}
