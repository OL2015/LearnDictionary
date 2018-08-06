using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.ViewModel
{
    public class TranslateWordNotificationMessage :  NotificationMessage
    {
        public TranslateWordNotificationMessage(string notification) : base(notification) { }
        public TranslateWordNotificationMessage(object sender, string notification) : base(sender, notification) { }
        public TranslateWordNotificationMessage(object sender, object target, string notification) : base(sender, target, notification) { }
        public string WordToTranslate { get; set; }
        public string FromLang { get; set; }
        public string ToLang { get; set; } 
        
    }
}
