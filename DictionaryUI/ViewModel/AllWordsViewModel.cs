using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GalaSoft.MvvmLight.CommandWpf;
using DictionaryUI.View;

namespace DictionaryUI.ViewModel
{
    public class AllWordsViewModel : ViewModelBase
    {
        private LearnDictionaryEntities efContext = null;
        private IOpenViewService openViewService;
        private IDictionaryDataService dictionaryDataService;
        private ILogService logService;
        
        private ObservableCollection<Language> languages = new ObservableCollection<Language>();
        private ObservableCollection<Book> books = new ObservableCollection<Book>();
        private ObservableCollection<Word> words = new ObservableCollection<Word>();
        private ObservableCollection<WordEntry> wordEntries = new ObservableCollection<WordEntry>();
        private List<string> sorting = new List<string>() { "None", "Ascending", "Descending" };
        private string selectedSortingItem;
        public int LastSessionPage { get; set; }

        public RelayCommand SelectBookCommand { get; private set; }
        public RelayCommand SelectLanguageCommand { get; private set; }
        public RelayCommand SelectSortingCommand { get; private set; }
        public RelayCommand AddNewWordCommand { get; private set; }
        public RelayCommand EditNewWordCommand { get; private set; }
        public RelayCommand DeleteWordCommand { get; private set; }
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            { Set(() => Languages, ref languages, value); }
        }

        public ObservableCollection<Book> Books
        {
            get { return books; }
            set
            { Set(() => Books, ref books, value); }
        }

        public ObservableCollection<Word> Words
        {
            get { return words; }
            set
            { Set(() => Words, ref words, value); }
        }

        public ObservableCollection<WordEntry> WordEntries
        {
            get { return wordEntries; }
            set
            { Set(() => WordEntries, ref wordEntries, value); }
        }

        private List<WordEntry> bookWordEntries;
        public List<WordEntry> BookWordEntries
        {
            get { return bookWordEntries; }
            set
            { Set(() => BookWordEntries, ref bookWordEntries, value);
            RaisePropertyChanged(() => BookWordEntriesCount);
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

        private Book selectedBook;
        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                Set(() => SelectedBook, ref selectedBook, value);
            }
        }

        private WordEntry selectedTest;
        public WordEntry SelectedTest
        {
            get { return selectedTest; }
            set
            {
                Set(() => SelectedTest, ref selectedTest, value);
            }
        }

        public string BookWordEntriesCount {
            get { return BookWordEntries == null ? "0" : BookWordEntries.Count.ToString(); }
            set { RaisePropertyChanged(() => BookWordEntriesCount); } 
        }

        public List<string> SortingItems
        {
            get { return sorting; }
            set { sorting = value; }
        }

        public string SelectedSortingItem
        {
            get { return selectedSortingItem; }
            set { Set(() => SelectedSortingItem, ref selectedSortingItem, value); }
        }

        public AllWordsViewModel(IOpenViewService openViewService , IDictionaryDataService dictionaryDataService, ILogService logService)
        {
            this.openViewService = openViewService;
            this.dictionaryDataService = dictionaryDataService;
            this.logService = logService;
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            efContext.Languages.Load();
            efContext.Books.Load();
            //efContext.WordEntries.Load();
            efContext.Words.Include("WordEntries").Load();
            Languages = efContext.Languages.Local;
            Books = efContext.Books.Local;
            Words = efContext.Words.Local;
            //WordEntries = efContext.WordEntries.Local;
            SelectedSortingItem = SortingItems[0];
            FilterWords();
            SelectBookCommand = new RelayCommand(FilterWords);
            SelectLanguageCommand = new RelayCommand(FilterWords);
            SelectSortingCommand = new RelayCommand(FilterWords);
            AddNewWordCommand = new RelayCommand(AddNewWord);
            EditNewWordCommand = new RelayCommand(EditNewWord);
            DeleteWordCommand = new RelayCommand(DeleteWord);
        }

        private void FilterWords()
        {
            if (selectedBook == null)
                return;           
            WordEntries = efContext.WordEntries.Local;
            var we = from w in WordEntries select w;

            if (SelectedBook != null)
                we = from w in we where w.Book_ID == SelectedBook.Book_ID select w;

            if (SelectedLanguage != null)
                we = from w in we where w.Word.Language_ID == SelectedLanguage.Language_ID select w;

            int orderBy = SortingItems.IndexOf(SortingItems.FirstOrDefault(z => z == SelectedSortingItem));
            switch (orderBy)
            {
                case 0:
                    BookWordEntries = we.OrderBy(z => z.Page).ToList();
                    break;
                case 1:
                    BookWordEntries = we.OrderBy(z => z.Word.Value).ToList();
                    break;
                case 2:
                    BookWordEntries = we.OrderByDescending(z => z.Word.Value).ToList();
                    break;
                default:
                    BookWordEntries = we.OrderBy(z => z.Word_ID).ToList();
                    break;
            }
        }
         
        private void AddNewWord()
        {
            WordBrowserViewModel wordsBrowserVM = new WordBrowserViewModel(dictionaryDataService, logService);
            wordsBrowserVM.SelectedBook = SelectedBook ?? null;
            wordsBrowserVM.SelectedLanguage = SelectedLanguage ?? null;
            wordsBrowserVM.EFContext = efContext;
            //wordsBrowserVM.EditWord(selectedTest);
            wordsBrowserVM.PreloadEntries();
            openViewService.OpenWordBrowserView(wordsBrowserVM);
        }
        
        private void EditNewWord()
        {
            WordBrowserViewModel wordsBrowserVM = new WordBrowserViewModel(dictionaryDataService, logService);
            wordsBrowserVM.SelectedBook = SelectedBook ?? null;
            wordsBrowserVM.SelectedLanguage = SelectedLanguage ?? null;
            wordsBrowserVM.EFContext = efContext;
            //wordsBrowserVM.EditWord(selectedTest);
            wordsBrowserVM.PreloadEntries();
            openViewService.OpenWordBrowserView(wordsBrowserVM);
            //FilterWords();
        }

        private void DeleteWord()
        {     
            //if (logService.GetConfirmation("Do you really want to delete this WordEntry?", "Delete WordEntry"))
            //    return;
            //WordEntry currentWordEntry = tvWords.SelectedItem as WordEntry;
            //foreach (var wm in currentWordEntry.WordMeanings.ToList())
            //{
            //    currentWordEntry.WordMeanings.Remove(wm);
            //}
            //efContext.WordEntries.Remove(currentWordEntry);
            //efContext.SaveChanges();
            //FilterWords();
        }
    }
}
