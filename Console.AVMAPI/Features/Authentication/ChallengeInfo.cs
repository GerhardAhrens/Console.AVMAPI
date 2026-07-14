//-----------------------------------------------------------------------
// <copyright file="ChallengeInfo.cs" company="Lifeprojects.de">
//     Class: ChallengeInfo
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Template für eine neue Record-Klasse
// </summary>
// <Remark>
// Die ChallengeInfo nimmt die ChallengeId auf
// </Remark>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    public sealed record ChallengeInfo(int Version, int Iteration1, ReadOnlyMemory<byte> Salt1, int Iteration2, ReadOnlyMemory<byte> Salt2);
}
