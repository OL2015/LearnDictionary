//using Yam.Microsoft.Translator;
//using Yam.Microsoft.Translator.TranslatorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows;
using System.Xml.Linq;
using System.Web;


namespace WPF_Language_Translator
{
    class TranslatorZZZ
    {
        private static string yandexKey = "dict.1.1.20160509T180521Z.e9d2fa185b293a93.4f71493279e0b728e917628709daf8b8f1af2f95";

        private List<Language> langList = new List<Language>();

        public TranslatorZZZ()
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
            // langList.Add(new Language { lang = "Arabic", langCode = "ar" });
            //  langList.Add(new Language { lang = "Czech", langCode = "cs" });
            // langList.Add(new Language { lang = "Danish", langCode = "da" });
            langList.Add(new Language { lang = "German", langCode = "de" });
            langList.Add(new Language { lang = "English", langCode = "en" });
            //langList.Add(new Language { lang = "Estonian", langCode = "et" });
            //langList.Add(new Language { lang = "Finnish", langCode = "fi" });
            //langList.Add(new Language { lang = "Dutch", langCode = "nl" });
            //langList.Add(new Language { lang = "Greek", langCode = "el" });
            //langList.Add(new Language { lang = "Hebrew", langCode = "he" });
            //langList.Add(new Language { lang = "Haitian Creole", langCode = "ht" });
            //langList.Add(new Language { lang = "Hindi", langCode = "hi" });
            //langList.Add(new Language { lang = "Hungarian", langCode = "hu" });
            //langList.Add(new Language { lang = "Indonesian", langCode = "id" });
            //langList.Add(new Language { lang = "Italian", langCode = "it" });
            //langList.Add(new Language { lang = "Japanese", langCode = "ja" });
            //langList.Add(new Language { lang = "Korean", langCode = "ko" });
            //langList.Add(new Language { lang = "Lithuanian", langCode = "lt" });
            //langList.Add(new Language { lang = "Latvian", langCode = "lv" });
            //langList.Add(new Language { lang = "Norwegian", langCode = "no" });
            //langList.Add(new Language { lang = "Polish", langCode = "pl" });
            //langList.Add(new Language { lang = "Portuguese", langCode = "pt" });
            //langList.Add(new Language { lang = "Romanian", langCode = "ro" });
            //langList.Add(new Language { lang = "Spanish", langCode = "es" });
            langList.Add(new Language { lang = "Russian", langCode = "ru" });
            //langList.Add(new Language { lang = "Slovak", langCode = "sk" });
            //langList.Add(new Language { lang = "Slovene", langCode = "sl" });
            //langList.Add(new Language { lang = "Swedish", langCode = "sv" });
            //langList.Add(new Language { lang = "Thai", langCode = "th" });
            //langList.Add(new Language { lang = "Turkish", langCode = "tr" });
            langList.Add(new Language { lang = "Ukranian", langCode = "uk" });
            //langList.Add(new Language { lang = "Vietnamese", langCode = "vi" });
            langList.Add(new Language { lang = "Simplified Chinese", langCode = "zh-CHS" });
            //langList.Add(new Language { lang = "Traditional Chinese", langCode = "zh-CHT" });
        }

        ///// <summary>
        ///// Get translated text from Bing Translator service.
        ///// </summary>
        ///// <param name="textToTranslate">Text to translate.</param>
        ///// <param name="fromLang">Language to translate from.</param>
        ///// <param name="toLang">Language to translate to.</param>
        ///// <returns>Translated text.</returns>
        //public string GetTranslatedText2(string textToTranslate, string fromLang, string toLang)
        //{
        //    string translation;

        //    if (fromLang != toLang)
        //    {
        //        string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?appId=" +
        //                    appID + "&text=" + textToTranslate + "&from=" + fromLang + "&to=" + toLang;

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

        //        try
        //        {
        //            WebResponse response = request.GetResponse();
        //            Stream strm = response.GetResponseStream();
        //            StreamReader sr = new StreamReader(strm);
        //            translation = sr.ReadToEnd();

        //            response.Close();
        //            sr.Close();
        //        }
        //        catch (WebException)
        //        {
        //            MessageBox.Show("Ensure that you are connected to the internet.",
        //                        "Translator", MessageBoxButton.OK, MessageBoxImage.Stop);
        //            return (string.Empty);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Will not translate to the same language.", "Translator",
        //                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        return (string.Empty);
        //    }

