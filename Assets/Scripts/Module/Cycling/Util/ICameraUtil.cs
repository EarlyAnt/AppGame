using UnityEngine;

namespace AppGame.Module.Cycling
{
    /// <summary>
    /// 相机视锥区域计算工具
    /// </summary>
    public interface ICameraUtil
    {
        /// <summary>
        /// 计算相机视锥在地图上投影区域的大小
        /// </summary>
        /// <param name="mapTransform">地图平面</param>
        /// <param name="cameraPosition">相机位置</param>
        /// <param name="playerPosition">玩家位置</param>
        /// <returns></returns>
        CameraEdge GetCameraEdge(Transform mapTransform, Vector3 cameraPosition, Vector3 playerPosition);
        /// <summary>
        /// 计算指定点是否超出地图边界
        /// </summary>
        /// <param name="mapRectTransform">地图平面</param>
        /// <param name="point">指定点的位置</param>
        /// <param name="mapScale">地图缩放比</param>
        /// <returns></returns>
        bool PointInEdge(RectTransform mapRectTransform, Vector3 point, float mapScale);
    }

    public class CameraEdge
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public Vector3 TopLeft { get; set; }
        public Vector3 TopRight { get; set; }
        public Vector3 BottomLeft { get; set; }
        public Vector3 BottomRight { get; set; }
    }
}
