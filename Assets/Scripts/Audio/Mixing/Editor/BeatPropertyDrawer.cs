using UnityEngine;
using UnityEditor;
using Audio.Mixing.Data;

namespace Audio.Mixing.Editor
{
    [CustomPropertyDrawer(typeof(Beat))]
    public class BeatPropertyDrawer : PropertyDrawer
    {
        const float CELL_WIDTH = 20;
        const float CELL_HEIGHT = 15;

        Rect position;
        SerializedProperty property;
        GUIContent label;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent lab)
        {
            position = pos;
            property = prop;
            label = lab;

            EditorGUI.BeginProperty(position, label, property);

            DrawLabel();
            DrawMatrix();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty p, GUIContent l)
        {
            return 5 * CELL_HEIGHT;
        }

        void DrawLabel()
        {
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.y += CELL_HEIGHT;
            EditorGUI.indentLevel = 0;
        }

        void DrawMatrix()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    DrawCell(c, r);
                }
            }
        }

        void DrawCell(int column, int row)
        {
            Vector2 cellPos = position.position;
            cellPos.x += CELL_WIDTH * column;
            cellPos.y += CELL_HEIGHT * row;

            var matrix = property.FindPropertyRelative("scheme");
            var cell = matrix.FindPropertyRelative("e" + column + row);

            bool toggle = EditorGUI.Toggle(
                new Rect(cellPos, new Vector2(CELL_WIDTH, CELL_HEIGHT)),
                cell.floatValue == 1,
                cell.floatValue == 2 ? EditorStyles.toggleGroup : EditorStyles.toggle
            );

            if (toggle && cell.floatValue == 0)
                cell.floatValue = 1;
            else
            if (!toggle && cell.floatValue == 1)
                cell.floatValue = 2;
            else
            if (toggle && cell.floatValue == 2)
                cell.floatValue = 0;

            //EditorGUI.PropertyField(
            //    new Rect(cellPos, new Vector2(CELL_WIDTH, CELL_HEIGHT)),
            //    cell,
            //    GUIContent.none
            //);
        }
    }
}
