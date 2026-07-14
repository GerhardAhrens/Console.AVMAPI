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

    using Console.AVMAPI.SimpleFritz;

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
            mainMenu.AddItem("Verbindung zur Fritz-Box über REST-API herstellen", MenuPoint1);
            mainMenu.AddItem("Verbindung zur Fritz-Box über TR-064 (SOAP) herstellen", MenuPoint2);
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
            if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pw) == false)
            {
                FritzOptions options = new FritzOptions() { UserName = user, Password = pw };

                HttpClient httpClient = new();
                var login = new LoginService(httpClient, options);
                SessionInfo session = await login.GetSessionInfoAsync();
                if (session != null)
                {
                    ChallengeInfo challenge = ChallengeParser.Parse(session.Challenge);
                    var calculator = new ChallengeResponseCalculator();
                    string response = calculator.Calculate(challenge, pw);

                    /*
                    string url = $"http://fritz.box/login_sid.lua?version=2&username={user}&response={response}";
                    */

                    var smartHomeService = new FritzSmartHomeService(httpClient, login, options);

                    try
                    {
                        IReadOnlyList<SmartHomeDevice> devices = await smartHomeService.GetDevicesAsync();

                        Console.WriteLine($"Gefundene Geräte: {devices.Count}");
                        Console.Line();

                        foreach (var device in devices)
                        {
                            Console.WriteLine($"Name        : {device.Name}");
                            Console.WriteLine($"AIN         : {device.Ain}");
                            Console.WriteLine($"Vorhanden   : {device.Present}");
                            Console.WriteLine($"Switch      : {device.IsSwitch}");
                            Console.WriteLine($"PowerMeter  : {device.HasPowerMeter}");
                            Console.WriteLine($"Temperatur  : {device.HasTemperatureSensor}");
                            Console.Line();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            Console.Wait();
        }

        private async static void MenuPoint2()
        {
            Console.Clear();

            string user = Console.ReadText("Fritz-Box Benutzer");
            string pw = Console.ReadText("Fritz-Box Passwort");
            if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pw) == false)
            {
                Console.Clear();
                FritzOptions options = new FritzOptions() { UserName = user, Password = pw };

                FritzSystemService systemService = new FritzSystemService(options);
                FritzBoxInfo info = await systemService.GetInfoAsync();

                Console.WriteLine($"Hersteller      : {info.Manufacturer}");
                Console.WriteLine($"ManufacturerOUI : {info.ManufacturerOUI}");
                Console.WriteLine($"Modell          : {info.ModelName}");
                Console.WriteLine($"Firmware        : {info.SoftwareVersion}");
                Console.WriteLine($"Hardware        : {info.HardwareVersion}");
                Console.WriteLine($"Seriennr.       : {info.SerialNumber}");
                Console.WriteLine($"Description.    : {info.Description}");
                Console.WriteLine($"ProductClass.   : {info.ProductClass}");
                Console.WriteLine($"SpecVersion.    : {info.SpecVersion}");
            }

            Console.Wait();
        }
    }
}
