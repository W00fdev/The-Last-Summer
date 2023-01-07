using System;
using UnityEngine;

namespace Audio.Mixing.Data
{
    [Serializable]
    public struct BarStyle
    {
        public GameObject singleBeatPrefab;
        public GameObject continiousBeatPrefab;
        public GameObject continiousHoldPrefab;

        public GameObject GetPrefabByTick(Tick tick) => tick switch
        {
            Tick.Single => singleBeatPrefab,
            Tick.Continious => continiousBeatPrefab,
            _ => null
        };
    }
}