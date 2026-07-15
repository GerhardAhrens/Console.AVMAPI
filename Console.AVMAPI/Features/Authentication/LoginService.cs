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

    public sealed class LoginService : IFritzAuthentication
    {
        private readonly HttpClient _httpClient;
        private readonly FritzOptions _options;
        private string _sid;

        public LoginService(HttpClient httpClient, FritzOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<SessionInfo> GetSessionInfoAsync()
        {
            SessionInfo sessionInfo = null;
            string xml = await _httpClient.GetStringAsync($"{_options.Host}/login_sid.lua?version=2");

            XmlSerializer serializer = new(typeof(SessionInfo));
            using StringReader reader = new(xml);
            try
            {
                sessionInfo = (SessionInfo)serializer.Deserialize(reader)!;
            }
            catch (InvalidOperationException ex)
            {
                string errorText = ex.Message;
            }

            return sessionInfo;
        }

        public async Task<string> GetSidAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_sid) == false)
            {
                return _sid;
            }

            _sid = await LoginAsync(cancellationToken);

            return _sid;
        }

        private async Task<string> LoginAsync( CancellationToken cancellationToken)
        {
            SessionInfo session = await GetSessionInfoAsync();

            ChallengeInfo challenge = ChallengeParser.Parse(session.Challenge);

            var calculator = new ChallengeResponseCalculator();

            string response = calculator.Calculate(challenge, _options.Password);

            string url = $"{_options.Host}/login_sid.lua" +  $"?version=2" + $"&username={Uri.EscapeDataString(_options.UserName)}" + $"&response={Uri.EscapeDataString(response)}";

            string xml = await _httpClient.GetStringAsync(url, cancellationToken);

            XmlSerializer serializer = new(typeof(SessionInfo));

            using StringReader reader = new(xml);

            SessionInfo login = (SessionInfo)serializer.Deserialize(reader)!;

            if (login.Sid == "0000000000000000")
            {
                throw new InvalidOperationException("Login fehlgeschlagen.");
            }

            return login.Sid;
        }
    }
}
