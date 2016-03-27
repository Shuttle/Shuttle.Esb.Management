using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shuttle.Core.Infrastructure
{
	public class XmlObjectSerializer
	{
		public string Serialize(object o)
		{
			return Serialize(o, null);
		}

		public string Serialize(object o, IEnumerable<KeyValuePair<string, string>> namespaces)
		{
			Guard.AgainstNull(o, "o");

			var result = new StringBuilder();

			var serializer = new XmlSerializer(o.GetType());

			var xmlSettings = new XmlWriterSettings
								  {
									  Encoding = Encoding.UTF8,
									  OmitXmlDeclaration = true,
									  Indent = false,
									  NewLineHandling = NewLineHandling.None
								  };

			var ns = new XmlSerializerNamespaces();

			if (namespaces != null)
			{
				foreach (var pair in namespaces)
				{
					ns.Add(pair.Key, pair.Value);
				}
			}

			using (var writer = XmlWriter.Create(result, xmlSettings))
			{
				if (writer != null)
				{
					serializer.Serialize(writer, o, ns);

					writer.Flush();
				}
			}

			return result.ToString();
		}

		public T Deserialize<T>(string xml) where T : class
		{
			return Deserialize<T>(xml, Encoding.Unicode);
		}

		public T Deserialize<T>(string xml, Encoding encoding) where T : class
		{
			var serializer = new XmlSerializer(typeof(T));

            using (var stream = new MemoryStream(encoding.GetBytes(xml)))
            using (var reader = XmlDictionaryReader.CreateTextReader(stream, encoding, new XmlDictionaryReaderQuotas(), null))
			{
				return serializer.Deserialize(reader) as T;
			}
		}
	}
}