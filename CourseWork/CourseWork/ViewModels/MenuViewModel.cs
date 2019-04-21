using CourseWork.Helpers;
using CourseWork.Models;
using CourseWork.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CourseWork.ViewModels
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private readonly ApiServices _apiServices = new ApiServices();

        public string[] Data { get; set; }

        public ICommand UsersCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var result = await _apiServices.GetUsersAsync(Settings.Token);
                    try
                    {
                        var d = JsonConvert.DeserializeObject<List<User>>(result).ToArray();
                        Data = new string[d.Length];
                        for(int i = 0; i < d.Length; i++)
                        {
                            Data[i] = d[i].ToString();
                        }
                        PropertyChanged(this, new PropertyChangedEventArgs("Data"));
                    }
                    catch
                    {

                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
