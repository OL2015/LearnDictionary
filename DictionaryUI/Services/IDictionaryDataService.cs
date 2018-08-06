using DictionaryLogic.ModelProviders.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.Services
{
    public interface IDictionaryDataService
    {
        LearnDictionaryEntities GetEFLearnDictionaryContext();
        void ShowMariamWebster(string value); 
    }
}
