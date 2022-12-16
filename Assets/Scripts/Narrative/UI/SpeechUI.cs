using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Narrative.Data;
using Utilities.UI;

namespace Narrative.UI
{
    public class SpeechUI : MonoBehaviour
    {
        [SerializeField]
        private Image _portrait;
        [SerializeField]
        private TextContainerImage _text;

        private Queue<SpeechData> _speeches = new();

        public void SetSequence(SpeechSequenceData data)
        {
            foreach (SpeechData speech in data)
                _speeches.Enqueue(speech);
        }

        public void Continue()
        {
            if (!gameObject.activeSelf)
                Open();

            if (_speeches.TryDequeue(out var data))
                SetSpeech(data);
            else
                Close();
        }

        public void Open() => gameObject.SetActive(true);
        public void Close() => gameObject.SetActive(false);

        private void SetSpeech(SpeechData data)
        {
            _portrait.sprite = data.portrait;
            _text.Text = data.text;
            Open();
        }

    }
}