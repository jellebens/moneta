using Newtonsoft.Json.Serialization;

namespace Moneta.Frontend.Commands
{
    public class CommandsBinder: ISerializationBinder
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

        public static CommandsBinder Instance() {
            return new CommandsBinder()
            {
                KnownTypes = typeof(ICommand).Assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t)).ToArray()
            };
        }
    }


}
