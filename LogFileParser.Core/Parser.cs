using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogFileParser.Core
{
    public class Parser
    {
        //private static FieldInfo[] TypeFields;

        public T Parse<T>(params string[] props) where T : class, new()
        {
            var instance = new T();
            var TypeFields = typeof(T).GetFields();

            for (int i = 0; i < props.Length; i++)
            {
                var converter = TypeDescriptor.GetConverter(TypeFields[i].FieldType);
                bool canConvert = converter.CanConvertFrom(props[i].GetType());

                if (canConvert)
                {
                    var convertedValue = converter.ConvertFrom(props[i]);
                    TypeFields[i].SetValue(instance, convertedValue);
                }
            }
            return instance;
        }
    }
}