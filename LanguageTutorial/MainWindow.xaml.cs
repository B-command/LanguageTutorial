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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.Entity;

using LanguageTutorial.DataModel;

namespace LanguageTutorial
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/0_8052d_c46e96c_XL.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;
        }

        /// <summary>
        /// Инициализация переменных и вызов методов после загрузки окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Update_ComboBox_Users();

            Clear_Control();
        }
        
        /// <summary>
        /// Обновление ComboBox данными из таблицы с пользователями.
        /// </summary>
        private void Update_ComboBox_Users()
        {
            using (var db = new LanguageTutorialContext())
            {
                combobox_Users.ItemsSource = db.User.ToList();
            }

        }

        /// <summary>
        /// Сброс управления на форме
        /// </summary>
        private void Clear_Control()
        {
            button_SignIn.IsEnabled = false;
        }

        /// <summary>
        /// Вход в приложение под своим профилем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SignIn_Click(object sender, RoutedEventArgs e)
        {
            if ( combobox_Users.SelectedIndex != -1 )
            {// Если пользователь выбран, то запоминаем и храним его глобально
                App.UserChanged = true;

                App.oActiveUser = combobox_Users.SelectedItem as User;

                using ( var db = new LanguageTutorialContext() )
                {
                    var result = db.Course.Where(course => course.Active == true && course.UserId == App.oActiveUser.Id);
 
                    if ( result != null )
                    {
                        foreach ( var c in result )
                        {
                            if ( c.LanguageId == 1 )
                            {
                                App.oCourseEnglish = c;
                            }
                            else
                            {
                                App.oCourseFrançais = c;
                            }
                        }
                    }
                }
                // Открываем окно главного меню
                MainMenuWindow oMainMenuWindow = new MainMenuWindow();

                oMainMenuWindow.Show();

                this.Close();
            }
        }

        /// <summary>
        /// Регистраия нового пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Registrarion_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow oRegistrationWindow = new RegistrationWindow();
            oRegistrationWindow.Title = "Регистрация";
            oRegistrationWindow.ShowDialog();

            if ( App.Registered )
            {// Если пользователь зарегестрировался
                MainMenuWindow oMainMenuWindow = new MainMenuWindow();

                oMainMenuWindow.Show();

                this.Close();
            }

        }

        /// <summary>
        /// Включение кнопки "Вход" при выборе профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combobox_Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( combobox_Users.SelectedIndex != -1 )
            {
                button_SignIn.IsEnabled = true;
            }
        }
    }
}
