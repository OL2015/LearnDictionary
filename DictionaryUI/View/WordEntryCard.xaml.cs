using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for WordWindow.xaml
    /// </summary>
    public partial class WordEntryCard : UserControl
    {
        //static CultureInfo cultureInfoEngCulture = new CultureInfo("en-US");
        //static CultureInfo cultureInfoUkrCulture = new CultureInfo("uk-UA");
        public TranslationLookUp popUpLookUp;
        CultureInfo cultureBeforeFocus = null;
        public WordEntryCard()
        {
            InitializeComponent();
            cultureBeforeFocus = InputLanguageManager.Current.CurrentInputLanguage;
            //popUpLookUp = new TranslationLookUp();
            //popUpLookUp.WindowStyle = WindowStyle.SingleBorderWindow;
            //popUpLookUp.Show();
            Messenger.Default.Register<RefreshViewNotificationMessage>(
             this,
              message =>
              {
                  WordEntryCardViewModel vm = this.DataContext as WordEntryCardViewModel;
                  if (vm == null || vm.Token != message.Token) return;

                  if (true)
                  {
                      ListBox lstAllMeanings = this.FindName("lstAllMeanings") as ListBox;
                      var dc = lstAllMeanings.DataContext; lstAllMeanings.DataContext = null; lstAllMeanings.DataContext = dc;
                  }
              });
            //TODO -  doesn't work this way!!!!
            //Messenger.Default.Register<SetInputNotificationMessage>(
            //this,
            // message =>
            // {
            //     WordEntryCardViewModel vm = this.DataContext as WordEntryCardViewModel;

            //     if (vm == null || vm.Token != message.Token) return;
            //     if (!message.GotFocus)
            //     { 
            //        //InputLanguageManager.Current.CurrentInputLanguage.ClearCachedData();
            //         //InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");

            //         //TextBox tbMeaning = this.FindName("tbMeaning") as TextBox;
            //         //InputLanguageManager.SetInputLanguage(tbMeaning, new CultureInfo("en-US"));
            //         return;
            //     }

            //     //cultureBeforeFocus = InputLanguageManager.Current.CurrentInputLanguage;
            //     CultureInfo currentCultureInfo;

            //     if (message.Language == "ukr")
            //         currentCultureInfo = new CultureInfo("uk-UA");
            //     else
            //         currentCultureInfo = new CultureInfo("en-US");

            //     if (message.WhatControl == "Meaning"  )
            //     {
            //         TextBox tbMeaning = this.FindName("tbMeaning") as TextBox; 
            //         InputLanguageManager.SetInputLanguage(tbMeaning, currentCultureInfo);
            //         InputLanguageManager.SetInputLanguage(tbMeaning, currentCultureInfo);
            //     }
            //     if (message.WhatControl == "Word")
            //     {
            //         WpfControls.Editors.AutoCompleteTextBox tbWord = this.FindName("tbWordActb") as WpfControls.Editors.AutoCompleteTextBox;
            //         InputLanguageManager.SetInputLanguage(tbWord.Editor, currentCultureInfo);
            //         InputLanguageManager.SetInputLanguage(tbWord.Editor, currentCultureInfo);
            //     }
            // });
        }

        private void ChangeSpeechPart(object sender, RoutedEventArgs e)
        { }
        private void PopUpLookUp(object sender, RoutedEventArgs e)
        { }

        //private void menuToWordMeaningsClick(object sender, RoutedEventArgs e)
        //{
        //    MenuItem menuItem = sender as MenuItem;
        //    int spart = ((SpeechPart)(menuItem.DataContext)).SpeechPart_ID;
        //    var item = lstMeanings.Items.CurrentItem as WordMeaning;
        //    foreach (var sel in lstMeanings.SelectedItems)
        //        ((WordMeaning)sel).SpeechPart_ID = spart;
        //}

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

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void tbWordActb_GotFocus(object sender, RoutedEventArgs e)
        {
            //cultureBeforeFocus = InputLanguageManager.Current.CurrentInputLanguage;
            CultureInfo currentCultureInfo;

            currentCultureInfo = new CultureInfo("en-US");

            //WpfControls.Editors.AutoCompleteTextBox tbWord = this.FindName("tbWordActb") as WpfControls.Editors.AutoCompleteTextBox;
            //InputLanguageManager.SetInputLanguage(tbWord.Editor, currentCultureInfo);
            InputLanguageManager.SetInputLanguage(tbWordActb.Editor, currentCultureInfo);

        }

        private void tbMeaning_GotFocus(object sender, RoutedEventArgs e)
        {
            CultureInfo currentCultureInfo;
            currentCultureInfo = new CultureInfo("uk-UA");
            //TextBox tbMeaning = this.FindName("tbMeaning") as TextBox;
            //InputLanguageManager.SetInputLanguage(tbMeaning, currentCultureInfo);
            InputLanguageManager.SetInputLanguage(tbMeaning, currentCultureInfo);

        }
    }
}
