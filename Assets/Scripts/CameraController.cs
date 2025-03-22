using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("目标设置")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);  

    [Header("边界设置")]
    public bool useBoundaries = true; 
    public float minX = -10f;         // 左边界
    public float maxX = 10f;          // 右边界
    public float minY = -10f;         // 下边界
    public float maxY = 10f;          // 上边界

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 newPosition = target.position + offset;

        if (useBoundaries)
        {
            float vertExtent = Camera.main.orthographicSize;
            float horizExtent = vertExtent * Screen.width / Screen.height;

            // 限制摄像机移动范围， 防止超出边界
            newPosition.x = Mathf.Clamp(newPosition.x, minX + horizExtent, maxX - horizExtent);
            newPosition.y = Mathf.Clamp(newPosition.y, minY + vertExtent, maxY - vertExtent);
        }

        transform.position = newPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (!useBoundaries)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0));
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(minX, maxY, 0));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(minX, minY, 0));
    }
}
