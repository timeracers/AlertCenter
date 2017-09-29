using System;
using System.Reflection;
using static Dapper.SqlMapper;

namespace AlertCenter.Dtos
{
    public class Topic : ITypeMap
    {
        public string Name { get; set; }

        public Topic(string topic)
        {
            Name = topic;
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return null;
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return typeof(Topic).GetConstructors()[0];
        }

        public IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            return null;
        }

        public IMemberMap GetMember(string columnName)
        {
            return null;
        }
    }
}
