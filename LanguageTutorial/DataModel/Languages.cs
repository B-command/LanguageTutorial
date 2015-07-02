using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    class Languages
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Languages() { }

        public Languages(List<Languages> lLanguages, string Name)
        {
            this.Id = Set_Id(lLanguages);

            this.Name = Name;
        }

        /// <summary>
        /// Поиск незанятого Id в списке.
        /// </summary>
        /// <param name="lUsers"></param>
        /// <returns></returns>
        private int Set_Id(List<Languages> lLanguages)
        {
            int Id = 0;

            bool find; // Флаг занятости Id

            do
            {
                find = false; // Id в списке не найден

                foreach (var l in lLanguages)
                {
                    if (l.Id == Id)
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
