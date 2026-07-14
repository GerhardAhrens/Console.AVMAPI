//-----------------------------------------------------------------------
// <copyright file="SmartHomeDevice.cs" company="Lifeprojects.de">
//     Class: SmartHomeDevice
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
// Die Parameternamen sollen in PascalCase geschrieben werden, da diese in der späteren
// als Property Getter verwendet werden.
// </Remark>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    public sealed record SmartHomeDevice(
        string Ain,
        string Name,
        string Identifier,
        bool Present,
        bool IsSwitch,
        bool HasTemperatureSensor,
        bool HasPowerMeter);
}
