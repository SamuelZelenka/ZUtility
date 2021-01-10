using UnityEngine;

namespace ZUtility.Unity
{
    class ZGrid<T>
    {
        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        private T[,] gridArray;
        private TextMesh[,] debugTextArray;
        private int debugFontSize = 10;

        public ZGrid(int width, int height, float cellSize, Vector3 originPosition, int debugFontSize)
        {
            CreateGrid(width, height, cellSize, originPosition, debugFontSize);
        }
        public ZGrid(int width, int height, float cellSize, Vector3 originPosition)
        {
            CreateGrid(width, height, cellSize, originPosition, debugFontSize);
        }
        private void CreateGrid(int width, int height, float cellSize, Vector3 originPosition, int debugFontSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            this.debugFontSize = debugFontSize;

            gridArray = new T[width, height];
            debugTextArray = new TextMesh[width, height];
        }

        /// <summary>Draw debug lines and display ToString() on each position.
        /// </summary>
        public void DrawDebugGrid()
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = TextUtility.CreateWorldText(null, gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, debugFontSize, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
        /// <summary>Set the Value of position x, y in the grid with coordinates.
        /// </summary>
        public void SetValue(int x, int y, T value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                debugTextArray[x, y].text = gridArray[x, y].ToString();
            }
        }
        /// <summary>Set the Value of position x, y in the grid from a world position.
        /// </summary>
        public void SetValue(Vector3 worldPosition, T value)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);
            SetValue(x, y, value);
        }
        /// <summary>Get the Value of position x, y in the grid from coordinates.
        /// </summary>
        public T GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return default;
            }
        }
        /// <summary>Get the Value of position x, y in the grid from a world position.
        /// </summary>
        public T GetValue(Vector3 worldPosition)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);
            return GetValue(x, y);
        }
        /// <summary>Get the Value of position x, y in the grid from a world position.
        /// </summary>
        public T GetGridValueFromWorldPos(Vector3 worldPosition)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);
            return GetValue(x, y);
        }
        /// <summary>Get world position of a grid coordinate.
        /// </summary>
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }
        /// <summary>Get grid coordinates from world position. 
        /// </summary>
        public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }
    }
}