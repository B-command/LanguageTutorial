using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<Dictionary> Dictionary { get; set; }
    }
}
