using UnityEngine;

namespace CodeExtensions
{
    public static class VectorExtension
    {
        public static Vector2 GetXZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        public static Vector3 SetXZ(this Vector3 vector3, Vector2 xz)
        {
            return new Vector3(xz.x, vector3.y, xz.y);
        }

        public static Vector3 AddXZ(this Vector3 vector3, Vector2 xz)
        {
            return vector3 + new Vector3(xz.x, 0, xz.y);
        }

        public static Vector3 ClampXZMagnitude(this Vector3 vector3, float maxLength)
        {
            return vector3.SetXZ(Vector2.ClampMagnitude(vector3.GetXZ(), maxLength));
        }
        
        public static Vector3 ClampMagnitude(this Vector3 vector3, float maxLength)
        {
            return Vector3.ClampMagnitude(vector3, maxLength);
        }

        public static Vector3 SetX(this Vector3 vector3, float x)
        {
            return new Vector3(x, vector3.y, vector3.z);
        }
        
        public static Vector3 SetY(this Vector3 vector3, float y)
        {
            return new Vector3(vector3.x, y, vector3.z);
        }
        
        public static Vector3 SetZ(this Vector3 vector3, float z)
        {
            return new Vector3(vector3.x, vector3.y, z);
        }
        
        public static Vector3 AddY(this Vector3 vector3, float y)
        {
            return vector3.SetY(vector3.y + y);
        }
    }
}