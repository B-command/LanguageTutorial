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
        public static UsersRepository oUsersRepository { get; set; } // БД Пользователи
        public static LanguagesRepository oLanguagesRepository { get; set; } // БД Языки 
        public static SettingsRepository oSettingsRepository { get; set; } // БД Настройки
        public static DictionaryRepository oDictionaryRepository { get; set; } // БД Словарь
        public static CourseRepository oCourseRepository { get; set; } // БД Курсов
        public static SessionRepository oSessionRepository { get; set; } // БД Сессий

        public static bool Registered { get; set; } // Проверка на успешную регистрацию

        public static Users oActiveUser { get; set; } // Активный профиль

        // Устанавлвиаются после нажатия клавиши "Принять" в окне настроек\регистрации
        public static Settings oActiveSettingsEnglish { get; set; } // Настройки английского языка активного профиля
        public static Settings oActiveSettingsFrançais { get; set; } // Настройки французского языка активного профиля

        // Устанавливаются после нажатия клавиши "Принять" в окне настроек конкретного языка
        //public static Settings oTempSettingsEnglish { get; set; } // Временные Настройки английского языка активного профиля
        //public static Settings oTempSettingsFrançais { get; set; } // Временные Настройки французского языка активного профиля

        
        //Переменные для работы таймера
        public static System.Windows.Threading.DispatcherTimer aTimer;
        public static int EngSession = 0;
        public static int FranSession = 0;
        //public static int SessionIncrement = 0;

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

            if (oLanguagesRepository.lLanguages.Count == 0)
            {
                Languages lang = new Languages(oLanguagesRepository.lLanguages, "English");
                oLanguagesRepository.lLanguages.Add(lang);
                lang = new Languages(oLanguagesRepository.lLanguages, "Français");
                oLanguagesRepository.lLanguages.Add(lang);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {

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
