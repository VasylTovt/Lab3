using CourseWork.App.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace CourseWork.App.ViewModels
{
    public class LoginViewModel
    {
        private readonly ApiServices _apiServices = new ApiServices();

        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var result = await _apiServices.LoginAsync(Email, Password);
                    await Application.Current.MainPage.DisplayAlert("Result", result, null, "Cancel");
                });
            }
        }
    }
}
