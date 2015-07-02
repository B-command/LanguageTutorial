using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

using LanguageTutorial.DataModel;

namespace LanguageTutorial.Repository
{
    public class CourseRepository
    {
        public List<Course> lCourse { get; set; }

        public CourseRepository()
        {
            lCourse = new List<Course>();
        }

        /// <summary>
        /// Загрузка списка из файла
        /// </summary>
        public void Load()
        {
            if (File.Exists("Course.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Course>));

                FileStream fs = new FileStream("Course.xml", FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    lCourse = serializer.Deserialize(sr) as List<Course>;
                }
            }

            if (lCourse == null)
            {
                lCourse = new List<Course>();
            }
        }

        /// <summary>
        /// Сохранение списка в файл
        /// </summary>
        /// <param name="settings"></param>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Course>));

            if (File.Exists("Course.xml"))
            {
                File.Delete("Course.xml");
            }

            FileStream fs = new FileStream("Course.xml", FileMode.CreateNew);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, lCourse);
            }
        }
    }
}
