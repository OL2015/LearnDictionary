using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using DictionaryLogic.ModelProviders.EFModel;
using System.Linq;
using DictionaryUI.Services;
using System.Data.Entity;

namespace DictionaryUI.ViewModel
{

    public class WordBrowserViewModel : ViewModelBase
    {
        private static readonly int maxEntries = 10;
        private IDictionaryDataService dictionaryDataService;

        private ILogService  logService; 
        public RelayCommand AddNewWordCommand { get; private set; }
        public RelayCommand DequeueCurrentCommand { get; private set; }
        public RelayCommand SaveCurrentCommand { get; private set; }
        public RelayCommand DeleteCurrentWordCommand { get; private set; }

        ObservableCollection<WordEntryCardViewModel> wordEntries = new ObservableCollection<WordEntryCardViewModel>();
        public ObservableCollection<WordEntryCardViewModel> WordEntries {  
            get { return wordEntries; }
            set
            {
                Set(() => WordEntries, ref wordEntries, value);
            } 
         }
        WordEntryCardViewModel currentEntry  ;
        public WordEntryCardViewModel CurrentEntry
        {
            get { return currentEntry; }
            set
            {
                Set(() => CurrentEntry, ref currentEntry, value);
            }
        }

        private LearnDictionaryEntities efContext;
        public LearnDictionaryEntities EFContext
        {
            get { return efContext; }
            set
            {
                Set(() => EFContext, ref efContext, value);
            }
        }

        private Book selectedBook;
        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                Set(() => SelectedBook, ref selectedBook, value);
            }
        }

        private Language selectedLanguage;
        public Language SelectedLanguage 
        {
            get { return selectedLanguage; }
            set
            {
                Set(() => SelectedLanguage, ref selectedLanguage, value);
            }
        }

        public WordBrowserViewModel(  IDictionaryDataService dictionaryDataService, ILogService dlogService )
        {
            this.dictionaryDataService = dictionaryDataService;
            this.logService = dlogService; 
            AddNewWordCommand = new RelayCommand(AddNewWord, CanAddNewWord);
            DeleteCurrentWordCommand = new RelayCommand(DeleteCurrentWord, () => { return CurrentEntry != null; });
        }

        private void DeleteCurrentWord()
        {
            CurrentEntry.DeleteWord();
            WordEntries.Remove(CurrentEntry);

            //throw new NotImplementedException();
        }

        private bool CanAddNewWord()
        {
            return true; ;
        }

        private void AddNewWord()
        {
            AppendNewWord();
        }

        public void RestoreLastContext()
        {
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            efContext.Languages.Load();
            efContext.Books.Load(); 
            efContext.Words.Include("WordEntries").Load();
            var wEntry = efContext.WordEntries
               .OrderByDescending(z => z.WordEntry_ID)
               .FirstOrDefault();
            SelectedBook = wEntry.Book;
            PreloadLastEntries();
        }
        private void PreloadLastEntries()
        {
            var wEntries = efContext.WordEntries
                .OrderByDescending(z => z.Page)
                .OrderByDescending(z => z.WordEntry_ID)
                .Take(maxEntries - 1);
            
            foreach (var we in wEntries)
            {
                var wr = efContext.Entry(we);
                selectedBook = we.Book;
                Language sLanguage = null;
                if (SelectedBook != null)
                    sLanguage = SelectedBook.Language;
                //var translationService = ViewModelLocator.getNewTranslationService();
                var translationService = ViewModelLocator.getTranslationService();
                WordEntryCardViewModel wecVM = new WordEntryCardViewModel(dictionaryDataService, logService, translationService) { WordEntry = we };
                WordEntries.Add(wecVM);
                
            }
            AppendNewWord();
            //this.CurrentEntry = WordEntries[0];
        }

        public void PreloadEntries()
        { 
            var wEntries = efContext.WordEntries.Where( we=>  we.Book_ID == SelectedBook.Book_ID)
                .OrderByDescending(z=> z.Page)
                .OrderByDescending(z => z.WordEntry_ID) 
                .Take(maxEntries-1) ; 

            Language sLanguage  = null;
            if (SelectedBook != null)
                sLanguage = SelectedBook.Language;
            foreach (var we in wEntries)
            {
                var wr = efContext.Entry(we);
                //var translationService = ViewModelLocator.getNewTranslationService();
                var translationService = ViewModelLocator.getTranslationService();  
                WordEntryCardViewModel wecVM = new WordEntryCardViewModel(dictionaryDataService, logService, translationService) {  WordEntry = we  };
                WordEntries.Add(wecVM);
            }
            AppendNewWord();
            //this.CurrentEntry = WordEntries[0];
        } 

        internal void AppendNewWord()
        {
            //var lastEntry = WordEntries.FirstOrDefault
            var LastSessionPage = WordEntries.Count>0 ? WordEntries.Max(p=>p.WordEntry.Page) :1;
            //lastEntry != null ? lastEntry.WordEntry.Page : 1;
            WordEntry we = new WordEntry() { Book = this.SelectedBook,Page= LastSessionPage,Date=DateTime.Today };
            //var translationService = ViewModelLocator.getNewTranslationService();
            var translationService = ViewModelLocator.getTranslationService();
            WordEntryCardViewModel wecVM = new WordEntryCardViewModel(dictionaryDataService, logService, translationService) {   WordEntry = we };
            WordEntries.Insert(0, wecVM);
            this.CurrentEntry = wecVM;
        }

        internal void EditWord(WordEntry selectedTest)
        {
            throw new NotImplementedException() ;
        }
    }
}