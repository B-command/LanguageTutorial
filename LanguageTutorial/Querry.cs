﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageTutorial {
    public static class Querry {

        static public int numberSessionsLanguageEng() {
            if (App.oCourseEnglish != null) {
                if (App.oCourseEnglish.Active == true) {
                    return App.oCourseEnglish.SeansPerDay;
                } else {
                    return 0;
                }
            } else {
                return 0;
            }
        }

        static public int numberSessionsLanguageFran() {
            if (App.oCourseFrançais != null) {
                if (App.oCourseFrançais.Active == true) {
                    return App.oCourseFrançais.SeansPerDay;
                } else {
                    return 0;
                }
            } else {
                return 0;
            }
        }
    }
}
