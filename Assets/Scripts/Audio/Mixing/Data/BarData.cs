using NaughtyAttributes;
using System;
using UnityEngine;

namespace Audio.Mixing.Data
{
    [CreateAssetMenu(fileName = "New Bar Setup", menuName = "Rhythm Game/Bar")]
    public class BarData : ScriptableObject
    {
        public BeatData[] beats;

        public TextAsset textFile;

        [Button]
        private void ImportFromFile()
        {
            if (textFile == null)
                throw new UnityException("Text file is not assigned");

            var patterns = textFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var newBeats = new BeatData[Mathf.CeilToInt(patterns.Length / 4f)];

            for (int i = 0; i < newBeats.Length; i++)
            {
                newBeats[i] = new BeatData()
                {
                    scheme = new Matrix4x4(GetColumn(i * 4),
                                           GetColumn(i * 4 + 1),
                                           GetColumn(i * 4 + 2),
                                           GetColumn(i * 4 + 3))
                };
            }

            beats = newBeats;

            Vector4 GetColumn(int id)
            {
                if (patterns.Length <= id)
                    return new(0, 0, 0, 0);

                if (patterns[id].Length != 4)
                    throw new UnityException("Text file row should have 4 characters");

                foreach (var symbol in patterns[id])
                    switch (symbol)
                    {
                        case '0' or '1' or '2':
                            break;
                        default:
                            throw new UnityException("Wrong character in text file");
                    }

                return new(
                    Parse(patterns[id][0]), 
                    Parse(patterns[id][1]),
                    Parse(patterns[id][2]), 
                    Parse(patterns[id][3]));

                int Parse(char symbol) => int.Parse(symbol.ToString());
            }
        }
    }
}