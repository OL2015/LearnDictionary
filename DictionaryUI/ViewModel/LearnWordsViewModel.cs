using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Services;
using DictionaryUI.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Speech.Synthesis;

namespace DictionaryUI.ViewModel
{
    public class LearningStrategy
    {
        public Strategy ID { get; set; }
        public string Name { get; set; }
    }
    public enum Strategy
    {
        all,
        highRating,
        lowRating,
        older,
        newest,
        twiceormore,
        morethentwice
    }

    /// <summary>
    /// This class contains properties that a View can data bind to. 
    /// The same instance of this class is used by LearnWordsView and LearningCardView
    /// </summary>
    public class LearnWordsViewModel : ViewModelBase
    {
        private const int toLearCnt = 12;
        private const int maxWords = 15;
        Random random = new Random();

        private LearnDictionaryEntities efContext = null;
        private IDictionaryDataService dictionaryDataService;
        private IOpenViewService openViewService;
        private ILogService logService;
        private List<Word> learningWords;
        private ObservableCollection<LearningStrategy> _learningStrategies = null;

        private LearningStrategy _selectedStrategy;
        private ObservableCollection<Book> books = new ObservableCollection<Book>();
        private ObservableCollection<Language> languagies = new ObservableCollection<Language>();
        private ObservableCollection<Word> wordsAll = null; // 
        private ObservableCollection<Word> wordsToLearn = null; // 
        private Language selectedLanguage;
        private Book selectedBook;

        //https://www.merriam-webster.com/dictionary/defy?pronunciation&lang=en_us&dir=d&file=defy0001


        public RelayCommand FillCandidatesCommand { get; private set; } 
        public RelayCommand StartLearningCommand { get; private set; }
        public RelayCommand ShowMariamWebsterCommand { get; private set; }
        public RelayCommand <Word> OpenWordCardCommand { get; private set; }

         
        public bool ActiveOnly { get; set; }
        public ObservableCollection<LearningStrategy> LearningStrategies
        {
            get { return _learningStrategies; }
            set
            {
                _learningStrategies = value;
                RaisePropertyChanged("LearningStrategies");
            }
        }

        public LearningStrategy SelectedStrategy
        {
            get { return _selectedStrategy; }
            set
            {
                _selectedStrategy = value;
                RaisePropertyChanged("SelectedStrategy");
            }
        }
        
        public ObservableCollection<Book> Books
        {
            get { return books; }
            set
            {
                books = value;
                RaisePropertyChanged("Books");
            }
        }

        public ObservableCollection<Language> Languagies
        {
            get { return languagies; }
            set
            {
                languagies = value;
                RaisePropertyChanged("Languagies");
            }
        }

