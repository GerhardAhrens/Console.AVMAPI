//-----------------------------------------------------------------------
// <copyright file="IFritzSmartHomeService.cs" company="Lifeprojects.de">
//     Class: IFritzSmartHomeService
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Template für eine neue Interface-Klasse
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    public interface IFritzSmartHomeService
    {
        Task<IReadOnlyList<SmartHomeDevice>> GetDevicesAsync(CancellationToken cancellationToken = default);

        Task<DeviceStatistics> GetDeviceStatisticsAsync(string ain, CancellationToken cancellationToken = default);
    }
}
