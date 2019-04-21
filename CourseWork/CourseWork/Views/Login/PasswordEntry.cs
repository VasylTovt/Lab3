using CourseWork.ViewModels;

using Xamarin.Forms;

namespace CourseWork.Views
{
    public class PasswordEntry : Entry
    {
        public static readonly BindableProperty LoginStatusProperty = BindableProperty.Create("LoginStatus", typeof(LoginStatus), typeof(LoginStatus), defaultBindingMode: BindingMode.OneWay, propertyChanged: LoginStatus_PropertyChanged);

        public LoginStatus LoginStatus
        {
            get => (LoginStatus)GetValue(LoginStatusProperty);
            set => SetValue(LoginStatusProperty, value);
        }

        public PasswordEntry() : base()
        {
        }

        private static void LoginStatus_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Entry entry)
            {
                if (newValue is LoginStatus status)
                {
                    switch (status)
                    {
                        case LoginStatus.WRONG_PASSWORD:
                            entry.Placeholder = "Wrong password";
                            entry.Text = "";
                            entry.PlaceholderColor = Color.Red;
                            break;
                    }
                }
            }
        }
    }
}