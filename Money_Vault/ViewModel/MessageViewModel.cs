using System;
using System.Windows;

namespace Money_Vault.ViewModel
{
    /// <summary>
    /// Логика для модального окна с информационным сообщением об ошибке/предупреждении
    /// </summary>
    public class MessageViewModel : BaseViewModel
    {
        private string _header;
        private string _message;
        private bool _result;

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

        public bool Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

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
    }
}
