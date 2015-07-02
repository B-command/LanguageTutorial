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
    class SettingsRepository
    {
        public List<Settings> lSettings { get; set; }

        public SettingsRepository()
        {
            lSettings = new List<Settings>();
        }

        /// <summary>
        /// Загрузка списка из файла
        /// </summary>
        public void Load()
        {
            if (File.Exists("Settings.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Settings>));

                FileStream fs = new FileStream("Settings.xml", FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    lSettings = serializer.Deserialize(sr) as List<Settings>;
                }
            }

            if (lSettings == null)
            {
                lSettings = new List<Settings>();
            }
        }

        /// <summary>
        /// Сохранение списка в файл
        /// </summary>
        /// <param name="settings"></param>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Settings>));

            if (File.Exists("Settings.xml"))
            {
                File.Delete("Settings.xml");
            }

            FileStream fs = new FileStream("Settings.xml", FileMode.CreateNew);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, lSettings);
            }
        }
    }
}
