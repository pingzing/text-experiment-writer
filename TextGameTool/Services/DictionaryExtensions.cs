using System.Collections.Generic;

namespace TextGameTool.Services
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key, TVal val)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = val;
            }
            else
            {
                dict.Add(key, val);
            }
        }
    }
}
