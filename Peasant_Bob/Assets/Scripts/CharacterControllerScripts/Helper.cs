using UnityEngine;

public static class Helper
{
    public struct ClipPlanePoints
    {
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerLeft;
        public Vector3 LowerRight;
    }
	public static float ClampAngle(float angle, float min, float max)
    {
        do
        {
            if (angle<-360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }
        } while (angle < -360 || angle > 360);

        return Mathf.Clamp(angle, min, max);

    }

    public static ClipPlanePoints ClipPlaneAtNear(Vector3 p_pos)
    {
        var clipPlanePoints = new ClipPlanePoints();

        if (Camera.main == null)
        {
            return clipPlanePoints;
        }

        var t_transform = Camera.main.transform;
        var t_halfFOV = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
        var t_aspect = Camera.main.aspect;
        var t_distance = Camera.main.nearClipPlane;
        var t_height = t_distance * Mathf.Tan(t_halfFOV);
        var t_width = t_height * t_aspect;

        clipPlanePoints.LowerRight = p_pos + t_transform.right * t_width;
        clipPlanePoints.LowerRight -= t_transform.up * t_height;
        clipPlanePoints.LowerRight += t_transform.forward * t_distance;

        clipPlanePoints.LowerLeft = p_pos - t_transform.right * t_width;
        clipPlanePoints.LowerLeft -= t_transform.up * t_height;
        clipPlanePoints.LowerLeft += t_transform.forward * t_distance;

        clipPlanePoints.UpperRight = p_pos + t_transform.right * t_width;
        clipPlanePoints.UpperRight += t_transform.up * t_height;
        clipPlanePoints.UpperRight += t_transform.forward * t_distance;

        clipPlanePoints.UpperLeft = p_pos - t_transform.right * t_width;
        clipPlanePoints.UpperLeft += t_transform.up * t_height;
        clipPlanePoints.UpperLeft += t_transform.forward * t_distance;

        return clipPlanePoints;
    }
}
