//-----------------------------------------------------------------------
// <copyright file="LoginService.cs" company="Lifeprojects.de">
//     Class: LoginService
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// der LoginService holt die Session Informationen, um daraus die ChallengeInfo ableiten zu können
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public sealed class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SessionInfo> GetSessionInfoAsync()
        {
            string xml = await _httpClient.GetStringAsync("http://fritz.box/login_sid.lua?version=2");

            XmlSerializer serializer = new(typeof(SessionInfo));

            using StringReader reader = new(xml);

            return (SessionInfo)serializer.Deserialize(reader)!;
        }
    }
}
