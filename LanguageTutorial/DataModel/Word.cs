﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    public class Word
    {
        public int Id { get; set; }

        public string Word { get; set; }
        public string Translate { get; set; }

        public int Language_Id { get; set; }

        public virtual Language Language { get; set; }

        public virtual ICollection<WordQueue> WordQueue { get; set; }
    }
}
