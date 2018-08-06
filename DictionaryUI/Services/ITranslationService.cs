using DictionaryUI.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.Services
{
    
    public interface ITranslationService
    {
        Task<ObservableCollection<TranslationItem>> GetTranslations(string textToTranslate, string fromLang, string toLang);
        Task<int> GetRating(TranslationItem item, string fromLang, string toLang);
        Task  SetItemRating(TranslationItem item, string fromLang, string toLang);
    }
}
