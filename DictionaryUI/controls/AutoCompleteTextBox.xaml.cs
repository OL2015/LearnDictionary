using DictionaryUI.ViewModel;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private TextBox textBox;
        private ComboBox comboBox;
        //private ObservableCollection<AutoCompleteEntry> autoCompletionList;
        private System.Timers.Timer keypressTimer;
        private delegate void TextChangedCallback();
        private bool insertText;
        private int delayTime;
        private int searchThreshold=2;
        AutoCompleteTextBoxViewModel autoCompleteViewModel;
        #endregion
        public static readonly DependencyProperty TextMeProperty = DependencyProperty.RegisterAttached(
              "TextMe",
              typeof(String),
              typeof(AutoCompleteTextBox),
              new FrameworkPropertyMetadata( "qqq" , FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static void SetTextMe(UIElement element, String value)
        {
            element.SetValue(TextMeProperty, value);
        }
        public static String GetTextMe(UIElement element)
        {
            return (String)element.GetValue(TextMeProperty);
        }
        public String TextMe
        {
            get { return (String)GetValue(TextMeProperty); }
            set { SetValue(TextMeProperty, value); }
        }

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
            DelayTime = 200;
            controls = new VisualCollection(this);
            InitializeComponent(); 
             
            // set up the key press timer
            keypressTimer = new System.Timers.Timer();
            keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            // set up the text box and the combo box
            comboBox = new ComboBox();
            comboBox.IsSynchronizedWithCurrentItem = true;
            comboBox.IsTabStop = false;
            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);
            comboBox.IsDropDownOpen = false;
            textBox = new TextBox();
            textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            textBox.VerticalContentAlignment = VerticalAlignment.Center;

            controls.Add(comboBox);
            controls.Add(textBox);

            this.IsKeyboardFocusWithinChanged += AutoCompleteTextBox_IsKeyboardFocusWithinChanged;

           
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
        public AutoCompleteTextBoxViewModel AutoCompleteViewModel
        {
            get { return autoCompleteViewModel; }
            set
            {
                insertText = true;
                autoCompleteViewModel = value;
                //this.DataContext = AutoCompleteViewModel;
                comboBox.DisplayMemberPath = AutoCompleteViewModel.LookupField;
                comboBox.ItemsSource = autoCompleteViewModel.Candidates;
           
                Binding myBinding = new Binding();
                myBinding.Source = AutoCompleteViewModel;
                myBinding.Path = new PropertyPath("Text");
                myBinding.Mode = BindingMode.TwoWay;
                myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(textBox, TextBox.TextProperty, myBinding);
        }

            }
        public string Text
        {
            get { return textBox.Text; }
            set
            {
                insertText = true;
                textBox.Text = value;
            }
        }

        public ComboBoxItem SelectedItem
        {
            get { return comboBox.SelectedItem as ComboBoxItem; }
        }


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

       

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != comboBox.SelectedItem)
            {
                insertText = true;
                if (comboBox.SelectedItem!= null);
                var value =comboBox.SelectedItem.GetType().GetProperty(autoCompleteViewModel.LookupField).GetValue(comboBox.SelectedItem, null);
                textBox.Text = value.ToString();
                autoCompleteViewModel.SelectedItem = comboBox.SelectedItem;
            }
        }

       
        private void TextChanged()
        {
            try
            {
                //comboBox.ItemsSource = null;
                if (autoCompleteViewModel.Text.Length >= searchThreshold)
                {
                   // autoCompleteViewModel.Text = Text;
                    autoCompleteViewModel.TextChanged();
                    //comboBox.ItemsSource = autoCompleteViewModel.Candidates; 
                    comboBox.IsDropDownOpen = comboBox.HasItems;
                }
                else
                {
                    comboBox.IsDropDownOpen = true; //todo
                }
                //comboBox.IsDropDownOpen = comboBox.HasItems;
            }
            catch (Exception Ex) 
            {
            }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new TextChangedCallback(this.TextChanged));
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // text was not typed, do nothing and consume the flag
            if (insertText == true) insertText = false;

            // if the delay time is set, delay handling of text changed
            else
            {
                if (delayTime > 0)
                {
                    keypressTimer.Interval = delayTime;
                    keypressTimer.Start();
                }
                else TextChanged();
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            textBox.Arrange(new Rect(arrangeSize));
            comboBox.Arrange(new Rect(arrangeSize));
            return base.ArrangeOverride(arrangeSize);
        }

        protected override Visual GetVisualChild(int index)
        {
            return controls[index];
        }

        protected override int VisualChildrenCount
        {
            get { return controls.Count; }
        }

      
        #endregion


    }
}