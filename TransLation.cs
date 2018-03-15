using System;
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
            MenuItems = new List<KeyValue>() { new KeyValue("Form1", "Language not available.") };
            Messages = new List<KeyValue>();
        }

        #region PROPERTIES
        public string SingleText { get; set; }

        public List<KeyValue> MenuItems { get; set; }
        public List<KeyValue> Messages { get; set; }
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

        internal string MSG(string format, params object[] args)
        {
            var idx = Messages.FindIndex(x => x.Key == format);
            if (idx == -1)
            {
                Messages.Add(new KeyValue(format, format));
                idx = Messages.Count - 1;
            }
            return String.Format(Messages[idx].Value, args);
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