        //    // Parse string into an XElement and get the XElement Value
        //    // which is returned as the translated text.
        //    return (XElement.Parse(translation).Value);
        //}


        /// <summary>
        /// Get translated text from Azure Translator service.
        /// https://blogs.msdn.microsoft.com/translation/walkthrough/gettingstarted2/
        /// </summary>
        /// <param name="textToTranslate">Text to translate.</param>
        /// <param name="fromLang">Language to translate from.</param>
        /// <param name="toLang">Language to translate to.</param>
        /// <returns>Translated text.</returns> 
        static string clientID = "Ama_Learning_Dictionary";
        static string clientSecret = "FSJd/iYc3ZbB/IuuCs/b9miRQNMtsHE4QRn9CeREJoE=";
        public string GetTranslatedText1(string textToTranslate, string fromLang, string toLang)
        {
            string translation;
            String strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
            String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret));
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(strTranslatorAccessURI);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strRequestDetails);
            webRequest.ContentLength = bytes.Length;
            using (System.IO.Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            System.Net.WebResponse webResponse = webRequest.GetResponse();
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AdmAccessToken));
            //Get deserialized object from JSON stream 
            AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());

            string headerValue = "Bearer " + token.access_token;
            if (fromLang != toLang)
            {

                try
                {
                    string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(textToTranslate)
                        + "&from=" + fromLang + "&to=" + toLang;
                    System.Net.WebRequest translationWebRequest = System.Net.WebRequest.Create(uri);
                    translationWebRequest.Headers.Add("Authorization", headerValue);
                    System.Net.WebResponse response = null;
                    response = translationWebRequest.GetResponse();
                    System.IO.Stream stream = response.GetResponseStream();
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    System.IO.StreamReader translatedStream = new System.IO.StreamReader(stream, encode);
                    System.Xml.XmlDocument xTranslation = new System.Xml.XmlDocument();
                    xTranslation.LoadXml(translatedStream.ReadToEnd());
                    translation = xTranslation.InnerText;

                }
                catch (WebException ex)
                {
                    MessageBox.Show("Ensure that you are connected to the internet.",
                                "Translator", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return (string.Empty);
                }
            }
            else
            {
                MessageBox.Show("Will not translate to the same language.", "Translator",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return (string.Empty);
            }

            // Parse string into an XElement and get the XElement Value
            // which is returned as the translated text.
            return translation;
        }


        //public string GetTranslatedText3(string textToTranslate, string fromLang, string toLang)
        //{
        //    string translation;
        //    String strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        //    String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
        //        HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret));
        //    System.Net.WebRequest webRequest = System.Net.WebRequest.Create(strTranslatorAccessURI);
        //    webRequest.ContentType = "application/x-www-form-urlencoded";
        //    webRequest.Method = "POST";

        //    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strRequestDetails);
        //    webRequest.ContentLength = bytes.Length;
        //    using (System.IO.Stream outputStream = webRequest.GetRequestStream())
        //    {
        //        outputStream.Write(bytes, 0, bytes.Length);
        //    }
        //    System.Net.WebResponse webResponse = webRequest.GetResponse();
        //    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AdmAccessToken));
        //    //Get deserialized object from JSON stream 
        //    AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());

        //    string headerValue = "Bearer " + token.access_token;

        //    string uri = "http://api.microsofttranslator.com/v2/Http.svc/GetTranslationsArray";
        //    string requestBody = String.Format("<GetTranslationsArrayRequest>" +
        //   "  <AppId>{0}</AppId>" +
        //   "  <From>{1}</From>" +
        //   "  <Options>" +
        //   "  <Category xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">general</Category>" +
        //   "  <ContentType xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">text/plain</ContentType>" +
        //   "  <ReservedFlags xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\"/>" +
        //   "  <State xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\"/>" +
        //   "  <Uri xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">{2}</Uri>" +
        //   "  <User xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">{3}</User>" +
        //   "  </Options>" +
        //   "  <Texts>{6}</Texts>" +
        //   "  <To>{4}</To>" +
        //   "  <MaxTranslations>{5}</MaxTranslations>" +
        //   "</GetTranslationsArrayRequest>", "", fromLang, "", clientID, toLang, "3", "{0}");

        //    string translationsCollection = String.Empty;
        //    string[] textTranslations = { textToTranslate };
        //    // build the Translations collection
        //    translationsCollection += String.Format("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">{0}</string>", textTranslations[0]);
        //    translationsCollection += String.Format("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">{0}</string>", textTranslations[1]);
        //    // update the body
        //    requestBody = string.Format(requestBody, translationsCollection);

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        //    request.ContentType = "text/xml";
        //    request.Headers.Add("Authorization", headerValue);
        //    request.Method = "POST";
        //    using (System.IO.Stream stream = request.GetRequestStream())
        //    {
        //        byte[] arrBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(requestBody);
        //        stream.Write(arrBytes, 0, arrBytes.Length);
        //    }
        //    WebResponse response = null;
        //    try
        //    {
        //        response = request.GetResponse();
        //        using (Stream respStream = response.GetResponseStream())
        //        {
        //            StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.ASCII);
        //            string strResponse = rdr.ReadToEnd();
        //            XDocument doc = XDocument.Parse(@strResponse);
        //            XNamespace ns = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2";
        //            int i = 0;
        //            foreach (XElement xe in doc.Descendants(ns + "GetTranslationsResponse"))
        //            {
        //                Console.WriteLine("\n\nSource Text: '{0}' Results", textTranslations[i++]);
        //                int j = 1;
        //                foreach (XElement xe2 in xe.Descendants(ns + "TranslationMatch"))
        //                {
        //                    Console.WriteLine("\nCustom translation :{0} ", j++);
        //                    foreach (var node in xe2.Elements())
        //                    {
        //                        Console.WriteLine("{0} = {1}", node.Name.LocalName, node.Value);
        //                    }
        //                }
        //            }
        //        }
        //        Console.WriteLine("Press any key to continue...");
        //        Console.ReadKey(true);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (response != null)
        //        {
        //            response.Close();
        //            response = null;
        //        }
        //    }
        //    return "";
        //}

        public string GetTranslatedText123(string textToTranslate, string fromLang, string toLang)
        {
            string translation = "";
            string headerValue = GetAccessTokenString();
            // AddTranslationMethod(headerValue);
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/GetTranslations?text=" + textToTranslate + "&from=" + fromLang + "&to=" + toLang + "&maxTranslations=5";


            string requestBody = GenerateTranslateOptionsRequestBody("general", "text/plain", "", "", "", "zzz2");

            string options = GenerateTranslateOptionsRequestBody("16");
            requestBody = options;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("Authorization", headerValue);
            request.ContentType = "text/xml";
            request.Method = "POST";
            using (System.IO.Stream stream = request.GetRequestStream())
            {

                byte[] arrBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(requestBody);
                stream.Write(arrBytes, 0, arrBytes.Length);
            }
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                using (Stream respStream = response.GetResponseStream())
                {
                    StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.UTF8);
                    string strResponse = rdr.ReadToEnd();

                    Console.WriteLine(string.Format("Available translations for source text '{0}' are", textToTranslate));
                    XDocument doc = XDocument.Parse(@strResponse);
                    XNamespace ns = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2";
                    int i = 1;
                    foreach (XElement xe in doc.Descendants(ns + "TranslationMatch"))
                    {
                        Console.WriteLine("{0}Result {1}", Environment.NewLine, i++);
                        foreach (var node in xe.Elements())
                        {
                            if (node.Name.LocalName == "TranslatedText")
                                translation += node.Value.ToString() + " ";
                            Console.WriteLine("{0} = {1}", node.Name.LocalName, node.Value);
                        }
                    }
                }

                Console.WriteLine("Press any key to continue...");
            }
            catch
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }


            return translation.Trim();
        }

        private string GetAccessTokenString()
        {
            String strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
            String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret));
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(strTranslatorAccessURI);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strRequestDetails);
            webRequest.ContentLength = bytes.Length;
            using (System.IO.Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            System.Net.WebResponse webResponse = webRequest.GetResponse();
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AdmAccessToken));
            //Get deserialized object from JSON stream 
            AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());

            string headerValue = "Bearer " + token.access_token;
            return headerValue;
        }

        private static string GenerateTranslateOptionsRequestBody(string category, string contentType, string ReservedFlags, string State, string Uri, string user)
        {
            string body = "<TranslateOptions xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">" +
    "  <Category>{0}</Category>" +
    "  <ContentType>{1}</ContentType>" +
    "  <ReservedFlags>{2}</ReservedFlags>" +
    "  <State>{3}</State>" +
    "  <Uri>{4}</Uri>" +
    "  <User>{5}</User>" +
     "  <IncludeMultipleMTAlternatives>{6}</IncludeMultipleMTAlternatives>" +
    "</TranslateOptions>";
            return string.Format(body, category, contentType, ReservedFlags, State, Uri, user, true);
        }
        private static void ProcessWebException(WebException e)
        {
            Console.WriteLine("{0}", e.ToString());
            // Obtain detailed error information
            string strResponse = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)e.Response)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream, System.Text.Encoding.ASCII))
                    {
                        strResponse = sr.ReadToEnd();
                    }
                }
            }
            Console.WriteLine("Http status code={0}, error message={1}", e.Status, strResponse);
        }
        private static void AddTranslationMethod(string authToken)
        {
            HttpWebRequest httpWebRequest = null;
            WebResponse response = null;
            string originaltext = "go";
            string translatedtext = "povzty";
            string from = "en";
            string to = "uk";
            string user = clientID;
            string addTranslationuri = "http://api.microsofttranslator.com/V2/Http.svc/AddTranslation?originaltext=" + originaltext
                                + "&translatedtext=" + translatedtext
                                + "&from=" + from
                                + "&to=" + to
                                + "&user=" + user;
            httpWebRequest = (HttpWebRequest)WebRequest.Create(addTranslationuri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            try
            {
                response = httpWebRequest.GetResponse();
                using (Stream strm = response.GetResponseStream())
                {
                    Console.WriteLine(String.Format("Translation for {0} has been added successfully.", originaltext));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
            Console.WriteLine("Press any key to continue...");

        }

        private static string GenerateTranslateOptionsRequestBody(string State)
        {
            string body = "<TranslateOptions xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">" +
    "  <State>{0}</State>" +
    "  <IncludeMultipleMTAlternatives>{1}</IncludeMultipleMTAlternatives>" +
    "</TranslateOptions>";
            return string.Format(body, State, true);
        }

        //public string GetTranslatedText12(string textToTranslate, string fromLang, string toLang)
        //{
        //    string translation = "";
        //    string headerValue = GetAccessTokenString();
        //    // AddTranslationMethod(headerValue);
        //    string uri = "http://api.microsofttranslator.com/v2/Http.svc/GetTranslations?text=" + textToTranslate + "&from=" + fromLang + "&to=" + toLang + "&maxTranslations=5";

        //    Yam.Microsoft.Translator.Translator tr = new Yam.Microsoft.Translator.Translator(clientID, clientSecret, fromLang, toLang);
        //    var zzz = tr.Translate(textToTranslate);

        //    Yam.Microsoft.Translator.TranslatorService.TranslateOptions to = new Yam.Microsoft.Translator.TranslatorService.TranslateOptions();
        //    to.IncludeMultipleMTAlternatives = true;
        //    string zzz2 = to.ToString();
        //    LanguageServiceClient ls = tr.LanguageService;
        //    string requestBody = GenerateTranslateOptionsRequestBody("general", "text/plain", "", "", "", "zzz2");
        //    AdmAuthentication auth = new AdmAuthentication(clientID, clientSecret);
        //    var tkn = auth.GetAccessToken();
        //    var tresp = ls.GetTranslations(headerValue, textToTranslate, fromLang, toLang, 10, to);
        //    foreach (var z in tresp.Translations.OrderByDescending(z => z.Rating))
        //        translation += z.TranslatedText + " (" + z.Rating.ToString() + ") ";
        //    return translation.Trim();
        //}

        //Yandex dictionary
        public string GetTranslatedText(string textToTranslate, string fromLang, string toLang)
        {
            string translation = "";
            string uri = string.Format("https://dictionary.yandex.net/api/v1/dicservice/lookup?key={0}&lang={1}-{2}&text={3}", yandexKey, fromLang, toLang, textToTranslate);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "text/xml";
            request.Method = "POST";

            WebResponse response = null;
            response = request.GetResponse();
            using (Stream respStream = response.GetResponseStream())
            {
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.UTF8);
                string strResponse = rdr.ReadToEnd();

                Console.WriteLine(string.Format("Available translations for source text '{0}' are", textToTranslate));
                XDocument doc = XDocument.Parse(@strResponse);
                translation = doc.ToString();
            }
            //foreach (var z in tresp.Translations.OrderByDescending(z => z.Rating))
            //    translation += z.TranslatedText + " (" + z.Rating.ToString() + ") ";

                       return translation;
        }

    }



    public class AdmAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }
}
