//-----------------------------------------------------------------------
// <copyright file="SessionInfo.cs" company="Lifeprojects.de">
//     Class: SessionInfo
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2026</date>
//
// <summary>
// Template für eine neue Enum-Klasse
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("SessionInfo")]
    public sealed class SessionInfo
    {
        [XmlElement("SID")]
        public string Sid { get; set; } = "";

        [XmlElement("Challenge")]
        public string Challenge { get; set; } = "";

        [XmlElement("BlockTime")]
        public int BlockTime { get; set; }

        [XmlArray("Users")]
        [XmlArrayItem("User")]
        public List<string> Users { get; set; } = [];
    }
}
