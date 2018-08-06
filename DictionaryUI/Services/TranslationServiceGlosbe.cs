using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI.Model;
using System.Net.Http;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Net;
using System.IO;

namespace DictionaryUI.Services
{
    public class TranslationServiceGlosbe : ITranslationService
    {
        private static HttpClient httpClient = new HttpClient();
        private static string globseURI = "https://glosbe.com/gapi/translate?from={0}&dest={1}&phrase={2}&format=xml&pretty=true";
        public TranslationServiceGlosbe()
        { httpClient.Timeout = new TimeSpan(0, 0, 20); }

        private XDocument GetTranslationAsync2(string textToTranslate, string fromLang, string toLang)
        {
            XDocument doc;
            string translation = "";
            string uri = string.Format(globseURI, fromLang, toLang, textToTranslate);
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
            return doc;
        }
        private async Task<XDocument> GetTranslationAsync(string textToTranslate, string fromLang, string toLang)
        {

            string translation = "";
            string uri = string.Format(globseURI,  fromLang, toLang, textToTranslate);

            string strResponse = await httpClient.GetStringAsync(uri);
            XDocument doc = XDocument.Parse(@strResponse);
            translation = doc.ToString();
            return doc;
        }

        public Task<int> GetRating(TranslationItem item, string fromLang, string toLang)
        {
            return
               Task.Run(async () =>
               {     
                   return 1;
               });
        }

        public async Task<ObservableCollection<TranslationItem>> GetTranslations(string textToTranslate, string fromLang, string toLang)
        {
            try
            {
                XDocument xml = await GetTranslationAsync(textToTranslate, fromLang, toLang);
                if (xml.Root == null)
                    return new ObservableCollection<TranslationItem>();
                string xpath = @"//list/map/entry[string='phrase']//values/string[1]";
                var defs = from a in xml.XPathSelectElements(xpath)
                           select a;
                var poses = from a in defs
                            select 
                            new TranslationItem()
                            {
                                OriginalWord = textToTranslate,
                                Translation =(string) a.Value,
                                MainMeaning = (string)a.Value,
                            };
                ObservableCollection<TranslationItem> TranslatedItems = new ObservableCollection<TranslationItem>(poses);
                return TranslatedItems;
            }
            catch (Exception ex)
            {
                throw; // rethrow exception untill smth wiser
            }
        }

        public Task SetItemRating(TranslationItem item, string fromLang, string toLang)
        {
            return
               Task.Run(async () =>
               {
                   Thread.Sleep(2000);
                   int index;
                   ObservableCollection<TranslationItem> translItems = await GetTranslations(item.Translation, fromLang, toLang);
                   var zzz = translItems.FirstOrDefault(z => z.Translation == item.OriginalWord && z.LangPart == item.LangPart);
                   index = zzz == null ? -1 : (100 - translItems.IndexOf(zzz));
                   item.Rating = index;
               });
        }
    }
}
