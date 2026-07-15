//-----------------------------------------------------------------------
// <copyright file="StatisticsParser.cs" company="Lifeprojects.de">
//     Class: StatisticsParser
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.07.2026</date>
//
// <summary>
// Template für eine neue C# Standard-Klasse
// </summary>
//-----------------------------------------------------------------------

namespace Console.AVMAPI.SimpleFritz
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    internal sealed class StatisticsParser
    {
        public DeviceStatistics Parse(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            XElement root = doc.Root!;

            return new DeviceStatistics
            {
                EnergyDaily = ParseEnergy(root, 86400, "Tagesverbrauch"),

                EnergyMonthly = ParseEnergy(root, 2678400, "Monatsverbrauch")
            };
        }

        private StatisticSeries ParseEnergy(XElement root, int grid, string name)
        {
            XElement energy = root.Element("energy");

            if (energy == null)
            {
                return null;
            }

            XElement stats = energy.Elements("stats").FirstOrDefault(s => (int?)s.Attribute("grid") == grid);

            if (stats == null)
            {
                return null;
            }

            return ParseSeries(stats, name);
        }

        private StatisticSeries ParseSeries(XElement stats, string name)
        {
            int grid = (int)stats.Attribute("grid")!;

            long unix = (long)stats.Attribute("datatime")!;

            DateTime last =  DateTimeOffset.FromUnixTimeSeconds(unix).LocalDateTime;

            string[] values = stats.Value.Split(',');

            List<StatisticValue> result = [];

            for (int i = 0; i < values.Length; i++)
            {
                if (!double.TryParse(values[i], out double value))
                {
                    continue;
                }

                result.Add(new StatisticValue(last.AddSeconds(-(grid * i)), value));
            }

            result.Reverse();

            return new StatisticSeries(name, grid, result);
        }
    }
}
