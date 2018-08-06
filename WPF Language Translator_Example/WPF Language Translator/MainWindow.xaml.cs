using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Xml.Linq;
using WPF_Language_Translator.Controls;

namespace WPF_Language_Translator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private TranslatorYandex langTranslator = new TranslatorYandex();
        private string textToTrans;
        private string fromLang;
        private string toLang;
        private string translation;

        public MainWindow()
        {
            InitializeComponent();
            FromLanguageCmbBox.ItemsSource = TranslationLookUp.LanguageList;
            ToLanguageCmbBox.ItemsSource = TranslationLookUp.LanguageList;
            FromLanguageCmbBox.SelectedIndex =0;
            ToLanguageCmbBox.SelectedIndex = 4;
            TrnslLookUp.LookUp += new RoutedEventHandler(LookUp_Changed);
            TrnslLookUp.LookUpMany += new RoutedEventHandler(LookUp_Changed);
        }

        private void LookUp_Changed(object sender, RoutedEventArgs e)
        {
            LookUpEventArgs args = (LookUpEventArgs)e;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Translations of word {0}\n", args.word);
            foreach(var a in args.items)
            {
                builder.AppendFormat("{0} = {1}\n", a.MainMeaning, a.Translation);
            }
            MessageBox.Show(builder.ToString());
        }

        //private void LookUp_Changed(object sender, RoutedEventArgs e)
        //{
        //    LookUpEventArgs args = (LookUpEventArgs)e;
        //    MessageBox.Show(string.Format("{0} = {1}", args.items.ElementAt(0).MainMeaning, args.items.ElementAt(0).Translation));
        //}

        // Close button click event handler.
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Minimize button click event handler.
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Translate button click event handler.
        private void TranslateButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextToTranslateTxtBox.Text != string.Empty)
            {
                textToTrans = TextToTranslateTxtBox.Text;
                fromLang = ((Language)FromLanguageCmbBox.SelectedValue).langCode;
                toLang = ((Language)ToLanguageCmbBox.SelectedValue).langCode;

                TranslatedTextTxtBox.Clear();
                TranslateButton.IsEnabled = false;

                Thread td = new Thread(GetTranslation);
                td.Start();
            }
            else
            {
                 MessageBox.Show("Enter text to translate.", "Translator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                 TextToTranslateTxtBox.Focus();
            }
        }

        private void GetTranslation()
        {
            XDocument doc = TrnslLookUp.GetTranslatedText(textToTrans, fromLang, toLang);
            translation = doc.ToString();
            this.Dispatcher.BeginInvoke(new ThreadStart(ShowTranslatedText), DispatcherPriority.Normal, null);
        }

        // Display translated text in text box.
        private void ShowTranslatedText()
        {
            TrnslLookUp.Refresh();
            TranslatedTextTxtBox.Text = translation;
            TranslateButton.IsEnabled = true;
        }


        // Clear button click event handler.
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            TextToTranslateTxtBox.Clear();
            TranslatedTextTxtBox.Clear();
            TextToTranslateTxtBox.Focus();
            TrnslLookUp.Clear();
        }        
  
    }
}
