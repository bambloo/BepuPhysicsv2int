using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;
using Math = BepuUtilities.Utils.Math;

namespace BepuUtilities
{
    public struct MatrixFloat
    {
        /// <summary>
        /// Row 1 of the matrix.
        /// </summary>
        public System.Numerics.Vector4 X;
        /// <summary>
        /// Row 2 of the matrix.
        /// </summary>
        public System.Numerics.Vector4 Y;
        /// <summary>
        /// Row 3 of the matrix.
        /// </summary>
        public System.Numerics.Vector4 Z;
        /// <summary>
        /// Row 4 of the matrix.
        /// </summary>
        public System.Numerics.Vector4 W;
    }
    /// <summary>
    /// Provides SIMD-aware 4x4 matrix math.
    /// </summary>
    /// <remarks>
    /// All functions assume row vectors.
    /// </remarks>
    public struct Matrix
    {
        /// <summary>
        /// Row 1 of the matrix.
        /// </summary>
        public Vector4 X;
        /// <summary>
        /// Row 2 of the matrix.
        /// </summary>
        public Vector4 Y;
        /// <summary>
        /// Row 3 of the matrix.
        /// </summary>
        public Vector4 Z;
        /// <summary>
        /// Row 4 of the matrix.
        /// </summary>
        public Vector4 W;

