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
                        if ( c.Languages_Id == 0 )
                        {
                            check_English.IsChecked = true;
                            button_Settings_English.IsEnabled = true;
                        }
                        else
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
                App.oSettingsEnglish = English;

                Settings Français = new Settings(App.oSettingsRepository.lSettings, 20, 50, 5, 5);
                App.oSettingsFrançais = Français;
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

                                Course oCourseEnglish = new Course(App.oCourseRepository.lCourse, newUser.Id, App.oSettingsEnglish.Id, 0);
                                App.oCourseRepository.lCourse.Add(oCourseEnglish);

                                App.oSettingsRepository.lSettings.Add(App.oSettingsEnglish);
                            }

                            if (check_Français.IsChecked == true)
                            {// Курсы французского

                                Course oCourseFrançais = new Course(App.oCourseRepository.lCourse, newUser.Id, App.oSettingsFrançais.Id, 1);
                                App.oCourseRepository.lCourse.Add(oCourseFrançais);

                                App.oSettingsRepository.lSettings.Add(App.oSettingsFrançais);
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
