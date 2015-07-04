using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using LanguageTutorial.DataModel;

namespace LanguageTutorial
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/catsdogs.png");
            BitmapImage bitmap = new BitmapImage(uri);
            //Image img = new Image();
            img.Source = bitmap;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            button_Settings_English.IsEnabled = false;
            button_Settings_Français.IsEnabled = false;

            if (App.oActiveUser != null)
            {// Заполняем значениями профиля

                grid.DataContext = App.oActiveUser;

                // Ставим галочки и активируем управление языков
                using ( var db = new LanguageTutorialContext() )
                {

                    var query = from Course in db.Course
                                where Course.UserId == App.oActiveUser.Id && Course.LanguageId == 0
                                select Course;

                    if (query != null)
                    {
                        App.oCourseEnglish = query as Course;

                        if ( App.oCourseEnglish.Active )
                        {
                            check_English.IsChecked = true;
                            button_Settings_English.IsEnabled = true;
                        }
                    }

                    query = from Course in db.Course
                                where Course.UserId == App.oActiveUser.Id && Course.LanguageId == 1
                                select Course;

                    if ( query != null )
                    {
                        App.oCourseFrançais = query as Course;

                        if (App.oCourseFrançais.Active)
                        {
                            check_Français.IsChecked = true;
                            button_Settings_Français.IsEnabled = true;
                        }
                    }

                }
            }
            else
            {// Заполняем стандартными значениями настройки языков
                App.oCourseEnglish = new Course() { LanguageId = 0, WordsPerSession = 20, WordsToStudy = 50, SeansPerDay = 5, TrueAnswers = 5 };
                App.oCourseFrançais = new Course() { LanguageId = 1, WordsPerSession = 20, WordsToStudy = 50, SeansPerDay = 5, TrueAnswers = 5 };
            }

            App.Registered = false;
        }

        /// <summary>
        /// Подтверждение регистрации, создание нового профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (App.oActiveUser == null)
            {// Создаём новый профиль, если профиль не выбран

                if ( textbox_Profile_Name.Text.Trim() != "" )
                {// Проверка на имя профиля

                    if ( num_Time_Between_Seans.Value != null )
                    {// Проверка на часы меж сеансами

                        if ((check_English.IsChecked == true || check_Français.IsChecked == true))
                        {// Проверка на то, что выбран хотя бы 1 язык

                            // Создаём пользователя
                            User nUser = new User() { Name = textbox_Profile_Name.Text.Trim(), SessionPeriod = (double)num_Time_Between_Seans.Value };

                            // Добавляем пользователя в БД
                            using ( var db = new LanguageTutorialContext())
                            {
                                db.User.Add(nUser);
                                db.SaveChanges();
                            }

                            // Делаем нового пользователя текущим
                            App.oActiveUser = nUser;

                            // Создаём привязку пользователя к курсам и сохраняем настройки
                            if (check_English.IsChecked == true)
                            {// Курсы английского

                                using (var db = new LanguageTutorialContext())
                                {
                                    var nCourse = new Course();

                                    nCourse.Active = true;
                                    nCourse.WordsPerSession = 20;
                                    nCourse.WordsToStudy = 50;
                                    nCourse.SeansPerDay = 5;
                                    nCourse.TrueAnswers = 5;
                                    nCourse.UserId = nUser.Id;

                                    var lang = from Language in db.Language
                                               where Language.Id == 0
                                               select Language;

                                    nCourse.LanguageId = db.Language.ToList()[0].Id;

                                    db.Course.Add(nCourse);
                                    db.SaveChanges();
                                }
                            }

                            if (check_Français.IsChecked == true)
                            {// Курсы французского

                                using (var db = new LanguageTutorialContext())
                                {
                                    var nCourse = new Course();

                                    nCourse.Active = true;
                                    nCourse.WordsPerSession = 20;
                                    nCourse.WordsToStudy = 50;
                                    nCourse.SeansPerDay = 5;
                                    nCourse.TrueAnswers = 5;
                                    nCourse.UserId = nUser.Id;
                                    nCourse.LanguageId = 1;
                                    
                                    db.Course.Add(nCourse);
                                    db.SaveChanges();
                                }
                            }

                            App.Registered = true;

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Язык обучения не выбран!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Количество часов не указанно!");
                    }
                }
                else
                {
                    MessageBox.Show("Имя профиля не указано!");
                }
                
            }
            else
            {// Обновляем текущий профиль
                if ( check_English.IsChecked == true || check_Français.IsChecked == true )
                {// Запоминаем новые данные

                    //Запоминаем изменения
                    App.oActiveUser.Name = textbox_Profile_Name.Text.Trim();
                    App.oActiveUser.SessionPeriod = (double)num_Time_Between_Seans.Value;

                    

                    // Загружаем обратно в бд
                    using (var db = new LanguageTutorialContext())
                    {
                        var original = db.User.Find(App.oActiveUser.Id);

                        if (original != null)
                        {
                            original.Name = textbox_Profile_Name.Text.Trim();
                            original.SessionPeriod = (double)num_Time_Between_Seans.Value;

                            db.SaveChanges();
                        } 
                    }

                    // Проверка изменения курсов пользователя
                    bool Course_Finded = false;

                    if (check_English.IsChecked == true)
                    {// Курсы английского
                        
                        //Проверка на наличие у пользователя неактивного курса английского
                        using ( var db = new LanguageTutorialContext())
                        {
                            var result = from Course in db.Course
                                         where Course.UserId == App.oActiveUser.Id && Course.LanguageId == 0
                                         select Course;

                            if ( result != null )
                            {
                                if ((result as Course).Active)
                                {
                                    Course_Finded = true;
                                }
                                else
                                {
                                    (result as Course).Active = true;
                                    db.SaveChanges();
                                }
                            }
                        }

                        if ( !Course_Finded )
                        {// Если курс не найден, но галочка стоит, то необходимо создать курс для пользователя
                            using (var db = new LanguageTutorialContext())
                            {
                                db.Course.Add(App.oCourseEnglish);
                                db.SaveChanges();
                            }
                        }
                        
                    }
                    else
                    {// Если галочки нет

                        //Проверка на наличие у пользователя активного курса английского
                        using ( var db = new LanguageTutorialContext())
                        {
                            var result = from Course in db.Course
                                         where Course.UserId == App.oActiveUser.Id && Course.LanguageId == 0 && Course.Active == true
                                         select Course;

                            (result as Course).Active = false;
                            db.SaveChanges();
                        }
                    }

                    Course_Finded = false;

                    if (check_Français.IsChecked == true)
                    {// Курсы английского

                        //Проверка на наличие у пользователя неактивного курса английского
                        using (var db = new LanguageTutorialContext())
                        {
                            var result = from Course in db.Course
                                         where Course.UserId == App.oActiveUser.Id && Course.LanguageId == 1
                                         select Course;

                            if (result != null)
                            {
                                if ((result as Course).Active)
                                {
                                    Course_Finded = true;
                                }
                                else
                                {
                                    (result as Course).Active = true;
                                    db.SaveChanges();
                                }
                            }
                        }

                        if (!Course_Finded)
                        {// Если курс не найден, но галочка стоит, то необходимо создать курс для пользователя
                            using (var db = new LanguageTutorialContext())
                            {
                                db.Course.Add(App.oCourseFrançais);
                                db.SaveChanges();
                            }
                        }

                    }
                    else
                    {// Если галочки нет

                        //Проверка на наличие у пользователя активного курса английского
                        using (var db = new LanguageTutorialContext())
                        {
                            var result = from Course in db.Course
                                         where Course.UserId == App.oActiveUser.Id && Course.LanguageId == 1 && Course.Active == true
                                         select Course;

                            (result as Course).Active = false;
                            db.SaveChanges();
                        }
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Язык обучения не выбран!");
                }
            }
        }

        /// <summary>
        /// Отмена регистрации, возврат к форме авторизации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Открытие настроек языка Английский
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Settings_English_Click(object sender, RoutedEventArgs e)
        {
            //SettingsWindow oSettingsWindow = new SettingsWindow("English");

            //oSettingsWindow.ShowDialog();
        }

        /// <summary>
        /// Открытие настроек языка Французский
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Settings_Français_Click(object sender, RoutedEventArgs e)
        {
            //SettingsWindow oSettingsWindow = new SettingsWindow("Français");

            //oSettingsWindow.ShowDialog();
        }

        /// <summary>
        /// Активация\Деактивация кнопки настроек при активации\деактивации галочки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_English_Click(object sender, RoutedEventArgs e)
        {
            if ( check_English.IsChecked == true )
            {
                button_Settings_English.IsEnabled = true;
            }
            else
            {
                button_Settings_English.IsEnabled = false;
            }
        }

        /// <summary>
        /// Активация\Деактивация кнопки настроек при активации\деактивации галочки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_Français_Click(object sender, RoutedEventArgs e)
        {
            if (check_Français.IsChecked == true)
            {
                button_Settings_Français.IsEnabled = true;
            }
            else
            {
                button_Settings_Français.IsEnabled = false;
            }
        }
    }
}
