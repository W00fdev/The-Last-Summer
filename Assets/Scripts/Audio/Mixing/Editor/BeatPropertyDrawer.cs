using UnityEngine;
using UnityEditor;
using Audio.Mixing.Data;

namespace Audio.Mixing.Editor
{
    [CustomPropertyDrawer(typeof(BeatData))]
    public class BeatPropertyDrawer : PropertyDrawer
    {
        private const float width = 20;
        private const float height = 15;

        private Rect position;
        private SerializedProperty property;
        private GUIContent label;

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

        public override float GetPropertyHeight(SerializedProperty p, GUIContent l) => 5 * height;

        private void DrawLabel()
        {
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.y += height;
            EditorGUI.indentLevel = 0;
        }

        private void DrawMatrix()
        {
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    DrawCell(c, r);
        }

        private void DrawCell(int column, int row)
        {
            var matrix = property.FindPropertyRelative("scheme");
            var cell = matrix.FindPropertyRelative("e" + column + row);

            var rect = new Rect(position.position + new Vector2(width * column, height * row), new Vector2(width, height));
            var enabled = cell.floatValue == 1;
            var style = cell.floatValue == 2 ? EditorStyles.toggleGroup : EditorStyles.toggle;

            // draw toggle with 3 possible values
            var toggle = EditorGUI.Toggle(rect, enabled, style);

            if (toggle && cell.floatValue == 0)
                cell.floatValue = 1;
            else
            if (!toggle && cell.floatValue == 1)
                cell.floatValue = 2;
            else
            if (toggle && cell.floatValue == 2)
                cell.floatValue = 0;
        }
    }
}
