using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    class Users
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double TimeBetweenSeans { get; set; }

        public Users() { }

        public Users ( List<Users> lUsers, string Name, double TimeBetweenSeans)
        {
            this.Id = Set_Id(lUsers);

            this.Name = Name;
            this.TimeBetweenSeans = TimeBetweenSeans;
        }

        /// <summary>
        /// Поиск незанятого Id в списке.
        /// </summary>
        /// <param name="lUsers"></param>
        /// <returns></returns>
        private int Set_Id( List<Users> lUsers)
        {
            int Id = 0;

            bool find; // Флаг занятости Id

            do
            {
                find = false; // Id в списке не найден

                foreach (var u in lUsers)
                {
                    if (u.Id == Id)
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
