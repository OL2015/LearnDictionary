using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Timers;

namespace DictionaryUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AutoCompleteTextBoxViewModel : ViewModelBase
    {        
        private IDictionaryDataService dictionaryDataService;

        private string text;
        private ObservableCollection<object> candidates = null;
        private object selectedItem;
        private bool insertText;
        private int searchThreshold;
        private IEnumerable lookupTable;
        private string lookupField; //name of the property of lookupTable objects
        private bool dropDownOpen;
        private Timer keypressTimer;
        public RelayCommand PossibleWordShowCommand { get; private set; }
        public RelayCommand TextChangedCommand { get; private set; }
        public string LookupField
        {
            get { return lookupField; }
            set
            { 
                Set(() => lookupField, ref lookupField, value);
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                insertText = true;
                Set(() => Text, ref text, value);
            }
        }

        public bool DropDownOpen
        {
            get { return dropDownOpen; }
            set
            {
                Set(() => DropDownOpen, ref dropDownOpen, value);
            }
        }

        public object SelectedItem
        {
            get { return selectedItem; }
            set { Set(() => SelectedItem, ref selectedItem, value); }
        }

        public ObservableCollection<object> Candidates
        {
            get { return candidates; }
            set
            {
                insertText = true;
                Set(() => Candidates, ref candidates, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the AutoCompleteTextBoxViewModel1 class.
        /// </summary>
        public AutoCompleteTextBoxViewModel(IEnumerable lookupTable, string lookupField, string text)
        {
            this.lookupTable = lookupTable;
            this.LookupField = lookupField;
            Candidates = new ObservableCollection<object>();
            searchThreshold = 2;        // default threshold to 2 char
            TextChangedCommand = new RelayCommand(TextChanged);
            Text = "text";
            //keypressTimer = new Timer();
            //keypressTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }
        public void Clear()
        {
            Text = "";
            Candidates.Clear();
            SelectedItem = null;
            DropDownOpen = false;
        }
        public void TextChanged()
        {
            Candidates.Clear();

            try
            {
                if (Text.Length >= searchThreshold)
                {
                    foreach (var src in lookupTable)
                    {
                        object word = src.GetType().GetProperty(lookupField).GetValue(src, null);
                        if (word == null)
                            continue;
                        if (word.ToString().StartsWith(Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Candidates.Add(src);
                        }
                    }
                    DropDownOpen = Candidates.Count > 0;
                }
                else
                {
                    DropDownOpen = false;
                }
            }
            catch { }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            keypressTimer.Stop(); 
            DispatcherHelper.CheckBeginInvokeOnUI(
             () => { this.TextChanged(); }
             );
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            //    new TextChangedCallback(this.TextChanged));
        }

    }
}