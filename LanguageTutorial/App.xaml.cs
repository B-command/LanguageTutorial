using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using LanguageTutorial.DataModel;
using LanguageTutorial.Repository;

namespace LanguageTutorial
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UsersRepository oUsersRepository { get; set; }
        public static LanguagesRepository oLanguagesRepository { get; set; }
        public static SettingsRepository oSettingsRepository { get; set; }
        public static DictionaryRepository oDictionaryRepository { get; set; }
        public static CourseRepository oCourseRepository { get; set; }
        public static SessionRepository oSessionRepository { get; set; }

        public static Users oActiveUser { get; set; } // Активный профиль
        public static bool Registered { get; set; } // Проверка на успешную регистрацию
        public static Settings oSettingsEnglish { get; set; }
        public static Settings oSettingsFrançais { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            oUsersRepository = new UsersRepository();
            oLanguagesRepository = new LanguagesRepository();
            oSettingsRepository = new SettingsRepository();
            oDictionaryRepository = new DictionaryRepository();
            oCourseRepository = new CourseRepository();
            oSessionRepository = new SessionRepository();

            oUsersRepository.Load();
            oLanguagesRepository.Load();
            oSettingsRepository.Load();
            oDictionaryRepository.Load();
            oCourseRepository.Load();
            oSessionRepository.Load();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //Languages lang = new Languages(oLanguagesRepository.lLanguages, "English");
            //oLanguagesRepository.lLanguages.Add(lang);
            //lang = new Languages(oLanguagesRepository.lLanguages, "Français");
            //oLanguagesRepository.lLanguages.Add(lang);
            oUsersRepository.Save();
            oLanguagesRepository.Save();
            oSettingsRepository.Save();
            oDictionaryRepository.Save();
            oCourseRepository.Save();
            oSessionRepository.Save();

            base.OnExit(e);
        }

    }
}
