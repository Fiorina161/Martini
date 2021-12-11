using System.Collections.Generic;
using System.IO;

namespace Martini
{
    public class IniFile
    {
        public class Container : SortedDictionary<string, Dictionary<string, string>> { }

        public Container Help = new Container(); // Known sections, keys and default values.
        public Container Data = new Container(); // Actual configuration.
        public string Filename;                  // Original ini file name.
        public bool HasHelpEntries;              // At least on key defined in help.

        public void Load(string filename)
        {
            Filename = filename;
            Parse(File.ReadAllLines(filename));
        }

        public void Save()
        {
            using (var writer = new StreamWriter(Filename, false))
            {
                writer.WriteLine("#");
                WriteDictionary(writer, Help, "# ");
                writer.WriteLine("");
                WriteDictionary(writer, Data);
            }

            // Use a local function to write either section,
            // the only difference is the comment character.
            void WriteDictionary(TextWriter writer, Container container, string prefix = "")
            {
                foreach (var kvp in container)
                {
                    // Special case for global section.
                    if (kvp.Key != "")
                        writer.WriteLine($@"{prefix}[{kvp.Key}]");

                    foreach (var kvp2 in kvp.Value)
                        writer.WriteLine($@"{prefix}{kvp2.Key}={kvp2.Value}");

                    writer.WriteLine($@"{prefix}");
                }
            }
        }

        public void SetValue(string section, string key, string value)
        {
            if (!Data.ContainsKey(section))
                Data[section] = new Dictionary<string, string>();
            Data[section][key] = value;
        }

        public string GetValue(string section, string key)
        {
            if (Data.ContainsKey(section) && Data[section].ContainsKey(key))
                return Data[section][key];
            return GetDefaultValue(section, key);
        }

        public string GetDefaultValue(string section, string key)
        {
            return Help.ContainsKey(section) && Help[section].ContainsKey(key) ? Help[section][key] : "";
        }

        public void Parse(string[] lines)
        {
            var helpSection = "";
            var dataSection = "";

            Help[helpSection] = new Dictionary<string, string>();
            Data[dataSection] = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                var comment = "";
                var key = "";
                var value = "";

                if (ParseComment(line, ref comment))
                {
                    if (ParseSection(comment, ref helpSection))
                        Help[helpSection] = new Dictionary<string, string>();
                    else if (ParseKeyValue(comment, ref key, ref value))
                    {
                        Help[helpSection][key] = value;
                        HasHelpEntries = true;
                    }
                }
                else
                {
                    if (ParseSection(line, ref dataSection))
                        Data[dataSection] = new Dictionary<string, string>();
                    else if (ParseKeyValue(line, ref key, ref value))
                        Data[dataSection][key] = value;
                }
            }
        }

        private static bool ParseComment(string line, ref string comment)
        {
            var isComment = line.StartsWith("#");
            if (isComment)
                comment = line.Substring(1).Trim();
            return isComment;
        }

        private static bool ParseSection(string line, ref string section)
        {
            var isSection = line.StartsWith("[") && line.EndsWith("]");
            if (isSection)
                section = line.TrimStart('[').TrimEnd(']').Trim();
            return isSection;
        }

        private static bool ParseKeyValue(string line, ref string key, ref string value)
        {
            var i = line.IndexOf('=');
            var isKeyValue = i > 0;
            if (isKeyValue)
            {
                key = line.Substring(0, i).Trim();
                value = line.Substring(i + 1).Trim();
            }
            return isKeyValue;
        }
    }
}