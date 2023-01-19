using Audio.Mixing.Data;
using UnityEngine;

namespace Audio.Mixing.Graphics
{
    public class Bar
    {
        public readonly Transform transform;
        public readonly BarStyle style;
        public readonly int distanceBetweenTicks;

        public Bar(BarStyle style, int distance)
        {
            var go = new GameObject("bar");
            this.transform = go.transform;
            this.style = style;
            this.distanceBetweenTicks = distance;

            go.AddComponent<Rigidbody2D>();
        }

        public void Push(float distance) => transform.localPosition += distance * Vector3.up;

        public void PutTick(Tick tick, int place)
        {
            var prefab = style.GetPrefabByTick(tick);
            if (prefab == null)
                return;

            Object.Instantiate(prefab, transform).transform.localPosition = distanceBetweenTicks * place * Vector3.up;
        }

        public void PutHolder(int place)
        {
            var position = style.continiousHoldPrefab.transform.localPosition.y + distanceBetweenTicks * place;

            for (int i = 0; i < distanceBetweenTicks; i++)
            {
                Object.Instantiate(style.continiousHoldPrefab, transform).transform.localPosition = position * Vector3.up;
                position++;
            }
        }
    }
}