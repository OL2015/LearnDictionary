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
    
    public partial class WordEntry
    {
        public WordEntry()
        {
            this.WordMeanings = new HashSet<WordMeaning>();
        }
    
        public int WordEntry_ID { get; set; }
        public int Book_ID { get; set; }
        public int Word_ID { get; set; }
        public int Page { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual Word Word { get; set; }
        public virtual ICollection<WordMeaning> WordMeanings { get; set; }
    }
}
