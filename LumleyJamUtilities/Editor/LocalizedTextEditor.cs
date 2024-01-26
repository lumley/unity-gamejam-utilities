using System.Linq;
using UnityEditor;

namespace Lumley.Localization.Editor {
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : UnityEditor.Editor {
        private class LocalizationFileTranslation {
            public LocalizationFile File;
            public LocalizedText LocalizedTextAsset;
            public string Translation;
        } 
        
        private LocalizationFile[] _files = null;
        private LocalizationFileTranslation[] AvailableTranslations;
        
        private void OnEnable() {
            if (_files is not { Length: > 0 }) {
                _files = AssetDatabase.FindAssets($"t:{nameof(LocalizationFile)}")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<LocalizationFile>)
                    .ToArray();
            }
            
            if (_files != null && AvailableTranslations == null) {
                UpdateTranslations();
            }
        }

        private void UpdateTranslations() {
            AvailableTranslations = _files.Select(x => {
                    string translation = null;
                    var keyToValues = x.GetKeyToValue();
                    for (var i = 0; i < keyToValues.Length; i++) {
                        if (keyToValues[i].Key == target) {
                            translation = keyToValues[i].Value;
                        }
                    }
                    var result = new LocalizationFileTranslation {
                        File = x,
                        LocalizedTextAsset = target as LocalizedText,
                        Translation = translation,
                    };
                    return result;
                })
                .ToArray();
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (AvailableTranslations == null || target == null) {
                return;
            }
            
            foreach (var translation in AvailableTranslations) {
                string originalTranslation = null;
                if (translation.File.HasValue(target.name, out var translated)) {
                    originalTranslation = translated;
                }
                    
                EditorGUILayout.ObjectField("Language", translation.File, typeof(LocalizationFile), false);
                var newTranslation= EditorGUILayout.DelayedTextField(translation.Translation);
                if (originalTranslation != newTranslation) {
                    translation.File.UpdateValue(target as LocalizedText, newTranslation);
                    UpdateTranslations();
                }
            }
        }
    }
}