using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    public class Session
    {
        public int Id { get; set; }

        public DateTime Datetime { get; set; }
        public int Points { get; set; }
        public int Words { get; set; }
        public int FinishedWords { get; set; }

        public int Course_Id { get; set; }

        public virtual Course Course { get; set; }
    }
}
