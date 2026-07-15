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
                string identifier = (string)device.Attribute("id") ?? "";
                bool present = ((string)device.Attribute("present")) == "1";
                string name = (string)device.Element("name") ?? "";
                int functionBitMask =  int.Parse((string)device.Attribute("functionbitmask") ?? "0",CultureInfo.CurrentCulture);

                result.Add(new SmartHomeDevice(Ain: ain,
                        Name: name,
                        Identifier: identifier,
                        Present: present,
                        IsSwitch: (functionBitMask & 0x200) != 0,
                        HasTemperatureSensor: (functionBitMask & 0x100) != 0,
                        HasPowerMeter: (functionBitMask & 0x04) != 0));
            }

            return result;
        }
    }
}
