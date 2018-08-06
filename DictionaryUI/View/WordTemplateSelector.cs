using DictionaryLogic.ModelProviders.EFModel;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryUI.View
{

    public class WordTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WordTemplate { get; set; } 

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Word)
                return WordTemplate; 
            return base.SelectTemplate(item, container);
        }
    }
}
