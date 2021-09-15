using UnityEngine;

namespace CodeExtensions
{
    public static class TransformExtension
    {
        public static void CopyTransformTo(this Transform source, Transform target)
        {
            target.position = source.position;
            target.rotation = source.rotation;
            target.localScale = source.localScale;
        }
        
        public static void CopyTransformFrom(this Transform target, Transform source)
        {
            target.position = source.position;
            target.rotation = source.rotation;
            target.localScale = source.localScale;
        }
    }
}