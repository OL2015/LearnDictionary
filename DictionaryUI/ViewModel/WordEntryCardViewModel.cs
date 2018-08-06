using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Model;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace DictionaryUI.ViewModel
{
    using System.Collections.Generic;
    using System.IO;
    using WpfControls.Editors;
    using System.Linq;
    using System.Threading;
    using System.Windows.Controls;
    public class DictionaryLookupNotificationMessage : NotificationMessage
    {
        public DictionaryLookupNotificationMessage(string notification) : base(notification) { }
        public DictionaryLookupNotificationMessage(object sender, string notification) : base(sender, notification) { }
        public DictionaryLookupNotificationMessage(object sender, object target, string notification) : base(sender, target, notification) { }
        public Guid Token { get; set; }
        public TranslationItem TtanslationItm { get; set; }
    }
    public class SetInputNotificationMessage : NotificationMessage
    {
        public SetInputNotificationMessage(string notification) : base(notification) { }
        public SetInputNotificationMessage(object sender, string notification) : base(sender, notification) { }
        public SetInputNotificationMessage(object sender, object target, string notification) : base(sender, target, notification) { }
        public string Language { get; set; }
        public string WhatControl { get; set; } = "";
        public Guid Token { get; set; }
        public bool GotFocus { get; set; }
    }
    public class RefreshViewNotificationMessage : NotificationMessage
    {
        public RefreshViewNotificationMessage(string notification) : base(notification) { }
        public RefreshViewNotificationMessage(object sender, string notification) : base(sender, notification) { }
        public RefreshViewNotificationMessage(object sender, object target, string notification) : base(sender, target, notification) { }
        public Guid Token { get; set; }
    }

    public class WordValuesSuggestionProvider : ISuggestionProvider
    {
        public LearnDictionaryEntities EFContext { private get; set; }

        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            if (string.IsNullOrEmpty(filter) || filter.Length < 3)
                return null;
            List<Word> lst = EFContext.Words.Where(z => z.Value.StartsWith(filter)).Take(12).ToList();
            return lst;
        }

    }

    public class WordEntryCardViewModel : ViewModelBase
    {
       // public RelayCommand AddMeaningCommand { get; private set; }
        public RelayCommand AddNewMeaningCommand { get; private set; }
        public RelayCommand AutoCompleteLostFocusCommand { get; private set; }
        public RelayCommand RemoveMeaningCommand { get; private set; }
        public RelayCommand SaveWordCommand { get; private set; }
        public RelayCommand SpeakCommand { get; private set; }
        public RelayCommand CancelWordCommand { get; private set; }
        public RelayCommand ShowMariamWebsterCommand { get; private set; }
        public RelayCommand<bool> MeaningGotFocusCommand { get; private set; }
        public RelayCommand<bool> WordGotFocusCommand { get; private set; }
        public RelayCommand LookUpWordCommand { get; private set; }


        public TranslationLookUpViewModel TranslationVM { get; private set; }
        private ILogService logService;
        private LearnDictionaryEntities efContext = null;
        private IDictionaryDataService dictionaryDataService;

        private ObservableCollection<Book> books = new ObservableCollection<Book>();
        private ObservableCollection<Language> languages = new ObservableCollection<Language>();

        public WordValuesSuggestionProvider WordSugesstions
        { get; private set; }

         
        public Guid Token { get; private set; } = Guid.NewGuid();
        private string enteredText;
        public string EnteredText
        {
            get { return enteredText; }
            set
            {
                Set(() => EnteredText, ref enteredText, value);
            }
        }

        public String Title
        {
            get
            {
                if (wordEntry == null || WordEntry.Word == null || String.IsNullOrEmpty(WordEntry.Word.Value))
                    return "<New Word>";
                return WordEntry.Word.Value;
            }
        }

        private WordEntry wordEntry = null;
        public WordEntry WordEntry
        {
            get { return wordEntry; }
            set
            {
                if (value == null)
                    return;
                Set(() => WordEntry, ref wordEntry, value);
                SuggestedWord = WordEntry.Word; 
                FillAllWordMeanings();
                RaisePropertyChanged("Title");
            }
        }

        private void TryEnterNewWord()
        {
            if (string.IsNullOrEmpty(EnteredText))
                return;
            if (WordEntry.Word == null) // create new word
            {
                Word w = new Word() { Value = EnteredText, Language = SelectedLanguage };
                WordEntry.Word = w;
                RaisePropertyChanged("WordEntry");
                RaisePropertyChanged("Title");
            }
        }

        private Word suggestedWord = null;
        public Word SuggestedWord
        {
            get { return suggestedWord; }
            set
            {
                if (value == null)
                {
                    // TryEnterNewWord();
                    return;
                }
                Set(() => SuggestedWord, ref suggestedWord, value);
                WordEntry.Word = SuggestedWord;
                FillAllWordMeanings();
                RaisePropertyChanged("Title");
            }
        }

        //private Word word = null;
        ObservableCollection<WordMeaning> wordMeanings = new ObservableCollection<WordMeaning>();
        List<WordMeaning> allMeanings = new List<WordMeaning>();


        // public RelayCommand SelectBookCommand { get; private set; }

        public ObservableCollection<Book> Books
        {
            get { return books; }
            set
            {
                Set(() => Books, ref books, value);
            }
        }

        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                Set(() => Languages, ref languages, value);
            }
        }

        public ObservableCollection<WordMeaning> WordMeanings
        {
            get { return wordMeanings; }
            set
            {
                Set(() => WordMeanings, ref wordMeanings, value);
                // RefreshWordTranslation();
            }
        }

        public List<WordMeaning> AllMeanings
        {
            get { return allMeanings; }
            set
            {
                Set(() => AllMeanings, ref allMeanings, value);

            }
        }
        private WordMeaning selectedFromAllMeaning;
        public WordMeaning SelectedFromAllMeaning
        {
            get { return selectedFromAllMeaning; }
            set
            {
                Set(() => SelectedFromAllMeaning, ref selectedFromAllMeaning, value);
            }
        }
        private WordMeaning selectedMeaning;
        public WordMeaning SelectedMeaning
        {
            get { return selectedMeaning; }
            set
            {
                Set(() => SelectedMeaning, ref selectedMeaning, value);
            }
        }
        
        private int entriesCount;
        public int EntriesCount
        {
            get { return entriesCount; }
            set
            {
                Set(() => EntriesCount, ref entriesCount, value);
            }
        }
        private string meaningText;
        public string MeaningText
        {
            get { return meaningText; }
            set
            {
                Set(() => MeaningText, ref meaningText, value);
            }
        }
        public Book SelectedBook
        {
            get
            {
                if (wordEntry != null && wordEntry.Book != null)
                    return wordEntry.Book;
                //Books.FirstOrDefault (z=> z.Book_ID== wordEntry.Book.Book_ID);
                return null;
            }
            set
            {
                //Debug.Assert(this.Books.Contains(value));
                if (wordEntry != null)
                    wordEntry.Book = value;
                RaisePropertyChanged("SelectedBook");
            }
        }

        Language selectedLanguage;
        public Language SelectedLanguage
        {
            get
            {
                if (wordEntry != null && wordEntry.Word != null)
                    selectedLanguage = wordEntry.Word.Language;
                //return Languages.FirstOrDefault(z => z.Language_ID == wordEntry.Word.Language_ID);
                return selectedLanguage;
            }
            set
            {
                if (wordEntry != null && wordEntry.Word != null)
                    wordEntry.Word.Language = value;
                Set(() => SelectedLanguage, ref selectedLanguage, value);
            }
        }
        Language wordMeaningLanguage;
        public Language WordMeaningLanguage
        {
            get
            {
                return wordMeaningLanguage;
            }
            set
            {
                Set(() => WordMeaningLanguage, ref wordMeaningLanguage, value);
            }
        }

        public bool CanEnterWord
        {
            get
            {
                return WordEntry.Word == null;
            }
        }
        /// <summary>
        /// Initializes a new instance of the WordEntryCardViewModel class.
        /// </summary>
        public WordEntryCardViewModel(IDictionaryDataService dictionaryDataService, ILogService dlogService, ITranslationService translationService)
        {
            this.logService = dlogService;
            TranslationVM = new TranslationLookUpViewModel(dlogService, translationService);
           // AddMeaningCommand = new RelayCommand(AddMeaning);
            AddNewMeaningCommand = new RelayCommand(AddNewMeaning);
            RemoveMeaningCommand = new RelayCommand(RemoveMeaning);
            SaveWordCommand = new RelayCommand(SaveChanges);
            SpeakCommand = new RelayCommand(Speak);
            ShowMariamWebsterCommand = new RelayCommand(ShowMariamWebster,
                () => { return WordEntry != null && WordEntry.Word != null; });
            CancelWordCommand = new RelayCommand(DeleteWord, () => { return WordEntry != null; });
            MeaningGotFocusCommand = new RelayCommand<bool>((p) =>
            {
                Messenger.Default.Send(
                new SetInputNotificationMessage(this, "") { Language = this.WordMeaningLanguage.ShortName, Token = this.Token, WhatControl = "Meaning", GotFocus = p });
            });

            WordGotFocusCommand = new RelayCommand<bool>((p) =>
            {
                Messenger.Default.Send(
                new SetInputNotificationMessage(this, "") { Language = this.SelectedLanguage.ShortName, Token = this.Token, WhatControl = "Word", GotFocus = p });
                Messenger.Default.Send(
                new SetInputNotificationMessage(this, "") { Language = this.SelectedLanguage.ShortName, Token = this.Token, WhatControl = "Word", GotFocus = p });

            });

            AutoCompleteLostFocusCommand = new RelayCommand(TryEnterNewWord);
            LookUpWordCommand = new RelayCommand(LookUpWord);
            this.dictionaryDataService = dictionaryDataService;
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            Messenger.Default.Register<DictionaryLookupNotificationMessage>(
            this,
            message =>
            {
                if (message.Token == this.TranslationVM.Token)
                {
                    string tr = message.TtanslationItm.Translation;
                     AppendNewMeaning(tr );
                }
            });


            efContext.Books.Load();
            efContext.Languages.Load();
            Books = efContext.Books.Local;
            Languages = efContext.Languages.Local;
            SelectedLanguage = Languages[0];
            WordMeaningLanguage = Languages[1];
            WordSugesstions = new WordValuesSuggestionProvider();
            WordSugesstions.EFContext = efContext;
        }

        private void LookUpWord()
        {

            TranslationVM.BaseWord = WordEntry.Word.Value;
            TranslationVM.FromLang = this.SelectedLanguage.ShortName;
            TranslationVM.ToLang = this.WordMeaningLanguage.ShortName;
            TranslationVM.DoTranslation();
        }

        private void ShowMariamWebster()
        {
            dictionaryDataService.ShowMariamWebster(WordEntry.Word.Value);  
        }

        public void DeleteWord()
        {
            try
            {
                if (WordEntry == null || WordEntry.Word == null)
                    return;

                //var we = efContext.Entry(WordEntry.Word);
                efContext.WordMeanings.RemoveRange(WordEntry.Word.WordMeanings);
                efContext.SaveChanges();
                //var wee = efContext.Entry(WordEntry);
                var w = WordEntry.Word;
                efContext.WordEntries.Remove(WordEntry);
                efContext.SaveChanges();
                if (w.WordEntries.Count == 0)
                {
                    efContext.Words.Remove(w);
                    efContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AddNewMeaning()
        {
            try { 
            string newMeaning = MeaningText.Trim();
            if (string.IsNullOrEmpty(newMeaning))
                return;
            // todo rewrite Speach part detection 

            AppendNewMeaning(newMeaning );
            MeaningText = "";
            }
            catch (Exception ex)
            {
               // throw;
            }
        }

        private void CheckOrCreateNewWord()
        {
            if (WordEntry.Word != null)
                return;
            if (string.IsNullOrEmpty(EnteredText))
                return;
            Word w = new Word() { Language = SelectedLanguage, Value = EnteredText };
            WordEntry.Word = w;
        }
        private void Speak()
        {
            try
            {
                Task.Run(
                    () =>
                    {
                        using (SpeechSynthesizer sth = new SpeechSynthesizer())
                        {
                            var voices = sth.GetInstalledVoices();
                            Random random = new Random();
                            int ind = random.Next(0, voices.Count);
                            sth.SelectVoice(voices[ind].VoiceInfo.Name);
                            sth.Speak(WordEntry.Word.Value);
                        }
                        //dictionaryDataService.ShowMariamWebster(WordEntry.Word.Value);
                    }
                );
            }
            catch (Exception) { };
        }

        //private void AddMeaning()
        //{
        //    zzz
        //    if (SelectedFromAllMeaning != null)
        //    {
        //        try
        //        {
        //            if (!WordEntry.WordMeanings.Any(z => z == SelectedFromAllMeaning))
        //            {
        //                WordEntry.WordMeanings.Add(SelectedFromAllMeaning);
        //                // Below is the hack to say that WordEntry.WordMeanings changed
        //                var x = WordEntry; WordEntry = null; WordEntry = x;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //MessageBox.Show(ex.ToString(), "Row is already in meaning list.");
        //        }
        //        RefreshWordTranslation();
        //    }
        //}

        private void RemoveMeaning()
        {
            if (SelectedMeaning != null)
            {
                try
                {
                    WordEntry.Word.WordMeanings.Remove(SelectedMeaning);
                    // Below is the hack to say that WordEntry.WordMeanings changed
                    var x = WordEntry; WordEntry = null; WordEntry = x;
                    //WordEntry.Word.Archived = false;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString(), "Row is already in meaning list.");
                }
                //  RefreshWordTranslation();
            }
        }

        private void AppendNewMeaning(string newMeaning )
        {
            CheckOrCreateNewWord();
            Word newWord = efContext.Words.Where(z => z.Value.ToLower() == newMeaning.ToLower()).FirstOrDefault();
            newWord = newWord ?? new Word() { Value = newMeaning, Language = WordMeaningLanguage }; //  
            if (!wordEntry.Word.WordMeanings.Any(z => z.Word1.Value == newMeaning)) // and speachpart
            {
                WordMeaning wm = new WordMeaning() { Word = WordEntry.Word, Word1 = newWord };
                
                wordEntry.Word.WordMeanings.Add(wm);
            }
            //newWord.Archived = false;
            RefreshWordTranslation();
            FillAllWordMeanings();
        }

        private void SaveChanges()
        {
            try
            {
                CheckOrCreateNewWord();
                var we = efContext.Entry(WordEntry.Word);
                if (we.State == System.Data.Entity.EntityState.Detached)
                {
                    efContext.Words.Add(WordEntry.Word);
                    efContext.SaveChanges();
                }
                foreach (var wm in WordEntry.Word.WordMeanings)
                {
                    var wme = efContext.Entry(wm);
                    if (wme.State == System.Data.Entity.EntityState.Detached)
                    {
                        efContext.WordMeanings.Add(wm);
                        efContext.SaveChanges();
                    }
                }
                var wee = efContext.Entry(WordEntry);
                if (wee.State == System.Data.Entity.EntityState.Detached)
                    efContext.WordEntries.Add(WordEntry); 
                efContext.SaveChanges();
            }
            catch (Exception ex)
            { 
                WordEntry = null;
                 logService.ShowException("Error saving changes", ex);
            }
        }

        private void RefreshWordTranslation()
        {
            Messenger.Default.Send(
                   new RefreshViewNotificationMessage(this, "") { Token = this.Token });
            //return;
            if (wordEntry.Word.WordMeanings.Count != 0)
            {
                WordMeanings = new ObservableCollection<WordMeaning>(wordEntry.Word.WordMeanings.Select(z => z));
            }
            else
                WordMeanings = null;
        }

        private void FillAllWordMeanings()
        {
            if (WordEntry != null && WordEntry.Word != null)
            {
                AllMeanings = WordEntry.Word.WordMeanings.ToList();
                EntriesCount = WordEntry.Word.WordEntries.Count; 
            }
            
        }

    }
}