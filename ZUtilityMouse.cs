using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZUtility.Unity
{
    public static class ZMouse
    {
        /// <summary>
        /// Return world position from mouse position.
        /// </summary>
        public static Vector3 GetMouseWorldPos()
        {
            Vector3 mousePos;
            mousePos.z = Camera.main.nearClipPlane;
            mousePos = Input.mousePosition;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}