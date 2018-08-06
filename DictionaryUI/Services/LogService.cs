using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DictionaryUI.Services
{
    public class LogService : ILogService
    {

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowException(string message, Exception ex)
        {
            string msg = ex.Message;
            Exception z = ex.InnerException;
            while (z!= null)
            {
                msg += "/ " + z.Message;
                    z = z.InnerException;
            }
            MessageBox.Show(msg, message, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool GetConfirmation(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes;
        }
    }
}
