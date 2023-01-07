using Audio.Mixing.Data;
using UnityEngine;
using NaughtyAttributes;

namespace Audio.Mixing.Graphics
{
    public class ScrollingHighway : MonoBehaviour
    {
        public const int tickPerBarCount = 4;

        [SerializeField]
        [Tooltip("On these transforms tracks will create")]
        private Transform[] _trackPositions;

        [SerializeField]
        [Tooltip("Move direction")]
        private bool _isInversed;

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("How much bars should be drawed at once")]
        private int _maxDrawedBarsCount;

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("A position where first bar will be placed")]
        private int _firstBarPosition;

        [SerializeField]
        [OnValueChanged("OnBarHeightChanged")]
        private int _barHeight;

        private BeatAndBarsNumerator _enumerator;
        private Track[] _tracks;

        public SongData SongData { get; private set; }
        public float PushDistance { get; private set; }

        private void Awake()
        {
            _tracks = new Track[_trackPositions.Length];

            for (int i = 0; i < _tracks.Length; i++)
                _tracks[i] = new Track(_trackPositions[i], _barHeight);
        }

        public void Setup(SongData data)
        {
            SongData = data;

            // beat and bars numerator
            _enumerator = new(SongData.bars);
            // distance the track advances each tick
            PushDistance = _barHeight * (SongData.bpm / 60f) * Time.fixedDeltaTime * (_isInversed ? -1 : 1);

            Recharge();
        }

        public void Recharge()
        {
            _enumerator.Reset();

            // erase everything
            foreach (var track in _tracks)
                while (track.Count > 0)
                    track.EraseFromBelow();

            // add '_maxDrawedBarsCount' bars
            for (int k = 0; k < _maxDrawedBarsCount; k++)
            {
                // skip first positions if needed
                if (k < _firstBarPosition)
                {
                    foreach (var track in _tracks)
                        track.PutOnTop(CreateBar());
                }
                else
                // put next bar
                if (_enumerator.MoveNext())
                {
                    for (int i = 0; i < _tracks.Length; i++)
                    {
                        _tracks[i].PutOnTop(CreateBar());
                        _tracks[i].DrawTicksAtLast(_enumerator.Current.GetColumn(i));
                    }
                }
            }
        }

        public void Push()
        {
            foreach (var track in _tracks)
                foreach (var bar in track)
                    bar.Push(PushDistance);
        }

        public void DrawNext()
        {
            if (_enumerator.MoveNext())
                for (int i = 0; i < _tracks.Length; i++)
                {
                    _tracks[i].EraseFromBelow();
                    _tracks[i].PutOnTop(CreateBar());
                    _tracks[i].DrawTicksAtLast(_enumerator.Current.GetColumn(i));
                }
        }

        private Bar CreateBar() => new(SongData.style, _barHeight / tickPerBarCount);

        private void OnBarHeightChanged()
        {
            if (_barHeight % tickPerBarCount != 0)
                throw new UnityException("Bar height must be divisible by " + tickPerBarCount);
        }
    }
}