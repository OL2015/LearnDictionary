using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.Services
{
    public interface ILogService
    {
        void ShowMessage(string message);
        void ShowException(string message, Exception ex);
        bool GetConfirmation(string message, string title);
    }
}
