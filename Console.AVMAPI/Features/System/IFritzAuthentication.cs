//-----------------------------------------------------------------------
// <copyright file="IFritzAuthentication.cs" company="Lifeprojects.de">
//     Class: IFritzAuthentication
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
    public interface IFritzAuthentication
    {
        Task<string> GetSidAsync(CancellationToken cancellationToken = default);
    }
}
