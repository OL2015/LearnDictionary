//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DictionaryLogic.ModelProviders.EFModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpeechPart
    {
        public SpeechPart()
        {
            this.WordMeanings = new HashSet<WordMeaning>();
        }
    
        public int SpeechPart_ID { get; set; }
        public string NameEng { get; set; }
        public string NameUkr { get; set; }
        public string ShortEng { get; set; }
        public string ShortUkr { get; set; }
    
        public virtual ICollection<WordMeaning> WordMeanings { get; set; }
    }
}