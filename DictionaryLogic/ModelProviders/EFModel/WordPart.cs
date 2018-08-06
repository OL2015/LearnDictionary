using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryLogic.ModelProviders.EFModel
{

    public partial class Word
    {
        public string Translations { get { return GetTranslationsString(); } }
        public string BookTitles { get { return GetBookTitles(); } }


        private const float minRating = 5F;
        public float Rating
        {
            get
            {
                var generalEntries = WordMeanings.SelectMany(z => z.LearnStatistics);
                int generalCount = generalEntries.Count();
                var correctAnswer = generalEntries.Where(z => z.Answer).Count();
                int res = (int)(generalCount > minRating ? ((float)correctAnswer * 100F) / (float)generalCount : ((float)correctAnswer * 100F) / minRating);
                return (float )res;
            } 
        }
        public int EntriesCount
        {
            get
            {
                return this.WordEntries.Count;
            }
        }
        private string GetTranslationsString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var wMeaning in this.WordMeanings)
            {
                if (builder.Length == 0)
                    builder.Append(String.Format("  ->  {0}", wMeaning.Word1.Value));
                else
                    builder.Append(String.Format(", {0}", wMeaning.Word1.Value));
            }

            return builder.ToString();
        }


        private string GetBookTitles()
        {
            StringBuilder builder = new StringBuilder(); 
            var we = WordEntries
                  .GroupBy(l => l.Book_ID)
                  .Select(g => new
                  {
                      Name = g.First().Book.Name ,
                      Count = g.Select(l => l.Book_ID).Count()
                  });
            foreach (var z in we)
            {
                string q = String.Format(" {0} ({1})", z.Name, z.Count);
                if (builder.Length == 0)
                    builder.Append(String.Format("-> {0}", q ));
                else
                    builder.Append(String.Format(", {0}", q));
            }
            return builder.ToString();
        }
    }

    public partial class WordEntry
    {
        public string Translations { get { return GetTranslationsString(); } }

        private string GetTranslationsString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var wMeaning in this.Word.WordMeanings)
            {
                if (builder.Length == 0)
                    builder.Append(String.Format("{0}", wMeaning.Word1.Value));
                else
                    builder.Append(String.Format(", {0}", wMeaning.Word1.Value));
            }

            return builder.ToString();
            
        }


    }
}
