using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using System.Linq;
#endif

namespace Lumley.Localization
{
    [CreateAssetMenu(fileName = nameof(LocalizationFile), menuName = "ScriptableObjects/"+nameof(LocalizationFile), order = 0)]
    public class LocalizationFile : ScriptableObject
    {
        [SerializeField, Tooltip("languageCode-country (ISO 639-1 ISO 3166). E.g. en-US, en-UK")] private string[] _cultureNames;

        [SerializeField] private KeyToValue[] _values;

        private Dictionary<string, string> _keyToValueDictionary;
        private bool _isInitialized;

        public string[] CultureNames => _cultureNames;

        private void OnEnable()
        {
            _isInitialized = false;
        }

        public string GetValue(string key)
        {
            HasValue(key, out string value);
            return value;
        }

        public bool HasValue(string key, out string value)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                var valuesLength = _values.Length;
                _keyToValueDictionary = new Dictionary<string, string>(valuesLength);
                for (var i = 0; i < valuesLength; i++)
                {
                    var keyToValue = _values[i];
                    _keyToValueDictionary[keyToValue.Key.name] = keyToValue.Value;
                }
            }

            return _keyToValueDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Sets all keys and values. Meant to be used only on Editor time (but if you need it in runtime, go ahead!
        /// </summary>
        /// <param name="keys">Array of <see cref="LocalizedText"/>. Length must match to <see cref="values"/></param>
        /// <param name="values">Array of <see cref="string"/>. Length must match to <see cref="keys"/></param>
        /// <returns>true when anything changed</returns>
        public bool SetAllKeysAndValues(LocalizedText[] keys, string[] values)
        {
            var keysLength = keys.Length;
            Assert.AreEqual(keysLength, values.Length);
            var keyToValues = new KeyToValue[keysLength];
            var didChange = _values.Length != keysLength;
            for (int i = 0; i < keysLength; i++)
            {
                var key = keys[i];
                var value = values[i];
                keyToValues[i] = new KeyToValue
                {
                    Key = key,
                    Value = value
                };

                if (!didChange)
                {
                    var currentKeyToValue = _values[i];
                    didChange |= currentKeyToValue.Key != key;
                    didChange |= currentKeyToValue.Value != value;
                }
            }

            if (didChange)
            {
                _values = keyToValues;
            }

            return didChange;
        }
        
        /// <summary>
        /// Sets all keys, removes those that did not match previously and keeps the ones that do. New keys are added with empty translation
        /// </summary>
        /// <param name="keys">Array of <see cref="LocalizedText"/></param>
        /// <returns>true when anything changed</returns>
        public bool SetAllKeys(LocalizedText[] keys)
        {
            HashSet<LocalizedText> newKeys = new HashSet<LocalizedText>(keys);
            var keyToValues = new List<KeyToValue>(_values);
            var equalKeys = new HashSet<LocalizedText>();
            bool didChange = false;
            for (var i = keyToValues.Count - 1; i >= 0; i--)
            {
                var keyToValue = keyToValues[i];
                var oldKey = keyToValue.Key;
                if (!newKeys.Contains(oldKey))
                {
                    keyToValues.RemoveAt(i);
                    didChange = true;
                }
                else
                {
                    equalKeys.Add(oldKey);
                }
            }
            
            foreach (var localizedText in keys)
            {
                if (!equalKeys.Contains(localizedText))
                {
                    didChange = true;
                    keyToValues.Add(new KeyToValue
                    {
                        Key = localizedText,
                        Value = string.Empty
                    });
                }
            }

            if (didChange)
            {
                keyToValues.Sort((left, right) => string.Compare(left.Key.name, right.Key.name, StringComparison.Ordinal));
                _values = keyToValues.ToArray();
            }

            return didChange;
        }

        /// <summary>
        /// So we don't need to initialize or hit the cache, we can work with the raw data
        /// </summary>
        /// <returns>Serialized data</returns>
        internal KeyToValue[] GetKeyToValue() => _values;

        internal void UpdateValue(LocalizedText localizedTextAsset, string newTranslation)
        {
#if UNITY_EDITOR
            for (var index = 0; index < _values.Length; index++) {
                if (_values[index].Key == localizedTextAsset) {
                    _values[index].Value = newTranslation;
                    UnityEditor.EditorUtility.SetDirty(this);
                    return;
                }
            }
            
            // Not Found
            var keyToValues = _values.Where(x => x.Key != null).ToList();
            keyToValues.Add(new KeyToValue {Key = localizedTextAsset, Value = newTranslation});
            keyToValues.Sort((a,b) => string.CompareOrdinal(a.Key.name, b.Key.name));
            _values = keyToValues.ToArray();
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        [Serializable]
        internal struct KeyToValue
        {
            public LocalizedText Key;
            public string Value;
        }
    }
}