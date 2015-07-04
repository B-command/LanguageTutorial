using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using LanguageTutorial.DataModel;


namespace LanguageTutorial
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool Registered { get; set; } // Проверка на успешную регистрацию

        public static bool ChangeUser { get; set; } // Проверка на смену пользователя

        public static User oActiveUser { get; set; } // Активный профиль

        public static Course oCourseEnglish { get; set; } // Английский курс и его настройки активного профиля
        public static Course oCourseFrançais { get; set; } // Французский курс и его настройки у активного профиля

        // Устанавливаются после нажатия клавиши "Принять" в окне настроек конкретного языка
        //public static Settings oTempSettingsEnglish { get; set; } // Временные Настройки английского языка активного профиля
        //public static Settings oTempSettingsFrançais { get; set; } // Временные Настройки французского языка активного профиля

        
        //Переменные для работы таймера
        public static System.Windows.Threading.DispatcherTimer aTimer;
        public static int EngSession = 0;
        public static int FranSession = 0;
        //public static int SessionIncrement = 0;


    }
}
