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
using System.Data.Entity;

namespace LanguageTutorial
{
    /// <summary>
    /// Interaction logic for ReturnKursWindow.xaml
    /// </summary>
    public partial class ReturnKursWindow
    {
        int LanguageID;
        public ReturnKursWindow(int language)
        {
            LanguageID = language;
            InitializeComponent();
        }

        private void btnNO_Click(object sender, RoutedEventArgs e)
        {
            Close();
            TestEndWindow test = new TestEndWindow();
            test.Close();
        }

        private void btnYES_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new LanguageTutorialContext())
            {
                //запилить новый курс или в старом обнулить все?
            }
        }
    }
}
