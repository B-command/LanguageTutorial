using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial {
    public static class Querry {

        static public int numberSessionsLanguage(string language) {
            List<LanguageTutorial.DataModel.Course> list;
            using (var db = new LanguageTutorialContext()) {
                list = db.Course.Where(q => q.UserId == App.oActiveUser.Id).Where(q => q.Language.Name == language).Where(q => q.Active == true).ToList();
            }
            if (list.Count != 0) {
                return list[0].SeansPerDay;
            } else {
                return 0;
            }
        }

        //static public int timerInterval() {
        //    int min;
        //    using (var db = new LanguageTutorialContext()) {
        //        min = (int)(App.oActiveUser.SessionPeriod * 60);
        //    }
        //    return min;
        //}
    }
}
