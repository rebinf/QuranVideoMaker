using QuranVideoMaker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuranVideoMaker.Serialization
{
    public static class JsonSerializationSettings
    {
        public static JsonSerializerOptions ProjectSerializationSettings { get; private set; }

        static JsonSerializationSettings()
        {
            var projectOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            projectOptions.Converters.Add(new JsonTrackItemConverter());
            projectOptions.Converters.Add(new JsonSKColorConverter());

            ProjectSerializationSettings = projectOptions;
        }
    }
}
