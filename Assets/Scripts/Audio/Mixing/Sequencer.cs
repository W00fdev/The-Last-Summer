using Audio.Mixing.Data;
using Audio.Mixing.Graphics;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace Audio.Mixing
{
    [RequireComponent(typeof(AudioSource))]
    public class Sequencer : MonoBehaviour
    {
        [Required]
        public ScrollingHighway MasterTrack;
        public TickGenerator TickGenerator { get; private set; }
        public AudioSource AudioSource { get; private set; }

        public void Load(SongData data)
        {
            MasterTrack.Setup(data);
            SetAudio(data.clip);
            SetRhythm(data.bpm);
        }

        public void PlayOrPause()
        {
            Action action;

            if (!TickGenerator.enabled)
            {
                // enabling is necessary for ticking
                TickGenerator.enabled = true;
                action = AudioSource.Play;
            }
            else
            {
                // disable rhythm and pause in one moment
                action = AudioSource.Pause;
                action += () => TickGenerator.enabled = false;
            }

            // in fact song will (start playing / be paused) after first rhythm tick
            TickGenerator.TickMeter += action;
            // that action will be called once
            TickGenerator.TickMeter += () => TickGenerator.TickMeter -= action;
        }

        public void Stop()
        {
            AudioSource.Stop();

            TickGenerator.Reload();
            TickGenerator.enabled = false;

            MasterTrack.Recharge();
        }

        private void SetAudio(AudioClip clip)
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = clip;
            AudioSource.loop = false;
            AudioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
        }

        private void SetRhythm(float bpm)
        {
            TickGenerator = gameObject.GetComponent<TickGenerator>();
            if (TickGenerator == null)
                TickGenerator = gameObject.AddComponent<TickGenerator>();

            TickGenerator.enabled = false;
            TickGenerator.BPM = bpm;
            TickGenerator.TickMeter += MasterTrack.Push;
            TickGenerator.BeatMeter += MasterTrack.DrawNext;
        }
    }
}