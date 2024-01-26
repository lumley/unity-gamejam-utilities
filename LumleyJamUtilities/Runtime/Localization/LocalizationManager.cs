using System;
using System.Collections.Generic;
using System.Globalization;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;    
#endif
using UnityEngine;

namespace Lumley.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField] private LocalizationFile _defaultFile;
        [SerializeField] private LocalizationFile[] _allFiles;
        [SerializeField] private string _language;

        private Dictionary<string, LocalizationFile> _languageToFileMap;
        private bool _isInitialized;

        public void SetLanguage(string cultureLanguage)
        {
            _language = cultureLanguage;
        }

        public string GetText(LocalizedText localizedText)
        {
            return GetText(localizedText.name);
        }

        public string GetText(string localizedTextKey)
        {
            HasText(localizedTextKey, out string value);
            return value;
        }

        public bool HasText(LocalizedText localizedText, out string value)
        {
            return HasText(localizedText.name, out value);
        }

        public bool HasText(string localizedTextKey, out string value)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                if (string.IsNullOrEmpty(_language))
                {
                    _language = CultureInfo.CurrentCulture.Name;
                }

                BuildCultureToFileMap();
            }

            if (!_languageToFileMap.TryGetValue(_language, out LocalizationFile file))
            {
                file = _defaultFile;
            }

            return file.HasValue(localizedTextKey, out value);
        }

        private void BuildCultureToFileMap()
        {
            var allFilesLength = _allFiles.Length;
            _languageToFileMap = new Dictionary<string, LocalizationFile>(allFilesLength);
            for (var i = 0; i < allFilesLength; i++)
            {
                var localizationFile = _allFiles[i];
                for (var j = 0; j < localizationFile.CultureNames.Length; j++)
                {
                    var fileCultureName = localizationFile.CultureNames[j];
#if UNITY_EDITOR
                    _languageToFileMap[fileCultureName] = Instantiate(localizationFile);
#else
                    _languageToFileMap[fileCultureName] = localizationFile;
#endif                    
                }
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Refresh all keys and files")]
        public void FillAllAvailableFilesInEditor()
        {
            var allFiles = AssetDatabase.FindAssets($"t:{nameof(LocalizationFile)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<LocalizationFile>)
                .ToArray();
            Array.Sort(allFiles, (left, right) => string.Compare(left.name, right.name, StringComparison.Ordinal));
            _allFiles = allFiles;

            var allKeys = AssetDatabase.FindAssets($"t:{nameof(LocalizedText)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<LocalizedText>)
                .ToArray();
            
            Array.Sort(allKeys, (left, right) => string.Compare(left.name, right.name, StringComparison.Ordinal));
            foreach (var localizationFile in allFiles)
            {
                if (localizationFile.SetAllKeys(allKeys))
                {
                    EditorUtility.SetDirty(localizationFile);
                }
            }
            EditorUtility.SetDirty(this);
        }
#endif
    }
}