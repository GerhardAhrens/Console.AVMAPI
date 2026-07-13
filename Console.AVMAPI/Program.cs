//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.2, 15.04.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.07.2026 09:36:54</date>
//
// <summary>
// Konsolen Applikation mit der der Zugriff auf eine Fritzbox 7590 demonstriert werden soll
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI
{
    /* Imports from NET Framework */
    using System;

    public class Program
    {
        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }

        private static void Main(string[] args)
        {
            CMenu mainMenu = new CMenu("Fritz 7590 Menü");
            mainMenu.AddItem("Verbindung zur Fritz-Box herstellen", MenuPoint1);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            /* http://fritz.box/login_sid.lua?version=2 */

            Console.Wait();
        }

        private static void UnterMenuPoint(string param)
        {
            Console.Clear();

            Console.Wait(param);
        }
    }
}
