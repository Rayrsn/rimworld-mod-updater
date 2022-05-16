using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RimworldModUpdater
{
    [JsonObject(MemberSerialization.Fields)]
    public class WorkshopFileDetails
    {
        public string publishedfileid;
        public int result;
        public string creator;
        public string preview_url;
        public string title;
        public string description;
        public long time_created;

        [JsonIgnore]
        public DateTimeOffset TimeUpdated => DateTimeOffset.FromUnixTimeSeconds(time_updated);
        [JsonIgnore]
        public DateTimeOffset TimeCreated => DateTimeOffset.FromUnixTimeSeconds(time_created);

        public long time_updated;
        public int visibility;
        public int banned;
        public int creator_app_id;
        public ulong file_size;

        public bool IsValidResult()
        {
            return result == 1 && creator_app_id == 294100 && visibility == 0 && banned == 0;
        }
    }
}
