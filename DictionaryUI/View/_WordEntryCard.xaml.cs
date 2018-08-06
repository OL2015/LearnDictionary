using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Model;
using DictionaryUI.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for WordWindow.xaml
    /// </summary>
    public partial class WordEntryCard : Window
    {
        private static WordEntryCard _wordWindow1;
        static CultureInfo cultureInfoEngCulture = new CultureInfo("en-US");
        static CultureInfo cultureInfoUkrCulture = new CultureInfo("uk-UA");
        private CultureInfo currentCultureInfo = null; 
        public TranslationLookUp popUpLookUp;
        public static WordEntryCard GetWordWindow()
        {
            if (_wordWindow1 == null)
            {
                _wordWindow1 = new WordEntryCard();
                _wordWindow1.Closed += _wordWindow_Closed;
            }
            return _wordWindow1;
        }
        private static void _wordWindow_Closed(object sender, System.EventArgs e)
        {
            _wordWindow1 = null;
        }

        LearnDictionaryEntities _context = null;
        Word _word = null;
        WordEntry _wordEntry = null;
        public WordEntry LastWordEntry { get { return _wordEntry; } }
        public int LastSessionPage { get; set; }
        public Book SelectedBook { get; set; }

        public AutoCompleteTextBoxViewModel autoCompleteViewModel;

        public WordEntryCard()
        {
            InitializeComponent();
            //Messenger.Default.Register<NotificationMessage<TranslationItem>>(
            //this,
            //message =>
            //{ 
            //    string tr = message.Content.Translation;
            //    SpeechPart sp = _context.SpeechParts.FirstOrDefault(z => z.NameEng.ToUpper() == message.Content.LangPart.ToUpper());
            //    AppendNewMeaning(tr, sp.SpeechPart_ID);
            //});
            popUpLookUp = new TranslationLookUp();

            popUpLookUp.WindowStyle = WindowStyle.SingleBorderWindow;

            popUpLookUp.Show();
        }

        

         
        public void Init(LearnDictionaryEntities context, WordEntry wordEntry, Book lastBook, int lastPage)
        {
            
            _context = context;
            if (wordEntry != null)
            {
                _wordEntry = wordEntry;
                _word = _wordEntry.Word;
                btnNextWord.IsEnabled = false;
            }
            else
            {
                LastSessionPage = lastPage;
                //SelectedBook = lastBook;
                _wordEntry = new WordEntry();
                //_wordEntry.Book = SelectedBook ?? _context.Books.First();
                _word = new Word() { Value = "", Language_ID = _context.Languages.First().Language_ID };
                _word.Language = _wordEntry.Book.Language;
                _wordEntry.Date = DateTime.Now;
                _wordEntry.Page = LastSessionPage;
                _wordEntry.Word = _word;
            }
            //this.DataContext = _wordEntry;
            ContextMenuToNewMeaningWord();
            ContextMenuToMeaningWords();
            RefreshWord();
        }
        private void word_AutoCompleteTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if ( autoCompleteViewModel.SelectedItem is Word)
            {
                 _word = autoCompleteViewModel.SelectedItem as Word;
                _wordEntry.Word = _word;
                RefreshWord();
            }
            else
            {
                _word.Value = autoCompleteViewModel.Text;// todo Logic from here;   
            }
            if (_word != null && !string.IsNullOrEmpty(_word.Value))
            { 
                Messenger.Default.Send(
                  new TranslateWordNotificationMessage(this, "") { WordToTranslate = _word.Value, FromLang = "en", ToLang = "uk" });
                 
            }
        }

        private void word_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != autoCompleteViewModel.SelectedItem)
            {
                //_word = autoCompleteViewModel.SelectedItem.DataContext as Word;
                RefreshWord();
            }
        }
        void word_DropDownClosed(object sender, EventArgs e)
        {
            if (null != autoCompleteViewModel.SelectedItem)
            {
                //_word = autoCompleteViewModel.SelectedItem.DataContext as Word;
                RefreshWord();
            }
            else
                _word.Value = autoCompleteViewModel.Text;
        }
        private void RefreshWord()
        {
            //cbBooks.ItemsSource = _context.Books.Local;
            //cbBooks.SelectedItem = _wordEntry.Book;
            //cbBooks.DisplayMemberPath = "Name";
            //cbWordLang.ItemsSource = _context.Languages.Local;
            //cbWordLang.DisplayMemberPath = "ShortName";
            //cbWordLang.SelectedValuePath = "Language_ID";
            //cbWordLang.DataContext = _word;
            cbMiningLang.ItemsSource = _context.Languages.Local;
            cbMiningLang.DisplayMemberPath = "ShortName";
            tblSpeechPart.DataContext = _context.SpeechParts.First();
            if (autoCompleteViewModel == null)
            {
                autoCompleteViewModel = new AutoCompleteTextBoxViewModel(_context.Words, "Value", _word.Value);
                autoCompleteViewModel.Text = "zzz"; 
                tbWord.AutoCompleteViewModel = autoCompleteViewModel; 
                tbWord.AutoCompleteTextBoxLostFocus += new RoutedEventHandler(word_AutoCompleteTextBoxLostFocus);
            }

            RefreshWordTranslation();
            cbWordLang.SelectedIndex = _word.Language_ID - 1; //TODO
            cbMiningLang.SelectedIndex = 1;
            FillAllWordMeanings();
        }
        private void ContextMenuToNewMeaningWord()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuPaste = new MenuItem();
            menuPaste.Command = ApplicationCommands.Paste;
            contextMenu.Items.Add(menuPaste);
            contextMenu.Items.Add(new Separator());

            foreach (var item in _context.SpeechParts)
            {
                MenuItem menu = new MenuItem();
                menu.DataContext = item;
                Binding binding = new Binding();
                binding.Source = item;
                binding.Path = new PropertyPath("NameEng");
                menu.SetBinding(MenuItem.HeaderProperty, binding);
                menu.Click += new RoutedEventHandler(menu_Click);
                contextMenu.Items.Add(menu);
            }


            tbMeaning.ContextMenu = contextMenu;
        }

        private void ContextMenuToMeaningWords()
        {
            ContextMenu contextMenu = new ContextMenu();

            foreach (var item in _context.SpeechParts)
            {
                MenuItem menu = new MenuItem();
                menu.DataContext = item;
                Binding binding = new Binding();
                binding.Source = item;
                binding.Path = new PropertyPath("NameEng");
                menu.SetBinding(MenuItem.HeaderProperty, binding);
                menu.Click += new RoutedEventHandler(menuToWordMeaningsClick);
                contextMenu.Items.Add(menu);
            }
            lstMeanings.ContextMenu = contextMenu;
        }

        private void menuToWordMeaningsClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            int spart = ((SpeechPart)(menuItem.DataContext)).SpeechPart_ID;
            var item = lstMeanings.Items.CurrentItem as WordMeaning;
            foreach (var sel in lstMeanings.SelectedItems)
                ((WordMeaning)sel).SpeechPart_ID = spart; 
        } 

        private void bindingPaste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ApplicationCommands.Paste.Execute(null, tbMeaning);
        }
        private void bindingPaste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ApplicationCommands.Paste.Execute(null, tbMeaning);
        } 

        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new CommandBinding(ApplicationCommands.Paste);
        }
        void menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            tblSpeechPart.DataContext = menuItem.DataContext;
        }
        private void RefreshWordTranslation()
        {
            if (_wordEntry.WordMeanings.Count != 0)
            {
                List<WordMeaning> wt = _wordEntry.WordMeanings.Select(z => z).ToList();
                lstMeanings.ItemsSource = wt;
                //lstMeanings.DisplayMemberPath = "Word1.Value";
            }
            else
                lstMeanings.ItemsSource = null;

        }
        

        private void FillAllWordMeanings()
        {
            List<Word> wl = _word.WordMeanings.Select(z => z.Word1).ToList();
            lstAllMeanings.ItemsSource = wl;
            lstAllMeanings.ItemsSource = _word.WordMeanings.ToList();
            lstAllMeanings.DisplayMemberPath = "Word1.Value";
        }

        private void AppendNewMeaning(string newMeaning, int spID)
        {
            Word newWord = _context.Words.Where(z => z.Value.ToLower() == newMeaning.ToLower()).FirstOrDefault();
            newWord = newWord ?? new Word() { Value = newMeaning, Language = (Language)cbMiningLang.SelectedItem };
            if (!_wordEntry.WordMeanings.Any(z => z.Word1.Value == newMeaning)) // and speachpart
            {
                WordMeaning wm = new WordMeaning() { Word = _word, Word1 = newWord };
                wm.SpeechPart_ID = spID;
                _wordEntry.WordMeanings.Add(wm);
            }
            RefreshWordTranslation();
            FillAllWordMeanings();
        }

        private void RemoveMeaningsFromWord()
        {
            foreach (var item in lstMeanings.SelectedItems)
            {
                var wm = item as WordMeaning;
                _wordEntry.WordMeanings.Remove(wm);
            }
            RefreshWordTranslation();
        }

        private void AddMeaningsToWord()
        {
            foreach (var item in lstAllMeanings.SelectedItems)
            {
                try
                {
                    var wm = item as WordMeaning;
                    if (!_wordEntry.WordMeanings.Any(z => z == wm))
                    { 
                        _wordEntry.WordMeanings.Add(wm);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Row is already in meaning list.");
                }
                RefreshWordTranslation();
            }
        }
        private void btnAddMeaning_Click(object sender, RoutedEventArgs e)
        {
            AddMeaningsToWord();
        }

        private void btnRemoveMeaning_Click(object sender, RoutedEventArgs e)
        {
            RemoveMeaningsFromWord();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        private void cbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedBook = cbBooks.SelectedItem as Book;
            if (SelectedBook == null)
                return;
            if (_wordEntry != null)
                _wordEntry.Book = SelectedBook;
            cbWordLang.SelectedValue = SelectedBook.Language_ID;
        }

        private void btnAddNewMeaning_Click(object sender, RoutedEventArgs e)
        {
            string newMeaning = tbMeaning.Text.Trim();
            if (string.IsNullOrEmpty(newMeaning))
                return;
            var sp = ((SpeechPart)tblSpeechPart.DataContext).SpeechPart_ID;
            string suffixWord = newMeaning.Substring(newMeaning.Length - 2);
            if (suffixWord == "ти" && ((SpeechPart)tblSpeechPart.DataContext).ShortEng == "n")
                tblSpeechPart.DataContext = _context.SpeechParts.FirstOrDefault(z => z.ShortEng == "v");

            AppendNewMeaning(newMeaning, sp);
            tbMeaning.Text = "";
        } 
         
        private void btnSaveWord_Click(object sender, RoutedEventArgs e)
        {
            // todo Check whether _word or _wordEntry dattached
            SaveChanges();
            //.DialogResult = true;
            this.Close();
        }

        private void SaveChanges()
        {
            var we = _context.Entry(_word);
            if (we.State == System.Data.Entity.EntityState.Detached)
                _context.Words.Add(_word);
            var wee = _context.Entry(_wordEntry);
            if (wee.State == System.Data.Entity.EntityState.Detached)
                _context.WordEntries.Add(_wordEntry);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            } 
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            int NumValue;
            if (int.TryParse(tbPage.Text, out NumValue))
            {
                tbPage.Text = (NumValue + 1).ToString();
            }
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            int NumValue;
            if (int.TryParse(tbPage.Text, out NumValue))
            {
                tbPage.Text = (NumValue - 1).ToString();
            }
        }
        private void btnCancelWord_Click(object sender, RoutedEventArgs e)
        {
            if (_context.ChangeTracker.HasChanges())
            {
                if (MessageBox.Show("There are unsaved changes. Close anyway?", "Changes not saved", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    return;
            }
            //DialogResult = false;
            this.Close();
        }

        private void btnNextWord_Click(object sender, RoutedEventArgs e)
        {
            btnAddNewMeaning_Click(null, null);
            SaveChanges(); 
            _wordEntry = null;
            int page;
            LastSessionPage = int.TryParse(tbPage.Text, out page) ? page : LastSessionPage;
            Init(_context, _wordEntry, SelectedBook, LastSessionPage);
            autoCompleteViewModel.Clear();
            tbWord.Focus(); 
        }

        private void btnSpeak_Click(object sender, RoutedEventArgs e)
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
                            sth.Speak(_word.Value);
                        }
                    }
                );
            }
            catch (Exception) { };
        }

        private void tbMeaning_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAddNewMeaning_Click(null, null);
                tbMeaning.Focus();
            }
        }

        private void tbMeaning_GotFocus(object sender, RoutedEventArgs e)
        {
            currentCultureInfo = InputLanguageManager.Current.CurrentInputLanguage;
            Language selectedLanguage = cbMiningLang.SelectedItem as Language;
            if (selectedLanguage.ShortName == "ukr")
                InputLanguageManager.Current.CurrentInputLanguage = WordEntryCard.cultureInfoUkrCulture;
        }

        private void tbWord_GotFocus(object sender, RoutedEventArgs e)
        {
            currentCultureInfo = InputLanguageManager.Current.CurrentInputLanguage;
            return;
            Language selectedLanguage = cbWordLang.SelectedItem as Language;
            if (selectedLanguage.ShortName == "eng")
                InputLanguageManager.Current.CurrentInputLanguage = WordEntryCard.cultureInfoEngCulture;
        } 

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                var item = sender as TextBlock;
                if (item.DataContext != null)
                {
                    var wm = item.DataContext as WordMeaning;
                    if (wm != null)
                    {
                        _wordEntry.WordMeanings.Remove(wm);
                        RefreshWordTranslation();
                    }
                }
            }

        } 
         
        private void PlaceholdersListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
                if (item != null)
                {
                    try
                    {
                        var wm = item.Content as WordMeaning;
                        if (!_wordEntry.WordMeanings.Any(z => z == wm))
                        {
                            _wordEntry.WordMeanings.Add(wm);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Row is already in meaning list.");
                    }
                    RefreshWordTranslation();
                }
            }
        }

        private void ChangeSpeechPart(object sender, RoutedEventArgs e)
        {
            if (lstMeanings.Items.Count == 0)
                return;

            var item = lstMeanings.Items[0];
            item = lstMeanings.SelectedItems.Count > 0 ? lstMeanings.SelectedItems[0] : item;
            {
                var wm = item as WordMeaning;
                var parts = _context.SpeechParts.ToList();
                var index = parts.IndexOf(parts.FirstOrDefault(z => z.SpeechPart_ID == wm.SpeechPart_ID));
                if (index < parts.Count - 1)
                    index++;
                else
                    index = 0;
                wm.SpeechPart_ID = parts.ElementAt(index).SpeechPart_ID;
            }
            RefreshWordTranslation();
        } 

        private void tbWord_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = currentCultureInfo; 
        }

        private void tbMeaning_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = currentCultureInfo;
        }

        private void lstMeanings_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private void popUpLookUp_Close(object sender, RoutedEventArgs e)
        {
            //popUpLookUp.IsOpen = false;
        } 


        private void PopUpLookUp(object sender, RoutedEventArgs e)
        {
            if (_word != null && !string.IsNullOrEmpty(_word.Value))
            Messenger.Default.Send(
                  new TranslateWordNotificationMessage( this, "") { WordToTranslate = _word.Value, FromLang = "en", ToLang = "uk" }); 
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            popUpLookUp.Close();
        }

    }
}
