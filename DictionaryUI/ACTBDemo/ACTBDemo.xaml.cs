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
using System.Windows.Shapes; 
using System.Data;
using DictionaryLogic.ModelProviders;
using DictionaryLogic;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for ACTBDemo.xaml
    /// </summary>
    public partial class ACTBDemo : Window
    {
        Words _dsWords = null;

        Words dsWords
        {
            get
            {
                if (_dsWords == null)
                {
                    DictionaryFacade facade = DictionaryFacade.GetFacade();
                    facade.ConnectionString = Properties.Settings.Default.ConnStr;
                    _dsWords = facade.GetTypedDataSetModel().GetWordsDataSet();
                }
                return _dsWords;
            }
        }


        public ACTBDemo()
        {
            InitializeComponent();
            textBox1.Text = string.Empty;
            textBox1.Clear();
            textBox1.SetLookup(dsWords.Word, "Value");
        }
       
        private void button2_Click(object sender, RoutedEventArgs e)
        { 
            if (textBox1.SelectedItem != null)
            {
                DataRow row = textBox1.SelectedItem.DataContext as DataRow;
                if (row != null)
                {
                    MessageBox.Show("Existing word selected " + row["Word_ID"] + "->" + row["Value"]);
                    return;
                }
            }             
                MessageBox.Show("New word entered " + textBox1.Text);
          
        }
    }
}
