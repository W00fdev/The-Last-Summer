using System;
using UnityEngine;

namespace Audio.Mixing
{
    public class TickGenerator : MonoBehaviour
    {
        public float BPM { get; set; } // beat per minute
        public float BPS => BPM / 60f; // beat per second
        public float TickPerBeat => 1f / (BPS * Time.fixedDeltaTime);

        public event Action TickMeter;
        public event Action BeatMeter;

        private int _numberOfTicks;
        private int _numberOfBeats;

        private float NextLastTick => TickPerBeat * (_numberOfBeats + 1);

        private void FixedUpdate()
        {
            TickMeter.Invoke();
            _numberOfTicks++;

            if (_numberOfTicks >= NextLastTick)
            {
                BeatMeter.Invoke();
                _numberOfBeats++;
            }
        }

        public void Reload() => _numberOfTicks = _numberOfBeats = 0;
    }
}