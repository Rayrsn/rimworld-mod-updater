using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace RimworldModUpdater
{
    public static class SteamWorkshop
    {
        public static JObject lastResult;

        public static Encoding GetResponseEncoding(HttpContent content, Encoding fallbackEncoding)
        {
            if (content.Headers.ContentType == null || content.Headers.ContentType.CharSet == null)
                return fallbackEncoding;

            try
            {
                return Encoding.GetEncoding(content.Headers.ContentType.CharSet);
            }
            catch (ArgumentException)
            {
                return fallbackEncoding;
            }
        }

        public static async Task<JObject> GetWorkshopFileDetailsJSON(string[] fileIds, bool collection = false)
        {
            string url = $"http://api.steampowered.com/ISteamRemoteStorage/{(collection ? "GetCollectionDetails" : "GetPublishedFileDetails")}/v0001/";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("RimworldModUpdater/" + Settings.Version);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var values = new Dictionary<string, string>();

            var ids = new List<string>();
            for (int i = 0; i < fileIds.Length; i++)
            {
                string id = fileIds[i];
                if (ids.Contains(id)) continue; // Don't allow requesting duplicates.

                values.Add($"publishedfileids[{ids.Count}]", id);
                ids.Add(id);
            }
            values.Add(collection ? "collectioncount" : "itemcount", ids.Count.ToString());
            values.Add("format", "json");

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, content);
            var data = await response.Content.ReadAsByteArrayAsync();

            var encoding = GetResponseEncoding(response.Content, Encoding.UTF8);

            string str = encoding.GetString(data);
            JObject obj = null;
            try
            {
                obj = JObject.Parse(str);
            }
            catch (JsonReaderException ex)
            {
                Log.Error("Failed to parse file details JSON. Reason: {0} Result:\n{1}", ex.Message, str);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while parsing JSON. Result:\n{0}", str);
            }

            return obj;
        }

        public static async Task<JObject> GetWorkshopFileDetailsJSON(BaseMod[] mods, bool collection = false)
        {
            string url = $"http://api.steampowered.com/ISteamRemoteStorage/{(collection ? "GetCollectionDetails" : "GetPublishedFileDetails")}/v0001/";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("RimworldModUpdater/"+ Settings.Version);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var values = new Dictionary<string, string>();

            var ids = new List<string>();
            for (int i = 0; i < mods.Length; i++)
            {
                string id = mods[i].ModId;
                if (ids.Contains(id)) continue; // Don't allow requesting duplicates.

                values.Add($"publishedfileids[{ids.Count}]", id);
                ids.Add(id);
            }

            values.Add(collection ? "collectioncount" : "itemcount", values.Count.ToString());
            values.Add("format", "json");

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, content);
            var data = await response.Content.ReadAsByteArrayAsync();

            var encoding = GetResponseEncoding(response.Content, Encoding.UTF8);

            string str = encoding.GetString(data);
            JObject obj = null;

            try
            {
                obj = JObject.Parse(str);
            }
            catch (JsonReaderException ex)
            {
                Log.Error("Failed to parse file details JSON. Reason: {0} Result:\n{1}", ex.Message, str);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while parsing JSON. Result:\n{0}", str);
            }

            return obj;
        }

        public static async Task<bool> IsWorkshopCollection(string fileId)
        {
            JObject obj = await GetWorkshopFileDetailsJSON(new []{fileId}, true);

            lastResult = obj;

            if (!ResultOK(obj, true))
            {
                string txt = obj != null ? obj.ToString(Formatting.Indented) : "null";
                Log.Error("Invalid JSON return from steam web api for IsWorkshopCollection. JSON:\n{0}", txt);
                return false;
            }

            return obj["response"]["resultcount"].ToObject<int>() == 1;
        }

        public static async Task<List<WorkshopFileDetails>> GetWorkshopFileDetailsFromCollection(string collectionId)
        {
            List<WorkshopFileDetails> list = new List<WorkshopFileDetails>();
            JObject obj = await GetWorkshopFileDetailsJSON(new []{collectionId}, true);
            JObject collectionResponse = obj["response"]["collectiondetails"].ToObject<JArray>()[0].ToObject<JObject>();
            JArray arr = collectionResponse["children"].ToObject<JArray>();

            if (arr != null)
            {
                int count = arr.Count;
                List<string> ids = new List<string>();

                foreach (var jToken in arr)
                {
                    JObject file = (JObject) jToken;
                    if (file.ContainsKey("publishedfileid"))
                    {
                        ids.Add(file["publishedfileid"].ToObject<string>());
                    }
                }

                JObject fobj = await GetWorkshopFileDetailsJSON(ids.ToArray());

                if (!ResultOK(fobj))
                {
                    string txt = obj != null ? obj.ToString(Formatting.Indented) : "null";
                    Log.Error("Invalid JSON return from steam web api for GetWorkshopFileDetailsFromCollection. JSON:\n{0}", txt);
                    return null;
                }

                return fobj["response"]["publishedfiledetails"].ToObject<List<WorkshopFileDetails>>();
            }
            else
            {
                Log.Error("Couldn't get array from collection request result. JSON:\n" + obj.ToString());
            }

            return list;
        }

        private static bool ResultOK(JObject obj, bool collection = false)
        {
            // equivalent to
            // obj != null && obj["response"] != null && obj["publishedfiledetails"] != null
            return obj?["response"]?[collection ? "collectiondetails" : "publishedfiledetails"] != null;
        }

        public static async Task<List<WorkshopFileDetails>> GetWorkshopFileDetails(BaseMod[] mods)
        {
            JObject obj = await GetWorkshopFileDetailsJSON(mods);

            lastResult = obj;

            if (!ResultOK(obj))
            {
                string txt = obj != null ? obj.ToString(Formatting.Indented) : "null";
                Log.Error("Invalid JSON return from steam web api for GetWorkshopFileDetails. JSON:\n{0}", txt);
                return null;
            }

            return obj["response"]["publishedfiledetails"].ToObject<JArray>().ToObject<List<WorkshopFileDetails>>();
        }

        public static async Task<WorkshopFileDetails> GetWorkshopFileDetails(string fileId)
        {
            JObject obj = await GetWorkshopFileDetailsJSON(new []{fileId});

            lastResult = obj;

            if (!ResultOK(obj))
            {
                string txt = obj != null ? obj.ToString(Formatting.Indented) : "null";
                Log.Error("Invalid JSON return from steam web api for GetWorkshopFileDetails. JSON:\n{0}", txt);
                return null;
            }

            return obj["response"]["publishedfiledetails"].ToObject<JArray>()[0].ToObject<WorkshopFileDetails>();
        }

        public static async Task<List<WorkshopFileDetails>> GetWorkshopFileDetails(string[] fileId)
        {
            JObject obj = await GetWorkshopFileDetailsJSON(fileId);

            lastResult = obj;

            if (!ResultOK(obj))
            {
                string txt = obj != null ? obj.ToString(Formatting.Indented) : "null";
                Log.Error("Invalid JSON return from steam web api for GetWorkshopFileDetails. JSON:\n{0}", txt);
                return null;
            }

            return obj["response"]["publishedfiledetails"].ToObject<List<WorkshopFileDetails>>();
        }
    }
}
