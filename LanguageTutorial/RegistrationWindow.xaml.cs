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
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            button_Settings_English.IsEnabled = false;
            button_Settings_Français.IsEnabled = false;

            if (App.oActiveUser != null)
            {// Заполняем значениями профиля

                grid.DataContext = App.oActiveUser;

                // Ставим галочки и активируем управление языков
                foreach ( var c in App.oCourseRepository.lCourse )
                {
                    if ( c.Users_Id == App.oActiveUser.Id )
                    {
                        if ( c.Languages_Id == 0 && c.Active == true )
                        {
                            check_English.IsChecked = true;
                            button_Settings_English.IsEnabled = true;
                        }

                        if ( c.Languages_Id == 1 && c.Active == true )
                        {
                            check_Français.IsChecked = true;
                            button_Settings_Français.IsEnabled = true;
                        }
                    }
                }
            }
            else
            {// Заполняем стандартными значениями настройки языков

                Settings English = new Settings(App.oSettingsRepository.lSettings, 20, 50, 5, 5);
                App.oActiveSettingsEnglish = English;

                Settings Français = new Settings(App.oSettingsRepository.lSettings, 20, 50, 5, 5);
                App.oActiveSettingsFrançais = Français;
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

                if ( textbox_Profile_Name.Text != "" )
                {// Проверка на имя профиля

                    if ( num_Time_Between_Seans.Value != null )
                    {// Проверка на часы меж сеансами

                        if ((check_English.IsChecked == true || check_Français.IsChecked == true))
                        {// Проверка на то, что выбран хотя бы 1 язык
                            // Создаём пользователя
                            Users newUser = new Users(App.oUsersRepository.lUsers, textbox_Profile_Name.Text, (double)num_Time_Between_Seans.Value);

                            // Добавляем пользователя в БД
                            App.oUsersRepository.lUsers.Add(newUser);

                            // Делаем нового пользователя текущим
                            App.oActiveUser = newUser;

                            // Создаём привязку пользователя к курсам и сохраняем настройки
                            if (check_English.IsChecked == true)
                            {// Курсы английского

                                Course oCourseEnglish = new Course(App.oCourseRepository.lCourse, newUser.Id, App.oActiveSettingsEnglish.Id, 0, true);
                                App.oCourseRepository.lCourse.Add(oCourseEnglish);

                                App.oSettingsRepository.lSettings.Add(App.oActiveSettingsEnglish);
                            }

                            if (check_Français.IsChecked == true)
                            {// Курсы французского

                                Course oCourseFrançais = new Course(App.oCourseRepository.lCourse, newUser.Id, App.oActiveSettingsFrançais.Id, 1, true);
                                App.oCourseRepository.lCourse.Add(oCourseFrançais);

                                App.oSettingsRepository.lSettings.Add(App.oActiveSettingsFrançais);
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

                    // Удаляем запись из базы
                    App.oUsersRepository.lUsers.Remove(App.oActiveUser);

                    //Запоминаем изменения
                    App.oActiveUser.Name = textbox_Profile_Name.Text;
                    App.oActiveUser.TimeBetweenSeans = (double)num_Time_Between_Seans.Value;

                    // Загружаем обратно в бд
                    App.oUsersRepository.lUsers.Add(App.oActiveUser);

                    // Проверка изменения курсов пользователя
                    bool Course_Finded = false;

                    if (check_English.IsChecked == true)
                    {// Курсы английского
                        
                        //Проверка на наличие у пользователя неактивного курса английского
                        foreach ( var c in App.oCourseRepository.lCourse )
                        {
                            if (c.Users_Id == App.oActiveUser.Id && c.Languages_Id == 0 )
                            {// Если у пользователя есть активный курс английского
                                Course_Finded = true;
                            }

                            if ( c.Users_Id == App.oActiveUser.Id && c.Languages_Id == 0 && c.Active == false )
                            {// Если у пользователя есть неактивный курс английского

                                // Активировать курс
                                App.oCourseRepository.lCourse.Remove(c);

                                c.Active = true;

                                App.oCourseRepository.lCourse.Add(c);

                                break;
                            }
                        }

                        if ( !Course_Finded )
                        {// Если курс не найден, но галочка стоит, то необходимо создать курс для пользователя

                            if ( App.oActiveSettingsEnglish == null )
                            {
                                App.oActiveSettingsEnglish = new Settings(App.oSettingsRepository.lSettings, 20, 50, 5, 5);
                            }

                            Course oCourseEnglish = new Course(App.oCourseRepository.lCourse, App.oActiveUser.Id, App.oActiveSettingsEnglish.Id, 0, true);
                            App.oCourseRepository.lCourse.Add(oCourseEnglish);

                            App.oSettingsRepository.lSettings.Add(App.oActiveSettingsEnglish);
                        }
                        
                    }
                    else
                    {// Если галочки нет

                        //Проверка на наличие у пользователя активного курса английского
                        foreach (var c in App.oCourseRepository.lCourse)
                        {

                            if (c.Users_Id == App.oActiveUser.Id && c.Languages_Id == 0 && c.Active == true)
                            {// Если у пользователя есть активный курс английского

                                // Деактивировать курс
                                App.oCourseRepository.lCourse.Remove(c);

                                c.Active = false;

                                App.oCourseRepository.lCourse.Add(c);

                                break;
                            }
                        }
                    }

                    Course_Finded = false;

                    if (check_Français.IsChecked == true)
                    {// Курсы французского

                        //Проверка на наличие у пользователя данных курсов
                        foreach (var c in App.oCourseRepository.lCourse)
                        {
                            if (c.Users_Id == App.oActiveUser.Id && c.Languages_Id == 1 )
                            {
                                Course_Finded = true;
                            }

                            if (c.Users_Id == App.oActiveUser.Id && c.Languages_Id == 1 && c.Active == false)
                            {// Если у пользователя есть неактивный курс французского
                                // Активировать курс
                                App.oCourseRepository.lCourse.Remove(c);

                                c.Active = true;

                                App.oCourseRepository.lCourse.Add(c);

                                break;
                            }
                        }

                        if (!Course_Finded)
                        {// Если курс не найден, но галочка стоит, то необходимо создать курс для пользователя
                            if (App.oActiveSettingsFrançais == null)
                            {
                                App.oActiveSettingsFrançais = new Settings(App.oSettingsRepository.lSettings, 20, 50, 5, 5);
                            }

                            Course oCourseFrançais = new Course(App.oCourseRepository.lCourse, App.oActiveUser.Id, App.oActiveSettingsEnglish.Id, 1, true);
                            App.oCourseRepository.lCourse.Add(oCourseFrançais);

                            App.oSettingsRepository.lSettings.Add(App.oActiveSettingsFrançais);
                        }

                    }
                    else
                    {
                        //Проверка на наличие у пользователя данного курса
                        foreach (var c in App.oCourseRepository.lCourse)
                        {
                            if (c.Users_Id == App.oActiveUser.Id && c.Languages_Id == 1 && c.Active == true)
                            {// Если у пользователя есть активный курс французского

                                // Деактивировать курс
                                App.oCourseRepository.lCourse.Remove(c);

                                c.Active = false;

                                App.oCourseRepository.lCourse.Add(c);

                                break;
                            }
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
