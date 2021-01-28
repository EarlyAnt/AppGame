using UnityEngine;

namespace AppGame.Module.Cycling
{
    /// <summary>
    /// 相机视锥区域计算工具
    /// </summary>
    public class CameraUtil : ICameraUtil
    {
        private const float FIELD_OF_VIEW_VERTICAL = 60f;
        private const float FIELD_OF_VIEW_HORIZONTAL = 35.98339f;

        /// <summary>
        /// 计算相机视锥在地图上投影区域的大小
        /// </summary>
        /// <param name="mapTransform">地图平面</param>
        /// <param name="cameraPosition">相机位置</param>
        /// <param name="playerPosition">玩家位置</param>
        /// <returns></returns>
        public CameraEdge GetCameraEdge(Transform mapTransform, Vector3 cameraPosition, Vector3 playerPosition)
        {
            CameraEdge cameraEdge = new CameraEdge();

            Vector3 localPos = mapTransform.InverseTransformPoint(cameraPosition);
            float dis = Vector3.Dot(localPos, Vector3.forward);
            Vector3 vectorNormal = Vector3.forward * dis;
            localPos = localPos - vectorNormal;
            Vector3 projection = mapTransform.TransformPoint(localPos);

            float distance = Vector3.Distance(cameraPosition, projection);
            cameraEdge.Height = 2.0f * distance * Mathf.Tan(FIELD_OF_VIEW_VERTICAL * 0.5f * Mathf.Deg2Rad);
            cameraEdge.Width = 2.0f * distance * Mathf.Tan(FIELD_OF_VIEW_HORIZONTAL * 0.5f * Mathf.Deg2Rad);
            Debug.LogFormat("distance: {0}, height: {1}, width: {2}", distance, cameraEdge.Height, cameraEdge.Width);

            Vector3 top = new Vector3(playerPosition.x, playerPosition.y + cameraEdge.Height / 2, mapTransform.position.z);
            Vector3 bottom = new Vector3(playerPosition.x, playerPosition.y - cameraEdge.Height / 2, mapTransform.position.z);
            Vector3 left = new Vector3(playerPosition.x + cameraEdge.Width / 2, playerPosition.y, mapTransform.position.z);
            Vector3 right = new Vector3(playerPosition.x - cameraEdge.Width / 2, playerPosition.y, mapTransform.position.z);

            cameraEdge.TopLeft = new Vector3(left.x, top.y, top.z);
            cameraEdge.TopRight = new Vector3(right.x, top.y, top.z);
            cameraEdge.BottomLeft = new Vector3(left.x, bottom.y, bottom.z);
            cameraEdge.BottomRight = new Vector3(right.x, bottom.y, bottom.z);

            return cameraEdge;
        }
        /// <summary>
        /// 计算指定点是否超出地图边界
        /// </summary>
        /// <param name="mapRectTransform">地图平面</param>
        /// <param name="point">指定点的位置</param>
        /// <param name="mapScale">地图缩放比</param>
        /// <returns></returns>
        public bool PointInEdge(RectTransform mapRectTransform, Vector3 point, float mapScale)
        {
            if (point.x >= -mapRectTransform.sizeDelta.x * mapScale / 2f &&
                point.x <= mapRectTransform.sizeDelta.x * mapScale / 2f &&
                point.y >= -mapRectTransform.sizeDelta.y * mapScale / 2f &&
                point.y <= mapRectTransform.sizeDelta.y * mapScale / 2f)
                return true;
            else
                return false;
        }
    }
}
