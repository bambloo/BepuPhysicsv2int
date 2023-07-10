using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;
using Math = BepuUtilities.Utils.Math;

namespace BepuUtilities
{
    /// <summary>
    /// Provides additional functionality and some lower overhead function variants for Quaternions.
    /// </summary>
    public static class QuaternionEx
    {
        /// <summary>
        /// Adds two quaternions together.
        /// </summary>
        /// <param name="a">First quaternion to add.</param>
        /// <param name="b">Second quaternion to add.</param>
        /// <param name="result">Sum of the addition.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(Quaternion a, Quaternion b, out Quaternion result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
            result.W = a.W + b.W;
        }

        /// <summary>
        /// Scales a quaternion.
        /// </summary>
        /// <param name="q">Quaternion to multiply.</param>
        /// <param name="scale">Amount to multiply each component of the quaternion by.</param>
        /// <param name="result">Scaled quaternion.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Scale(Quaternion q, Number scale, out Quaternion result)
        {
            result.X = q.X * scale;
            result.Y = q.Y * scale;
            result.Z = q.Z * scale;
            result.W = q.W * scale;
        }

        /// <summary>
        /// Concatenates the transforms of two quaternions together such that the resulting quaternion, applied as an orientation to a vector v, is equivalent to
        /// transformed = (v * a) * b.
        /// Assumes that neither input parameter overlaps the output parameter.
        /// </summary>
        /// <param name="a">First quaternion to concatenate.</param>
        /// <param name="b">Second quaternion to concatenate.</param>
        /// <param name="result">Product of the concatenation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ConcatenateWithoutOverlap(Quaternion a, Quaternion b, out Quaternion result)
        {
            result.X = a.W * b.X + a.X * b.W + a.Z * b.Y - a.Y * b.Z;
            result.Y = a.W * b.Y + a.Y * b.W + a.X * b.Z - a.Z * b.X;
            result.Z = a.W * b.Z + a.Z * b.W + a.Y * b.X - a.X * b.Y;
            result.W = a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z;
        }

        /// <summary>
        /// Concatenates the transforms of two quaternions together such that the resulting quaternion, applied as an orientation to a vector v, is equivalent to
        /// transformed = (v * a) * b.
        /// </summary>
        /// <param name="a">First quaternion to concatenate.</param>
        /// <param name="b">Second quaternion to concatenate.</param>
        /// <param name="result">Product of the concatenation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Concatenate(Quaternion a, Quaternion b, out Quaternion result)
        {
            ConcatenateWithoutOverlap(a, b, out var temp);
            result = temp;
        }


        /// <summary>
        /// Concatenates the transforms of two quaternions together such that the resulting quaternion, applied as an orientation to a vector v, is equivalent to
        /// transformed = (v * a) * b.
        /// </summary>
        /// <param name="a">First quaternion to multiply.</param>
        /// <param name="b">Second quaternion to multiply.</param>
        /// <returns>Product of the multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Concatenate(Quaternion a, Quaternion b)
        {
            ConcatenateWithoutOverlap(a, b, out var result);
            return result;
        }

