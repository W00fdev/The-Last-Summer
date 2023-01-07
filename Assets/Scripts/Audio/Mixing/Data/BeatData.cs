using UnityEngine;

namespace Audio.Mixing.Data
{
    [System.Serializable]
    public class BeatData
    {
        public Matrix4x4 scheme;

        public Tick[] GetColumn(int index)
        {
            var col = scheme.GetRow(index); // matrix is inverted
            return new Tick[4] { ToTick(col.x), ToTick(col.y), ToTick(col.z), ToTick(col.w) };
        }

        public static Tick ToTick(float value)
        {
            return value switch
            {
                0 or 1 or 2 => (Tick)value,
                _ => throw new UnityException("Unsupported tick type: " + value),
            };
        }
    }
}
