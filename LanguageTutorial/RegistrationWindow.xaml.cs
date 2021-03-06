﻿using System;
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

using System.Data.Entity;

using LanguageTutorial.DataModel;
using System.Windows.Threading;

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
            num_Time_Between_Seans.Value = 2;
        }

        /// <summary>
        /// Переменная, хранящая текущее значение промежутка между сеансами 
        /// </summary>
        double currentTimeBetweenSessions = 2;

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            button_Settings_English.IsEnabled = false;
            button_Settings_Français.IsEnabled = false;

            if (App.oActiveUser != null && !App.ChangeUser)
            {// Заполняем значениями профиля

                User tempUser = new User();

                tempUser.Name = App.oActiveUser.Name;
                tempUser.SessionPeriod = App.oActiveUser.SessionPeriod;

                grid.DataContext = tempUser;

                // Ставим галочки и активируем управление языков
                using (var db = new LanguageTutorialContext())
                {
                    // Вытаскиваем Английский курс пользователя
                    var result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 1);

                    if (result != null)
                    {
                        // Запоминаем Английский курс пользователя
                        App.oCourseEnglish = result as Course;

                        // Активируем элементы управления, если курс активен
                        if (App.oCourseEnglish.Active)
                        {
                            check_English.IsChecked = true;
                            button_Settings_English.IsEnabled = true;
                        }
                    }

                    // Вытаскиваем Французский курс пользователя
                    result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 2);

                    if (result != null)
                    {
                        // Запоминаем Французский курс пользователя
                        App.oCourseFrançais = result as Course;

                        // Активируем элементы управления, если курс активен
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

                App.oCourseEnglish = new Course() { LanguageId = 1, WordsPerSession = 20, WordsToStudy = 50, SeansPerDay = 5, TrueAnswers = 3 };
                App.oCourseFrançais = new Course() { LanguageId = 2, WordsPerSession = 20, WordsToStudy = 50, SeansPerDay = 5, TrueAnswers = 3 };
            }

            App.Registered = false;

            currentTimeBetweenSessions = (double)num_Time_Between_Seans.Value;
        }

        /// <summary>
        /// Подтверждение регистрации, создание нового профиля или обновление данных активного профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_Profile_Name.Text.Trim() != "")
            {// Проверка на имя профиля

                if (num_Time_Between_Seans.Value != null)
                {// Проверка на часы меж сеансами

                    if ((check_English.IsChecked == true || check_Français.IsChecked == true))
                    {// Проверка на то, что выбран хотя бы 1 язык

                        if (App.oActiveUser == null || App.ChangeUser)
                        {// Если активного пользователя нет или пользователь меняет профиль, то создаём новый профиль ( регистрация

                            // Создаём пользователя
                            User nUser = new User() { Name = textbox_Profile_Name.Text.Trim(), SessionPeriod = (double)num_Time_Between_Seans.Value };

                            // Добавляем пользователя в БД
                            using (var db = new LanguageTutorialContext())
                            {
                                var result = db.User.FirstOrDefault(user => user.Name == nUser.Name);

                                if (result == null)
                                {
                                    db.User.Add(nUser);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    MessageBox.Show("Такое имя пользователя уже зарегистрировано!");
                                    return;
                                }

                            }

                            // Делаем текущего пользователя активным
                            App.oActiveUser = nUser;

                            // Создаём привязку пользователя к курсам и сохраняем настройки
                            if (check_English.IsChecked == true)
                            {// Курсы английского

                                using (var db = new LanguageTutorialContext())
                                {
                                    var nCourse = new Course();

                                    nCourse.Active = true;
                                    nCourse.WordsPerSession = App.oCourseEnglish.WordsPerSession;
                                    nCourse.WordsToStudy = App.oCourseEnglish.WordsToStudy;
                                    nCourse.SeansPerDay = App.oCourseEnglish.SeansPerDay;
                                    nCourse.TrueAnswers = App.oCourseEnglish.TrueAnswers;
                                    nCourse.UserId = nUser.Id;

                                    var lang = from Language in db.Language
                                               where Language.Id == 0
                                               select Language;

                                    nCourse.LanguageId = 1;

                                    db.Course.Add(nCourse);
                                    db.SaveChanges();

                                    for (int i = 1; i <= App.oCourseEnglish.WordsToStudy; i++)
                                    {
                                        db.WordQueue.Add(new WordQueue() { TrueAnswers = 0, IsLearned = false, UserId = nUser.Id, WordDictionaryId = i });
                                    }

                                    db.SaveChanges();
                                }
                            }

                            if (check_Français.IsChecked == true)
                            {// Курсы французского

                                using (var db = new LanguageTutorialContext())
                                {
                                    var nCourse = new Course();

                                    nCourse.Active = true;
                                    nCourse.WordsPerSession = App.oCourseFrançais.WordsPerSession;
                                    nCourse.WordsToStudy = App.oCourseFrançais.WordsToStudy;
                                    nCourse.SeansPerDay = App.oCourseFrançais.SeansPerDay;
                                    nCourse.TrueAnswers = App.oCourseFrançais.TrueAnswers;
                                    nCourse.UserId = nUser.Id;
                                    nCourse.LanguageId = 2;

                                    db.Course.Add(nCourse);
                                    db.SaveChanges();

                                    for (int i = 496; i < 496 + App.oCourseFrançais.WordsToStudy; i++)
                                    {
                                        db.WordQueue.Add(new WordQueue() { TrueAnswers = 0, IsLearned = false, UserId = nUser.Id, WordDictionaryId = i });
                                    }

                                    db.SaveChanges();
                                }
                            }

                            App.Registered = true;
                            App.UserChanged = true;

                            DispatcherTimer Timer = new DispatcherTimer();
                            Timer.Tick += new EventHandler(TimerMet.OnTimedEvent);
                            Timer.Interval = new TimeSpan(0, (int)(App.oActiveUser.SessionPeriod * 60), 0);
                            App.aTimer = Timer;

                            this.Close();
                        }
                        else
                        {// Если есть активный пользователь, то необходимо обновить его настройки

                            // Обновление параметров профиля в БД
                            using (var db = new LanguageTutorialContext())
                            {
                                var result = db.User.FirstOrDefault(user => user.Name == textbox_Profile_Name.Text.Trim());

                                if (result != null)
                                { // Если пользователь с таким именем найден

                                    if (App.oActiveUser.Name != textbox_Profile_Name.Text.Trim())
                                    {// Если пользователь сменил имя

                                        // Делаем прерывание т.к. имя уже занято
                                        MessageBox.Show("Такое имя пользователя уже зарегистрировано!");
                                        return;
                                    }
                                    else
                                    { // Если пользователь не сменил имя

                                        // Обновляем данные активного пользователя
                                        App.oActiveUser.SessionPeriod = (double)num_Time_Between_Seans.Value;

                                        // Обновляем базу данных
                                        var original = db.User.Find(App.oActiveUser.Id);

                                        if (original != null)
                                        {
                                            original.SessionPeriod = (double)num_Time_Between_Seans.Value;

                                            db.SaveChanges();
                                        }
                                    }
                                }
                                else
                                { // Если пользователь с таким именем не найден, значит произошла смена имени.
                                    
                                    // Обновляем данные активного пользователя
                                    App.oActiveUser.Name = textbox_Profile_Name.Text.Trim();
                                    App.oActiveUser.SessionPeriod = (double)num_Time_Between_Seans.Value;

                                    // Обновляем базу данных
                                    var original = db.User.Find(App.oActiveUser.Id);

                                    if (original != null)
                                    {
                                        original.Name = textbox_Profile_Name.Text.Trim();
                                        original.SessionPeriod = (double)num_Time_Between_Seans.Value;

                                        db.SaveChanges();
                                    }
                                }
                            }

                            App.aTimer.Stop();
                            App.aTimer.Interval = new TimeSpan(0, (int)((double)num_Time_Between_Seans.Value * 60), 0);
                            App.aTimer.Start();

                            // Проверка изменения курсов пользователя
                            bool Course_Finded = false;

                            if (check_English.IsChecked == true)
                            {// Курсы английского

                                //Проверка на наличие у пользователя неактивного курса английского
                                using (var db = new LanguageTutorialContext())
                                {
                                    var result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 1);

                                    if (result != null)
                                    {// Если нашли курс, то запоминаем это
                                        Course_Finded = true;

                                        if ((result as Course).Active == false)
                                        { // Если курс был неактивен, то делаем его активным
                                            (result as Course).Active = true;
                                            db.SaveChanges();
                                        }
                                    }
                                }

                                if (Course_Finded)
                                {// Если курс найден, то применяем к нему обновлённые настройки
                                    using (var db = new LanguageTutorialContext())
                                    {
                                        var original = db.Course.Find(App.oCourseEnglish.Id);

                                        original.WordsPerSession = App.oCourseEnglish.WordsPerSession;
                                        original.WordsToStudy = App.oCourseEnglish.WordsToStudy;
                                        original.SeansPerDay = App.oCourseEnglish.SeansPerDay;
                                        original.TrueAnswers = App.oCourseEnglish.TrueAnswers;

                                        db.SaveChanges();
                                    }
                                }
                                else
                                {// Если курс не найден, но галочка стоит, то необходимо создать курс для пользователя

                                    using (var db = new LanguageTutorialContext())
                                    {
                                        var nCourse = new Course();

                                        nCourse.Active = true;
                                        nCourse.WordsPerSession = 20;
                                        nCourse.WordsToStudy = 50;
                                        nCourse.SeansPerDay = 5;
                                        nCourse.TrueAnswers = 3;
                                        nCourse.UserId = App.oActiveUser.Id;
                                        nCourse.LanguageId = 1;

                                        db.Course.Add(nCourse);
                                        db.SaveChanges();
                                        App.oCourseEnglish = nCourse;

                                        for (int i = 1; i <= App.oCourseEnglish.WordsToStudy; i++)
                                        {
                                            db.WordQueue.Add(new WordQueue() { TrueAnswers = 0, IsLearned = false, UserId = App.oActiveUser.Id, WordDictionaryId = i });
                                        }

                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {// Если галочки нет

                                //Проверка на наличие у пользователя активного курса английского
                                using (var db = new LanguageTutorialContext())
                                {
                                    var result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 1 && Course.Active == true);

                                    if (result != null)
                                    {
                                        (result as Course).Active = false;
                                        db.SaveChanges();
                                    }
                                }
                            }

                            Course_Finded = false;

                            if (check_Français.IsChecked == true)
                            {// Курсы английского

                                //Проверка на наличие у пользователя неактивного курса французского
                                using (var db = new LanguageTutorialContext())
                                {
                                    var result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 2);

                                    if (result != null)
                                    {// Если нашли курс, то запоминаем это
                                        Course_Finded = true;

                                        if ((result as Course).Active == false)
                                        { // Если курс был неактивен, то делаем его активным
                                            (result as Course).Active = true;
                                            db.SaveChanges();
                                        }
                                    }
                                }

                                if (Course_Finded)
                                {// Если курс найден, то применяем к нему обновлённые настройки
                                    using (var db = new LanguageTutorialContext())
                                    {
                                        var original = db.Course.Find(App.oCourseFrançais.Id);

                                        original.WordsPerSession = App.oCourseFrançais.WordsPerSession;
                                        original.WordsToStudy = App.oCourseFrançais.WordsToStudy;
                                        original.SeansPerDay = App.oCourseFrançais.SeansPerDay;
                                        original.TrueAnswers = App.oCourseFrançais.TrueAnswers;

                                        db.SaveChanges();
                                    }
                                }
                                else
                                {// Если курс не найден, но галочка стоит, то необходимо создать курс для пользователя

                                    using (var db = new LanguageTutorialContext())
                                    {
                                        var nCourse = new Course();

                                        nCourse.Active = true;
                                        nCourse.WordsPerSession = 20;
                                        nCourse.WordsToStudy = 50;
                                        nCourse.SeansPerDay = 5;
                                        nCourse.TrueAnswers = 3;
                                        nCourse.UserId = App.oActiveUser.Id;
                                        nCourse.LanguageId = 2;

                                        db.Course.Add(nCourse);
                                        db.SaveChanges();

                                        App.oCourseFrançais = nCourse;
                                        for (int i = 496; i < 496 + App.oCourseFrançais.WordsToStudy; i++)
                                        {
                                            db.WordQueue.Add(new WordQueue() { TrueAnswers = 0, IsLearned = false, UserId = App.oActiveUser.Id, WordDictionaryId = i });
                                        }

                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {// Если галочки нет

                                //Проверка на наличие у пользователя активного курса французского
                                using (var db = new LanguageTutorialContext())
                                {
                                    var result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 2 && Course.Active == true);

                                    if (result != null)
                                    {
                                        (result as Course).Active = false;
                                        db.SaveChanges();
                                    }
                                }
                            }

                            this.Close();
                        }
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

        /// <summary>
        /// Отмена регистрации, возврат к форме авторизации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            num_Time_Between_Seans.Value = currentTimeBetweenSessions;
            this.Close();
        }


        /// <summary>
        /// Открытие настроек языка Английский
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Settings_English_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow oSettingsWindow = new SettingsWindow("English");

            oSettingsWindow.ShowDialog();
        }

        /// <summary>
        /// Открытие настроек языка Французский
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Settings_Français_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow oSettingsWindow = new SettingsWindow("Français");

            oSettingsWindow.ShowDialog();
        }

        /// <summary>
        /// Активация\Деактивация кнопки настроек при активации\деактивации галочки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_English_Click(object sender, RoutedEventArgs e)
        {
            if (check_English.IsChecked == true)
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

        private void textbox_Profile_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            textbox_Profile_Name.Text = textbox_Profile_Name.Text.Trim();
        }

        // Перед закрытием окна обновить глобальные переменные настроек курса пользователя
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            using (var db = new LanguageTutorialContext())
            {
                if (App.oActiveUser != null)
                {
                    var result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 1);

                    if (result != null)
                    {
                        App.oCourseEnglish = result;
                    }
                    else
                    {
                        App.oCourseEnglish = null;
                    }

                    result = db.Course.FirstOrDefault(Course => Course.UserId == App.oActiveUser.Id && Course.LanguageId == 2);

                    if (result != null)
                    {
                        App.oCourseFrançais = result;
                    }
                    else
                    {
                        App.oCourseFrançais = null;
                    }
                }
            }

            base.OnClosing(e);
        }

        private void num_Time_Between_Seans_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void num_Time_Between_Seans_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (num_Time_Between_Seans.Value == null) num_Time_Between_Seans.Value = currentTimeBetweenSessions;
        }
    }
}
