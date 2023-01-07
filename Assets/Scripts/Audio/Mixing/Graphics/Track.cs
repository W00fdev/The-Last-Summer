using Audio.Mixing.Data;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Audio.Mixing.Graphics
{
    public class Track : IEnumerable<Bar>
    {
        public readonly Transform transform;
        public readonly int distanceBetweenBars;

        private readonly LinkedList<Bar> _bars = new();
        private Tick _lastTick = Tick.None;

        public int Count => _bars.Count;

        private float LastPosition => _bars.Count > 0 ? _bars.Last.Value.transform.localPosition.y : -distanceBetweenBars;

        public Track(Transform transform, int distance)
        {
            this.transform = transform;
            this.distanceBetweenBars = distance;
        }

        public void PutOnTop(Bar bar)
        {
            bar.transform.parent = transform;
            // put new bar above the last bar
            bar.transform.localPosition = Vector3.up * (distanceBetweenBars + LastPosition);
            _bars.AddLast(bar);
        }

        public void EraseFromBelow()
        {
            if (_bars.Count == 0)
                return;

            // remove and destroy
            var bar = _bars.First.Value;
            _bars.RemoveFirst();

            Object.Destroy(bar.transform.gameObject);
        }

        public void DrawTicksAtLast(params Tick[] ticks)
        {
            //_bars.Last.Value.TickHeight = height / ticks.Length;

            for (int i = 0; i < ticks.Length; i++)
            {
                switch (ticks[i])
                {
                    case Tick.Single:
                        _bars.Last.Value.PutTick(Tick.Single, i);
                        break;

                    case Tick.Continious:
                        if (_lastTick != Tick.Continious)
                            _bars.Last.Value.PutTick(Tick.Continious, i);
                        _bars.Last.Value.PutHolder(i);
                        break;
                }

                _lastTick = ticks[i];
            }
        }

        public IEnumerator<Bar> GetEnumerator() => ((IEnumerable<Bar>)_bars).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _bars.GetEnumerator();
    }
}