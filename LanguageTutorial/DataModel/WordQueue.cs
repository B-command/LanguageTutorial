using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    class WordQueue
    {
        public int Id { get; set; }

        public int TrueAnswers { get; set; }
        public bool IsLearned { get; set; }

        public int User_Id { get; set; }
        public int Word_Id { get; set; }

        public virtual Users User { get; set; }
        public virtual Word Word { get; set; }
    }
}
