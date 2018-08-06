using System.Collections.Generic;
using System.Data;
using System.Windows;
using DictionaryLogic;
using DictionaryLogic.Entities;
using DictionaryLogic.ModelProviders;

using System;

using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for WordWindow.xaml
    /// </summary>
    public partial class WordWindow : Window
    {
        private static WordWindow _wordWindow;
        public static WordWindow GetWordWindow(Words.WordEntryRow word)
        {
            if (_wordWindow == null)
            {
                _wordWindow = new WordWindow(word);
                _wordWindow.Closed += _wordWindow_Closed;
            }
            return _wordWindow;
        }
        private static void _wordWindow_Closed(object sender, System.EventArgs e)
        {
            _wordWindow = null;
        }

        Words dsWord;
        private Words.WordEntryRow _word = null;
        public Words.WordEntryRow Word
        {
            get { return _word; }
            set
            {
                _word = value;
                this.DataContext = _word;
            }
        }
        public WordWindow(Words.WordEntryRow word)
        {
            InitializeComponent();
            Word = word;
            dsWord = (Words)_word.Table.DataSet;
            cbBooks.ItemsSource = dsWord.Book;
            RefreshWord();
        }

        private void RefreshWord()
        {
            Words.WordRow word = (Words.WordRow)Word.GetParentRow("FK_WordEntry_Word");
            tbWord.Text = word.Value;

            Words.WordTranslationRow[] weRows = (Words.WordTranslationRow[])Word.GetChildRows("FK_WordTranslation_WordEntry");
            var ats =  weRows.SelectMany(z => (Words.WordMeaningRow[])z.GetParentRows("FK_WordTranslation_WordMeaning"));
            var meanings = ats.Select (z => z.GetParentRow("FK_WordMeaning_TranslateWord"));
            lstMeanings.ItemsSource = meanings; 
            //lstMeanings.DisplayMemberPath = "TranslateWord_ID";
            lstMeanings.DisplayMemberPath = "Value";

            cbWordLang.ItemsSource = dsWord.Language;
            cbWordLang.DisplayMemberPath = "ShortName";
            cbWordLang.SelectedIndex = word.Language_ID - 1;

            cbMiningLang.ItemsSource = dsWord.Language;
            cbMiningLang.DisplayMemberPath = "ShortName";
            cbMiningLang.SelectedIndex = 1;

        }
    }
}
