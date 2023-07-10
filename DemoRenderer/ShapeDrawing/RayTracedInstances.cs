
namespace DemoRenderer.ShapeDrawing
{
    /// <summary>
    /// GPU-relevant information for the rendering of a single sphere instance.
    /// </summary>
    public struct SphereInstance
    {
        public System.Numerics.Vector3 Position;
        public float Radius;
        public System.Numerics.Vector3 PackedOrientation;
        public uint PackedColor;
    }
    /// <summary>
    /// GPU-relevant information for the rendering of a single capsule instance.
    /// </summary>
    public struct CapsuleInstance
    {
        public System.Numerics.Vector3 Position;
        public float Radius;
        public ulong PackedOrientation;
        public float HalfLength;
        public uint PackedColor;
    }
    /// <summary>
    /// GPU-relevant information for the rendering of a single cylinder instance.
    /// </summary>
    public struct CylinderInstance
    {
        public System.Numerics.Vector3 Position;
        public float Radius;
        public ulong PackedOrientation;
        public float HalfLength;
        public uint PackedColor;
    }

}
