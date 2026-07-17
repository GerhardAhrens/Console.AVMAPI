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

    using Windows.Foundation;

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
            mainMenu.AddItem("Verbindung zur Fritz-Box über TR-064 (SOAP) herstellen", MenuPoint2);
            mainMenu.AddItem("Verbindung zur Fritz-Box über REST-API herstellen", MenuPoint1);
            mainMenu.AddItem("Smart Home, Liste der Aktoren über REST-API", MenuPoint3);
            mainMenu.AddItem("Smart Home, Aktoren Statistik", MenuPoint4);
            mainMenu.AddItem("Smart Home, Aktoren Statistik - Historisch", MenuPoint5);
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

        private async static void MenuPoint2()
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

                    string url = $"http://fritz.box/login_sid.lua?version=2&username={user}&response={response}";
                    Console.WriteSuccess(url);
                }
            }

            Console.Wait();
        }

        private async static void MenuPoint3()
        {
            Console.Clear();

            string user = Console.ReadText("Fritz-Box Benutzer");
            string pw = Console.ReadText("Fritz-Box Passwort");
            if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pw) == false)
            {
                FritzOptions options = new FritzOptions() { UserName = user, Password = pw };

                HttpClient httpClient = new();
                var login = new LoginService(httpClient, options);
                if (login != null)
                {
                    /* Liste der Smart Home Aktoren abrufen */
                    var smartHomeService = new FritzSmartHomeService(httpClient, login, options);

                    try
                    {
                        Console.Clear();
                        IReadOnlyList<SmartHomeDevice> devices = await smartHomeService.GetDevicesAsync();

                        Console.WriteLine($"Gefundene Geräte: {devices.Count}");
                        Console.Line();

                        foreach (var device in devices)
                        {
                            Console.WriteLine($"AIN         : {device.Ain}");
                            Console.WriteLine($"Name        : {device.Name}");
                            Console.WriteLine($"Id          : {device.Id}");
                            Console.WriteLine($"Vorhanden   : {device.Present}");
                            Console.WriteLine($"Switch      : {device.IsSwitch}");
                            Console.WriteLine($"Temperatur  : {device.HasTemperatureSensor}");
                            Console.WriteLine($"PowerMeter  : {device.HasPowerMeter}");
                            Console.WriteLine($"Spannung V  : {device.PowerMeter?.VoltageVolts}");
                            Console.WriteLine($"Leistung W  :{device.PowerMeter?.PowerWatts}");
                            Console.WriteLine($"Temparatur  : {device.Temperature?.Celsius}");
                            Console.WriteLine($"Status      : {device.Switch?.State}"); 
                            Console.Line();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteError(ex.Message);
                    }
                }
            }

            Console.Wait();
        }

        private async static void MenuPoint4()
        {
            Console.Clear();

            string user = Console.ReadText("Fritz-Box Benutzer");
            string pw = Console.ReadText("Fritz-Box Passwort");
            if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pw) == false)
            {
                FritzOptions options = new FritzOptions() { UserName = user, Password = pw };

                HttpClient httpClient = new();
                var login = new LoginService(httpClient, options);
                var smartHome = new FritzSmartHomeService(httpClient, login, options);

                try
                {
                    Console.Clear();
                    IReadOnlyList<SmartHomeDevice> devices = await smartHome.GetDevicesAsync();
                    Console.Line();
                    Console.WriteText($"Gefundene Geräte : {devices.Count}");
                    Console.Line();

                    foreach (var device in devices)
                    {
                        Console.WriteLine($"{device.Name} ({device.Ain})");
                    }

                    SmartHomeDevice device1 = devices.FirstOrDefault(d => d.Ain == devices[2].Ain);
                    if (device1 == null)
                    {
                        Console.WriteLine();
                        Console.WriteError("Kein Powermeter gefunden.");
                        return;
                    }

                    Console.Line();
                    Console.WriteText($"Ausgewähltes Gerät : {device1.Name}");
                    Console.WriteText($"AIN                : {device1.Ain}");

                    //-----------------------------------------------------
                    // Aktuelle Leistungsdaten
                    //-----------------------------------------------------

                    if (device1.PowerMeter != null)
                    {
                        Console.Line();
                        Console.WriteText("Aktuelle Messwerte");
                        Console.WriteText($"Leistung : {device1.PowerMeter.PowerWatts:F3} W");
                        Console.WriteText($"Spannung : {device1.PowerMeter.VoltageVolts:F1} V");
                        Console.WriteText($"Gesamt   : {device1.PowerMeter.EnergyKWh:F3} kWh");
                    }

                    //-----------------------------------------------------
                    // Historische Statistik
                    //-----------------------------------------------------

                    DeviceStatistics statistics = await smartHome.GetDeviceStatisticsAsync(device1.Ain);

                    Console.Line();
                    Console.WriteText("Tagesverbrauch");

                    if (statistics.EnergyDaily != null)
                    {
                        foreach (var value in statistics.EnergyDaily.Values)
                        {
                            Console.WriteLine($"{value.Timestamp:dd.MM.yyyy}  {value.Value / 1000} KW/h");
                        }
                    }

                    Console.Line();
                    Console.WriteText("Monatsverbrauch");

                    if (statistics.EnergyMonthly != null)
                    {
                        foreach (var value in statistics.EnergyMonthly.Values)
                        {
                            Console.WriteLine($"{value.Timestamp:MM.yyyy}  {value.Value / 1000} KW/h");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteError(ex.Message);
                }
            }

            Console.Wait();
        }

        private async static void MenuPoint5()
        {
            Console.Clear();

            string user = Console.ReadText("Fritz-Box Benutzer");
            string pw = Console.ReadText("Fritz-Box Passwort");
            if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pw) == false)
            {
                FritzOptions options = new FritzOptions() { UserName = user, Password = pw };

                HttpClient httpClient = new();
                var login = new LoginService(httpClient, options);
                if (login != null)
                {
                    try
                    {
                        var smartHome = new FritzSmartHomeService(httpClient, login, options);
                        Console.Clear();
                        IReadOnlyList<SmartHomeDevice> devices = await smartHome.GetDevicesAsync();
                        Console.Line();
                        Console.WriteText($"Gefundene Geräte : {devices.Count}");
                        Console.Line();

                        foreach (var device in devices)
                        {
                            Console.WriteLine($"{device.Name} ({device.Ain})");
                        }

                        SmartHomeDevice device1 = devices.FirstOrDefault(d => d.Ain == devices[2].Ain);
                        if (device1 == null)
                        {
                            Console.WriteLine();
                            Console.WriteError("Kein Powermeter gefunden.");
                            return;
                        }

                        Console.Line();
                        Console.WriteText($"Ausgewähltes Gerät : {device1.Name}");
                        Console.WriteText($"AIN                : {device1.Ain}");

                        //-----------------------------------------------------
                        // Aktuelle Leistungsdaten
                        //-----------------------------------------------------

                        if (device1.PowerMeter != null)
                        {
                            Console.Line();
                            Console.WriteText("Aktuelle Messwerte");
                            Console.WriteText($"Leistung : {device1.PowerMeter.PowerWatts:F3} W");
                            Console.WriteText($"Spannung : {device1.PowerMeter.VoltageVolts:F1} V");
                            Console.WriteText($"Gesamt   : {device1.PowerMeter.EnergyKWh:F3} kWh");
                        }

                        //-----------------------------------------------------
                        // Historische Statistik
                        //-----------------------------------------------------

                        DeviceStatistics statistics = await smartHome.GetDeviceStatisticsAsync(device1.Ain);

                        Console.Line();
                        Console.WriteText("Aktueller Monat");

                        if (statistics.EnergyDaily != null)
                        {
                            double monatSum = statistics.EnergyDaily.Values.Where(w => w.Timestamp.Year == DateTime.Now.Year && w.Timestamp.Month == DateTime.Now.Month).Sum(s => s.Value);
                            Console.WriteLine($"Summe für Monat/Jahr {DateTime.Now:MM.yyyy}  {monatSum / 1000} KW/h");
                        }

                        Console.Line();
                        Console.WriteText("Aktuelles jahr");

                        if (statistics.EnergyMonthly != null)
                        {
                            double jahrSum= statistics.EnergyMonthly.Values.Where(w => w.Timestamp.Year == DateTime.Now.Year).Sum(s => s.Value);
                            Console.WriteLine($"Summe für Jahr {DateTime.Now:yyyy}  {jahrSum / 1000} KW/h");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteError(ex.Message);
                    }
                }

            }

            Console.Wait();
        }
    }
}
