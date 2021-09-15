using UnityEngine;

namespace CodeExtensions
{
    public static class ComponentExtension
    {
        public static void CopyTransformTo(this Component source, GameObject target) =>
            source.transform.CopyTransformTo(target.transform);
        
        public static void CopyTransformFrom(this Component target, GameObject source) =>
            target.transform.CopyTransformFrom(source.transform);
        
        public static void CopyTransformTo(this Component source, Component target) =>
            source.transform.CopyTransformTo(target.transform);
        
        public static void CopyTransformFrom(this Component target, Component source) =>
            target.transform.CopyTransformFrom(source.transform);
    }
}