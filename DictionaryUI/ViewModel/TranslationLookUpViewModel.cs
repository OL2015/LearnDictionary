using DictionaryUI.Model;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DictionaryUI.ViewModel
{  
    public class TranslationLookUpViewModel :  ViewModelBase  //, INotifyPropertyChanged
    {
        private ITranslationService translationService;
        private ILogService logService;
        public string baseWord= "@zzz";
        public Guid Token { get; private set; } = Guid.NewGuid();
        public string BaseWord
        {
            get { return baseWord; }
            set
            {
                baseWord = value;
                RaisePropertyChanged("BaseWord");
            }
        }
        public string fromLang = "zzz- eng";
        public string FromLang
        {
            get { return fromLang; }
            set
            {
                fromLang = value;
                RaisePropertyChanged("FromLang");
            }
        }
        public string toLang = "zzz- ukr";
        public string ToLang
        {
            get { return toLang; }
            set
            {
                toLang = value;
                RaisePropertyChanged("ToLang");
            }
        }

        public RelayCommand ChooseWordCommand { get; private set; } 
        
        private TranslationItem selectedWord; 
        public TranslationItem SelectedWord
        {
            get { return selectedWord; }
            set { 
                selectedWord = value; 
                RaisePropertyChanged("SelectedWord");
            }
        }
        private ObservableCollection<TranslationItem> _translatedItems = new ObservableCollection<TranslationItem>();
        public ObservableCollection<TranslationItem> TranslatedItems
        {
            get { return _translatedItems; }
            set
            {
                _translatedItems = value; 
                RaisePropertyChanged("TranslatedItems");
            }
        }

        public TranslationLookUpViewModel(ILogService dlogService, ITranslationService translationService)
        {
            this.logService = dlogService;
            this.translationService = translationService;
            ChooseWordCommand = new RelayCommand(InformSelectedWord);
            Messenger.Default.Register<TranslateWordNotificationMessage>(
            this,
            message =>
            {
                BaseWord = message.WordToTranslate;
                GetTranslatedText(message.WordToTranslate, message.FromLang, message.ToLang);
            });
        }
        ~TranslationLookUpViewModel()
        {
            Messenger.Default.Unregister(this);
        }
        public void DoTranslation()
        {
            GetTranslatedText(BaseWord, FromLang, ToLang);
        }
        internal async void GetTranslatedText(string baseWord, string fromLang, string toLang)
        {
            try
            {
                TranslatedItems.Clear();
                BaseWord = baseWord;
                FromLang = fromLang;
                ToLang = toLang;
                var z = await translationService.GetTranslations(BaseWord, fromLang, toLang);
                 
                     TranslatedItems = z;
                     // GetRatings(); 

            }
            catch( Exception ex)
            {
                 //logService.ShowException ("Error getting word translation", ex);
            }
        }
        private void GetRatings ()
        {
            Parallel.ForEach(TranslatedItems, new ParallelOptions { MaxDegreeOfParallelism = 2}, async (item) => 
            {
                try
                {
                    item.Rating = await translationService.GetRating(item, ToLang, FromLang);
                   //await translationService.SetItemRating(item, ToLang, FromLang);
                }
                catch (Exception ex)
                {
                    logService.ShowException("Cannot Load GetRatings", ex); 
                    throw;
                }
            });
        }
        private async void GetRatings2  ()
        {
            TranslatedItems.AsParallel().Select(
                 async (item) =>
                {
                    try
                    {
                         item.Rating =await translationService.GetRating(item, ToLang, FromLang) ;
                          // translationService.SetItemRating(item, ToLang, FromLang);
                    }
                    catch (Exception ex)
                    {
                        logService.ShowException("Cannot Load GetRatings", ex); 
                        throw;
                    }
                    return false;
                }
                );
            return;

            foreach (var item in TranslatedItems) 
            {
                try
                {
                     // item.Rating = await translationService.GetRating(item, ToLang, FromLang) ;
                      await translationService.SetItemRating(item, ToLang, FromLang);
                     var r = item.Rating ;
                     item.Rating = 1;
                     item.Rating = r;
                }
                catch (Exception ex)
                {
                    logService.ShowException("Cannot Set Item Ratings", ex); 
                    throw;
                }
                 
            }  
        } 
        
        public void InformSelectedWord()
        {
            if (SelectedWord != null)
            {
                DictionaryLookupNotificationMessage  message = new DictionaryLookupNotificationMessage(this, "") { TtanslationItm = SelectedWord, Token=this.Token };
                Messenger.Default.Send(message);
            }

        }
    }
}
