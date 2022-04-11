using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    /// <summary>
    /// Логика для модального окна с информационным сообщением
    /// </summary>
    public class MessageViewModel : BaseViewModel
    {
        private RelayCommand _closeWindowCommand;

        private string _header;
        private string _message;

        public MessageViewModel()
        {
            Header = "Внимание";
            Message = "Сообщение";
        }

        public MessageViewModel(string header, string message)
        {
            Header = header;
            Message = message;
        }

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand((args) =>
                {
                    try
                    {
                        (Application.Current as App).displayRootRegistry.HidePresentation(this);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }));
            }
        }
    }
}
