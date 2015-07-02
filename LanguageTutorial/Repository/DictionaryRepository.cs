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
    class DictionaryRepository
    {
        public List<Dictionary> lDictionary { get; set; }

        public DictionaryRepository()
        {
            lDictionary = new List<Dictionary>();
        }

        /// <summary>
        /// Загрузка списка из файла
        /// </summary>
        public void Load()
        {
            if (File.Exists("Dictionary.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Dictionary>));

                FileStream fs = new FileStream("Dictionary.xml", FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    lDictionary = serializer.Deserialize(sr) as List<Dictionary>;
                }
            }

            if (lDictionary == null)
            {
                lDictionary = new List<Dictionary>();
            }
        }

        /// <summary>
        /// Сохранение списка в файл
        /// </summary>
        /// <param name="settings"></param>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Dictionary>));

            if (File.Exists("Dictionary.xml"))
            {
                File.Delete("Dictionary.xml");
            }

            FileStream fs = new FileStream("Dictionary.xml", FileMode.CreateNew);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, lDictionary);
            }
        }
    }
}
