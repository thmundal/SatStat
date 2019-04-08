using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SatStat
{
    public class JSON
    {
        private static List<string> json_errors = new List<string>();
        private static bool json_error = false;

        private static JsonSerializerSettings json_settings = new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    json_errors.Add(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                }
            };

        /// <summary>
        /// Parse a JSON string to a C# object of type T
        /// </summary>
        /// <typeparam name="T">Desired return type</typeparam>
        /// <param name="input">JSON string to be parsed</param>
        /// <returns>The parsed object</returns>
        public static T parse<T>(string input)
        {
            T parsed = JsonConvert.DeserializeObject<T>(input);
            if (json_error)
            {
                Debug.Log(GetLastJsonError());
            }
            return parsed;
        }

        /// <summary>
        /// Serialize a C# object to a JSON string
        /// </summary>
        /// <param name="input">Object to serialize</param>
        /// <returns></returns>
        public static string serialize(object input)
        {
            return JsonConvert.SerializeObject(input);
        }

        /// <summary>
        /// Return the latest error that has happened in relation to JSON parsing or serialization
        /// </summary>
        /// <returns></returns>
        public static string GetLastJsonError()
        {
            return json_errors[json_errors.Count - 1];
        }
    }
}
