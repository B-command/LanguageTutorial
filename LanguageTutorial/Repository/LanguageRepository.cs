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
    class LanguagesRepository
    {
        public List<Languages> lLanguages { get; set; }

        public LanguagesRepository()
        {
            lLanguages = new List<Languages>();
        }

        /// <summary>
        /// Загрузка списка из файла
        /// </summary>
        public void Load()
        {
            if (File.Exists("Languages.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Languages>));

                FileStream fs = new FileStream("Languages.xml", FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    lLanguages = serializer.Deserialize(sr) as List<Languages>;
                }
            }

            if (lLanguages == null)
            {
                lLanguages = new List<Languages>();
            }
        }

        /// <summary>
        /// Сохранение списка в файл
        /// </summary>
        /// <param name="settings"></param>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Languages>));

            if (File.Exists("Languages.xml"))
            {
                File.Delete("Languages.xml");
            }

            FileStream fs = new FileStream("Languages.xml", FileMode.CreateNew);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, lLanguages);
            }
        }
    }
}
