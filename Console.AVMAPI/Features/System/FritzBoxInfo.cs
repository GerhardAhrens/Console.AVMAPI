//-----------------------------------------------------------------------
// <copyright file="FritzBoxInfo.cs" company="Lifeprojects.de">
//     Class: FritzBoxInfo
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Template für eine neue Record-Klasse
// </summary>
// <Remark>
// In diese Klasse werden die Basis / Meta Fritz-Box Informationen geschrieben
// 
// </Remark>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    public sealed record FritzBoxInfo(
        string Manufacturer,
        string ManufacturerOUI,
        string ModelName,
        string Description,
        string ProductClass,
        string SerialNumber,
        string SoftwareVersion,
        string HardwareVersion);
}