        /// <summary>
        /// Quaternion representing the identity transform.
        /// </summary>
        public static Quaternion Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Quaternion(0, 0, 0, 1);
            }
        }


        /// <summary>
        /// Constructs a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix to create the quaternion from.</param>
        /// <param name="q">Quaternion based on the rotation matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromRotationMatrix(in Matrix3x3 r, out Quaternion q)
        {
            Number t;
            if (r.Z.Z < 0)
            {
                if (r.X.X > r.Y.Y)
                {
                    t = 1 + r.X.X - r.Y.Y - r.Z.Z;
                    q.X = t;
                    q.Y = r.X.Y + r.Y.X;
                    q.Z = r.Z.X + r.X.Z;
                    q.W = r.Y.Z - r.Z.Y;
                }
                else
                {
                    t = 1 - r.X.X + r.Y.Y - r.Z.Z;
                    q.X = r.X.Y + r.Y.X;
                    q.Y = t;
                    q.Z = r.Y.Z + r.Z.Y;
                    q.W = r.Z.X - r.X.Z;
                }
            }
            else
            {
                if (r.X.X < -r.Y.Y)
                {
                    t = 1 - r.X.X - r.Y.Y + r.Z.Z;
                    q.X = r.Z.X + r.X.Z;
                    q.Y = r.Y.Z + r.Z.Y;
                    q.Z = t;
                    q.W = r.X.Y - r.Y.X;
                }
                else
                {
                    t = 1 + r.X.X + r.Y.Y + r.Z.Z;
                    q.X = r.Y.Z - r.Z.Y;
                    q.Y = r.Z.X - r.X.Z;
                    q.Z = r.X.Y - r.Y.X;
                    q.W = t;
                }
            }
            Scale(q, Constants.C0p5 / (Number)Math.Sqrt(t), out q);
        }

        /// <summary>
        /// Creates a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix used to create a new quaternion.</param>
        /// <returns>Quaternion representing the same rotation as the matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion CreateFromRotationMatrix(in Matrix3x3 r)
        {
            CreateFromRotationMatrix(r, out var toReturn);
            return toReturn;
        }


        /// <summary>
        /// Constructs a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix to create the quaternion from.</param>
        /// <param name="q">Quaternion based on the rotation matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromRotationMatrix(in Matrix r, out Quaternion q)
        {
            Matrix3x3.CreateFromMatrix(r, out var rotation3x3);
            CreateFromRotationMatrix(rotation3x3, out q);
        }

        /// <summary>
        /// Constructs a quaternion from a rotation matrix.
        /// </summary>
        /// <param name="r">Rotation matrix to create the quaternion from.</param>
        /// <returns>Quaternion based on the rotation matrix.</returns>
        public static Quaternion CreateFromRotationMatrix(in Matrix r)
        {
            Matrix3x3.CreateFromMatrix(r, out var rotation3x3);
            CreateFromRotationMatrix(rotation3x3, out var q);
            return q;
        }

        /// <summary>
        /// Ensures the quaternion has unit length.
        /// </summary>
        /// <param name="quaternion">Quaternion to normalize.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref Quaternion quaternion)
        {
            ref var q = ref Unsafe.As<Quaternion, Vector4>(ref quaternion);
            q = q / (Number)Math.Sqrt(Vector4.Dot(q, q)); //not great; MathF when available or perhaps alternatives?
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Normalize(Quaternion quaternion)
        {
            Normalize(ref quaternion);
            return quaternion;
        }

        /// <summary>
        /// Computes the squared length of the quaternion.
        /// </summary>
        /// <returns>Squared length of the quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number LengthSquared(ref Quaternion quaternion)
        {
            return Unsafe.As<Quaternion, Vector4>(ref quaternion).LengthSquared();
        }

        /// <summary>
        /// Computes the length of the quaternion.
        /// </summary>
        /// <returns>Length of the quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number Length(ref Quaternion quaternion)
        {
            return Unsafe.As<Quaternion, Vector4>(ref quaternion).Length();
        }

        /// <summary>
        /// Blends two quaternions together to get an intermediate state.
        /// </summary>
        /// <param name="start">Starting point of the interpolation.</param>
        /// <param name="end">Ending point of the interpolation.</param>
        /// <param name="interpolationAmount">Amount of the end point to use.</param>
        /// <param name="result">Interpolated intermediate quaternion.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Slerp(Quaternion start, Quaternion end, Number interpolationAmount, out Quaternion result)
        {
            Number cosHalfTheta = start.W * end.W + start.X * end.X + start.Y * end.Y + start.Z * end.Z;
            if (cosHalfTheta < 0)
            {
                //Negating a quaternion results in the same orientation, 
                //but we need cosHalfTheta to be positive to get the shortest path.
                end.X = -end.X;
                end.Y = -end.Y;
                end.Z = -end.Z;
                end.W = -end.W;
                cosHalfTheta = -cosHalfTheta;
            }
            // If the orientations are similar enough, then just pick one of the inputs.
            if (cosHalfTheta > (Constants.C1 - Constants.C1em12))
            {
                result.W = start.W;
                result.X = start.X;
                result.Y = start.Y;
                result.Z = start.Z;
                return;
            }
            // Calculate temporary values.
            Number halfTheta = Math.Acos(cosHalfTheta);
            Number sinHalfTheta = Math.Sqrt(Constants.C1 - cosHalfTheta * cosHalfTheta);

            Number aFraction = Math.Sin((1 - interpolationAmount) * halfTheta) / sinHalfTheta;
            Number bFraction = Math.Sin(interpolationAmount * halfTheta) / sinHalfTheta;

            //Blend the two quaternions to get the result!
            result.X = (Number)(start.X * aFraction + end.X * bFraction);
            result.Y = (Number)(start.Y * aFraction + end.Y * bFraction);
            result.Z = (Number)(start.Z * aFraction + end.Z * bFraction);
            result.W = (Number)(start.W * aFraction + end.W * bFraction);
        }

        /// <summary>
        /// Blends two quaternions together to get an intermediate state.
        /// </summary>
        /// <param name="start">Starting point of the interpolation.</param>
        /// <param name="end">Ending point of the interpolation.</param>
        /// <param name="interpolationAmount">Amount of the end point to use.</param>
        /// <returns>Interpolated intermediate quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Slerp(Quaternion start, Quaternion end, Number interpolationAmount)
        {
            Slerp(start, end, interpolationAmount, out Quaternion toReturn);
            return toReturn;
        }


        /// <summary>
        /// Computes the conjugate of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to conjugate.</param>
        /// <param name="result">Conjugated quaternion.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Conjugate(Quaternion quaternion, out Quaternion result)
        {
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = quaternion.W;
        }

        /// <summary>
        /// Computes the conjugate of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to conjugate.</param>
        /// <returns>Conjugated quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Conjugate(Quaternion quaternion)
        {
            Conjugate(quaternion, out Quaternion toReturn);
            return toReturn;
        }



        /// <summary>
        /// Computes the inverse of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to invert.</param>
        /// <param name="result">Result of the inversion.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Inverse(Quaternion quaternion, out Quaternion result)
        {
            Number inverseSquaredNorm = quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W;
            result.X = -quaternion.X * inverseSquaredNorm;
            result.Y = -quaternion.Y * inverseSquaredNorm;
            result.Z = -quaternion.Z * inverseSquaredNorm;
            result.W = quaternion.W * inverseSquaredNorm;
        }

        /// <summary>
        /// Computes the inverse of the quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to invert.</param>
        /// <returns>Result of the inversion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Inverse(Quaternion quaternion)
        {
            Inverse(quaternion, out var result);
            return result;

        }

        /// <summary>
        /// Negates the components of a quaternion.
        /// </summary>
        /// <param name="a">Quaternion to negate.</param>
        /// <param name="b">Negated result.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(Quaternion a, out Quaternion b)
        {
            b.X = -a.X;
            b.Y = -a.Y;
            b.Z = -a.Z;
            b.W = -a.W;
        }

        /// <summary>
        /// Negates the components of a quaternion.
        /// </summary>
        /// <param name="q">Quaternion to negate.</param>
        /// <returns>Negated result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Negate(Quaternion q)
        {
            Negate(q, out var result);
            return result;
        }

        /// <summary>
        /// Transforms the vector using a quaternion, assuming that the output does not alias with the input.
        /// </summary>
        /// <param name="v">Vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformWithoutOverlap(Vector3 v, Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            Number x2 = rotation.X + rotation.X;
            Number y2 = rotation.Y + rotation.Y;
            Number z2 = rotation.Z + rotation.Z;
            Number xx2 = rotation.X * x2;
            Number xy2 = rotation.X * y2;
            Number xz2 = rotation.X * z2;
            Number yy2 = rotation.Y * y2;
            Number yz2 = rotation.Y * z2;
            Number zz2 = rotation.Z * z2;
            Number wx2 = rotation.W * x2;
            Number wy2 = rotation.W * y2;
            Number wz2 = rotation.W * z2;
            //Defer the component setting since they're used in computation.
            result.X = v.X * (Constants.C1 - yy2 - zz2) + v.Y * (xy2 - wz2) + v.Z * (xz2 + wy2);
            result.Y = v.X * (xy2 + wz2) + v.Y * (Constants.C1 - xx2 - zz2) + v.Z * (yz2 - wx2);
            result.Z = v.X * (xz2 - wy2) + v.Y * (yz2 + wx2) + v.Z * (Constants.C1 - xx2 - yy2);
        }


        /// <summary>
        /// Transforms the vector using a quaternion.
        /// </summary>
        /// <param name="v">Vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Vector3 v, Quaternion rotation, out Vector3 result)
        {
            TransformWithoutOverlap(v, rotation, out var temp);
            result = temp;
        }

        /// <summary>
        /// Transforms the vector using a quaternion.
        /// </summary>
        /// <param name="v">Vector to transform.</param>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <returns>Transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Transform(Vector3 v, Quaternion rotation)
        {
            TransformWithoutOverlap(v, rotation, out var toReturn);
            return toReturn;
        }

        /// <summary>
        /// Transforms the unit X direction using a quaternion.
        /// </summary>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformUnitX(Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            Number y2 = rotation.Y + rotation.Y;
            Number z2 = rotation.Z + rotation.Z;
            Number xy2 = rotation.X * y2;
            Number xz2 = rotation.X * z2;
            Number yy2 = rotation.Y * y2;
            Number zz2 = rotation.Z * z2;
            Number wy2 = rotation.W * y2;
            Number wz2 = rotation.W * z2;
            result.X = Constants.C1 - yy2 - zz2;
            result.Y = xy2 + wz2;
            result.Z = xz2 - wy2;

        }

        /// <summary>
        /// Transforms the unit Y vector using a quaternion.
        /// </summary>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformUnitY(Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            Number x2 = rotation.X + rotation.X;
            Number y2 = rotation.Y + rotation.Y;
            Number z2 = rotation.Z + rotation.Z;
            Number xx2 = rotation.X * x2;
            Number xy2 = rotation.X * y2;
            Number yz2 = rotation.Y * z2;
            Number zz2 = rotation.Z * z2;
            Number wx2 = rotation.W * x2;
            Number wz2 = rotation.W * z2;
            result.X = xy2 - wz2;
            result.Y = Constants.C1 - xx2 - zz2;
            result.Z = yz2 + wx2;
        }

        /// <summary>
        /// Transforms the unit Z vector using a quaternion.
        /// </summary>
        /// <param name="rotation">Rotation to apply to the vector.</param>
        /// <param name="result">Transformed vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformUnitZ(Quaternion rotation, out Vector3 result)
        {
            //This operation is an optimized-down version of v' = q * v * q^-1.
            //The expanded form would be to treat v as an 'axis only' quaternion
            //and perform standard quaternion multiplication.  Assuming q is normalized,
            //q^-1 can be replaced by a conjugation.
            Number x2 = rotation.X + rotation.X;
            Number y2 = rotation.Y + rotation.Y;
            Number z2 = rotation.Z + rotation.Z;
            Number xx2 = rotation.X * x2;
            Number xz2 = rotation.X * z2;
            Number yy2 = rotation.Y * y2;
            Number yz2 = rotation.Y * z2;
            Number wx2 = rotation.W * x2;
            Number wy2 = rotation.W * y2;
            result.X = xz2 + wy2;
            result.Y = yz2 - wx2;
            result.Z = Constants.C1 - xx2 - yy2;
        }

        /// <summary>
        /// Creates a quaternion from an axis and angle.
        /// </summary>
        /// <param name="axis">Axis of rotation.</param>
        /// <param name="angle">Angle to rotate around the axis.</param>
        /// <returns>Quaternion representing the axis and angle rotation.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion CreateFromAxisAngle(Vector3 axis, Number angle)
        {
            Number halfAngle = angle * Constants.C0p5;
            Number s = Math.Sin(halfAngle);
            Quaternion q;
            q.X = (Number)(axis.X * s);
            q.Y = (Number)(axis.Y * s);
            q.Z = (Number)(axis.Z * s);
            q.W = (Number)Math.Cos(halfAngle);
            return q;
        }

        /// <summary>
        /// Creates a quaternion from an axis and angle.
        /// </summary>
        /// <param name="axis">Axis of rotation.</param>
        /// <param name="angle">Angle to rotate around the axis.</param>
        /// <param name="q">Quaternion representing the axis and angle rotation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromAxisAngle(Vector3 axis, Number angle, out Quaternion q)
        {
            Number halfAngle = angle * Constants.C0p5;
            Number s = Math.Sin(halfAngle);
            q.X = (Number)(axis.X * s);
            q.Y = (Number)(axis.Y * s);
            q.Z = (Number)(axis.Z * s);
            q.W = (Number)Math.Cos(halfAngle);
        }

        /// <summary>
        /// Constructs a quaternion from yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Yaw of the rotation.</param>
        /// <param name="pitch">Pitch of the rotation.</param>
        /// <param name="roll">Roll of the rotation.</param>
        /// <returns>Quaternion representing the yaw, pitch, and roll.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion CreateFromYawPitchRoll(Number yaw, Number pitch, Number roll)
        {
            CreateFromYawPitchRoll(yaw, pitch, roll, out var toReturn);
            return toReturn;
        }

        /// <summary>
        /// Constructs a quaternion from yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Yaw of the rotation.</param>
        /// <param name="pitch">Pitch of the rotation.</param>
        /// <param name="roll">Roll of the rotation.</param>
        /// <param name="q">Quaternion representing the yaw, pitch, and roll.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromYawPitchRoll(Number yaw, Number pitch, Number roll, out Quaternion q)
        {
            Number halfRoll = roll * Constants.C0p5;
            Number halfPitch = pitch * Constants.C0p5;
            Number halfYaw = yaw * Constants.C0p5;

            Number sinRoll = Math.Sin(halfRoll);
            Number sinPitch = Math.Sin(halfPitch);
            Number sinYaw = Math.Sin(halfYaw);

            Number cosRoll = Math.Cos(halfRoll);
            Number cosPitch = Math.Cos(halfPitch);
            Number cosYaw = Math.Cos(halfYaw);

            Number cosYawCosPitch = cosYaw * cosPitch;
            Number cosYawSinPitch = cosYaw * sinPitch;
            Number sinYawCosPitch = sinYaw * cosPitch;
            Number sinYawSinPitch = sinYaw * sinPitch;

            q.X = (Number)(cosYawSinPitch * cosRoll + sinYawCosPitch * sinRoll);
            q.Y = (Number)(sinYawCosPitch * cosRoll - cosYawSinPitch * sinRoll);
            q.Z = (Number)(cosYawCosPitch * sinRoll - sinYawSinPitch * cosRoll);
            q.W = (Number)(cosYawCosPitch * cosRoll + sinYawSinPitch * sinRoll);

        }

        /// <summary>
        /// Computes the angle change represented by a normalized quaternion.
        /// </summary>
        /// <param name="q">Quaternion to be converted.</param>
        /// <returns>Angle around the axis represented by the quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number GetAngleFromQuaternion(Quaternion q)
        {
            Number qw = Math.Abs(q.W);
            if (qw > 1)
                return 0;
            return 2 * (Number)Math.Acos(qw);
        }

        /// <summary>
        /// Computes the axis angle representation of a normalized quaternion.
        /// </summary>
        /// <param name="q">Quaternion to be converted.</param>
        /// <param name="axis">Axis represented by the quaternion.</param>
        /// <param name="angle">Angle around the axis represented by the quaternion.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetAxisAngleFromQuaternion(Quaternion q, out Vector3 axis, out Number angle)
        {
            Number qw = q.W;
            if (qw > 0)
            {
                axis.X = q.X;
                axis.Y = q.Y;
                axis.Z = q.Z;
            }
            else
            {
                axis.X = -q.X;
                axis.Y = -q.Y;
                axis.Z = -q.Z;
                qw = -qw;
            }

            Number lengthSquared = axis.LengthSquared();
            if (lengthSquared > Constants.C1em14)
            {
                axis /= (Number)Math.Sqrt(lengthSquared);
                angle = 2 * (Number)Math.Acos(MathHelper.Clamp(qw, -1, 1));
            }
            else
            {
                axis = Vector3.UnitY;
                angle = 0;
            }
        }

        /// <summary>
        /// Computes the quaternion rotation between two normalized vectors.
        /// </summary>
        /// <param name="v1">First unit-length vector.</param>
        /// <param name="v2">Second unit-length vector.</param>
        /// <param name="q">Quaternion representing the rotation from v1 to v2.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetQuaternionBetweenNormalizedVectors(Vector3 v1, Vector3 v2, out Quaternion q)
        {
            Number dot = Vector3.Dot(v1, v2);
            //For non-normal vectors, the multiplying the axes length squared would be necessary:
            //Number w = dot + (Number)Math.Sqrt(v1.LengthSquared() * v2.LengthSquared());
            if (dot < -Constants.C0p9999) //parallel, opposing direction
            {
                //If this occurs, the rotation required is ~180 degrees.
                //The problem is that we could choose any perpendicular axis for the rotation. It's not uniquely defined.
                //The solution is to pick an arbitrary perpendicular axis.
                //Project onto the plane which has the lowest component magnitude.
                //On that 2d plane, perform a 90 degree rotation.
                Number absX = Math.Abs(v1.X);
                Number absY = Math.Abs(v1.Y);
                Number absZ = Math.Abs(v1.Z);
                if (absX < absY && absX < absZ)
                    q = new Quaternion(0, -v1.Z, v1.Y, 0);
                else if (absY < absZ)
                    q = new Quaternion(-v1.Z, 0, v1.X, 0);
                else
                    q = new Quaternion(-v1.Y, v1.X, 0, 0);
            }
            else
            {
                var axis = Vector3.Cross(v1, v2);
                q = new Quaternion(axis.X, axis.Y, axis.Z, dot + 1);
            }
            Normalize(ref q);
        }

        //The following two functions are highly similar, but it's a bit of a brain teaser to phrase one in terms of the other.
        //Providing both simplifies things.

        /// <summary>
        /// Computes the rotation from the start orientation to the end orientation such that end = Quaternion.Concatenate(start, relative).
        /// Assumes that neither input parameter overlaps with the output parameter.
        /// </summary>
        /// <param name="start">Starting orientation.</param>
        /// <param name="end">Ending orientation.</param>
        /// <param name="relative">Relative rotation from the start to the end orientation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRelativeRotationWithoutOverlap(Quaternion start, Quaternion end, out Quaternion relative)
        {
            Conjugate(start, out var startInverse);
            ConcatenateWithoutOverlap(startInverse, end, out relative);
        }


        /// <summary>
        /// Transforms the rotation into the local space of the target basis such that rotation = Quaternion.Concatenate(localRotation, targetBasis)
        /// Assumes that neither input parameter overlaps with the output parameter.
        /// </summary>
        /// <param name="rotation">Rotation in the original frame of reference.</param>
        /// <param name="targetBasis">Basis in the original frame of reference to transform the rotation into.</param>
        /// <param name="localRotation">Rotation in the local space of the target basis.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetLocalRotationWithoutOverlap(Quaternion rotation, Quaternion targetBasis, out Quaternion localRotation)
        {
            Conjugate(targetBasis, out var basisInverse);
            ConcatenateWithoutOverlap(rotation, basisInverse, out localRotation);
        }
    }
}
