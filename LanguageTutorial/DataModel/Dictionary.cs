using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    class Dictionary
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Translate { get; set; }
        public int NumberOfTrueAnswers { get; set; }
        public bool Learned { get; set; }
        public int Languages_Id { get; set; }
        public int Course_Id { get; set; }

        public Dictionary() { }

        public Dictionary(List<Dictionary> lDictionary, string Word, string Translate, int NumberOfTrueAnswers, bool Learned, int Languages_Id, int Course_Id)
        {
            this.Id = Set_Id(lDictionary);

            this.Word = Word;
            this.Translate = Translate;
            this.NumberOfTrueAnswers = NumberOfTrueAnswers;
            this.Learned = Learned;
            this.Languages_Id = Languages_Id;
            this.Course_Id = Course_Id;
        }

        /// <summary>
        /// Поиск незанятого Id в списке.
        /// </summary>
        /// <param name="lUsers"></param>
        /// <returns></returns>
        private int Set_Id(List<Dictionary> lDictionary)
        {
            int Id = 0;

            bool find; // Флаг занятости Id

            do
            {
                find = false; // Id в списке не найден

                foreach (var d in lDictionary)
                {
                    if (d.Id == Id)
                    {
                        find = true; // Id в списке найден
                        Id++; // Проверяем следующий

                        break;
                    }
                }

            } while (find);

            return Id;
        }

    }
}
