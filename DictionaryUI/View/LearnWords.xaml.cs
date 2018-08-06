using DictionaryLogic;
using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for LearnWords.xaml
    /// </summary>
    public partial class LearnWords : Window
    {     
        public LearnWords()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
         

        private void lstvAllMeanings_LostFocus(object sender, RoutedEventArgs e)
        {
            lstvAllMeanings.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, lstvAllMeanings);
        }
    }
}
