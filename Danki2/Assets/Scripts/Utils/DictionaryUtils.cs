using System;
using System.Collections.Generic;

namespace Utils
{
    public static class DictionaryUtils
    {
        /// <summary>
        /// Instantiates a dictionary, keyed on the enum type, with
        /// every value of the dictionary set to the default value.
        /// </summary>
        public static Dictionary<TEnum, TValue> EnumDictionary<TEnum, TValue>(TValue defaultValue) where TEnum : Enum
        {
            Dictionary<TEnum, TValue> dictionary = new Dictionary<TEnum, TValue>();
            foreach (TEnum key in Enum.GetValues(typeof(TEnum)))
            {
                dictionary.Add(key, defaultValue);
            }
            return dictionary;
        }
    }
}
