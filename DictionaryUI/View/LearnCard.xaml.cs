using DictionaryLogic;
using DictionaryLogic.ModelProviders.EFModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for LearnCard.xaml
    /// </summary>
    public partial class LearnCard : Window
    {
        //LearnDictionaryEntities _context = null;
        //DictionaryFacade facade = null;
        //WordMeaning _wordMeaningRow = null;
        //private WordMeaningStatistic dsStatistic = null;
        //private Word _learningWord;
        //public delegate Word GetNextWordDelegate();
        //public delegate void WordResultDelegate(Word word, bool result);
        //public GetNextWordDelegate GetNextWord;
        //public WordResultDelegate WordResult;
        //Random random = new Random();
        //private bool _speakWorb = true;
        //public bool IsReverse { get; set; }
        //private Word TranslateWord { get; set; }

        public LearnCard()
        {
            InitializeComponent();
            //IsReverse = false;
            //ExtractContext();
        }

        //public LearnCard(Word word)
        //{
        //    InitializeComponent();
        //    IsReverse = false;
        //    ExtractContext();
        //    _learningWord = word;
        //    tbLearningWord.DataContext = _learningWord;
        //    FillComboboxValues();
        //    FillTranslationAlternatives();
        //}

        //private void ExtractContext()
        //{
        //    facade = DictionaryFacade.GetFacade();
        //    facade.ConnectionString = Properties.Settings.Default.ConnStr;
        //    _context = facade.GetEFLearnDictionaryContext("LearnDictionaryConnectionString");
        //    _context.Words.Load();
        //    _context.WordMeanings.Load();
        //    _context.Languages.Load();
        //}

        //private void FillComboboxValues()
        //{
        //    int j = 1;
        //    while (j < 6)
        //    {
        //        ComboBoxItem item = new ComboBoxItem();
        //        item.Content = j;
        //        if (j == 3)
        //            item.IsSelected = true;
        //        cbVariantAmount.Items.Add(item);
        //        j++;
        //    }
        //}

        //private void FillTranslationAlternatives()
        //{
        //    lvWordMeanings.ItemsSource = null;
        //    var wordMeanings = _context.WordMeanings.Where(z => z.MasterWord_ID == _learningWord.Word_ID).ToList();
        //    if (wordMeanings.Count() == 0)
        //    {
        //        btnNext_Click(null, null);
        //        return;
        //    }

        //    int indexTranslateWord = random.Next(0, wordMeanings.Count());
        //    _wordMeaningRow = wordMeanings.ElementAt(indexTranslateWord);
        //    TranslateWord = _wordMeaningRow.Word1;

        //    Word baseWord = !IsReverse ? TranslateWord : _learningWord;
        //    //Word secondWord = direct ?_learningWord : TranslateWord;
        //    tbLearningWord.DataContext = !IsReverse ? _learningWord : TranslateWord;
        //    var langId = (!IsReverse ? TranslateWord : _learningWord).Language_ID;
        //    ICollection<Word> allWordsBySpeechPart;
        //    var allWordsByLang = _context.Words.Where(z => z.Language_ID == baseWord.Language_ID);
        //    if (!IsReverse)
        //        allWordsBySpeechPart =
        //             allWordsByLang.Where(z => z.WordMeanings1.Any(y => y.SpeechPart_ID == _wordMeaningRow.SpeechPart_ID)).ToList();
        //    else
        //        allWordsBySpeechPart =
        //        allWordsByLang.Where(z => z.WordMeanings.Any(y => y.SpeechPart_ID == _wordMeaningRow.SpeechPart_ID)).ToList();
        //    List<Word> chosenWords = randomAllWords(wordMeanings, allWordsBySpeechPart);
        //    chosenWords.Add(baseWord);
        //    var rndChosenWords = chosenWords.OrderBy(item => random.Next());
        //    lvWordMeanings.ItemsSource = rndChosenWords;
        //    //if (_speakWorb) 
        //    //    btnSpeak_Click(null, null);
        //}

        //private List<Word> randomAllWords(ICollection<WordMeaning> wmRows, ICollection<Word> allWordsByLang)
        //{
        //    List<Word> randomWords = new List<Word>();
        //    int attemptCount = 0;
        //    int variants = Int32.Parse(cbVariantAmount.Text);
        //    while (randomWords.Count < variants && attemptCount++ < 10)
        //    {
        //        int candidate = random.Next(0, allWordsByLang.Count());
        //        var candidateRow = wmRows.SingleOrDefault(z => z.TranslateWord_ID == allWordsByLang.ElementAt(candidate).Word_ID) as WordMeaning;
        //        Word element = allWordsByLang.ElementAt(candidate);
        //        if (candidateRow == null && !randomWords.Contains(element))
        //            randomWords.Add(element);
        //    }
        //    return randomWords;
        //}

        //private void btnShowAnswer_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowRightAnswer();
        //    btnNext.IsEnabled = true;
        //    btnShowAnswer.IsEnabled = false;
        //    this.spWordMeanins.IsEnabled = false;
        //}

        //private bool ShowRightAnswer()
        //{
        //    bool result = false;
        //    Word selectedItem = lvWordMeanings.SelectedItem as Word;
        //    LearnStatistic newStatisticRow = new LearnStatistic();
        //    newStatisticRow.AttempTime = DateTime.Now;
        //    newStatisticRow.LearnDirection = true;
        //    newStatisticRow.WordMeaning_ID = _wordMeaningRow.WordMeaning_ID;

        //    Word baseWord = !IsReverse ? TranslateWord : _learningWord;
        //    ListBoxItem correctItem = lvWordMeanings.ItemContainerGenerator.ContainerFromItem(baseWord) as ListBoxItem;
        //    correctItem.Style = (Style)correctItem.FindResource("CorrectAnswer");
        //    newStatisticRow.Answer = false;
        //    if (selectedItem != null)
        //    {
        //        if (selectedItem.Word_ID != baseWord.Word_ID)
        //        {
        //            //lvWordMeanings.UpdateLayout();
        //            ListBoxItem incorrectItem = lvWordMeanings.ItemContainerGenerator.ContainerFromItem(selectedItem) as ListBoxItem;
        //            incorrectItem.Style = (Style)incorrectItem.FindResource("IncorrectAnswer");
        //        }
        //        else
        //        {
        //            newStatisticRow.Answer = true;
        //            result = true;
        //        }

        //    }

        //    _context.LearnStatistics.Add(newStatisticRow);
        //    _context.SaveChanges();
        //    if (WordResult != null)
        //        WordResult(_wordMeaningRow.Word as Word, result);
        //    return result;
        //}

        //private void btnRefresh_Click(object sender, RoutedEventArgs e)
        //{
        //    //lvWordMeanings.Items.Clear();
        //    FillTranslationAlternatives();
        //}

        //private void btnSpeak_Click(object sender, RoutedEventArgs e)
        //{
        //    using (SpeechSynthesizer sth = new SpeechSynthesizer())
        //    {
        //        var voices = sth.GetInstalledVoices();
        //        Random random = new Random();
        //        int ind = random.Next(0, voices.Count);
        //        sth.SelectVoice(voices[ind].VoiceInfo.Name);
        //        sth.Speak(_learningWord.Value);
        //        //string z ="The next part that doesn't just use default pronunciation is the date. We use the special SayAs enumeration to specify that the date should be read out as an actual date and not just a set of numbers, spaces and special characters.";
        //        // sth.Speak(z);
        //    }
        //}

        //private void btnNext_Click(object sender, RoutedEventArgs e)
        //{
        //    _learningWord = GetNextWord();
        //    if (_learningWord == null)
        //    {
        //        btnNext.IsEnabled = false;
        //        return;
        //    }
        //    this.spWordMeanins.IsEnabled = true;
        //    btnShowAnswer.IsEnabled = true;
        //    tbLearningWord.DataContext = _learningWord;
        //    FillTranslationAlternatives();

        //}

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (_learningWord == null)
        //    {
        //        FillComboboxValues();
        //        btnNext_Click(null, null);
        //    }

        //}

        //private void WordMeaningsSelection(object sender, SelectionChangedEventArgs e)
        //{
        //    //MessageBox.Show(e.OriginalSource.ToString());
        //    if (e.AddedItems.Count == 0)
        //        return;
        //    bool res = ShowRightAnswer();
        //    btnNext.IsEnabled = true;
        //    btnShowAnswer.IsEnabled = false;
        //    this.spWordMeanins.IsEnabled = false;
        //    if (res)
        //    {
        //        if (_speakWorb)
        //            btnSpeak_Click(null, null);
        //        btnNext_Click(null, null);
        //    }

        //}

        //private void btnEditWord_Click(object sender, RoutedEventArgs e)
        //{
        //    WordEntry wordEntry = _wordMeaningRow.WordEntries.LastOrDefault();
        //    if (wordEntry == null)
        //        return;
        //    WordEntryCard wordWindow = WordEntryCard.GetWordWindow();
        //    wordWindow.Init(_context, wordEntry, null, 0);
        //    wordWindow.ShowDialog();
        //    if (wordWindow.DialogResult.HasValue)
        //        FillTranslationAlternatives();
        //}

    }
}
