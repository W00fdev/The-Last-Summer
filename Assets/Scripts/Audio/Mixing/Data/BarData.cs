using UnityEngine;

namespace Audio.Mixing.Data
{
    [CreateAssetMenu(fileName = "New Bar Setup", menuName = "Rhythm Game/Bar")]
    public class BarData : ScriptableObject
    {
        public Beat[] beats;
    }
}