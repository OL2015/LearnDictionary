using DictionaryLogic;
using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.ViewModel;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class BooksList : Window
    { 
        public BooksList()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        } 
    }
}