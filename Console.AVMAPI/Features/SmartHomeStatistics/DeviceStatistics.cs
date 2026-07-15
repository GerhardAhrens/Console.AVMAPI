//-----------------------------------------------------------------------
// <copyright file="DeviceStatistics.cs" company="Lifeprojects.de">
//     Class: DeviceStatistics
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
    public sealed record DeviceStatistics
    {
        public StatisticSeries EnergyDaily { get; init; }

        public StatisticSeries EnergyMonthly { get; init; }
    }
}
