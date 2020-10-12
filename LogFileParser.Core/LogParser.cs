using System;
using System.ComponentModel;
using LogFileParser.Core.Interfaces;

namespace LogFileParser.Core
{
    public class LogParser : ILogParser
    {
        //private static FieldInfo[] TypeFields;

        public T Parse<T>(params string[] logFields) where T : class, new()
        {
            var instance = new T();
            var TypeFields = typeof(T).GetFields(); //todo throw exception if both does not match

            for (int i = 0; i < logFields.Length; i++)
            {
                var converter = TypeDescriptor.GetConverter(TypeFields[i].FieldType);
                bool canConvert = converter.CanConvertFrom(logFields[i].GetType());

                if (!canConvert) continue;
                var convertedValue = converter.ConvertFrom(logFields[i]);
                TypeFields[i].SetValue(instance, convertedValue);
            }
            return instance;
        }

        public T TryParse<T>(params string[] logFields) where T : class, new()
        {
            var instance = new T();
            var TypeFields = typeof(T).GetFields();

            for (int i = 0; i < logFields.Length; i++)
            {
                if (string.IsNullOrEmpty(logFields[i]) || logFields[i] == "-") // - means no value in W3CLogFormat standard change is other have similar
                                                                               // to make it centralized class level
                {
                    continue;
                }
                var targetType = TypeFields[i].FieldType;
                Type[] argTypes = { typeof(string), targetType.MakeByRefType() };
                var tryParseMethodInfo = targetType.GetMethod("TryParse", argTypes);
                if (tryParseMethodInfo != null)
                {
                    object[] args = { logFields[i], null };
                    var successfulParse = (bool)tryParseMethodInfo.Invoke(null, args);
                    if (successfulParse)
                    {
                        TypeFields[i].SetValue(instance, args[1]);
                    }
                    //For non successful Parse we are leaving it to default value
                }
                else
                {
                    //TypeFields[i].SetValue(instance, Convert.ChangeType(logFields[i], targetType));//For future reference
                    TypeFields[i].SetValue(instance, logFields[i]); //For our current data type it will be string
                }
            }
            return instance;
        }
    }
}