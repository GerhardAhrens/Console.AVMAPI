//-----------------------------------------------------------------------
// <copyright file="FritzSystemService.cs" company="Lifeprojects.de">
//     Class: FritzSystemService
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// FritzSystemService, TR-064 Kommunikation
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    using System;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public sealed class FritzSystemService : IFritzSystemService
    {
        private readonly HttpClient _httpClient;
        private readonly FritzOptions _options;

        public FritzSystemService(FritzOptions options)
        {
            _options = options;

            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(options.UserName, options.Password)
            };

            _httpClient = new HttpClient(handler);
        }

        public async Task<FritzBoxInfo> GetInfoAsync(CancellationToken cancellationToken = default)
        {
            using HttpRequestMessage request = CreateRequest();

            using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            string xml = await response.Content.ReadAsStringAsync(cancellationToken);

            XDocument doc = XDocument.Parse(xml);
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XElement responseElement = doc.Root!.Element(soap + "Body")!.Elements().Single();
            string responseXml = responseElement.ToString();

            XDocument doc1 = XDocument.Parse(responseXml);
            XElement root = doc1.Root!;

            string deviceLog = root.ValueOrEmpty("NewDeviceLog");

            return new FritzBoxInfo(
                root.ValueOrEmpty("NewManufacturerName"),
                root.ValueOrEmpty("NewManufacturerOUI"),
                root.ValueOrEmpty("NewModelName"),
                root.ValueOrEmpty("NewDescription"),
                root.ValueOrEmpty("NewProductClass"),
                root.ValueOrEmpty("NewSerialNumber"),
                root.ValueOrEmpty("NewSoftwareVersion"),
                root.ValueOrEmpty("NewHardwareVersion"),
                root.ValueOrEmpty("NewSpecVersion"),
                root.UIntValue("NewUpTime"));
        }

        private HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Host}:49000/upnp/control/deviceinfo");

            request.Headers.Add("SOAPACTION", "\"urn:dslforum-org:service:DeviceInfo:1#GetInfo\"");

            request.Content = new StringContent(CreateEnvelope(), Encoding.UTF8, "text/xml");

            return request;
        }

        private static T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var reader = new StringReader(xml);

            return (T)serializer.Deserialize(reader)!;
        }

        private static string CreateEnvelope()
        {
            return """
                <?xml version="1.0" encoding="utf-8"?>
                <s:Envelope
                 xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"
                 s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">
                  <s:Body>
                    <u:GetInfo xmlns:u="urn:dslforum-org:service:DeviceInfo:1"/>
                  </s:Body>
                </s:Envelope>
                """;
        }
    }

    internal static class XmlExtensions
    {
        public static string ValueOrEmpty(this XElement element, string name)
        {
            return (string)element.Element(name) ?? string.Empty;
        }

        public static uint UIntValue(this XElement element, string name)
        {
            bool v = uint.TryParse((string)element.Element(name), out uint value);

            return value;
        }
    }
}
