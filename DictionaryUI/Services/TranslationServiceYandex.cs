
using DictionaryUI.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;
namespace DictionaryUI.Services
{

    public class TranslationServiceYandex : ITranslationService
    {
        private static HttpClient httpClient = new HttpClient();
        private static string yandexKey = "dict.1.1.20160509T180521Z.e9d2fa185b293a93.4f71493279e0b728e917628709daf8b8f1af2f95";
        private static string yandexURI = "https://dictionary.yandex.net/api/v1/dicservice/lookup?key={0}&lang={1}-{2}&text={3}";
        public TranslationServiceYandex()
        { httpClient.Timeout = new TimeSpan(0, 0, 20); }

        private XDocument GetTranslationAsync2(string textToTranslate, string fromLang, string toLang)
        {
            XDocument doc;
            string translation = "";
            string uri = string.Format(yandexURI, yandexKey, fromLang, toLang, textToTranslate);
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
            string uri = string.Format(yandexURI, yandexKey, fromLang, toLang, textToTranslate);
            
            string strResponse = await httpClient.GetStringAsync(uri );
            XDocument doc = XDocument.Parse(@strResponse);
            translation = doc.ToString();
            return doc; 
        }
        public async Task<ObservableCollection<TranslationItem>> GetTranslations(  string textToTranslate, string fromLang, string toLang)
        {
            try
            { 
                XDocument xml = await  GetTranslationAsync(textToTranslate, fromLang, toLang);
                if (xml.Root == null)
                    return new ObservableCollection<TranslationItem>(); ;
                var defs = from a in xml.Root.Elements("def")
                           select a;
                var poses = from a in defs
                            let langPart = (string)a.Attribute("pos")
                            from b in a.Descendants("tr")
                            select new TranslationItem()
                            {
                                OriginalWord = textToTranslate,
                                LangPart = langPart,
                                Translation = (string)b.Element("text"),
                                MainMeaning = b.Element("mean") == null ? null : (string)b.Element("mean").Element("text")
                            };
                ObservableCollection<TranslationItem> TranslatedItems = new ObservableCollection<TranslationItem>(poses); 
                return TranslatedItems;
            }
            catch (Exception ex)
            {
                throw; // rethrow exception untill smth wiser
            }

        }

        public Task<int> GetRating(TranslationItem item, string fromLang, string toLang)
        {
            return
               Task.Run(async () =>
               {
                   
                   int index;
                   ObservableCollection<TranslationItem> translItems = await GetTranslations(item.Translation, fromLang, toLang);
                   var zzz = translItems.FirstOrDefault(z => z.Translation == item.OriginalWord && z.LangPart == item.LangPart);
                   index = zzz == null ? -1 : (100 - translItems.IndexOf(zzz));
                   return index;
               });
        }
        public  Task SetItemRating(TranslationItem item, string fromLang, string toLang)
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
        //return;
            //return
            //    Task.Run(() =>
            //    {
            //        var item1 = item;
            //        Random rnd = new Random(item.Translation.Length);
            //        int delay = rnd.Next(1000, 10000);

            //        Thread.Sleep(delay);
            //        Random rnd1 = new Random(delay);
            //        int res = rnd.Next(1, 15) + rnd1.Next(0, 15);
            //        item.Rating = delay;
            //        return delay; //res;
            //    });
        }
    }
}
