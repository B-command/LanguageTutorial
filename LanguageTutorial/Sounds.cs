using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LanguageTutorial
{
     /// <summary>
    /// Класс звуков
    /// </summary>
   public class Sounds
    {
        /// <summary>
        /// Звуки
        /// </summary>
        private static int[,] m_minor0_ =
        {
            { 500,  20 },
            { 150,  70 },
            { 70,   80 },
            { 40,   100 }
        };
 
        private static int[,] m_notification_ =
        {
            { 100,  20 }
        };

        /// <summary>
        /// Звук ошибки
        /// </summary>
        /// <param name="fNewThread">запуск музыки в отдельном потоке</param>
        /// <returns>продолжительность музыки в мс</returns>
        public static int error(bool fNewThread = false)
        {
            return player(m_minor0_, 1, false, fNewThread);
        }
 
 
        /// <summary>
        /// Звук одобрения
        /// </summary>
        /// <param name="fNewThread">запуск музыки в отдельном потоке</param>
        /// <returns>продолжительность музыки в мс</returns>
        public static int good(bool fNewThread = false)
        {
            return player(m_minor0_, 1, true, fNewThread);
        }
 
        /// <summary>
        /// Оповещение
        /// </summary>
        /// <param name="fNewThread">запуск музыки в отдельном потоке</param>
        /// <returns>продолжительность музыки в мс</returns>
        public static int notification(bool fNewThread = false)
        {
            return player(m_notification_, 1, false, fNewThread);
        }
 
 
        /// <summary>
        /// Проигрывает звуки
        /// </summary>
        /// <param name="sounds">звуки</param>
        /// <param name="numberOfRepetitions">число повторений</param>
        /// <param name="fRevers">играет в обратном порядке</param>
        /// <param name="fNewThread">запуск музыки в отдельном потоке</param>
        /// <returns>продолжительность музыки в мс</returns>
        private static int player(int[,] sounds, int numberOfRepetitions = 1, bool fRevers = false, bool fNewThread = false)
        {
            int musicDuration = 0;
            for (int i = 0; i < sounds.GetLength(0); i++)
                musicDuration += sounds[i, 1];
 
            if (fNewThread)
            {
                Thread t = new Thread(delegate()
                {
                    for (int i = 0; i < numberOfRepetitions; i++)
                    {
                        if (!fRevers)
                            for (int j = 0; j < sounds.GetLength(0); j++)
                                Console.Beep(sounds[j, 0], sounds[j, 1]);
                        else
                            for (int j = (sounds.GetLength(0) - 1); j > 0; j--)
                                Console.Beep(sounds[j, 0], sounds[j, 1]);
                    }
                });
                t.Start();
            }
            else
            {
                for (int i = 0; i < numberOfRepetitions; i++)
                {
                    if (!fRevers)
                        for (int j = 0; j < sounds.GetLength(0); j++)
                            Console.Beep(sounds[j, 0], sounds[j, 1]);
                    else
                        for (int j = (sounds.GetLength(0) - 1); j > 0; j--)
                            Console.Beep(sounds[j, 0], sounds[j, 1]);
                }
            }
            return musicDuration;
        }
    }
}
