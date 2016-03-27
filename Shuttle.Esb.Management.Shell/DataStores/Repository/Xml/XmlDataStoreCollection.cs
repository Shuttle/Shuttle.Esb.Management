using System.Collections.Generic;
using System.Xml.Serialization;

namespace Shuttle.Esb.Management.Shell
{
	[XmlType("dataStores")]
	public class XmlDataStoreCollection : List<DataStore>
	{
	}
}