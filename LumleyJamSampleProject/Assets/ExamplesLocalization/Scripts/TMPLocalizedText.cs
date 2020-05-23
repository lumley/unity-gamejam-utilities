using TMPro;
using UnityEngine;

namespace Lumley.Localization.Example
{
    public class TMPLocalizedText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _targetText;
        [SerializeField] private LocalizedText _localizedText;

        private void Start()
        {
            var localizationManager = Toolbox.Toolbox.Get<LocalizationManager>();
            _targetText.text = localizationManager.GetText(_localizedText);
        }
    }
}