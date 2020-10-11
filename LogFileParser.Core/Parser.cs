using System;
using System.ComponentModel;

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

        public T TryParse<T>(params string[] props) where T : class, new()
        {
            var instance = new T();
            var TypeFields = typeof(T).GetFields();

            for (int i = 0; i < props.Length; i++)
            {
                if (!string.IsNullOrEmpty(props[i]) && !(props[i] == "-")) // - means no value in W3C standard change is other have similar
                                                                           // to make it centralised class level
                {
                    var targetType = TypeFields[i].FieldType;
                    Type[] argTypes = { typeof(string), targetType.MakeByRefType() };
                    var tryParseMethodInfo = targetType.GetMethod("TryParse", argTypes);
                    if (tryParseMethodInfo != null)
                    {
                        object[] args = { props[i], null };
                        var successfulParse = (bool)tryParseMethodInfo.Invoke(null, args);
                        if (successfulParse)
                        {
                            TypeFields[i].SetValue(instance, args[1]);
                        }
                        //For non successfulParse we are leaving it to default value
                    }
                    else
                    {
                        //TypeFields[i].SetValue(instance, Convert.ChangeType(props[i], targetType));//For future
                        TypeFields[i].SetValue(instance, props[i]);
                    }
                }
            }
            return instance;
        }
    }
}