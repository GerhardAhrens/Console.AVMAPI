//-----------------------------------------------------------------------
// <copyright file="SwitchInfo.cs" company="Lifeprojects.de">
//     Class: SwitchInfo
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.07.2026</date>
//
// <summary>
// Template für eine neue Record-Klasse
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    public sealed record SwitchInfo(bool State, bool Lock, bool DeviceLock);
}
