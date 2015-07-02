using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial.DataModel
{
    class Course
    {
        public int Id { get; set; }
        public int Users_Id { get; set; }
        public int Settings_Id { get; set; }
        public int Languages_Id { get; set; }

        public Course() { }

        public Course(List<Course> lCourse, int Users_Id, int Settings_Id, int Languages_Id)
        {
            this.Id = Set_Id(lCourse);

            this.Users_Id = Users_Id;
            this.Settings_Id = Settings_Id;
            this.Languages_Id = Languages_Id;
        }

        /// <summary>
        /// Поиск незанятого Id в списке.
        /// </summary>
        /// <param name="lUsers"></param>
        /// <returns></returns>
        private int Set_Id(List<Course> lCourse)
        {
            int Id = 0;

            bool find; // Флаг занятости Id

            do
            {
                find = false; // Id в списке не найден

                foreach (var c in lCourse)
                {
                    if (c.Id == Id)
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
