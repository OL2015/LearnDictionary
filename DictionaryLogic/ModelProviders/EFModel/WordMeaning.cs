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
    
    public partial class WordMeaning
    {
        public WordMeaning()
        {
            this.LearnStatistics = new HashSet<LearnStatistic>();
            this.WordMeaningStatistics = new HashSet<WordMeaningStatistic>();
            this.WordEntries = new HashSet<WordEntry>();
        }
    
        public int WordMeaning_ID { get; set; }
        public int MasterWord_ID { get; set; }
        public int TranslateWord_ID { get; set; }
        public Nullable<int> SpeechPart_ID { get; set; }
    
        public virtual ICollection<LearnStatistic> LearnStatistics { get; set; }
        public virtual Word Word { get; set; }
        public virtual Word Word1 { get; set; }
        public virtual ICollection<WordMeaningStatistic> WordMeaningStatistics { get; set; }
        public virtual SpeechPart SpeechPart { get; set; }
        public virtual ICollection<WordEntry> WordEntries { get; set; }
    }
}
