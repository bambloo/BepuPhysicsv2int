using BepuPhysics;
using BepuUtilities;
using BepuUtilities.Collections;
using BepuUtilities.Numerics;
using BepuUtilities.Utils;
using System.Runtime.CompilerServices;

namespace DemoRenderer.Constraints
{
    public static class ContactLines
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BuildOrthonormalBasis(Vector3 normal, out Vector3 t1, out Vector3 t2)
        {
            //No frisvad or friends here- just want a simple and consistent basis with only one singularity.
            //Could be faster if needed.
            t1 = Vector3.Cross(normal, new Vector3(1, -1, 1));
            var lengthSquared = t1.LengthSquared();
            if (lengthSquared < Constants.C1em8)
            {
                t1 = Vector3.Cross(normal, new Vector3(-1, 1, 1));
                lengthSquared = t1.LengthSquared();
            }
            t1 /= MathF.Sqrt(lengthSquared);
            t2 = Vector3.Cross(normal, t1);
        }

        public static void Add(in RigidPose poseA, ref Vector3Wide offsetAWide, ref Vector3Wide normalWide, ref Vector<Number> depthWide,
            Vector3 tint, ref QuickList<LineInstance> lines)
        {
            Vector3Wide.ReadFirst(offsetAWide, out var offsetA);
            Vector3Wide.ReadFirst(normalWide, out var normal);
            var depth = depthWide[0];
            var contactPosition = offsetA + poseA.Position;
            BuildOrthonormalBasis(normal, out var t1, out var t2);
            var packedColor = Helpers.PackColor(tint * (depth >= 0 ? new Vector3(0,1,0) : new Vector3(Constants.C0p15, Constants.C0p25, Constants.C0p15)));
            t1 *= Constants.C0p5;
            t2 *= Constants.C0p5;
            var t1Line = new LineInstance(contactPosition - t1, contactPosition + t1, packedColor, 0);
            lines.AddUnsafely(t1Line);
            var t2Line = new LineInstance(contactPosition - t2, contactPosition + t2, packedColor, 0);
            lines.AddUnsafely(t2Line);

        }
    }
}
