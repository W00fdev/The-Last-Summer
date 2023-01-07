using UnityEngine;

namespace Audio.Mixing.Data
{
    [CreateAssetMenu(fileName = "New Song", menuName = "Rhythm Game/Song")]
    public class SongData : ScriptableObject
    {
        public float bpm;
        public AudioClip clip;
        public BarStyle style;
        public BarData[] bars;
    }
}