        public static Matrix Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Matrix result;
                result.X = new Vector4(1, 0, 0, 0);
                result.Y = new Vector4(0, 1, 0, 0);
                result.Z = new Vector4(0, 0, 1, 0);
                result.W = new Vector4(0, 0, 0, 1);
                return result;
            }
        }


        public Vector3 Translation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector3(W.X, W.Y, W.Z);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                W = new Vector4(value, W.W);
            }
        }

        struct MFloat
        {
            public float M11, M12, M13, M14;
            public float M21, M22, M23, M24;
            public float M31, M32, M33, M34;
            public float M41, M42, M43, M44;
        }

        struct MNumber
        {
            public Number M11, M12, M13, M14;
            public Number M21, M22, M23, M24;
            public Number M31, M32, M33, M34;
            public Number M41, M42, M43, M44;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe static void Transpose(MNumber* m, MFloat* transposed)
        {
            //A weird function! Why?
            //1) Missing some helpful instructions for actual SIMD accelerated transposition.
            //2) Difficult to get SIMD types to generate competitive codegen due to lots of componentwise access.

            Number m12 = m->M12;
            Number m13 = m->M13;
            Number m14 = m->M14;
            Number m23 = m->M23;
            Number m24 = m->M24;
            Number m34 = m->M34;
            transposed->M11 = (float)m->M11;
            transposed->M12 = (float)m->M21;
            transposed->M13 = (float)m->M31;
            transposed->M14 = (float)m->M41;

            transposed->M21 = (float)m12;
            transposed->M22 = (float)m->M22;
            transposed->M23 = (float)m->M32;
            transposed->M24 = (float)m->M42;

            transposed->M31 = (float)m13;
            transposed->M32 = (float)m23;
            transposed->M33 = (float)m->M33;
            transposed->M34 = (float)m->M43;

            transposed->M41 = (float)m14;
            transposed->M42 = (float)m24;
            transposed->M43 = (float)m34;
            transposed->M44 = (float)m->M44;

        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void Transpose(Matrix* m, MatrixFloat* transposed)
        {
            Transpose((MNumber*)m, (MFloat*)transposed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transpose(in Matrix m, out Matrix transposed)
        {
            //Not an ideal implementation. Shuffles would be handy.

            var xy = m.X.Y;
            var xz = m.X.Z;
            var xw = m.X.W;
            var yz = m.Y.Z;
            var yw = m.Y.W;
            var zw = m.Z.W;
            transposed.X = new Vector4(m.X.X, m.Y.X, m.Z.X, m.W.X);
            transposed.Y = new Vector4(xy, m.Y.Y, m.Z.Y, m.W.Y);
            transposed.Z = new Vector4(xz, yz, m.Z.Z, m.W.Z);
            transposed.W = new Vector4(xw, yw, zw, m.W.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static MatrixFloat Transpose(Matrix m)
        {
            MatrixFloat transposed;
            Transpose(&m, &transposed);
            return transposed;
        }

        /// <summary>
        /// Transforms a vector with a transposed matrix.
        /// </summary>
        /// <param name="v">Row vector to transform.</param>
        /// <param name="m">Matrix whose transpose will be applied to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformTranspose(in Vector4 v, in Matrix m, out Vector4 result)
        {
            result = new Vector4(
                Vector4.Dot(v, m.X),
                Vector4.Dot(v, m.Y),
                Vector4.Dot(v, m.Z),
                Vector4.Dot(v, m.W));
        }

        /// <summary>
        /// Transforms a vector with a matrix.
        /// </summary>
        /// <param name="v">Row vector to transform.</param>
        /// <param name="m">Matrix to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(in Vector4 v, in Matrix m, out Vector4 result)
        {
            var x = new Vector4(v.X);
            var y = new Vector4(v.Y);
            var z = new Vector4(v.Z);
            var w = new Vector4(v.W);
            result = m.X * x + m.Y * y + m.Z * z + m.W * w;
        }

        /// <summary>
        /// Transforms a vector with a matrix. Implicitly uses 1 as the fourth component of the input vector.
        /// </summary>
        /// <param name="v">Row vector to transform.</param>
        /// <param name="m">Matrix to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Vector3 v, in Matrix m, out Vector4 result)
        {
            var x = new Vector4(v.X);
            var y = new Vector4(v.Y);
            var z = new Vector4(v.Z);
            result = m.X * x + m.Y * y + m.Z * z + m.W;
        }


        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="a">First matrix.</param>
        /// <param name="b">Second matrix.</param>
        /// <param name="result">Result of the matrix multiplication.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(in Matrix a, in Matrix b, out Matrix result)
        {
            var bX = b.X;
            var bY = b.Y;
            var bZ = b.Z;
            {
                var x = new Vector4(a.X.X);
                var y = new Vector4(a.X.Y);
                var z = new Vector4(a.X.Z);
                var w = new Vector4(a.X.W);
                result.X = (x * bX + y * bY) + (z * bZ + w * b.W);
            }

            {
                var x = new Vector4(a.Y.X);
                var y = new Vector4(a.Y.Y);
                var z = new Vector4(a.Y.Z);
                var w = new Vector4(a.Y.W);
                result.Y = (x * bX + y * bY) + (z * bZ + w * b.W);
            }

            {
                var x = new Vector4(a.Z.X);
                var y = new Vector4(a.Z.Y);
                var z = new Vector4(a.Z.Z);
                var w = new Vector4(a.Z.W);
                result.Z = (x * bX + y * bY) + (z * bZ + w * b.W);
            }

            {
                var x = new Vector4(a.W.X);
                var y = new Vector4(a.W.Y);
                var z = new Vector4(a.W.Z);
                var w = new Vector4(a.W.W);
                result.W = (x * bX + y * bY) + (z * bZ + w * b.W);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromAxisAngle(Vector3 axis, Number angle, out Matrix result)
        {
            //TODO: Could be better simdified.
            Number xx = axis.X * axis.X;
            Number yy = axis.Y * axis.Y;
            Number zz = axis.Z * axis.Z;
            Number xy = axis.X * axis.Y;
            Number xz = axis.X * axis.Z;
            Number yz = axis.Y * axis.Z;

            Number sinAngle = (Number)Math.Sin(angle);
            Number oneMinusCosAngle = 1 - (Number)Math.Cos(angle);

            result.X = new Vector4(
                1 + oneMinusCosAngle * (xx - 1),
                axis.Z * sinAngle + oneMinusCosAngle * xy,
                -axis.Y * sinAngle + oneMinusCosAngle * xz,
                0);

            result.Y = new Vector4(
                -axis.Z * sinAngle + oneMinusCosAngle * xy,
                1 + oneMinusCosAngle * (yy - 1),
                axis.X * sinAngle + oneMinusCosAngle * yz,
                0);

            result.Z = new Vector4(
                axis.Y * sinAngle + oneMinusCosAngle * xz,
                -axis.X * sinAngle + oneMinusCosAngle * yz,
                1 + oneMinusCosAngle * (zz - 1),
                0);

            result.W = new Vector4(0, 0, 0, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix CreateFromAxisAngle(Vector3 axis, Number angle)
        {
            CreateFromAxisAngle(axis, angle, out Matrix result);
            return result;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromQuaternion(Quaternion quaternion, out Matrix result)
        {
            Number qX2 = quaternion.X + quaternion.X;
            Number qY2 = quaternion.Y + quaternion.Y;
            Number qZ2 = quaternion.Z + quaternion.Z;
            Number XX = qX2 * quaternion.X;
            Number YY = qY2 * quaternion.Y;
            Number ZZ = qZ2 * quaternion.Z;
            Number XY = qX2 * quaternion.Y;
            Number XZ = qX2 * quaternion.Z;
            Number XW = qX2 * quaternion.W;
            Number YZ = qY2 * quaternion.Z;
            Number YW = qY2 * quaternion.W;
            Number ZW = qZ2 * quaternion.W;

            result.X = new Vector4(
                1 - YY - ZZ,
                XY + ZW,
                XZ - YW,
                0);

            result.Y = new Vector4(
                XY - ZW,
                1 - XX - ZZ,
                YZ + XW,
                0);

            result.Z = new Vector4(
                XZ + YW,
                YZ - XW,
                1 - XX - YY,
                0);

            result.W = new Vector4(
                0,
                0,
                0,
                1);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix CreateFromQuaternion(Quaternion quaternion)
        {
            CreateFromQuaternion(quaternion, out Matrix toReturn);
            return toReturn;
        }

        /// <summary>
        /// Concatenates two matrices.
        /// </summary>
        /// <param name="m1">First input matrix.</param>
        /// <param name="m2">Second input matrix.</param>
        /// <returns>Concatenated transformation of the form m1 * m2.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix operator *(in Matrix m1, in Matrix m2)
        {
            Multiply(m1, m2, out Matrix toReturn);
            return toReturn;
        }

        /// <summary>
        /// Creates a right-handed perspective matrix.
        /// </summary>
        /// <param name="fieldOfView">Vertical field of view of the perspective in radians.</param>
        /// <param name="aspectRatio">Width of the viewport over the height of the viewport.</param>
        /// <param name="nearClip">Near clip plane of the perspective.</param>
        /// <param name="farClip">Far clip plane of the perspective.</param>
        /// <param name="perspective">Resulting perspective matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreatePerspectiveFieldOfView(Number fieldOfView, Number aspectRatio, Number nearClip, Number farClip, out Matrix perspective)
        {
            Number h = Constants.C1 / ((Number)Math.Tan(fieldOfView * Constants.C0p5));
            Number w = h / aspectRatio;
            Number m33 = farClip / (nearClip - farClip);
            perspective.X = new Vector4(w, 0, 0, 0);
            perspective.Y = new Vector4(0, h, 0, 0);
            perspective.Z = new Vector4(0, 0, m33, -1);
            perspective.W = new Vector4(0, 0, nearClip * m33, 0);

        }

        /// <summary>
        /// Creates a left-handed perspective matrix.
        /// </summary>
        /// <param name="fieldOfView">Vertical field of view of the perspective in radians.</param>
        /// <param name="aspectRatio">Width of the viewport over the height of the viewport.</param>
        /// <param name="nearClip">Near clip plane of the perspective.</param>
        /// <param name="farClip">Far clip plane of the perspective.</param>
        /// <param name="perspective">Resulting perspective matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreatePerspectiveFieldOfViewLH(Number fieldOfView, Number aspectRatio, Number nearClip, Number farClip, out Matrix perspective)
        {
            Number h = Constants.C1 / ((Number)Math.Tan(fieldOfView * Constants.C0p5));
            Number w = h / aspectRatio;
            Number m33 = farClip / (farClip - nearClip);
            perspective.X = new Vector4(w, 0, 0, 0);
            perspective.Y = new Vector4(0, h, 0, 0);
            perspective.Z = new Vector4(0, 0, m33, 1);
            perspective.W = new Vector4(0, 0, -nearClip * m33, 0);

        }

        /// <summary>
        /// Creates a right-handed perspective matrix.
        /// </summary>
        /// <param name="verticalFieldOfView">Vertical field of view of the perspective in radians.</param>
        /// <param name="horizontalFieldOfView">Horizontal field of view of the perspective in radians.</param>
        /// <param name="nearClip">Near clip plane of the perspective.</param>
        /// <param name="farClip">Far clip plane of the perspective.</param>
        /// <returns>Resulting perspective matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix CreatePerspectiveFromFieldOfViews(Number verticalFieldOfView, Number horizontalFieldOfView, Number nearClip, Number farClip)
        {
            Matrix perspective;
            CreatePerspectiveFromFieldOfViews(verticalFieldOfView, horizontalFieldOfView, nearClip, farClip, out perspective);
            return perspective;
        }


        /// <summary>
        /// Creates a right-handed perspective matrix.
        /// </summary>
        /// <param name="verticalFieldOfView">Vertical field of view of the perspective in radians.</param>
        /// <param name="horizontalFieldOfView">Horizontal field of view of the perspective in radians.</param>
        /// <param name="nearClip">Near clip plane of the perspective.</param>
        /// <param name="farClip">Far clip plane of the perspective.</param>
        /// <param name="perspective">Resulting perspective matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreatePerspectiveFromFieldOfViews(Number verticalFieldOfView, Number horizontalFieldOfView, Number nearClip, Number farClip, out Matrix perspective)
        {
            Number h = Constants.C1 / ((Number)Math.Tan(verticalFieldOfView * Constants.C0p5));
            Number w = Constants.C1 / ((Number)Math.Tan(horizontalFieldOfView * Constants.C0p5));
            Number m33 = farClip / (nearClip - farClip);
            perspective.X = new Vector4(w, 0, 0, 0);
            perspective.Y = new Vector4(0, h, 0, 0);
            perspective.Z = new Vector4(0, 0, m33, -1);
            perspective.W = new Vector4(0, 0, nearClip * m33, 0);
        }


        /// <summary>
        /// Creates a right-handed perspective matrix.
        /// </summary>
        /// <param name="fieldOfView">Vertical field of view of the perspective in radians.</param>
        /// <param name="aspectRatio">Width of the viewport over the height of the viewport.</param>
        /// <param name="nearClip">Near clip plane of the perspective.</param>
        /// <param name="farClip">Far clip plane of the perspective.</param>
        /// <returns>Resulting perspective matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix CreatePerspectiveFieldOfView(Number fieldOfView, Number aspectRatio, Number nearClip, Number farClip)
        {
            Matrix toReturn;
            CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClip, farClip, out toReturn);
            return toReturn;
        }

        /// <summary>
        /// Creates a right handed orthographic projection.
        /// </summary>
        /// <param name="left">Leftmost coordinate of the projected area.</param>
        /// <param name="right">Rightmost coordinate of the projected area.</param>
        /// <param name="bottom">Bottom coordinate of the projected area.</param>
        /// <param name="top">Top coordinate of the projected area.</param>
        /// <param name="zNear">Near plane of the projection.</param>
        /// <param name="zFar">Far plane of the projection.</param>
        /// <param name="projection">The resulting orthographic projection matrix.</param>
        public static void CreateOrthographic(Number left, Number right, Number bottom, Number top, Number zNear, Number zFar, out Matrix projection)
        {
            Number width = right - left;
            Number height = top - bottom;
            Number depth = zFar - zNear;
            projection.X = new Vector4(Constants.C2 / width, 0, 0, 0);
            projection.Y = new Vector4(0, Constants.C2 / height, 0, 0);
            projection.Z = new Vector4(0, 0, Constants.Cm1 / depth, 0);
            projection.W = new Vector4((left + right) / -width, (top + bottom) / -height, zNear / -depth, Constants.C1);

        }


        /// <summary>
        /// Inverts the matrix.
        /// </summary>
        /// <param name="m">Matrix to invert.</param>
        /// <param name="inverted">Inverted version of the matrix.</param>
        public static void Invert(in Matrix m, out Matrix inverted)
        {
            //TODO: This could be quite a bit faster, especially once shuffles exist... But inverting a 4x4 matrix should approximately never occur.
            Number s0 = m.X.X * m.Y.Y - m.Y.X * m.X.Y;
            Number s1 = m.X.X * m.Y.Z - m.Y.X * m.X.Z;
            Number s2 = m.X.X * m.Y.W - m.Y.X * m.X.W;
            Number s3 = m.X.Y * m.Y.Z - m.Y.Y * m.X.Z;
            Number s4 = m.X.Y * m.Y.W - m.Y.Y * m.X.W;
            Number s5 = m.X.Z * m.Y.W - m.Y.Z * m.X.W;

            Number c5 = m.Z.Z * m.W.W - m.W.Z * m.Z.W;
            Number c4 = m.Z.Y * m.W.W - m.W.Y * m.Z.W;
            Number c3 = m.Z.Y * m.W.Z - m.W.Y * m.Z.Z;
            Number c2 = m.Z.X * m.W.W - m.W.X * m.Z.W;
            Number c1 = m.Z.X * m.W.Z - m.W.X * m.Z.Z;
            Number c0 = m.Z.X * m.W.Y - m.W.X * m.Z.Y;

            Number inverseDeterminant = Constants.C1 / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

            Number m11 = m.X.X;
            Number m12 = m.X.Y;
            Number m13 = m.X.Z;
            Number m14 = m.X.W;
            Number m21 = m.Y.X;
            Number m22 = m.Y.Y;
            Number m23 = m.Y.Z;
            Number m31 = m.Z.X;
            Number m32 = m.Z.Y;
            Number m33 = m.Z.Z;

            Number m41 = m.W.X;
            Number m42 = m.W.Y;

            inverted.X = inverseDeterminant * new Vector4(
                m.Y.Y * c5 - m.Y.Z * c4 + m.Y.W * c3,
                -m.X.Y * c5 + m.X.Z * c4 - m.X.W * c3,
                m.W.Y * s5 - m.W.Z * s4 + m.W.W * s3,
                -m.Z.Y * s5 + m.Z.Z * s4 - m.Z.W * s3);

            inverted.Y = inverseDeterminant * new Vector4(
                -m.Y.X * c5 + m.Y.Z * c2 - m.Y.W * c1,
                m11 * c5 - m13 * c2 + m14 * c1,
                -m.W.X * s5 + m.W.Z * s2 - m.W.W * s1,
                m.Z.X * s5 - m.Z.Z * s2 + m.Z.W * s1);

            inverted.Z = inverseDeterminant * new Vector4(
                m21 * c4 - m22 * c2 + m.Y.W * c0,
                -m11 * c4 + m12 * c2 - m14 * c0,
                m.W.X * s4 - m.W.Y * s2 + m.W.W * s0,
                -m31 * s4 + m32 * s2 - m.Z.W * s0);

            inverted.W = inverseDeterminant * new Vector4(
                -m21 * c3 + m22 * c1 - m23 * c0,
                m11 * c3 - m12 * c1 + m13 * c0,
                -m41 * s3 + m42 * s1 - m.W.Z * s0,
                m31 * s3 - m32 * s1 + m33 * s0);
        }


        /// <summary>
        /// Inverts the matrix.
        /// </summary>
        /// <param name="m">Matrix to invert.</param>
        /// <returns>Inverted version of the matrix.</returns>
        public static Matrix Invert(in Matrix m)
        {
            Invert(m, out Matrix inverted);
            return inverted;
        }

        /// <summary>
        /// Creates a view matrix pointing from a position to a target with the given up vector.
        /// </summary>
        /// <param name="position">Position of the camera.</param>
        /// <param name="target">Target of the camera.</param>
        /// <param name="upVector">Up vector of the camera.</param>
        /// <param name="viewMatrix">Look at matrix.</param>
        public static void CreateLookAt(Vector3 position, Vector3 target, Vector3 upVector, out Matrix viewMatrix)
        {
            Vector3 forward = target - position;
            CreateView(position, forward, upVector, out viewMatrix);
        }

        /// <summary>
        /// Creates a view matrix pointing from a position to a target with the given up vector.
        /// </summary>
        /// <param name="position">Position of the camera.</param>
        /// <param name="target">Target of the camera.</param>
        /// <param name="upVector">Up vector of the camera.</param>
        /// <returns>Look at matrix.</returns>
        public static Matrix CreateLookAt(Vector3 position, Vector3 target, Vector3 upVector)
        {
            CreateView(position, target - position, upVector, out Matrix lookAt);
            return lookAt;
        }


        /// <summary>
        /// Creates a view matrix pointing in a direction with a given up vector.
        /// </summary>
        /// <param name="position">Position of the camera.</param>
        /// <param name="forward">Forward direction of the camera.</param>
        /// <param name="upVector">Up vector of the camera.</param>
        /// <param name="viewMatrix">Look at matrix.</param>
        public static void CreateView(Vector3 position, Vector3 forward, Vector3 upVector, out Matrix viewMatrix)
        {
            Number length = forward.Length();
            var z = forward / -length;
            var x = Vector3.Cross(upVector, z);
            x = Vector3.Normalize(x);
            var y = Vector3.Cross(z, x);

            viewMatrix.X = new Vector4(x.X, y.X, z.X, 0);
            viewMatrix.Y = new Vector4(x.Y, y.Y, z.Y, 0);
            viewMatrix.Z = new Vector4(x.Z, y.Z, z.Z, 0);
            viewMatrix.W = new Vector4(
                -Vector3.Dot(x, position),
                -Vector3.Dot(y, position),
                -Vector3.Dot(z, position), 1);

        }

        /// <summary>
        /// Creates a view matrix pointing looking in a direction with a given up vector.
        /// </summary>
        /// <param name="position">Position of the camera.</param>
        /// <param name="forward">Forward direction of the camera.</param>
        /// <param name="upVector">Up vector of the camera.</param>
        /// <returns>Look at matrix.</returns>
        public static Matrix CreateView(Vector3 position, Vector3 forward, Vector3 upVector)
        {
            Matrix lookat;
            CreateView(position, forward, upVector, out lookat);
            return lookat;
        }



        /// <summary>
        /// Creates a rigid world matrix from a rotation and position.
        /// </summary>
        /// <param name="rotation">Rotation of the transform.</param>
        /// <param name="position">Position of the transform.</param>
        /// <param name="world">4x4 matrix representing the combined transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateRigid(in Matrix3x3 rotation, Vector3 position, out Matrix world)
        {
            world.X = new Vector4(rotation.X, 0);
            world.Y = new Vector4(rotation.Y, 0);
            world.Z = new Vector4(rotation.Z, 0);
            world.W = new Vector4(position, 1);
        }

        /// <summary>
        /// Creates a rigid world matrix from a rotation and position.
        /// </summary>
        /// <param name="rotation">Rotation of the transform.</param>
        /// <param name="position">Position of the transform.</param>
        /// <param name="world">4x4 matrix representing the combined transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateRigid(Quaternion rotation, Vector3 position, out Matrix world)
        {
            Matrix3x3.CreateFromQuaternion(rotation, out var rotationMatrix);
            world.X = new Vector4(rotationMatrix.X, 0);
            world.Y = new Vector4(rotationMatrix.Y, 0);
            world.Z = new Vector4(rotationMatrix.Z, 0);
            world.W = new Vector4(position, 1);
        }

        /// <summary>
        /// Creates a 4x4 matrix from a 3x3 matrix. All extra columns and rows filled with 0 except the W.W, which is set to 1.
        /// </summary>
        /// <param name="matrix3x3">Smaller matrix to base the larger matrix on.</param>
        /// <param name="matrix4x4">Larger matrix that has the smaller matrix as a submatrix.</param>
        public static void CreateFrom3x3(in Matrix3x3 matrix3x3, out Matrix matrix4x4)
        {
            matrix4x4.X = new Vector4(matrix3x3.X, 0);
            matrix4x4.Y = new Vector4(matrix3x3.Y, 0);
            matrix4x4.Z = new Vector4(matrix3x3.Z, 0);
            matrix4x4.W = new Vector4(0, 0, 0, 1);
        }
        /// <summary>
        /// Creates a 4x4 matrix from a 3x3 matrix. All extra columns and rows filled with 0 except the W.W, which is set to 1.
        /// </summary>
        /// <param name="matrix3x3">Smaller matrix to base the larger matrix on.</param>
        /// <returns>Larger matrix that has the smaller matrix as a submatrix.</returns>
        public static Matrix CreateFrom3x3(in Matrix3x3 matrix3x3)
        {
            CreateFrom3x3(matrix3x3, out var matrix4x4);
            return matrix4x4;
        }
    }
}
