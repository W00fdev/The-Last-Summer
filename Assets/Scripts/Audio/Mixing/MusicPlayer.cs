using Audio.Mixing.Data;
using UnityEngine;

namespace Audio.Mixing
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField]
        private SongData _setup;
        [SerializeField]
        private Transform _trackContainer;

        private Track[] _tracks;
        private float _lateStartTime = 2f;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _tracks = _trackContainer.GetComponentsInChildren<Track>();
            _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
        }

        private void Start()
        {
            _audioSource.clip = _setup.clip;

            foreach (var track in _tracks)
                track.Set(_setup);
        }

        private void Update()
        {
            if (Time.time >= _lateStartTime)
            {
                LateStart();
                _lateStartTime = Mathf.Infinity;
            }
        }

        private void LateStart()
        {
            _audioSource.Play();

            foreach (var track in _tracks)
                track.Play();
        }
    }
}
