//-----------------------------------------------------------------------
// <copyright file="FritzSmartHomeService.cs" company="Lifeprojects.de">
//     Class: FritzSmartHomeService
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Template für eine neue C# Standard-Klasse
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    public sealed class FritzSmartHomeService : IFritzSmartHomeService
    {
        private readonly HttpClient _httpClient;
        private readonly IFritzAuthentication _authentication;
        private readonly FritzOptions _options;

        public FritzSmartHomeService(HttpClient httpClient, IFritzAuthentication authentication, FritzOptions options)
        {
            _httpClient = httpClient;
            _authentication = authentication;
            _options = options;
        }

        private async Task<string> CreateUrlAsync(CancellationToken cancellationToken)
        {
            string sid =  await _authentication.GetSidAsync(cancellationToken);

            return $"{_options.Host}/webservices/homeautoswitch.lua?switchcmd=getdevicelistinfos&sid={sid}";
        }

        public async Task<IReadOnlyList<SmartHomeDevice>> GetDevicesAsync(CancellationToken cancellationToken = default)
        {
            string url = await CreateUrlAsync(cancellationToken);

            string xml = await _httpClient.GetStringAsync(url, cancellationToken);

            return Parse(xml);
        }

        private static List<SmartHomeDevice> Parse(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            List<SmartHomeDevice> result = [];

            foreach (XElement device in doc.Descendants("device"))
            {
                string ain = (string)device.Attribute("identifier") ?? "";
                int id = Convert.ToInt32((string)device.Attribute("id") ?? "0");
                int functionBitMask = int.Parse((string)device.Attribute("functionbitmask") ?? "0", CultureInfo.CurrentCulture);

                bool present = ((string)device.Element("present")) == "1" ? true : false;
                string name = (string)device.Element("name") ?? "";

                XElement switchElement = device.Element("switch");
                XElement powerElement = device.Element("powermeter");
                XElement tempElement = device.Element("temperature");

                SwitchInfo switchInfo = null;

                if (present == true)
                {
                    if (switchElement != null)
                    {
                        switchInfo = new SwitchInfo(

                            State:
                                (string)switchElement.Element("state") == "1",

                            Lock:
                                (string)switchElement.Element("lock") == "1",

                            DeviceLock:
                                (string)switchElement.Element("devicelock") == "1");
                    }
                }

                PowerMeterInfo power = null;

                if (present == true)
                {
                    if (powerElement != null)
                    {
                        power = new PowerMeterInfo(

                            Power:
                                int.Parse((string)powerElement.Element("power") ?? "0") / 1000.0,

                            Voltage:
                                int.Parse((string)powerElement.Element("voltage") ?? "0") / 1000.0,

                            Energy:
                                int.Parse((string)powerElement.Element("energy") ?? "0")); /* als Wh */
                    }
                }

                TemperatureInfo temperature = null;

                if (present == true)
                {
                    if (tempElement != null)
                    {
                        temperature = new TemperatureInfo(

                            Celsius:
                                int.Parse((string)tempElement.Element("celsius") ?? "0") / 10.0,

                            Offset:
                                int.Parse((string)tempElement.Element("offset") ?? "0") / 10.0);
                    }
                }

                var actor = new SmartHomeDevice(ain,
                    id,
                    name,
                    present,
                    functionBitMask)
                {
                    Switch = switchInfo,
                    PowerMeter = power,
                    Temperature = temperature
                };

                result.Add(actor);
            }

            return result;
        }
    }
}
