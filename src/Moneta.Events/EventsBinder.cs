using Newtonsoft.Json.Serialization;

namespace Moneta.Events
{
    public class EventsBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }

        public Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName;
            typeName = serializedType.Name;
        }

        public static EventsBinder Instance()
        {
            return new EventsBinder()
            {
                KnownTypes = typeof(IEvent).Assembly.GetTypes().Where(t => typeof(IEvent).IsAssignableFrom(t)).ToArray()
            };
        }
    }


}
