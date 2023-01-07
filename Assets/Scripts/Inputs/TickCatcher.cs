using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs
{
    public class TickCatcher : MonoBehaviour
    {
        [SerializeField]
        private KeyCode key;

        private bool IsKeyDown => Input.GetKeyDown(key);
        private bool IsKeyHold => Input.GetKey(key);

        private SpriteRenderer _singleTick;
        private SpriteRenderer _continiousTick;
        private HashSet<SpriteRenderer> _holders = new();

        private void Update()
        {
            if (IsKeyDown)
            {
                if (_singleTick != null)
                {
                    _singleTick.color = Color.green;
                    _singleTick = null;
                }
                else
                if (_continiousTick != null)
                {
                    _continiousTick.color = Color.green;
                    _continiousTick = null;
                    StartCoroutine(Holding());
                }
            }
        }

        private IEnumerator Holding()
        {
            yield return new WaitUntil(() => _holders.Count > 0);

            while (_holders.Count > 0)
            {
                if (!IsKeyHold)
                    yield break;

                foreach (var holder in _holders)
                    holder.color = Color.green;

                yield return new WaitForEndOfFrame();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.layer)
            {
                case 10:
                    _holders.Add(other.GetComponent<SpriteRenderer>());
                    break;

                case 11:
                    if (_singleTick != null)
                        throw new UnityException("tick catcher collider is too long");

                    _singleTick = other.GetComponent<SpriteRenderer>();
                    break;

                case 12:
                    if (_continiousTick != null)
                        throw new UnityException("tick catcher collider is too long");

                    _continiousTick = other.GetComponent<SpriteRenderer>();
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            switch (other.gameObject.layer)
            {
                case 10:
                    _holders.Remove(other.GetComponent<SpriteRenderer>());
                    break;
                case 11:
                    _singleTick = null;
                    break;
                case 12:
                    _continiousTick = null;
                    break;
            }
        }
    }
}