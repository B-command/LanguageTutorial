using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    public class Settings
    {
        public int Id { get; set; }
        public int NumberOfWordsPerSeans { get; set; }
        public int NumberOfWordsToStudy { get; set; }
        public int NumberOfSeansPerDay { get; set; }
        public int NumberOfTrueAnswers { get; set; }

        public Settings() { }

        public Settings( List<Settings> lSettings, int NumberOfWordsPerSeans, int NumberOfWordsToStudy, int NumberOfSeansPerDay, int NumberOfTrueAnswers)
        {
            this.Id = Set_Id(lSettings);

            this.NumberOfWordsPerSeans = NumberOfWordsPerSeans;
            this.NumberOfWordsToStudy = NumberOfWordsToStudy;
            this.NumberOfSeansPerDay = NumberOfSeansPerDay;
            this.NumberOfTrueAnswers = NumberOfTrueAnswers;
        }

        /// <summary>
        /// Поиск незанятого Id в списке.
        /// </summary>
        /// <param name="lUsers"></param>
        /// <returns></returns>
        private int Set_Id(List<Settings> lSettings)
        {
            int Id = 0;

            bool find; // Флаг занятости Id

            do
            {
                find = false; // Id в списке не найден

                foreach (var s in lSettings)
                {
                    if (s.Id == Id)
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
