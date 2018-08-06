using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace WPF_Language_Translator
{
    public class TranslatorYandex1
    {
        private static string yandexKey = "dict.1.1.20160509T180521Z.e9d2fa185b293a93.4f71493279e0b728e917628709daf8b8f1af2f95";

        private List<Language> langList = new List<Language>();

        public TranslatorYandex1()
        {
            this.PopulateLanguageList();
        }

        /// <summary>
        /// Returns a List(Of Language)
        /// </summary>
        public List<Language> LanguageList
        {
            get
            {
                return (langList.OrderBy(l => l.lang).ToList());
            }
        }

        private void PopulateLanguageList()
        {
            langList.Add(new Language { lang = "German", langCode = "de" });
            langList.Add(new Language { lang = "English", langCode = "en" });
            langList.Add(new Language { lang = "Russian", langCode = "ru" });
            langList.Add(new Language { lang = "Ukranian", langCode = "uk" });
            langList.Add(new Language { lang = "Simplified Chinese", langCode = "zh-CHS" });
        }

        //Yandex dictionary
        public XDocument GetTranslatedText(string textToTranslate, string fromLang, string toLang)
        {
            string translation = "";
            string uri = string.Format("https://dictionary.yandex.net/api/v1/dicservice/lookup?key={0}&lang={1}-{2}&text={3}", yandexKey, fromLang, toLang, textToTranslate);
            XDocument doc;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "application/xml";
            //request.Method = "PUT";
            //request.Proxy = new WebProxy("proxy.pmay.crp", 3128);
            //request.Proxy.Credentials = new NetworkCredential ("oelm", "oelm%~095858");
            WebResponse response = null;
            response = request.GetResponse();
            //using (Stream stream = response.GetResponseStream())
            //{
            //    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            //    System.IO.StreamReader translatedStream = new System.IO.StreamReader(stream, encode);
            //    System.Xml.XmlDocument xTranslation = new System.Xml.XmlDocument();
            //    xTranslation.LoadXml(translatedStream.ReadToEnd());
            //    translation = xTranslation.InnerXml;
            //}
            //return translation;

            using (Stream respStream = response.GetResponseStream())
            {
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.UTF8);
                string strResponse = rdr.ReadToEnd();
                doc = XDocument.Parse(@strResponse);
                translation = doc.ToString();
            }
            //foreach (var z in tresp.Translations.OrderByDescending(z => z.Rating))
            //    translation += z.TranslatedText + " (" + z.Rating.ToString() + ") ";
            return doc;
        }

    }


}
