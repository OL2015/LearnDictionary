using DictionaryLogic;
using DictionaryLogic.ModelProviders.EFModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for AllWords1.xaml
    /// </summary>
    public partial class AllWordsWindow : Window
    {               
        public AllWordsWindow()
        {
            InitializeComponent();
        }
        private void WordEntriesExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem wordEntries = ((TreeViewItem)e.OriginalSource);

            var wordEntry = wordEntries.DataContext as WordEntry;
            if (wordEntry == null)
                return;
            wordEntries.ItemsSource = wordEntry.Word.WordEntries.ToList();
            //wordEntries.Items.Clear();
        }
    }
}