        public Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                RaisePropertyChanged("SelectedLanguage");
            }
        }

        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                RaisePropertyChanged("SelectedBook");
            }
        } 

        public ObservableCollection<Word> WordsAll
        {
            get { return wordsAll; }
            set
            {
                wordsAll = value;
                RaisePropertyChanged("WordsAll");
            }
        }

        public ObservableCollection<Word> WordsToLearn
        {
            get { return wordsToLearn; }
            set
            {
                wordsToLearn = value;
                RaisePropertyChanged("WordsToLearn");
            }
        }
        
        public LearnWordsViewModel(IDictionaryDataService dictionaryDataService, IOpenViewService openViewService, ILogService logService)
        {
            this.dictionaryDataService = dictionaryDataService;
            this.openViewService = openViewService;
            this.logService = logService;
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            efContext.Books.Load();
            efContext.Languages.Load();
            Books = efContext.Books.Local;
            Languagies = efContext.Languages.Local;
            SelectedLanguage = Languagies[0];
            FillCandidatesCommand = new RelayCommand(FillAllWords); 
            //LearnCommand = new RelayCommand(LearnWords, CanLearnWords);
            StartLearningCommand = new RelayCommand(StartLearning, () => WordsToLearn != null && WordsToLearn.Count > 0);
            ShowMariamWebsterCommand = new RelayCommand(ShowMariamWebster);
            OpenWordCardCommand = new RelayCommand<Word>(
                        w =>
                        {
                            OpenWordCard(w);
                        });
            FillStrategies();
        }

        private void OpenWordCard(Word w)
        {
            WordCardViewModel vm = new WordCardViewModel(dictionaryDataService, logService);
            vm.Word = w;
            openViewService.OpenWordCard(vm) ;
        }

        private void ShowMariamWebster()
        {
            dictionaryDataService.ShowMariamWebster(SelectedLWord.Value);
        }

        private Action<NotificationMessage<Book>> LoadBookContext()
        {
            return message =>
            {
                SelectedBook = message.Content;
                efContext.Words.Load();
                WordsAll = efContext.Words.Local;
            };
        }

        private void StartLearning()
        {
            openViewService.OpenLearnCard(this);
            //Messenger.Default.Send(new NotificationMessage<List<Word>>(LearningWords, "")); 
        }

        private void FillStrategies()
        {
            _learningStrategies = new ObservableCollection<LearningStrategy>(
                new LearningStrategy[]{ 
                    new LearningStrategy {ID = Strategy.all,  Name = "All"}, 
                    new LearningStrategy {ID = Strategy.highRating,  Name = "High rating"},
                    new LearningStrategy {ID = Strategy.lowRating,  Name = "Low rating"},
                    new LearningStrategy {ID = Strategy.older,  Name = "Older"},
                    new LearningStrategy {ID = Strategy.newest,  Name = "Newest"},
                    new LearningStrategy {ID = Strategy.twiceormore,  Name = "More then once"},
                    new LearningStrategy {ID = Strategy.morethentwice,  Name = "Three or more"},
            });
            LearningStrategies = _learningStrategies;
            SelectedStrategy = _learningStrategies[4];
        }

        private void FillAllWords()
        {
            var query =
                        from word in efContext.Words
                        select word;
            if (SelectedLanguage != null)
                query = query.Where(z => z.Language_ID == SelectedLanguage.Language_ID);
            if (SelectedBook != null)
                query = query.Where(z => z.WordEntries.Any(w => w.Book_ID == SelectedBook.Book_ID));
            //if (ActiveOnly)
            //    query = query.Where(z => !z.Archived.HasValue || !z.Archived.Value);
            WordsAll = new ObservableCollection<Word>(query.ToList());
            WordsToLearn = new ObservableCollection<Word>( GetRandomLearningWords());
            
        }

        private bool CanLearnWords()
        {
            return WordsAll != null && WordsAll.Count > 0;
        }

        
    #region "Word Selector"

        private List<Word> GetRandomLearningWords()
        {
            List<Word> wordsToLearn = new List<Word>();
            switch (SelectedStrategy.ID)
            {
                case Strategy.all:
                    wordsToLearn = PureRandom();
                    break;
                case Strategy.older:
                    wordsToLearn = OlderRandom();
                    break;
                case Strategy.newest:
                    wordsToLearn = NewerRandom();
                    break;
                case Strategy.lowRating:
                    wordsToLearn = LowRatingRandom();
                    break;
                case Strategy.twiceormore:
                    wordsToLearn = TwiceOrMore();
                    break;
                case Strategy.morethentwice:
                    wordsToLearn = MoreThenTwice();
                    break;

                default:
                    break;
            }
            return wordsToLearn;
        }
        private List<Word> PureRandom()
        {
            List<Word> words = new List<Word>();
            var wordEntries = WordsAll.SelectMany(z => z.WordEntries);
            words = (from w in WordsAll
                     where w.Rating <= 81 
                     select w).Take(maxWords).ToList();
            //words = data.Take(maxWords).ToList();
            List<Word> rndChosenWords = words.OrderBy(z => random.Next()).Take(toLearCnt).ToList();
            return rndChosenWords; ;
        }

        private List<Word> MoreThenTwice()
        {
            List<Word> words = new List<Word>();
            var wordEntries = WordsAll.SelectMany(z => z.WordEntries);
            words = (from w in WordsAll
                     where w.WordEntries.Count > 2
                     && w.Rating <= 89
                     orderby w.WordEntries.Count descending
                     select w).Take(maxWords).ToList();
            //words = data.Take(maxWords).ToList();
            List<Word> rndChosenWords = words.Take(toLearCnt).ToList();
            return rndChosenWords;
        }
        private List<Word> TwiceOrMore()
        {
            List<Word> words = new List<Word>();
            var wordEntries = WordsAll.SelectMany(z => z.WordEntries);
            words = (from w in WordsAll
                     where w.WordEntries.Count > 1
                     && w.Rating <= 89
                     orderby w.WordEntries.Count descending
                     select w).Take(maxWords).ToList();
            //words = data.Take(maxWords).ToList();
            List<Word> rndChosenWords = words.Take(toLearCnt).ToList();
            return rndChosenWords;
        }

        private List<Word> LowRatingRandom()
        {
            List<Word> words = new List<Word>();
            var wordEntries = WordsAll.SelectMany(z => z.WordEntries);
            words = (from w in WordsAll
                     where w.Rating <= 81
                     orderby w.Rating ascending
                     select w).Take(maxWords).ToList();
            //words = data.Take(maxWords).ToList();
            List<Word> rndChosenWords = words.OrderBy(z => random.Next()).Take(toLearCnt).ToList();
            return rndChosenWords; ;
        }

        private List<Word> NewerRandom()
        {
            List<Word> words = new List<Word>();
            var wordEntries = WordsAll.SelectMany(z => z.WordEntries);
            words = (from WordEntry wordEntry in wordEntries
                     where wordEntry.Word.Rating <= 89
                     orderby wordEntry.Date descending
                     group wordEntry by wordEntry.Word_ID into g
                     let result = g.FirstOrDefault()
                     select result.Word).Take(maxWords).ToList();
            //words = data.Take(maxWords).ToList();
            return words;
        }

        private List<Word> OlderRandom()
        {
            List<Word> words = new List<Word>();
            int attemptCount = 0;
            var wordEntries = WordsAll.SelectMany(z => z.WordEntries);
            words = (from WordEntry wordEntry in wordEntries
                     where wordEntry.Word.Rating <= 89
                     orderby wordEntry.Date ascending
                     group wordEntry by wordEntry.Word_ID into g
                     let result = g.FirstOrDefault()
                     select result.Word).Take(maxWords).ToList();
            //words = data.Take(maxWords).ToList();
            return words;
        }

        #endregion

        #region "Learn Session" 
        
        private int correctAnswer = 0; 
        public readonly Guid token = Guid.NewGuid(); 
        private List<Word> selectedWords; 
        public List<Word> SelectedWords
        {
            get { return selectedWords; }
            set { Set(() => SelectedWords, ref selectedWords, value); }
        } 

        private void WordResult(Word word, bool result)
        {
            if (result)
                ++correctAnswer;
        }
        public Word GetNextWd()
        {
            //if (_IWords.MoveNext())
            //{
            //    SelectedWord = _IWords.Current;
            //    return _IWords.Current;
            //}
            if (correctAnswer > 0)
                logService.ShowMessage("Well Done! Correct answers " + correctAnswer);
            else
                logService.ShowMessage("Dull! No correct answers ");
            return null;
        }
        #endregion

        #region "Learning Card"


        private ObservableCollection<WordMeaning> wordMeanings = new ObservableCollection<WordMeaning>();
        
        private ObservableCollection<Language> languages = new ObservableCollection<Language>();
        private ObservableCollection<Word> chosenWordMeanings = new ObservableCollection<Word>();
        private bool checkedReverse;
        private bool nextEnabled;
        private bool showAnswerEnabled;
        private bool wordMeaningsEnabled;
         
        private List<Word>.Enumerator numerator;
        private Word chosenWordMeaning;
        private Word selectedLearningWord;

        public RelayCommand EditWordCommand { get; private set; }
        public RelayCommand ShowAnswerCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }
        public RelayCommand SpeakCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand WordMeaningsSelectionCommand { get; private set; } 
        WordMeaning _wordMeaningRow = null;
         
        private bool _speakWorb = true;
        private Word TranslateWord { get; set; }  
         
        public ObservableCollection<WordMeaning> WordMeanings
        {
            get { return wordMeanings; }
            set
            { Set(() => WordMeanings, ref wordMeanings, value); }
        }

        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            { Set(() => Languages, ref languages, value); }
        }

        public List<Word> LearningWords
        {
            get { return learningWords; }
            set
            {
                learningWords = value;
                if (learningWords != null)
                {
                    NextEnabled = learningWords.Count > 0;
                    numerator = learningWords.GetEnumerator();
                    SelectedLWord = numerator.MoveNext() ? numerator.Current : null;
                }
                else
                {
                    NextEnabled = false;
                    numerator = new List<Word>.Enumerator();
                    SelectedLWord = null;
                }
                RaisePropertyChanged(() => LearningWords);
            }
        }

        public Word SelectedLWord
        {
            get { return selectedLearningWord; }
            set
            { Set(() => SelectedLWord, ref selectedLearningWord, value); }
        }

        public bool CheckedReverse
        {
            get { return checkedReverse; }
            set { Set(() => CheckedReverse, ref checkedReverse, value); }
        }

        public bool NextEnabled
        {
            get { return nextEnabled; }
            set { Set(() => NextEnabled, ref nextEnabled, value); }
        }

        public bool ShowAnswerEnabled
        {
            get { return showAnswerEnabled; }
            set { Set(() => ShowAnswerEnabled, ref showAnswerEnabled, value); }
        }

        public bool WordMeaningsEnabled
        {
            get { return wordMeaningsEnabled; }
            set { Set(() => WordMeaningsEnabled, ref wordMeaningsEnabled, value); }
        }
        public ObservableCollection<Word> ChosenWordMeanings
        {
            get { return chosenWordMeanings; }
            set
            { Set(() => ChosenWordMeanings, ref chosenWordMeanings, value); }
        }

        public Word ChosenWordMeaning
        {
            get { return chosenWordMeaning; }
            set
            { Set(() => ChosenWordMeaning, ref chosenWordMeaning, value); }
        }

        public List<int> VariantAmount { get; set; }
        public int VariantAmountSelected { get; set; }


        /// <summary>
        /// Initializes a new instance of the LearnCardViewModel class.
        /// </summary>
        public void LearnCardViewModel(IDictionaryDataService dictionaryDataService)
        {
            this.dictionaryDataService = dictionaryDataService;
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            efContext.Words.Load();
            efContext.WordMeanings.Load();
            efContext.Languages.Load();
            WordsAll = efContext.Words.Local;
            WordMeanings = efContext.WordMeanings.Local;
            Languages = efContext.Languages.Local;
            Messenger.Default.Register<NotificationMessage<List<Word>>>(this, (m) =>
            {
                LearningWords = m.Content;
                FillTranslationAlternatives();
            });

            VariantAmount = new List<int>() { 1, 2, 3, 4, 5, 6 };
            VariantAmountSelected = 4;
            //FillTranslationAlternatives();
            EditWordCommand = new RelayCommand(EditWord, () => LearningWords != null && LearningWords.Count > 0);
            ShowAnswerCommand = new RelayCommand(ShowAnswer, () => ShowAnswerEnabled);
            NextCommand = new RelayCommand(NextWord, () => NextEnabled);
            SpeakCommand = new RelayCommand(Speak);
            RefreshCommand = new RelayCommand(Refresh);
            WordMeaningsSelectionCommand = new RelayCommand(WordMeaningsSelection);
        }

        private void FillTranslationAlternatives()
        {
            var wordMeanings = efContext.WordMeanings.Where(z => z.MasterWord_ID == SelectedLWord.Word_ID).ToList();
            //if (wordMeanings.Count() == 0)
            //{
            //    btnNext_Click(null, null);
            //    return;
            //}

            int indexTranslateWord = random.Next(0, wordMeanings.Count());
            _wordMeaningRow = wordMeanings.ElementAt(indexTranslateWord);
            TranslateWord = _wordMeaningRow.Word1;

            Word baseWord = !(bool)CheckedReverse ? TranslateWord : SelectedLWord;
            //Word secondWord = direct ?_learningWord : TranslateWord;
            SelectedLWord = !(bool)CheckedReverse ? SelectedLWord : TranslateWord;
            var langId = (!(bool)CheckedReverse ? TranslateWord : SelectedLWord).Language_ID;
            ICollection<Word> allWordsBySpeechPart;
            var allWordsByLang = efContext.Words.Where(z => z.Language_ID == baseWord.Language_ID);
            if (!(bool)CheckedReverse)
                allWordsBySpeechPart =
                     allWordsByLang.Where(z => z.WordMeanings1.Any(y => y.SpeechPart_ID == _wordMeaningRow.SpeechPart_ID)).ToList();
            else
                allWordsBySpeechPart =
                allWordsByLang.Where(z => z.WordMeanings.Any(y => y.SpeechPart_ID == _wordMeaningRow.SpeechPart_ID)).ToList();
            ObservableCollection<Word> chosenWords = randomAllWords(wordMeanings, allWordsBySpeechPart);
            chosenWords.Add(baseWord);
            var rndChosenWords = chosenWords.OrderBy(item => random.Next());
            ChosenWordMeanings = new ObservableCollection<Word>(rndChosenWords);
        }

        private ObservableCollection<Word> randomAllWords(ICollection<WordMeaning> wmRows, ICollection<Word> allWordsByLang)
        {
            ObservableCollection<Word> randomWords = new ObservableCollection<Word>();
            int attemptCount = 0;
            while (randomWords.Count < VariantAmountSelected && attemptCount++ < 10)
            {
                int candidate = random.Next(0, allWordsByLang.Count());
                var candidateRow = wmRows.SingleOrDefault(z => z.TranslateWord_ID == allWordsByLang.ElementAt(candidate).Word_ID) as WordMeaning;
                Word element = allWordsByLang.ElementAt(candidate);
                if (candidateRow == null && !randomWords.Contains(element))
                    randomWords.Add(element);
            }
            return randomWords;
        }
         
        private void ShowAnswer()
        {
            ShowRightAnswer();
            NextEnabled = true;
            ShowAnswerEnabled = false;
            WordMeaningsEnabled = false;
        }

        private bool ShowRightAnswer()
        {
            bool result = false;
            LearnStatistic newStatisticRow = new LearnStatistic();
            newStatisticRow.AttempTime = DateTime.Now;
            newStatisticRow.LearnDirection = true;
            newStatisticRow.WordMeaning_ID = _wordMeaningRow.WordMeaning_ID;

            Word baseWord = !(bool)CheckedReverse ? TranslateWord : SelectedLWord;
            //ListBoxItem correctItem = lvWordMeanings.ItemContainerGenerator.ContainerFromItem(baseWord) as ListBoxItem;
            //correctItem.Style = (Style)correctItem.FindResource("CorrectAnswer");
            newStatisticRow.Answer = false;
            if (ChosenWordMeaning != null)
            {
                if (ChosenWordMeaning.Word_ID != baseWord.Word_ID)
                {
                    //lvWordMeanings.UpdateLayout();
                    //ListBoxItem incorrectItem = lvWordMeanings.ItemContainerGenerator.ContainerFromItem(ChosenWordMeaning) as ListBoxItem;
                    //incorrectItem.Style = (Style)incorrectItem.FindResource("IncorrectAnswer");
                }
                else
                {
                    newStatisticRow.Answer = true;
                    result = true;
                }

            }

            efContext.LearnStatistics.Add(newStatisticRow);
            efContext.SaveChanges();
            //if (WordResult != null)
            //    WordResult(_wordMeaningRow.Word as Word, result);
            return result;
        }

        private void Refresh()
        {
            //lvWordMeanings.Items.Clear();
            FillTranslationAlternatives();
        }

        private void Speak()
        {
            using (SpeechSynthesizer sth = new SpeechSynthesizer())
            {
                var voices = sth.GetInstalledVoices();
                Random random = new Random();
                int ind = random.Next(0, voices.Count);
                sth.SelectVoice(voices[ind].VoiceInfo.Name);
                sth.Speak(SelectedLWord.Value);

                //string z ="The next part that doesn't just use default pronunciation is the date. We use the special SayAs enumeration to specify that the date should be read out as an actual date and not just a set of numbers, spaces and special characters.";
                // sth.Speak(z);
            }
        }

        private void NextWord()
        {
            if (!numerator.MoveNext())
            {
                NextEnabled = false;
                return;
            }
            SelectedLWord = numerator.Current;
            WordMeaningsEnabled = true;
            ShowAnswerEnabled = true;
            FillTranslationAlternatives();
        }

        private void WordMeaningsSelection()
        {
            //MessageBox.Show(e.OriginalSource.ToString());
            //if (e.AddedItems.Count == 0)
            //    return;
            bool res = ShowRightAnswer();
            NextEnabled = true;
            ShowAnswerEnabled = false;
            WordMeaningsEnabled = false;
            if (res)
            {
                if (_speakWorb)
                    Speak();
                NextWord();
            }
        }

        private void EditWord()
        {
            WordEntry wordEntry = _wordMeaningRow.Word.WordEntries.LastOrDefault();
            if (wordEntry == null)
                return;
            WordBrowserView wordWindow = new WordBrowserView();
            //wordWindow.Init(efContext, wordEntry, null, 0);
            wordWindow.ShowDialog();
            if (wordWindow.DialogResult.HasValue)
                FillTranslationAlternatives();
        }
        #endregion
    }

}


 