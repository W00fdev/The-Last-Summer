using Audio.Mixing.Data;
using System.Collections;
using UnityEngine;

namespace Audio.Mixing
{
    public class BeatAndBarsNumerator : IEnumerator
    {
        private readonly BarData[] _bars;

        // current bar and beat position on a track
        private int _barId = 0;
        private int _beatId = -1;

        object IEnumerator.Current => Current;

        public BeatData Current => IsCurrentExists()
            ? _bars[_barId].beats[_beatId]
            : throw new UnityException("Beat out of bounds");

        public BeatAndBarsNumerator(BarData[] data) => _bars = data;

        public bool MoveNext()
        {
            _beatId++;

            if (!IsCurrentExists())
            {
                _barId++;
                _beatId = 0;

                if (!IsCurrentExists())
                    return false;
            }

            return true;
        }

        public void Reset()
        {
            _barId = 0;
            _beatId = -1;
        }

        public bool IsCurrentExists()
        {
            if (_bars.Length <= _barId)
                return false;

            if (_bars[_barId].beats.Length <= _beatId)
                return false;

            return true;
        }
    }
}
