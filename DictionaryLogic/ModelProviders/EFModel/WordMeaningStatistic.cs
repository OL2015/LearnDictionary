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
    
    public partial class WordMeaningStatistic
    {
        public int WordMeaningStatistic_ID { get; set; }
        public int WordMeaning_ID { get; set; }
        public bool NewEntry { get; set; }
        public System.DateTime EntryTime { get; set; }
    
        public virtual WordMeaning WordMeaning { get; set; }
    }
}