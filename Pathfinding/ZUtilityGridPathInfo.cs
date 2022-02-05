using UnityEngine;

namespace ZUtility.Unity
{
    public class ZGridPathInfo
    {
        public Vector2Int startPos { get; private set; }
        public Vector2Int endPos { get; private set; }
        public Vector2Int[] path { get; private set; }

        public int pathLength { get { return path.Length; } }

        public ZGridPathInfo(Vector2Int startPos, Vector2Int endPos, Vector2Int[] path)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.path = path;
        }
    }
}