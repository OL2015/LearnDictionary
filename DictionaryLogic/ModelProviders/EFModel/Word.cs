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
    
    public partial class Word
    {
        public Word()
        {
            this.WordMeanings = new HashSet<WordMeaning>();
            this.WordMeanings1 = new HashSet<WordMeaning>();
            this.WordEntries = new HashSet<WordEntry>();
        }
    
        public int Word_ID { get; set; }
        public string Value { get; set; }
        public int Language_ID { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual ICollection<WordMeaning> WordMeanings { get; set; }
        public virtual ICollection<WordMeaning> WordMeanings1 { get; set; }
        public virtual ICollection<WordEntry> WordEntries { get; set; }
    }
}