using System.IO;
using System.Xml;
using System.Xml.Serialization;
using GameEngine;

namespace LevelGenerator
{
    public static class Generator
    {
        public static void GenerateFile(string FileLocation, Level lvl)
        {
            var xsSubmit = new XmlSerializer(typeof(Level));

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, lvl);
                    File.WriteAllText(FileLocation, sww.ToString());
                }
            }
        }
    }
}