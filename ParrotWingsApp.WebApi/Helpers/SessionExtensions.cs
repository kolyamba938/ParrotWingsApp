using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ParrotWingsApp.WebApi.Helpers
{
    public static class SessionExtensions
    {
        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            var str = session.GetString(key);
            if (String.IsNullOrEmpty(str)) return null;
            return  JsonConvert.DeserializeObject<T>(str);
        }

        public static void SaveObject<T>(this ISession session, string key, T obj) where T : class
        {
            var str = JsonConvert.SerializeObject(obj);
            session.SetString(key, str);
        }
    }
}
