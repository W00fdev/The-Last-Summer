using UnityEngine;

namespace Narrative.Data
{
    [CreateAssetMenu(fileName = "New Speech", menuName = "Narrative/Speech")]
    public class SpeechData : ScriptableObject
    {
        public Sprite portrait;
        [Multiline]
        public string text;
    }
}