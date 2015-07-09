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

            App.EngSession = 0;
            App.FranSession = 0;
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
            int eng = TimerMet.numberSessionsLanguageEng();
            int fr = TimerMet.numberSessionsLanguageFran();
            openTesting(eng, fr);
        }

        public static void openTesting(int eng, int fr)
        {
            if (App.EngSession < eng && App.FranSession < fr)
            { 
                WindowLanguage winLan = new WindowLanguage();
                winLan.ShowDialog();
            }
            else if (App.EngSession < eng || App.FranSession < fr)
            {
                TestWindow test;
                if (App.EngSession < eng)
                {
                    test = new TestWindow(1);
                    test.ShowDialog();
                }
                else
                {
                    test = new TestWindow(2);
                    test.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Все тесты на сегодня пройдены");
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

            textblock_Username.DataContext = null;
            textblock_Username.DataContext = App.oActiveUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow oStatisticsWindow = new StatisticsWindow();
            oStatisticsWindow.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Change_User_Click(object sender, RoutedEventArgs e)
        {
            // Происходит смена пользователя
            App.ChangeUser = true;

            // Скрываем главное меню
            this.Visibility = System.Windows.Visibility.Hidden;

            App.aTimer.Stop();

            MainWindow oMainWindow = new MainWindow();
            // Открываем диалог с окном авторизации
            oMainWindow.ShowDialog();


            if (App.UserChanged)
            {// Если пользователь был сменён или зарегестрирован новый, Закрываем меню для открытия нового

                App.UserChanged = false;

                CanClose = true;

                this.Close();
            }
            else
            {// Если окно авторизации было закрыто, Возвращаем видимость главного меню

                this.Visibility = System.Windows.Visibility.Visible;
                App.aTimer.Start();
            }

            // Конец смены пользователя
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

        public System.Windows.Forms.NotifyIcon TrayIcon = null;
        public ContextMenu TrayMenu = null;


        public bool createTrayIcon()
        {
            bool result = false;
            if (TrayIcon == null)
            { // только если мы не создали иконку ранее
                TrayIcon = new System.Windows.Forms.NotifyIcon(); // создаем новую
                TrayIcon.Icon = LanguageTutorial.Properties.Resources.Tray_Icon; // изображение для трея
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
                        //Activate(); // нужно отдать окну фокус, см. ниже
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
            if (App.Current.Windows.Count == 1)
            {
                App.aTimer.Stop();
                int eng = TimerMet.numberSessionsLanguageEng();
                int fr = TimerMet.numberSessionsLanguageFran();
                openTesting(eng, fr);
            }
            else
            {
                App.Current.Windows[App.Current.Windows.Count - 1].Activate();
            }
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
            if (App.Current.Windows.Count == 1)
            {
                RegistrationWindow oRegistrationWindow = new RegistrationWindow();

                oRegistrationWindow.ShowDialog();

                textblock_Username.DataContext = null;
                textblock_Username.DataContext = App.oActiveUser;

                oRegistrationWindow.Activate(); // обязательно нужно отдать фокус окну,
                // иначе пользователь сильно удивится, когда увидит окно
                // но не сможет в него ничего ввести с клавиатуры
            }
            else
            {
                App.Current.Windows[App.Current.Windows.Count - 1].Activate();
            }

        }

        /// <summary>
        /// Пункт меню в трее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHideChangeUserWindow(object sender, RoutedEventArgs e)
        {
            if (App.Current.Windows.Count == 1)
            {
                // Происходит смена пользователя
                App.ChangeUser = true;

                // Скрываем главное меню
                this.Visibility = System.Windows.Visibility.Hidden;

                App.aTimer.Stop();

                MainWindow oMainWindow = new MainWindow();
                // Открываем диалог с окном авторизации
                oMainWindow.ShowDialog();

                if (App.UserChanged)
                {// Если пользователь был сменён или зарегестрирован новый, Закрываем меню для открытия нового

                    App.UserChanged = false;

                    CanClose = true;

                    this.Close();
                }
                else
                {// Если окно авторизации было закрыто, Возвращаем видимость главного меню

                    this.Visibility = System.Windows.Visibility.Visible;
                    App.aTimer.Start();
                }

                // Конец смены пользователя
                App.ChangeUser = false;
            }
            else
            {
                App.Current.Windows[App.Current.Windows.Count - 1].Activate();
            }
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
            if (App.Current.Windows.Count == 1)
            {
                StatisticsWindow oStatisticsWindow = new StatisticsWindow();

                oStatisticsWindow.ShowDialog();
                oStatisticsWindow.Activate(); // обязательно нужно отдать фокус окну,
                // иначе пользователь сильно удивится, когда увидит окно
                // но не сможет в него ничего ввести с клавиатуры
            }
            else
            {
                App.Current.Windows[App.Current.Windows.Count - 1].Activate();
            }

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
            Environment.Exit(0);
        }

        /// <summary>
        /// Кнопка в строке заголовка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Help_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
