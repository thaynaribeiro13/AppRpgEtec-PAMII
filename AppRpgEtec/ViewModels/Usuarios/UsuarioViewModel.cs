using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService _uService;

        public ICommand AutenticarCommand { get; set; }
        public ICommand RegistrarCommand { get; set; }

        public UsuarioViewModel()
        {
            _uService = new UsuarioService();
            InicializarCommands();
        }

        public void InicializarCommands()
        {
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
            RegistrarCommand = new Command(async () => await RegistrarUsuario());
        }

        #region AtributosPropriedades
        private string login = string.Empty;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string senha = string.Empty;

        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public async Task AutenticarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Senha;

                Usuario uAutenticado = await _uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem vindo {u.Username}";

                    Preferences.Set("UsuarioToken", uAutenticado.Token);
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);

                    await Application.Current.MainPage
                        .DisplayAlert("Informação", mensagem, "OK");

                    Application.Current.MainPage = new MainPage();
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados Incorretos", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }

        #region Métodos

        public async Task RegistrarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Senha;

                Usuario uRegistrado = await _uService.PostRegistrarUsuarioAsync(u);

                if (uRegistrado.Id != 0)
                {
                    string mensagem = $"Usuário Id {uRegistrado.Id} registrado com sucesso.";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "OK");

                    await Application.Current.MainPage
                        .Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }
        #endregion
    }
}