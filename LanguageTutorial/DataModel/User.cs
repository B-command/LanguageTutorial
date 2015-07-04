using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public double SessionPeriod { get; set; }

        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<WordQueue> WordQueue { get; set; }
    }
}
