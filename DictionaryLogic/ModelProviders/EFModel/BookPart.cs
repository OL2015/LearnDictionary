using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DictionaryLogic.ModelProviders.EFModel
{
   public partial class Book
    {
        public string AuthorsString
        {
            get
            {
                return GetAuthorsString(this.Authors.ToList());
            }
        }
        private static string GetAuthorsString(List<Author> authorsList)
        {
            if (authorsList == null || authorsList.Count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (Author author in authorsList)
            {
                if (sb.Length == 0)
                    sb.AppendFormat("{0}", author.Name);
                else
                    sb.AppendFormat(", {0}", author.Name);
            }
            return sb.ToString();
        }
    }
}
