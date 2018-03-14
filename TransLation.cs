using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Translator
{
	public class TransLation
	{
        private static string thisFileName = "Data\\English.xml";

        public TransLation()
		{
			SingleText = "Set Default";
			MenuItems = new List<KeyValue>
			{
				new KeyValue("Form1", "Language not available.")
			};
		}

		#region PROPERTIES
		public string SingleText { get; set; }

		public List<KeyValue> MenuItems { get; set; }
		#endregion
		#region METHODES
		internal static TransLation Load(string fileName)
		{
            thisFileName = fileName;
			var transLation = new TransLation(); // Load defaults.
			if (File.Exists(fileName)) using (TextReader reader = new StreamReader(fileName))
				{
					transLation = new XmlSerializer(typeof(TransLation)).Deserialize(reader) as TransLation;
				}
			return transLation;
		}

		internal void Save()
		{
			using (var writer = new StreamWriter(thisFileName))
			{
				new XmlSerializer(typeof(TransLation)).Serialize(writer, this);
			}
		}
		#endregion
	}
	public class KeyValue
	{
		public KeyValue() { }

		public KeyValue(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		[XmlAttribute] public string Key { get; set; }

		[XmlAttribute] public string Value { get; set; }

		public override string ToString()
		{
			return $"{Key} {Value}";
		}
	}
}