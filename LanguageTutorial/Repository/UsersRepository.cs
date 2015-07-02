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
    public class UsersRepository
    {
        public List<Users> lUsers { get; set; }

        public UsersRepository()
        {
            lUsers = new List<Users>();
        }

        /// <summary>
        /// Загрузка списка из файла
        /// </summary>
        public void Load()
        {
            if (File.Exists("Users.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Users>));

                FileStream fs = new FileStream("Users.xml", FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    lUsers = serializer.Deserialize(sr) as List<Users>;
                }
            }

            if (lUsers == null)
            {
                lUsers = new List<Users>();
            }
        }

        /// <summary>
        /// Сохранение списка в файл
        /// </summary>
        /// <param name="settings"></param>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Users>));

            if (File.Exists("Users.xml"))
            {
                File.Delete("Users.xml");
            }

            FileStream fs = new FileStream("Users.xml", FileMode.CreateNew);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, lUsers);
            }
        }
    }
}
