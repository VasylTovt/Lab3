using CourseWork.Helpers;
using CourseWork.Services;
using CourseWork.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace CourseWork.ViewModels
{
    public class RegisterViewModel
    {
        private readonly ApiServices _apiServices = new ApiServices();

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var result = await _apiServices.RegisterAsync(Email, Password, ConfirmPassword);

                    if (result.isSucceded)
                    {
                        Settings.Email = Email;
                        Settings.Password = Password;
                    }

                    await Application.Current.MainPage.DisplayAlert(null, result.message, null, "Cancel");
                });
            }
        }

        public ICommand GotoLogin
        {
            get
            {
                return new Command(() =>
                {
                    Application.Current.MainPage = new LoginPage();
                });
            }
        }
    }
}
