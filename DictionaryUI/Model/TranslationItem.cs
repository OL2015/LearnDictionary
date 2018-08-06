using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.Model
{
    public class TranslationItem : ObservableObject
    {
        private string originalWord;
        public string OriginalWord
        {
            get { return originalWord; }
            set
            {
                originalWord = value;
                RaisePropertyChanged("OriginalWord");
            }
        }
        private string translation;
        public string Translation
        {
            get { return translation; }
            set
            {
                translation = value;
                RaisePropertyChanged("Translation");
            }
        }
        private string mainMeaning;
        public string MainMeaning
        {
            get { return mainMeaning; }
            set
            {
                mainMeaning = value;
                RaisePropertyChanged("MainMeaning");
            }
        }
        private string langPart = "Noun";
        public string LangPart
        {
            get { return langPart; }
            set
            {
                langPart = value;
                RaisePropertyChanged("LangPart");
            }
        }
        private int rating;
        public int Rating
        {
            get { return rating; }
            set
            {
                rating = value;
                RaisePropertyChanged("Rating");
            }
        }
    }
}
