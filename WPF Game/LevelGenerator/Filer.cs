using System.IO;
using System.Xml;
using System.Xml.Serialization;
using GameEngine;

namespace LevelGenerator
{
    public static class Filer
    {
        public static void GenerateFile(string FileLocation, Level lvl)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Level));

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, lvl);
                    File.WriteAllText(FileLocation, sww.ToString());
                }
            }
        }
    }
}