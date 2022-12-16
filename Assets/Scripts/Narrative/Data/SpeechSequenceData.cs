using System.Collections;
using UnityEngine;

namespace Narrative.Data
{
    [CreateAssetMenu(fileName = "New Sequence", menuName = "Narrative/Sequence")]
    public class SpeechSequenceData : ScriptableObject, IEnumerable
    {
        public SpeechData[] list;

        public IEnumerator GetEnumerator() => list.GetEnumerator();
    }
}