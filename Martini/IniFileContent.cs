using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Martini
{
    public class IniFileContent
    {
        private List<IniKeyValue> _iniEntries = new List<IniKeyValue>();

        public string FileName;

        public bool IsSupported => _iniEntries.Count > 0;
        public IEnumerable<string> GetSectionNames() => _iniEntries.Select(x => x.Section).Distinct();
        public IEnumerable<string> GetKeyNames(string section) => _iniEntries.Where(x => x.Section == section).Select(x => x.Key);
        public IEnumerable<IniKeyValue> GetIniEntries(string section) => _iniEntries.Where(x => x.Section == section);
        public IniKeyValue GetIniEntry(string section, string key) => GetIniEntries(section).FirstOrDefault(x => x.Key == key);

        public void Load(string filename)
        {
            Parse(File.ReadAllLines(filename));
            FileName = filename;
        }

        public void Save()
        {
            var lines = new List<string>();

            lines.Add("############################################################");
            lines.Add("#");
            var spaced = false;

            foreach (var section in GetSectionNames())
            {
                if (!string.IsNullOrEmpty(section))
                    lines.Add($"# [{section}]");

                foreach (var entry in GetIniEntries(section))
                {
                    var shouldSpaceOut = entry.HasNote || entry.IsEnumeration;
                    if (shouldSpaceOut && !spaced)
                        lines.Add("#");

                    if (entry.HasNote)
                        lines.Add($"# {{{entry.Note}}}");
                    if (entry.IsEnumeration)
                        lines.Add($"# <{string.Join("|", entry.AllowedValues)}>");
                    lines.Add($"# {entry.Key} = {entry.DefaultValue}");
                    if (shouldSpaceOut)
                        lines.Add("#");
                    spaced = shouldSpaceOut;
                }
                lines.Add("#");
            }
            lines.Add("############################################################");

            foreach (var section in GetSectionNames())
            {
                lines.Add(string.IsNullOrEmpty(section) ? "" : $"[{section}]");
                foreach (var entry in GetIniEntries(section))
                    lines.Add($"{entry.Key} = {entry.CurrentOrDefault}");
                lines.Add("");
            }
            File.WriteAllLines(FileName, lines);
        }

        public void Parse(string lines)
        {
            Parse(Regex.Split(lines, "\r\n|\r|\n"));
        }

        public void Parse(IEnumerable<string> lines)
        {
            _iniEntries = new List<IniKeyValue>();
            var section = "";
            var effectiveSection = "";
            var note = "";
            var allowedValues = new string[] { };
            var key = "";
            var value = "";
            foreach (var line in lines.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)))
            {
                var uncommented = "";

                if (ParseComment(line, ref uncommented))
                {
                    if (ParseSection(uncommented, ref section))
                        continue;
                    if (ParseNote(uncommented, ref note))
                        continue;
                    if (ParseAllowedValues(uncommented, ref allowedValues))
                        continue;
                    if (ParseKeyValue(uncommented, ref key, ref value))
                    {
                        var entry = new IniKeyValue(section, key, value, note, allowedValues);
                        note = "";
                        allowedValues = new string[] { };
                        _iniEntries.Add(entry);
                    }
                }
                else
                {
                    key = value = "";

                    if (ParseSection(line, ref effectiveSection))
                        continue;
                    if (ParseKeyValue(line, ref key, ref value))
                    {
                        var entry = _iniEntries.Find(x => x.Section == effectiveSection && x.Key == key);
                        if (entry != null)
                            entry.CurrentValue = value;
                    }
                }
            }
        }

        private static bool ParseComment(string line, ref string uncommented)
        {
            var isComment = line.StartsWith("#");
            if (isComment)
                uncommented = line.TrimStart('#').Trim();
            return isComment;
        }

        private static bool ParseSection(string line, ref string name)
        {
            var isSectionMarker = line.StartsWith("[") && line.EndsWith("]");
            if (isSectionMarker)
                name = line.TrimStart('[').TrimEnd(']').Trim();
            return isSectionMarker;
        }

        private static bool ParseKeyValue(string line, ref string key, ref string value)
        {
            var i = line.IndexOf('=');
            if (i > 0)
            {
                key = line.Substring(0, i).Trim();
                value = line.Substring(i + 1).Trim();
                return true;
            }
            return false;
        }

        private static bool ParseNote(string line, ref string note)
        {
            var isNote = line.StartsWith("{") && line.EndsWith("}");
            if (isNote)
                note = line.TrimStart('{').TrimEnd('}').Trim();
            return isNote;
        }

        private static bool ParseAllowedValues(string line, ref string[] values)
        {
            var isValueSet = line.StartsWith("<") && line.EndsWith(">");
            if (isValueSet)
                values = line.TrimStart('<').TrimEnd('>')
                    .Split('|')
                    .Select(x => x.Trim())
                    .ToArray();
            return isValueSet;
        }
    }
}
