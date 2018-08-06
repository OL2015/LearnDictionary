using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml;
using System.Xml.Linq;

namespace WPF_Language_Translator.Controls
{
    public class TranslationItem
    {
        public string LangPart { get; set; }
        public string Translation { get; set; }
        public string MainMeaning { get; set; }
    }

    public class LookUpEventArgs : RoutedEventArgs
    {
        public IEnumerable<TranslationItem> items;
        public string word;
    }
    /// <summary>
    /// Interaction logic for TranslationLookUp.xaml
    /// </summary>
    public partial class TranslationLookUp : UserControl
    {
        public List<TranslationItem> TranslatedItems { get; private set; }
        public static readonly RoutedEvent LookUpEvent;
        public static readonly RoutedEvent LookUpManyEvent;
        private static List<Language> langList = null;
        private static string yandexKey = "dict.1.1.20160509T180521Z.e9d2fa185b293a93.4f71493279e0b728e917628709daf8b8f1af2f95";
        public static List<Language> LanguageList
        {
            get
            {
                if (langList == null)
                {
                    langList = new List<Language>();
                    langList.Add(new Language { lang = "German", langCode = "de" });
                    langList.Add(new Language { lang = "English", langCode = "en" });
                    langList.Add(new Language { lang = "Russian", langCode = "ru" });
                    langList.Add(new Language { lang = "Ukranian", langCode = "uk" });
                    langList.Add(new Language { lang = "Simplified Chinese", langCode = "zh-CHS" });
                }
                return (langList.OrderBy(l => l.lang).ToList());
            }
        }
        public string word;
        public TranslationLookUp()
        {
            InitializeComponent();
            TranslatedItems = new List<TranslationItem>();
            //this.dgLookTranslation.DataContext = TranslatedItems;
        }      
        static TranslationLookUp()
        {
            LookUpEvent = EventManager.RegisterRoutedEvent("LookUp", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(TranslationLookUp));
            LookUpManyEvent = EventManager.RegisterRoutedEvent("LookUpMany", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(TranslationLookUp));
        }
        public event RoutedEventHandler LookUp
        {
            add { AddHandler(LookUpEvent, value); }
            remove { RemoveHandler(LookUpEvent, value); }
        }

        public event RoutedEventHandler LookUpMany
        {
            add { AddHandler(LookUpManyEvent, value); }
            remove { AddHandler(LookUpManyEvent, value); }
        }

        public void ParseXML(XDocument xml)
        {
            if (xml.Root == null)
                return;
            word = (string)xml.Root.Element("def").Element("text");
            var defs = from a in xml.Root.Elements("def")
                       select a;            
            //var poses = defs.Elements("tr")
            //    .Select(z=> new  TranslationItem()
            //            {
            //                LangPart = (string)z.Parent.Attribute("pos"),
            //                Translation = (string)z.Element("text"),
            //                MainMeaning = z.Element("mean") == null ? null : (string)z.Element("mean").Element("text")
            //            });
            var poses = from a in defs
                        let langPart = (string)a.Attribute("pos")
                        from b in a.Descendants("tr")
                        select new TranslationItem()
                        {
                            LangPart = langPart,
                            Translation = (string)b.Element("text"),
                            MainMeaning = b.Element("mean") == null ? null : (string)b.Element("mean").Element("text")
                        };
            TranslatedItems = poses.ToList();
        }

        public void Refresh()
        {
            //this.dgLookTranslation.ItemsSource = null;
            this.dgLookTranslation.ItemsSource = TranslatedItems;
            TranslationItem translationItem = TranslatedItems.ElementAt(0);
            LookUpEventArgs args = new LookUpEventArgs() { RoutedEvent = LookUpEvent, items = new[] { translationItem }, word = this.word };
            LookUpEventArgs argsMany = new LookUpEventArgs() { RoutedEvent = LookUpManyEvent, items = TranslatedItems, word = this.word};
           // RaiseEvent(argsMany);
           // args.RoutedEvent = LookUpEvent;
           // RaiseEvent( args); 
        }

        public void Clear()
        {
            if (TranslatedItems.Count > 0)
            {
                LookUpEventArgs args = new LookUpEventArgs() { RoutedEvent = LookUpManyEvent, items = TranslatedItems, word = this.word };
                RaiseEvent(args);
            }
            this.dgLookTranslation.ItemsSource = null;
        }

        private void dgLookTranslation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = dgLookTranslation.SelectedItem as TranslationItem;
            LookUpEventArgs selectedItemArgs = new LookUpEventArgs() { RoutedEvent = LookUpEvent, items = new[] { selectedItem }, word = this.word };
            RaiseEvent(selectedItemArgs);
        }




        internal XDocument GetTranslatedText(string textToTranslate, string fromLang, string toLang)
        {
            string translation = "";
            string uri = string.Format("https://dictionary.yandex.net/api/v1/dicservice/lookup?key={0}&lang={1}-{2}&text={3}", yandexKey, fromLang, toLang, textToTranslate);
            XDocument doc;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "application/xml";
            WebResponse response = null;
            response = request.GetResponse();

            using (Stream respStream = response.GetResponseStream())
            {
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.UTF8);
                string strResponse = rdr.ReadToEnd();
                doc = XDocument.Parse(@strResponse);
                translation = doc.ToString();
            }
            ParseXML(doc);
            return doc;
        }
    }
}
