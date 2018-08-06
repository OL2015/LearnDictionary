using DictionaryLogic;
using DictionaryUI.ViewModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //System.Windows.Controls.AutoCompleteBox b;
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}
