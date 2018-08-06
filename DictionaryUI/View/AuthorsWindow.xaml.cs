using DictionaryUI.ViewModel;
using System.Windows;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for AuthorsWindow.xaml
    /// </summary>
    public partial class AuthorsWindow : Window
    {
       
        public AuthorsWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

    }
}
