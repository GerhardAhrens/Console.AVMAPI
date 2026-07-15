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
    public sealed record SmartHomeDevice(string Ain, int Id, string Name, bool Present, int FunctionBitMask)
    {
        public SwitchInfo Switch { get; init; }

        public PowerMeterInfo PowerMeter { get; init; }

        public TemperatureInfo Temperature { get; init; }

        public bool IsSwitch => (FunctionBitMask & 0x200) != 0;

        public bool HasPowerMeter => (FunctionBitMask & 0x04) != 0;

        public bool HasTemperatureSensor => (FunctionBitMask & 0x100) != 0;
    }
}
