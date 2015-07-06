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

using System.Timers;
using System.Windows.Threading;


namespace LanguageTutorial
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenuWindow
    {
        public MainMenuWindow()
        {            
            InitializeComponent();
            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/cat.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            textblock_Username.DataContext = App.oActiveUser;

            timer();
            App.aTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Start_Testing_Click(object sender, RoutedEventArgs e)
        {
            App.aTimer.Stop();
            if (App.EngSession < Querry.numberSessionsLanguage("English") && App.FranSession < Querry.numberSessionsLanguage("Français")) { //заменить константы на данные из бд
                WindowLanguage winLan = new WindowLanguage();
                winLan.ShowDialog();
            } else {
                TestWindow test;
                if (App.EngSession < Querry.numberSessionsLanguage("English") && App.oCourseEnglish != null)
                {
                    test = new TestWindow(1);
                    test.ShowDialog();
                    App.EngSession++;
                }
                if (App.FranSession < Querry.numberSessionsLanguage("Français") && App.oCourseFrançais != null)
                {
                    test = new TestWindow(2);
                    test.ShowDialog();
                    App.FranSession++;
                }
                //запускаем тест по которому доступны сеансы
                //MessageBox.Show("Начать Тестирование - Меню (заглушка)");
               // App.EngSession++; //убрать когда появится тест
                if (App.EngSession < Querry.numberSessionsLanguage("English") || App.FranSession < Querry.numberSessionsLanguage("Français")) {//переместить код в тест
                    App.aTimer.Start();
                } // если не закончились сессии по английскому и французскому
                //заменить константы на данные из бд
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Settings_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow oRegistrationWindow = new RegistrationWindow();

            oRegistrationWindow.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Statistics_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Статистика - Меню (заглушка)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Change_User_Click(object sender, RoutedEventArgs e)
        {
            App.ChangeUser = true;

            this.Visibility = System.Windows.Visibility.Hidden;

            MainWindow oMainWindow = new MainWindow();

            oMainWindow.ShowDialog();

            if ( App.UserChanged )
            {
                App.UserChanged = false;

                CanClose = true;

                this.Close();
            }
            else
            {
                this.Visibility = System.Windows.Visibility.Visible;
            }

            App.ChangeUser = false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Exit_Click(object sender, RoutedEventArgs e)
        {
            CanClose = true;

            this.Close();
        }

        /// <summary>
        /// Переопределяем обработку первичной инициализации приложения
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e); // базовый функционал приложения в момент запуска
            createTrayIcon(); // создание нашей иконки
        }

        private System.Windows.Forms.NotifyIcon TrayIcon = null;
        private ContextMenu TrayMenu = null;

        private bool createTrayIcon()
        {
            bool result = false;
            if (TrayIcon == null)
            { // только если мы не создали иконку ранее
                TrayIcon = new System.Windows.Forms.NotifyIcon(); // создаем новую
                TrayIcon.Icon = LanguageTutorial.Properties.Resources.Bulb; // изображение для трея
                // обратите внимание, за ресурсом с картинкой мы лезем в свойства проекта, а не окна,
                // поэтому нужно указать полный namespace
                TrayIcon.Text = "Учебник иностранных языков"; // текст подсказки, всплывающей над иконкой
                TrayMenu = Resources["TrayMenu"] as ContextMenu; // а здесь уже ресурсы окна и тот самый x:Key

                // сразу же опишем поведение при щелчке мыши, о котором мы говорили ранее
                // это будет просто анонимная функция, незачем выносить ее в класс окна
                TrayIcon.Click += delegate(object sender, EventArgs e)
                {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        // по левой кнопке показываем или прячем окно
                        ShowHideMainWindow(sender, null);
                    }
                    else
                    {
                        // по правой кнопке (и всем остальным) показываем меню
                        TrayMenu.IsOpen = true;
                        Activate(); // нужно отдать окну фокус, см. ниже
                    }
                };
                result = true;
            }
            else
            { // все переменные были созданы ранее
                result = true;
            }
            TrayIcon.Visible = true; // делаем иконку видимой в трее
            return result;
        }

        /// <summary>
        /// Пункт меню в трее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false; // спрячем менюшку, если она вдруг видима
            if (IsVisible)
            {// если окно видно на экране
                // прячем его
                Hide();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Показать";
            }
            else
            { // а если не видно
                // показываем
                Show();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Скрыть";
                WindowState = CurrentWindowState;
                Activate(); // обязательно нужно отдать фокус окну,
                // иначе пользователь сильно удивится, когда увидит окно
                // но не сможет в него ничего ввести с клавиатуры
            }
        }

        /// <summary>
        /// Пункт меню в трее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTesting(object sender, RoutedEventArgs e)
        {
            App.aTimer.Stop();
            ///!!!!!!!!!если английский, то передать параметр 1 в конструктор, если нет(французский) -2
            TestWindow test = new TestWindow();
            test.ShowDialog();
            //MessageBox.Show("Тестирование - Трей (заглушка)");
            App.FranSession++; //убрать когда появится тест
            if (App.EngSession < Querry.numberSessionsLanguage("English") || App.FranSession < Querry.numberSessionsLanguage("Français")) {//переместить код в тест
                App.aTimer.Start();
            } // если не закончились сессии по английскому и французскому
            //заменить константы на данные из бд
        }

        /// <summary>
        /// Пункт меню в трее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHideSettingsWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false; // спрячем менюшку, если она вдруг видима

            // показываем
            RegistrationWindow oRegistrationWindow = new RegistrationWindow();

            oRegistrationWindow.ShowDialog();

            oRegistrationWindow.Activate(); // обязательно нужно отдать фокус окну,
            // иначе пользователь сильно удивится, когда увидит окно
            // но не сможет в него ничего ввести с клавиатуры

        }

        /// <summary>
        /// Пункт меню в трее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHideChangeUserWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false; // спрячем менюшку, если она вдруг видима

            App.ChangeUser = true;

            this.Visibility = System.Windows.Visibility.Visible;

            // показываем
            MainWindow oMainWindow = new MainWindow();

            oMainWindow.ShowDialog();

            oMainWindow.Activate(); // обязательно нужно отдать фокус окну,
            // иначе пользователь сильно удивится, когда увидит окно
            // но не сможет в него ничего ввести с клавиатуры

            if (App.UserChanged)
            {
                App.UserChanged = false;

                CanClose = true;

                this.Close();
            }
            else
            {
                this.Visibility = System.Windows.Visibility.Visible;
            }

            App.ChangeUser = false;
        }

        /// <summary>
        /// Пункт меню в трее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHideStatisticsWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false; // спрячем менюшку, если она вдруг видима

            // показываем
            MessageBox.Show("Статистика - Трей (заглушка)");
            Activate(); // обязательно нужно отдать фокус окну,
            // иначе пользователь сильно удивится, когда увидит окно
            // но не сможет в него ничего ввести с клавиатуры

        }

        private WindowState fCurrentWindowState = WindowState.Normal;

        public WindowState CurrentWindowState
        {
            get { return fCurrentWindowState; }
            set { fCurrentWindowState = value; }
        }

        /// <summary>
        /// Переопределяем встроенную реакцию на изменение состояния сознания окна
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e); // системная обработка
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                // если окно минимизировали, просто спрячем
                Hide();
                // и поменяем надпись на менюшке
                (TrayMenu.Items[0] as MenuItem).Header = "Показать";
            }
            else
            {
                // в противном случае запомним текущее состояние
                CurrentWindowState = WindowState;
            }
        }

        private bool fCanClose = false;

        public bool CanClose
        { // флаг, позволяющий или запрещающий выход из приложения
            get { return fCanClose; }
            set { fCanClose = value; }
        }

        /// <summary>
        /// Переопределяем обработчик запроса выхода из приложения
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e); // встроенная обработка
            if (!CanClose)
            {    // если нельзя закрывать
                e.Cancel = true; //выставляем флаг отмены закрытия
                // запоминаем текущее состояние окна
                CurrentWindowState = this.WindowState;
                // меняем надпись в менюшке
                (TrayMenu.Items[0] as MenuItem).Header = "Показать";
                // прячем окно
                Hide();
            }
            else
            { // все-таки закрываемся
                // убираем иконку из трея
                TrayIcon.Visible = false;
            }
        }

        /// <summary>
        /// Пункт меню в трее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuExitClick(object sender, RoutedEventArgs e)
        {
            CanClose = true;
            Close();
        }

        public void timer() {
            int min;
            using (var db = new LanguageTutorialContext()) {
                min = (int)(App.oActiveUser.SessionPeriod * 60);
            }

            App.aTimer = new DispatcherTimer();
            App.aTimer.Tick += new EventHandler(OnTimedEvent);
            App.aTimer.Interval = new TimeSpan(0, min, 0); //изменить время на время из базы
        }



        // СОБЫТИЕ ТАЙМЕРА
        private static void OnTimedEvent(object source, EventArgs e)
        {
                App.aTimer.Stop();
                WindowTimerTest timerWin = new WindowTimerTest();
                timerWin.ShowDialog();

        }
    }
}
