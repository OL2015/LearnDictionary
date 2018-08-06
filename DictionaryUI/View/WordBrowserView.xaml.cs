using DictionaryUI.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryUI.View
{
    /// <summary>
    /// Description for WordBrowserView.
    /// </summary>
    public partial class WordBrowserView : Window
    {
        /// <summary>
        /// Initializes a new instance of the WordBrowserView class.
        /// </summary>
        public WordBrowserView()
        {
            InitializeComponent();
            //Messenger.Default.Register<RefreshViewNotificationMessage>(
            // this,
            //  message =>
            //  {
            //      //VisualTreeDisplay treeDisplay = new VisualTreeDisplay();
            //      //treeDisplay.ShowVisualTree(this);
            //      //treeDisplay.Show();
            //      WordEntryCardViewModel vm = message.Sender as WordEntryCardViewModel;
            //      if (vm == null) return;  // TODO also check token here
            //      TabControl tabs = this.FindName("tabWords") as TabControl;
            //      if (tabs != null)
            //      { 
            //           tabs.InvalidateVisual();
            //          //var dc = tabs.DataContext;
            //          //tabs.DataContext = null;
            //          //tabs.DataContext = dc; 
            //      } 
            //  });
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {

        }
    }
}