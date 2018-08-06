using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for AutoCompleteTextBox.xaml
    /// </summary>    
    public partial class AutoCompleteTextBox : Canvas
    {
        #region Members
        private VisualCollection controls;       
        //private ObservableCollection<AutoCompleteEntry> autoCompletionList;
        private System.Timers.Timer keypressTimer;
        private delegate void TextChangedCallback();
        private bool insertText;
        private int delayTime;
        private int searchThreshold;
        
        #endregion

        #region CustomControl
        public static readonly RoutedEvent AutoCompleteTextBoxLostFocusEvent =
        EventManager.RegisterRoutedEvent("AutoCompleteTextBoxLostFocus",
                                         RoutingStrategy.Bubble,
                                         typeof(RoutedEventHandler),
                                         typeof(AutoCompleteTextBox));
        public event RoutedEventHandler AutoCompleteTextBoxLostFocus
        {
            add { AddHandler(AutoCompleteTextBoxLostFocusEvent, value); }
            remove { RemoveHandler(AutoCompleteTextBoxLostFocusEvent, value); }
        }

        #endregion

        #region Constructor
        public AutoCompleteTextBox()
        {
            //controls = new VisualCollection(this);
            InitializeComponent();

            //autoCompletionList = new ObservableCollection<AutoCompleteEntry>();
            //searchThreshold = 2;        // default threshold to 2 char

            // set up the key press timer
            //keypressTimer = new System.Timers.Timer();
            //keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            
            

            //this.IsKeyboardFocusWithinChanged += AutoCompleteTextBox_IsKeyboardFocusWithinChanged;
        }
        #endregion

        #region ChangedEvent
        void AutoCompleteTextBox_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.OldValue == true && (bool)e.NewValue == false)
            {
                RaiseEvent(new RoutedEventArgs(AutoCompleteTextBox.AutoCompleteTextBoxLostFocusEvent, this));
            }
        }
        #endregion

        #region Methods
        //public string Text
        //{
        //    get { return text; }
        //    set
        //    {
        //        insertText = true;
        //        text = value;
        //    }
        //}

        //public ComboBoxItem SelectedItem
        //{
        //    get { return comboBox.SelectedItem as ComboBoxItem; }
        //}


        public int DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        public int Threshold
        {
            get { return searchThreshold; }
            set { searchThreshold = value; }
        }

        //private string _lookupField;
        //private IEnumerable _lookupTable;

        //public void SetLookup(IEnumerable lookupTable, string lookupField)
        //{
        //    _lookupField = lookupField;
        //    _lookupTable = lookupTable;
        //}

        public void Clear()
        {
            // autoCompletionList.Clear();

        }


        //public void AddItem(AutoCompleteEntry entry)
        //{
        //    autoCompletionList.Add(entry);
        //}

        //private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (null != comboBox.SelectedItem)
        //    {
        //        insertText = true;
        //        ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
        //        textBox.Text = cbItem.Content.ToString();
        //    }
        //}

        //private void TextChanged()
        //{

        //    try
        //    {
        //        comboBox.Items.Clear();
        //        if (textBox.Text.Length >= searchThreshold)
        //        {
        //            foreach (AutoCompleteEntry entry in autoCompletionList)
        //            {
        //                foreach (string word in entry.KeywordStrings)
        //                {
        //                    if (word.StartsWith(textBox.Text, StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        ComboBoxItem cbItem = new ComboBoxItem();
        //                        cbItem.Content = entry.ToString();
        //                        cbItem.DataContext = entry.Context;
        //                        comboBox.Items.Add(cbItem);
        //                        break;
        //                    }
        //                }
        //            }
        //            comboBox.IsDropDownOpen = comboBox.HasItems;
        //        }
        //        else
        //        {
        //            comboBox.IsDropDownOpen = false;
        //        }
        //    }
        //    catch { }
        //}

        //private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        //{
        //    keypressTimer.Stop();
        //    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
        //        new TextChangedCallback(this.TextChanged));
        //}

        //private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    // text was not typed, do nothing and consume the flag
        //    if (insertText == true) insertText = false;

        //    // if the delay time is set, delay handling of text changed
        //    else
        //    {
        //        if (delayTime > 0)
        //        {
        //            keypressTimer.Interval = delayTime;
        //            keypressTimer.Start();
        //        }
        //        else TextChanged();
        //    }
        //}

        //protected override Size ArrangeOverride(Size arrangeSize)
        //{
        //    tbInputText.Arrange(new Rect(arrangeSize));
        //    cbPossibleItems.Arrange(new Rect(arrangeSize));
        //    return base.ArrangeOverride(arrangeSize);
        //}

        //protected override Visual GetVisualChild(int index)
        //{
        //    return controls[index];
        //}

        //protected override int VisualChildrenCount
        //{
        //    get { return controls.Count; }
        //}

      
        #endregion


    }
}