using System.Linq;

namespace Martini
{
    public class IniKeyValue
    {
        public string Section;
        public string Key;
        public string CurrentValue;
        public string DefaultValue;
        public string Note;
        public string[] AllowedValues;

        public string CurrentOrDefault
        {
            get
            {
	            var currentOrDefault = CurrentValue??DefaultValue;

	            if (IsEnumeration) 
		            return AllowedValues.Contains(currentOrDefault) ? currentOrDefault : AllowedValues.FirstOrDefault();
	            
	            return currentOrDefault;
            }
        }

        public bool HasNote => !string.IsNullOrEmpty(Note);
        public bool HasChanged => CurrentValue != null && CurrentValue != DefaultValue;
        public bool IsEnumeration => AllowedValues.Length > 0;
        public bool IsBoolean => DefaultValue == "true" || DefaultValue == "false";
        public bool IsGlobal => string.IsNullOrEmpty(Section);

        public IniKeyValue(string section, string key, string defaultValue, string note, string[] allowedValues)
        {
            Section = section;
            Key = key;
            DefaultValue = defaultValue;
            Note = note;
            AllowedValues = allowedValues;
        }
    }
}