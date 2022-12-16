using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Utilities.UI
{
    [AddComponentMenu("UI/Image with text")]
    public class TextContainerImage : Image
    {
        private TMP_Text _text;

        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }

        protected override void Awake()
        {
            base.Awake();
            _text = GetComponentInChildren<TMP_Text>();
        }
    }
}