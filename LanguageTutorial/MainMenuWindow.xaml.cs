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
            
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            textblock_Username.DataContext = App.oActiveUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Start_Testing_Click(object sender, RoutedEventArgs e)
        {

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
            MessageBox.Show("Статистика");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Change_User_Click(object sender, RoutedEventArgs e)
        {
            App.oActiveUser = null;
            App.oSettingsEnglish = null;
            App.oSettingsFrançais = null;

            MainWindow oMainWindow = new MainWindow();

            oMainWindow.ShowDialog();

            CanClose = true;

            this.Close();
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
                TrayIcon.Text = "Here is tray icon text."; // текст подсказки, всплывающей над иконкой
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
            MessageBox.Show("Тестирование - Трей");
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

            // показываем
            MainWindow oMainWindow = new MainWindow();

            oMainWindow.ShowDialog();

            oMainWindow.Activate(); // обязательно нужно отдать фокус окну,
            // иначе пользователь сильно удивится, когда увидит окно
            // но не сможет в него ничего ввести с клавиатуры

            CanClose = true;

            this.Close();
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
            MessageBox.Show("Статистика");
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

        // ПРИМЕР ТАЙМЕРА
        private void timer_example()
        {
            // Create a timer with a ten second interval.
            Timer aTimer = new Timer();

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 5000;
            aTimer.Enabled = true;
        }

        // СОБЫТИЕ ТАЙМЕРА
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            MessageBox.Show("Работает таймер");
        }
    }
}
