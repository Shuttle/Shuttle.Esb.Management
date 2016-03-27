using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb.Management.Messages
{
	public interface IMessageConfiguration
	{
		string SerializerType { get; }

		ISerializer GetSerializer();
	}
}