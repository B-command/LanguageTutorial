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
    class SessionRepository
    {
        public List<Session> lSession { get; set; }

        public SessionRepository()
        {
            lSession = new List<Session>();
        }

        /// <summary>
        /// Загрузка списка из файла
        /// </summary>
        public void Load()
        {
            if (File.Exists("Session.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Session>));

                FileStream fs = new FileStream("Session.xml", FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    lSession = serializer.Deserialize(sr) as List<Session>;
                }
            }

            if (lSession == null)
            {
                lSession = new List<Session>();
            }
        }

        /// <summary>
        /// Сохранение списка в файл
        /// </summary>
        /// <param name="settings"></param>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Session>));

            if (File.Exists("Session.xml"))
            {
                File.Delete("Session.xml");
            }

            FileStream fs = new FileStream("Session.xml", FileMode.CreateNew);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, lSession);
            }
        }
    }
}
