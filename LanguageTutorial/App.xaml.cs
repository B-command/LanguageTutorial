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

        public static User oActiveUser { get; set; } // Активный профиль

        public static Course oCourseEnglish { get; set; } // Английский курс и его настройки активного профиля
        public static Course oCourseFrançais { get; set; } // Французский курс и его настройки у активного профиля
    }
}
