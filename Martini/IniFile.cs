using System.Collections.Generic;
using System.IO;

namespace Martini
{
    public class IniFile
    {
        public Matrix Spec = new Matrix(); // Known sections, keys and default values.
        public Matrix Tips = new Matrix(); // Tips associated to known keys.
        public Matrix Opts = new Matrix(); // Options associated to known keys.
        public Matrix Data = new Matrix(); // Actual configuration.

        public string Filename;            // Original ini file name.
        public bool HasSpec;               // At least on key defined in help.

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
                WriteDictionary(writer, Spec, "# ");
                writer.WriteLine("");
                WriteDictionary(writer, Data);
            }

            // Use a local function to write either section,
            // the only difference is the comment character.
            void WriteDictionary(TextWriter writer, Matrix container, string prefix = "")
            {
                foreach (var kvp1 in container)
                {
                    // Special case for global section.
                    if (kvp1.Key != "")
                        writer.WriteLine($@"{prefix}[{kvp1.Key}]");

                    foreach (var kvp2 in kvp1.Value)
                    {
                        var tooltip = Tips.Get(kvp1.Key, kvp2.Key);
                        if (prefix != "" && !string.IsNullOrEmpty(tooltip))
                            writer.WriteLine($@"{prefix}{{{tooltip}}}");
                        writer.WriteLine($@"{prefix}{kvp2.Key}={kvp2.Value}");
                    }

                    writer.WriteLine($@"{prefix}");
                }
            }
        }

        public void Parse(string[] lines)
        {
            var specSection = "";
            var dataSection = "";
            var tooltip = "";

            Spec[specSection] = new Dictionary<string, string>();
            Data[dataSection] = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                var options = "";
                var comment = "";
                var key = "";
                var value = "";

                if (ParseComment(line, ref comment))
                {
                    ParseTooltip(comment, ref tooltip);
                    ParseOptions(comment, ref options);

                    if (ParseSection(comment, ref specSection))
                        Spec[specSection] = new Dictionary<string, string>();
                    else if (ParseKeyValue(comment, ref key, ref value))
                    {
                        Tips.Set(specSection, key, tooltip);
                        Opts.Set(specSection,key,options);
                        Spec[specSection][key] = value;
                        HasSpec = true;
                        tooltip = "";
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

        private static void ParseOptions(string line, ref string options)
        {
            var isOptions = line.StartsWith("{") && line.EndsWith("}");
            if (isOptions)
                options = line.TrimStart('{').TrimEnd('}').Trim();
        }

        private static void ParseTooltip(string line, ref string tooltip)
        {
            var isTooltip = line.StartsWith("{") && line.EndsWith("}");
            if (isTooltip)
                tooltip = line.TrimStart('{').TrimEnd('}').Trim();
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


        public class Matrix : SortedDictionary<string, Dictionary<string, string>>
        {
            public void Set(string section, string key, string value)
            {
                if (!ContainsKey(section))
                    this[section] = new Dictionary<string, string>();
                this[section][key] = value;
            }

            public string Get(string section, string key)
            {
                if (ContainsKey(section) && this[section].ContainsKey(key))
                    return this[section][key];
                return null;
            }
        }
    }
}