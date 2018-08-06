using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Controls;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using System.Speech.Synthesis;
using DictionaryUI.View;

namespace DictionaryUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LearnCardViewModel : ViewModelBase
    {
        private LearnDictionaryEntities efContext = null;
        private IDictionaryDataService dictionaryDataService;

        private ObservableCollection<WordMeaning> wordMeanings = new ObservableCollection<WordMeaning>();
        private ObservableCollection<Word> words = new ObservableCollection<Word>();
        private ObservableCollection<Language> languages = new ObservableCollection<Language>();
        private ObservableCollection<Word> chosenWordMeanings = new ObservableCollection<Word>();
        private bool checkedReverse;
        private bool nextEnabled;
        private bool showAnswerEnabled;
        private bool wordMeaningsEnabled;
        private List<Word> learningWords;
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
        private WordMeaningStatistic dsStatistic = null;
        //private Word _learningWord;
        //public delegate Word GetNextWordDelegate();
        //public delegate void WordResultDelegate(Word word, bool result);
        //public GetNextWordDelegate GetNextWord;
        //public WordResultDelegate WordResult;
        Random random = new Random();
        private bool _speakWorb = true;
        private Word TranslateWord { get; set; }



        public ObservableCollection<Word> Words
        {
            get { return words; }
            set
            { Set(() => Words, ref words, value); }
        }
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
                    SelectedLearningWord = numerator.MoveNext() ? numerator.Current : null;
                }
                else
                {
                    NextEnabled = false;
                    numerator = new List<Word>.Enumerator(); 
                    SelectedLearningWord = null;
                }
                RaisePropertyChanged(() => LearningWords);
            }
        }

        public Word SelectedLearningWord
        {
            get { return selectedLearningWord; }
            set
            { Set(() => SelectedLearningWord, ref selectedLearningWord, value); }
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
        public LearnCardViewModel(IDictionaryDataService dictionaryDataService)
        {
            this.dictionaryDataService = dictionaryDataService;
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            efContext.Words.Load();
            efContext.WordMeanings.Load();
            efContext.Languages.Load();
            Words = efContext.Words.Local;
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
            var wordMeanings = efContext.WordMeanings.Where(z => z.MasterWord_ID == SelectedLearningWord.Word_ID).ToList();
            //if (wordMeanings.Count() == 0)
            //{
            //    btnNext_Click(null, null);
            //    return;
            //}

            int indexTranslateWord = random.Next(0, wordMeanings.Count());
            _wordMeaningRow = wordMeanings.ElementAt(indexTranslateWord);
            TranslateWord = _wordMeaningRow.Word1;

            Word baseWord = !(bool)CheckedReverse ? TranslateWord : SelectedLearningWord;
            //Word secondWord = direct ?_learningWord : TranslateWord;
            SelectedLearningWord = !(bool)CheckedReverse ? SelectedLearningWord : TranslateWord;
            var langId = (!(bool)CheckedReverse ? TranslateWord : SelectedLearningWord).Language_ID;
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

            Word baseWord = !(bool)CheckedReverse ? TranslateWord : SelectedLearningWord;
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
                sth.Speak(SelectedLearningWord.Value);
                 
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
            SelectedLearningWord = numerator.Current; 
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
    }
}