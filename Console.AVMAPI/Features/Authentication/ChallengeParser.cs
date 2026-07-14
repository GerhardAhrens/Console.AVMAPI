//-----------------------------------------------------------------------
// <copyright file="ChallengeParser.cs" company="Lifeprojects.de">
//     Class: ChallengeParser
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Prüft die ChallengeId und gibt diese über die ChallengeInfo zurück
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public static class ChallengeParser
    {
        public static ChallengeInfo Parse(string challenge)
        {
            var parts = challenge.Split('$');

            if (parts.Length != 5)
            {
                throw new InvalidOperationException("Ungültige Challenge.");
            }

            return new ChallengeInfo(Version: int.Parse(parts[0], CultureInfo.InvariantCulture),
                Iteration1: int.Parse(parts[1], CultureInfo.InvariantCulture),
                Salt1: Convert.FromHexString(parts[2]),
                Iteration2: int.Parse(parts[3], CultureInfo.InvariantCulture),
                Salt2: Convert.FromHexString(parts[4]));
        }
    }
}
