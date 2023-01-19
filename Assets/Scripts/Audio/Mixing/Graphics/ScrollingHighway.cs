using Audio.Mixing.Data;
using UnityEngine;
using NaughtyAttributes;

namespace Audio.Mixing.Graphics
{
    public class ScrollingHighway : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("On these transforms tracks will create")]
        private Transform[] _trackPositions;

        [BoxGroup]
        [InfoBox("Manage bar movement markers with places, which are measured by marker place height.")]
        [ValidateInput(nameof(IsDivisibleBy4), "Marker place height must be divisible by 4.")]
        [SerializeField]
        private int _markerPlaceHeight;

        [BoxGroup]
        [SerializeField]
        [MinMaxSlider(0, 10)]
        private Vector2Int _initialSpawnPlaces;

        private BeatAndBarsNumerator _enumerator;
        private Track[] _tracks;

        public SongData SongData { get; private set; }
        public float PushDistance { get; private set; }

        private void Awake()
        {
            _tracks = new Track[_trackPositions.Length];

            for (int i = 0; i < _tracks.Length; i++)
                _tracks[i] = new Track(_trackPositions[i], _markerPlaceHeight);
        }

        public void Setup(SongData data)
        {
            SongData = data;

            // beat and bars numerator
            _enumerator = new(SongData.bars);
            // distance the track advances each tick
            PushDistance = -_markerPlaceHeight * (SongData.bpm / 60f) * Time.fixedDeltaTime;

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
            for (int k = 0; k < _initialSpawnPlaces.y; k++)
            {
                // skip first positions if needed
                if (k < _initialSpawnPlaces.x)
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

        private Bar CreateBar()
        {
            if (!IsDivisibleBy4(_markerPlaceHeight))
                throw new UnityException("Marker place height must be divisible by 4");

            return new(SongData.style, _markerPlaceHeight / 4);
        }

        private bool IsDivisibleBy4(int value) => value % 4 == 0;
    }
}