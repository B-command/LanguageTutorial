using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    public class WordQueue
    {
        public int Id { get; set; }

        public int TrueAnswers { get; set; }
        public bool IsLearned { get; set; }

        public int UserId { get; set; }
        public int WordDictionaryId { get; set; }

        public virtual User User { get; set; }
        public virtual WordDictionary WordDictionary { get; set; }
    }
}
