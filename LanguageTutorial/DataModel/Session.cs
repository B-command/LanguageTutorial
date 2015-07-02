using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    class Session
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public int NumberOfPoints { get; set; }
        public int NumberOfWords { get; set; }
        public int NumberOfFinishedWords { get; set; }
        public int Users_Id { get; set; }
        public int Languages_Id { get; set; }

        public Session() { }

        public Session( List<Session> lSession, DateTime Datetime, int NumberOfPoints, int NumberOfWords, int NumberOfFinishedWords, int Users_Id, int Languages_Id )
        {
            this.Id = Set_Id(lSession);

            this.Datetime = Datetime;
            this.NumberOfPoints = NumberOfPoints;
            this.NumberOfWords = NumberOfWords;
            this.NumberOfFinishedWords = NumberOfFinishedWords;
            this.Users_Id = Users_Id;
            this.Languages_Id = Languages_Id;
        }

        /// <summary>
        /// Поиск незанятого Id в списке.
        /// </summary>
        /// <param name="lUsers"></param>
        /// <returns></returns>
        private int Set_Id(List<Session> lSession)
        {
            int Id = 0;

            bool find; // Флаг занятости Id

            do
            {
                find = false; // Id в списке не найден

                foreach (var s in lSession)
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
