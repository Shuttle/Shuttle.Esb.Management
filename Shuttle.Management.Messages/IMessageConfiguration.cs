using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Messages
{
	public interface IMessageConfiguration
	{
		string SerializerType { get; }

		ISerializer GetSerializer();
	}
}