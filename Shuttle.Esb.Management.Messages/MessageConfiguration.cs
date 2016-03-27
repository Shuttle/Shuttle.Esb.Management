using System;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace Shuttle.Esb.Management.Messages
{
	public class MessageConfiguration : IMessageConfiguration
	{
		private static ConfigurationItem<string> serializerType;

		public MessageConfiguration()
		{
			serializerType = ConfigurationItem<string>.ReadSetting("SerializerType", string.Empty);
		}

		public string SerializerType
		{
			get { return serializerType.GetValue(); }
		}

		public ISerializer GetSerializer()
		{
			if (string.IsNullOrEmpty(SerializerType))
			{
				Log.Information(MessageResources.NoSerializerDefaultSerializer);

				return new DefaultSerializer();
			}

			ISerializer serializer;
			
			try
			{
				serializer = (ISerializer) Activator.CreateInstance(Type.GetType(SerializerType));
			}
			catch (Exception ex)
			{
				Log.Error(string.Format(MessageResources.SerializerTypeExceptionDefaultSerializer, SerializerType, ex.AllMessages()));

				serializer = new DefaultSerializer();
			}
			
			return serializer;
		}
	}
}