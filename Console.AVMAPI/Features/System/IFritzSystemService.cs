//-----------------------------------------------------------------------
// <copyright file="IFritzSystemService.cs" company="Lifeprojects.de">
//     Class: IFritzSystemService
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Interface-Klasse für IFritzSystemService
// Schritt 15
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    public interface IFritzSystemService
    {
        Task<FritzBoxInfo> GetInfoAsync(CancellationToken cancellationToken = default);
    }
}
