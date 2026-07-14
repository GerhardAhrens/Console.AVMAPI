//-----------------------------------------------------------------------
// <copyright file="DeviceInfoResponse.cs" company="Lifeprojects.de">
//     Class: DeviceInfoResponse
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
    using System.Xml.Serialization;

    [XmlRoot("GetInfoResponse", Namespace = "urn:dslforum-org:service:DeviceInfo:1")]
    public sealed class DeviceInfoResponse
    {
        public string NewManufacturerName { get; set; } = "";

        public string NewManufacturerOUI { get; set; } = "";

        public string NewModelName { get; set; } = "";

        public string NewDescription { get; set; } = "";

        public string NewProductClass { get; set; } = "";

        public string NewSerialNumber { get; set; } = "";

        public string NewSoftwareVersion { get; set; } = "";

        public string NewHardwareVersion { get; set; } = "";

        public string NewSpecVersion { get; set; } = "";

        public string NewProvisioningCode { get; set; } = "";

        public uint NewUpTime { get; set; }

        public string NewDeviceLog { get; set; } = "";
    }
}
