using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZUtility.Unity
{
    public static class TextUtility
    {
        /// <summary>Create a textmesh object with default color.
        /// </summary>
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, TextAnchor textAnchor, TextAlignment textAlignment)
        {
            return CreateWorldText(parent, text, localPosition, fontSize, Color.white, textAnchor, textAlignment);
        }
        /// <summary>Create a textmesh object with custom color.
        /// </summary>
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            return textMesh;
        }
    }
}
