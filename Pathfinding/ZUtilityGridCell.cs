using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZUtility.Unity
{
    public class GridCell<T>
    {
        // gCost: Distance to startCell
        // hCost: Distance to endCell
        // fCost: gCost + hCost

        public T value;
        public int fCost;
        public Vector2Int pos { get; private set; }
        public GridCell<T> parent = null;

        public bool occupied;
        public GridCell(Vector2Int pos)
        {
            this.pos = pos;
            occupied = false;
        }
    }
}