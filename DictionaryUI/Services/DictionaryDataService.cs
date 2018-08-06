using DictionaryLogic;
using DictionaryLogic.ModelProviders.EFModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.Services
{
    public class DictionaryDataService : IDictionaryDataService
    {
        DictionaryFacade facade = null;
        public LearnDictionaryEntities GetEFLearnDictionaryContext()
        {
            if (facade == null)
            {  
                facade = DictionaryFacade.GetFacade();
                facade.ConnectionString = Properties.Settings.Default.ConnStr;
                facade.ConnectionStringEF = ConfigurationManager.ConnectionStrings["LearnDictionaryEntities"].ConnectionString;
            }

            return facade.GetEFLearnDictionaryContext("LearnDictionaryConnectionString");
        }

        public void ShowMariamWebster(string value)
        {  
            string uri = string.Format(@"https://www.merriam-webster.com/dictionary/{0}?pronunciation&lang=en_us&dir=d", value);
            System.Diagnostics.Process.Start(uri); 
        }
    }
}
