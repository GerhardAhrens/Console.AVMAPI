//-----------------------------------------------------------------------
// <copyright file="ChallengeResponseCalculator.cs" company="Lifeprojects.de">
//     Class: ChallengeResponseCalculator
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Template für eine neue C# Standard-Klasse
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public sealed class ChallengeResponseCalculator
    {
        public string Calculate(ChallengeInfo challenge, string password)
        {
            // erster PBKDF2-Durchlauf
            byte[] hash1 = Rfc2898DeriveBytes.Pbkdf2(password, challenge.Salt1.Span, challenge.Iteration1, HashAlgorithmName.SHA256, 32);

            // zweiter PBKDF2-Durchlauf
            byte[] hash2 = Rfc2898DeriveBytes.Pbkdf2(hash1, challenge.Salt2.Span, challenge.Iteration2, HashAlgorithmName.SHA256, 32);

            string hash = Convert.ToHexString(hash2).ToLowerInvariant();

            string salt2 = Convert.ToHexString(challenge.Salt2.Span).ToLowerInvariant();

            return $"{salt2}${hash}";
        }
    }
}
