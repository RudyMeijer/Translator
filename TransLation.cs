using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Translator
{
	public class TransLation
	{
		public TransLation()
		{
			SingleText = "Set Default";
		}

		public string SingleText { get; set; }
		public List<string> Combo { get; set; }
		public List<KeyValue> MenuItems { get; set; }

		internal static TransLation Load(string fileName)
		{
			var transLation = new TransLation(); // Load defaults.
			if (File.Exists(fileName)) using (TextReader reader = new StreamReader(fileName))
					transLation = new XmlSerializer(typeof(TransLation)).Deserialize(reader) as TransLation;
			return transLation;
		}

		internal void Save(string fileName)
		{
			using (var writer = new StreamWriter(fileName))
			{
				new XmlSerializer(typeof(TransLation)).Serialize(writer, this);
			}
		}
	}
	public class KeyValue
	{
		public KeyValue() { }

		public KeyValue(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		[XmlAttribute]public string Key { get; set; }
		[XmlAttribute] public string Value { get; set; }
	}
}