using System;
using System.ComponentModel;
using LogFileParser.Core.Interfaces;

namespace LogFileParser.Core
{
    public class LogParser : ILogParser
    {
        //private static FieldInfo[] TypeFields; todo use this to create once and reuse for each instance
        private const string InvalidOperationMessage = "Invalid Log file Format selected for this operation";

        public T Parse<T>(params string[] logFields) where T : class, new()
        {
            var instance = new T();
            var typeFields = typeof(T).GetFields();

            if (typeFields.Length != logFields.Length) throw new InvalidOperationException(InvalidOperationMessage);

            for (int i = 0; i < logFields.Length; i++)
            {
                var converter = TypeDescriptor.GetConverter(typeFields[i].FieldType);
                bool canConvert = converter.CanConvertFrom(logFields[i].GetType());

                if (canConvert)
                {
                    var convertedValue = converter.ConvertFrom(logFields[i]);
                    typeFields[i].SetValue(instance, convertedValue);
                }
            }
            return instance;
        }

        public T TryParse<T>(params string[] logFields) where T : class, new()
        {
            var instance = new T();
            var typeFields = typeof(T).GetFields();

            if (typeFields.Length != logFields.Length) throw new InvalidOperationException(InvalidOperationMessage);

            for (int i = 0; i < logFields.Length; i++)
            {
                if (string.IsNullOrEmpty(logFields[i]) || logFields[i] == "-") // - means no value in W3CLogFormat standard change is other have similar
                                                                               // to make it centralized class level
                {
                    continue;
                }
                var targetType = typeFields[i].FieldType;
                Type[] argTypes = { typeof(string), targetType.MakeByRefType() };
                var tryParseMethodInfo = targetType.GetMethod("TryParse", argTypes);
                if (tryParseMethodInfo != null)
                {
                    object[] args = { logFields[i], null };
                    var successfulParse = (bool)tryParseMethodInfo.Invoke(null, args);
                    if (successfulParse)
                    {
                        typeFields[i].SetValue(instance, args[1]);
                    }
                    //For non successful Parse we are leaving it to default value
                }
                else
                {
                    //TypeFields[i].SetValue(instance, Convert.ChangeType(logFields[i], targetType));//For future reference
                    typeFields[i].SetValue(instance, logFields[i]); //For our current data type it will be string
                }
            }
            return instance;
        }
    }
}