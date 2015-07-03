using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    public class Course
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int WordsPerSession { get; set; }
        public int WordsToStudy { get; set; }
        public int SeansPerDay { get; set; }
        public int TrueAnswers { get; set; }

    }
}
