using Audio.Mixing.Data;
using UnityEngine;

namespace Audio.Mixing
{
    public class Track : MonoBehaviour
    {
        [SerializeField]
        private int _trackId;
        [SerializeField]
        private int _maxTactCount = 3;
        [SerializeField]
        private RectTransform _barContainer;
        [SerializeField]
        private RectTransform _barPrefab;
        [SerializeField]
        private RectTransform _beatPrefab;

        private float _height;
        private float _barStartTime = Mathf.Infinity;

        private int _fixedTickCount = 0;
        public float FixedUpdatesPerBeat => 60f / (Time.fixedDeltaTime * _setup.bpm);

        private int _barId;
        private int _beatId;
        private SongData _setup;

        private void Awake() => _height = _barPrefab.sizeDelta.y;

        private void FixedUpdate()
        {
            if (_barStartTime == Mathf.Infinity)
                return;

            _barContainer.anchoredPosition = new(0f, _barContainer.anchoredPosition.y - _height / FixedUpdatesPerBeat);
            _fixedTickCount++;

            if (_fixedTickCount >= FixedUpdatesPerBeat) 
            {
                Destroy(_barContainer.GetChild(0).gameObject);
                AddBar();
                _barStartTime = Time.time;
                _fixedTickCount = 0;
            }
        }

        public void Set(SongData song)
        {
            _setup = song;

            while (_barContainer.childCount < _maxTactCount)
                AddBar();
        }

        public void Play() => _barStartTime = Time.time;
        public void Stop() => _barStartTime = Mathf.Infinity;

        private void AddBar()
        {
            float lastPosition = 0f;

            if (_barContainer.childCount > 0)
            {
                var latBar = _barContainer.GetChild(_barContainer.childCount - 1) as RectTransform;
                lastPosition = latBar.anchoredPosition.y;
            }

            var newBar = Instantiate(_barPrefab, _barContainer);
            newBar.anchoredPosition = new(0f, _height + lastPosition);

            if (_setup.bars.Length <= _barId)
                return;

            var bar = _setup.bars[_barId];
            var beat = bar.beats[_beatId];
            var row = beat.scheme.GetRow(_trackId);

            if (row.x > 0)
                AddBeat(0, newBar);

            if (row.y > 0)
                AddBeat(1, newBar);

            if (row.z > 0)
                AddBeat(2, newBar);

            if (row.w > 0)
                AddBeat(3, newBar);

            _beatId++;

            if (_setup.bars[_barId].beats.Length <= _beatId)
            {
                _barId++;
                _beatId = 0;

                if (_setup.bars.Length <= _barId)
                {
                    Stop();
                }
            }
        }

        private void AddBeat(int index, RectTransform parent)
        {
            var beat = Instantiate(_beatPrefab, parent);
            beat.anchoredPosition = new(0f, _height / 4f * index);
        }
    }
}