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
    using Console.AVMAPI.SimpleFritz;
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

        private async static void MenuPoint1()
        {
            Console.Clear();

            string user = Console.ReadText("Fritz-Box Benutzer");
            string pw = Console.ReadText("Fritz-Box Passwort");
            if (string.IsNullOrEmpty(pw) == false)
            {
                /* http://fritz.box/login_sid.lua?version=2 */
                var login = new LoginService(new HttpClient());
                SessionInfo session = await login.GetSessionInfoAsync();
                ChallengeInfo challenge = ChallengeParser.Parse(session.Challenge);
                var calculator = new ChallengeResponseCalculator();
                string response = calculator.Calculate(challenge, pw);

                string url = $"http://fritz.box/login_sid.lua?version=2&username={user}&response={response}";
            }

            Console.Wait();
        }

        private static void UnterMenuPoint(string param)
        {
            Console.Clear();

            Console.Wait(param);
        }
    }
}
