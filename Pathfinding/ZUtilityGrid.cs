using System.Collections.Generic;
using UnityEngine;

namespace ZUtility.Unity
{
    class ZGrid<T>
    {
        public GridCell<T>[,] gridArray;

        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
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

            gridArray = new GridCell<T>[width, height];
            debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = new GridCell<T>(new Vector2Int(x,y));
                }
            }
        }
        /// <summary>Draw debug lines and display on each position
        /// </summary>
        public void DrawDebugGrid()
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
        #region SetValue
        /// <summary>Set the Value of position x, y in the grid with coordinates.
        /// </summary>
        public void SetValue(int x, int y, T newValue)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y].value = newValue;
            }
        }
        /// <summary>Set the Value of position x, y in the grid from a world position.
        /// </summary>
        public void SetValue(Vector3 worldPosition, T newValue)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);
            SetValue(x, y, newValue);
        }
        #endregion
        #region GetValue
        /// <summary>Get the Value of position x, y in the grid from coordinates.
        /// </summary>
        public T GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y].value;
            }
            else
            {
                return default;
            }
        }
        /// <summary>Get the Value of position x, y in the grid from coordinates by 2D vector.
        /// </summary>
        public T GetValue(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                return gridArray[pos.x, pos.y].value;
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
        public T GetValueFromWorldPos(Vector3 worldPosition)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);
            return GetValue(x, y);
        }
        #endregion
        #region GetCell
        public GridCell<T> GetCell(int x, int y)
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

        public GridCell<T> GetCell(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                return gridArray[pos.x, pos.y];
            }
            else
            {
                return default;
            }
        }
        public GridCell<T> GetCell(Vector3 worldPosition)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);
            return GetCell(x, y);
        }
        #endregion
        #region WorldPosition
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
        #endregion
        #region Pathfinding
        public ZGridPathInfo FindPath(Vector2Int startPos, Vector2Int endPos)
        {
            List<GridCell<T>> openCells = new List<GridCell<T>>();
            List<GridCell<T>> closedCells = new List<GridCell<T>>();

            openCells.Add(new GridCell<T>(startPos));

            GridCell<T> currentCell = openCells[0];

            while (currentCell.pos != endPos)
            {
                currentCell = openCells[0];
                foreach (GridCell<T> cell in openCells)
                {
                    if (cell.fCost < currentCell.fCost)
                    {
                        currentCell = cell;
                    }
                }
                openCells.Remove(currentCell);
                closedCells.Add(currentCell);

                if (currentCell.pos == endPos)
                {
                    break;
                }

                GridCell<T>[] neighbours = GetNeighbours(currentCell);
                GridCell<T> bestNeighbour = null;

                if (neighbours.Length > 0)
                {
                    bestNeighbour = neighbours[0];
                }
                for (int i = 0; i < neighbours.Length; i++)
                {
                    if (closedCells.Contains(neighbours[i]) || neighbours[i].occupied) //Conditions for searching a cell
                    {
                        continue;
                    }
                    if (neighbours[i].fCost < bestNeighbour.fCost || !openCells.Contains(neighbours[i]))
                    {
                        neighbours[i].parent = currentCell;
                        if (!openCells.Contains(neighbours[i]))
                        {
                            openCells.Add(neighbours[i]);
                        }
                    }
                }
            }

            List<Vector2Int> newPath = new List<Vector2Int>();
            GetPath(GetCell(endPos));
            return new ZGridPathInfo(startPos, endPos, newPath.ToArray());

            void GetPath(GridCell<T> cell)
            {
                if (cell.parent != null)
                {
                    newPath.Add(cell.parent.pos);
                    GetPath(cell.parent);
                }
            }

            GridCell<T>[] GetNeighbours(GridCell<T> cell)
            {
                List<GridCell<T>> neighbours = new List<GridCell<T>>();

                NewNeighbour(new Vector2Int(cell.pos.x + 1, cell.pos.y), cell);
                NewNeighbour(new Vector2Int(cell.pos.x - 1, cell.pos.y), cell);
                NewNeighbour(new Vector2Int(cell.pos.x, cell.pos.y + 1), cell);
                NewNeighbour(new Vector2Int(cell.pos.x, cell.pos.y - 1), cell);

                return neighbours.ToArray();

                void NewNeighbour(Vector2Int pos, GridCell<T> parent)
                {
                    if (ZFunctions.IsInRange(pos.x, 0, width - 1) && ZFunctions.IsInRange(pos.y, 0, height - 1) && gridArray[pos.x, pos.y] != null)
                    {
                        CalcFCost(gridArray[pos.x, pos.y], startPos, endPos);
                        neighbours.Add(gridArray[pos.x, pos.y]);
                    }
                }
            }
        }
        public void CalcFCost(GridCell<T> cell, Vector2Int startCell, Vector2Int endCell)
        {
            int gCost, hCost;
            gCost = Mathf.RoundToInt(Vector2.Distance(startCell, cell.pos));
            hCost = Mathf.RoundToInt(Vector2.Distance(endCell, cell.pos));
            cell.fCost = gCost + hCost;
        }
        #endregion
    }
}