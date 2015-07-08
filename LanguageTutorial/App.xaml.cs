using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.IO;
using System.Data.Entity;

using LanguageTutorial.DataModel;


namespace LanguageTutorial
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool Registered { get; set; } // Проверка на успешную регистрацию.

        public static bool ChangeUser { get; set; } // Проверка на состояние смену пользователя.
        public static bool UserChanged { get; set; } // Смена пользователя произошла.

        public static User oActiveUser { get; set; } // Активный профиль

        public static Course oCourseEnglish { get; set; } // Английский курс и его настройки активного профиля
        public static Course oCourseFrançais { get; set; } // Французский курс и его настройки у активного профиля

        //Переменные для работы таймера
        public static System.Windows.Threading.DispatcherTimer aTimer;
        public static int EngSession = 0;
        public static int FranSession = 0;

        //public static bool activeWin = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.ChangeUser = false;

            // Загрузка языков и их слов при 1 запуске программы
            //using ( var db = new LanguageTutorialContext())
            //{
            //    if ( db.Language.ToList().Count == 0 )
            //    { // Если таблица пуста, значит программа запущена впервые.

            //        // Заполнение языков
            //        db.Language.Add(new Language() { Name = "English" });
            //        db.SaveChanges();

            //        db.Language.Add(new Language() { Name = "Français"});
            //        db.SaveChanges();

            //        // Заполнение словаря Английского языка
            //        using ( var loadDictionary = new StreamReader("Dictionary\\English.txt"))
            //        {
            //            string str = String.Empty;

            //            while (str != null)
            //            {
            //                str = loadDictionary.ReadLine();
            //                // добавляем новый элемент в коллекцию, хранящую словарь
            //                if (str != null)
            //                {
            //                    string[] wordAndTranslationd = str.Trim().Split(new char[] { ',' });

            //                    db.WordDictionary.Add(new WordDictionary() { LanguageId = 1, Word = wordAndTranslationd[0], Translate = wordAndTranslationd[1] });
            //                    db.SaveChanges();
            //                }
            //            }

            //            loadDictionary.Close();
            //        }

            //        // Заполнение словаря Французского языка
            //        using (var loadDictionary = new StreamReader("Dictionary\\Français.txt"))
            //        {
            //            string str = String.Empty;

            //            while (str != null)
            //            {
            //                str = loadDictionary.ReadLine();

            //                if (str != null)
            //                {
            //                    string[] wordAndTranslationd = str.Trim().Split(new char[] { ',' });

            //                    db.WordDictionary.Add(new WordDictionary() { LanguageId = 2, Word = wordAndTranslationd[0], Translate = wordAndTranslationd[1] });
            //                    db.SaveChanges();
            //                }
            //            }

            //            loadDictionary.Close();
            //        }
            //    }
            //}
        }


    }
}
