using Audio.Mixing.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Audio.Mixing
{
    public class SongLoader : MonoBehaviour
    {
        [SerializeField][Required]
        private SongData _songData;

        public UnityEvent<SongData> LoadEvent;

        private void Start() => LoadEvent.Invoke(_songData);
    }
}