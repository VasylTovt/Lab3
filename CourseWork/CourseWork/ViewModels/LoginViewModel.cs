using CourseWork.Helpers;
using CourseWork.Services;
using CourseWork.Views;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CourseWork.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ApiServices _apiServices = new ApiServices();
        
        public string Email { get; set; }

        public string Password { get; set; }

        private LoginStatus loginStatus;
        public LoginStatus LoginStatus
        {
            get => loginStatus;
            set
            {
                if (value != loginStatus)
                {
                    loginStatus = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("LoginStatus"));
                }
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!Validators.EmailValidator.IsValid(Email))
                    {
                        LoginStatus = LoginStatus.WRONG_EMAIL_FORMAT;
                        return;
                    }

                    var result = await _apiServices.LoginAsync(Email, Password);

                    if (System.Enum.TryParse(result, out LoginStatus status))
                    {
                        //has error
                        LoginStatus = status;
                    }
                    else
                    {
                        Settings.Token = result;
                        Application.Current.MainPage = new MenuPage();
                    }

                    /*if (result.isSucceded)
                    {
                        LoginStatus = LoginStatus.OK;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", result.message, null, "Cancel");
                    }*/
                });
            }
        }

        public ICommand GotoRegister
        {
            get
            {
                return new Command(() =>
                {
                    Application.Current.MainPage = new RegisterPage();

                    Settings.Email = Email;
                    Settings.Password = Password;
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {
            Email = Settings.Email;
            Password = Settings.Password;
        }
    }

    public enum LoginStatus
    {
        OK, WRONG_EMAIL, WRONG_PASSWORD, WRONG_EMAIL_FORMAT
    }
}
