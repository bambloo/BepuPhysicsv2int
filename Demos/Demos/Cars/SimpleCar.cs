﻿
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Numerics;

namespace Demos.Demos.Cars
{
    struct SimpleCar
    {
        public BodyHandle Body;
        public WheelHandles FrontLeftWheel;
        public WheelHandles FrontRightWheel;
        public WheelHandles BackLeftWheel;
        public WheelHandles BackRightWheel;

        private Vector3 suspensionDirection;
        private AngularHinge hingeDescription;

        public void Steer(Simulation simulation, in WheelHandles wheel, Number angle)
        {
            var steeredHinge = hingeDescription;
            Matrix3x3.CreateFromAxisAngle(suspensionDirection, -angle, out var rotation);
            Matrix3x3.Transform(hingeDescription.LocalHingeAxisA, rotation, out steeredHinge.LocalHingeAxisA);
            simulation.Solver.ApplyDescription(wheel.Hinge, steeredHinge);
        }

        public void SetSpeed(Simulation simulation, in WheelHandles wheel, Number speed, Number maximumForce)
        {
            simulation.Solver.ApplyDescription(wheel.Motor, new AngularAxisMotor
            {
                LocalAxisA = new Vector3(0, -1, 0),
                Settings = new MotorSettings(maximumForce, 1e-6f),
                TargetVelocity = speed
            });
        }

        public static WheelHandles CreateWheel(Simulation simulation, CollidableProperty<CarBodyProperties> properties, in RigidPose bodyPose,
            TypedIndex wheelShape, BodyInertia wheelInertia, Number wheelFriction, BodyHandle bodyHandle, ref SubgroupCollisionFilter bodyFilter, Vector3 bodyToWheelSuspension, Vector3 suspensionDirection, Number suspensionLength,
            in AngularHinge hingeDescription, in SpringSettings suspensionSettings, Quaternion localWheelOrientation)
        {
            RigidPose wheelPose;
            RigidPose.Transform(bodyToWheelSuspension + suspensionDirection * suspensionLength, bodyPose, out wheelPose.Position);
            QuaternionEx.ConcatenateWithoutOverlap(localWheelOrientation, bodyPose.Orientation, out wheelPose.Orientation);
            WheelHandles handles;
            handles.Wheel = simulation.Bodies.Add(BodyDescription.CreateDynamic(wheelPose, wheelInertia, new(wheelShape, Constants.C0p5), Constants.C0p01));

            handles.SuspensionSpring = simulation.Solver.Add(bodyHandle, handles.Wheel, new LinearAxisServo
            {
                LocalPlaneNormal = suspensionDirection,
                TargetOffset = suspensionLength,
                LocalOffsetA = bodyToWheelSuspension,
                LocalOffsetB = default,
                ServoSettings = ServoSettings.Default,
                SpringSettings = suspensionSettings
            });
            handles.SuspensionTrack = simulation.Solver.Add(bodyHandle, handles.Wheel, new PointOnLineServo
            {
                LocalDirection = suspensionDirection,
                LocalOffsetA = bodyToWheelSuspension,
                LocalOffsetB = default,
                ServoSettings = ServoSettings.Default,
                SpringSettings = new SpringSettings(30, 1)
            });
            //We're treating braking and acceleration as the same thing. It is, after all, a *simple* car! Maybe it's electric or something.
            //It would be fairly easy to split brakes and drive motors into different motors.
            handles.Motor = simulation.Solver.Add(handles.Wheel, bodyHandle, new AngularAxisMotor
            {
                LocalAxisA = new Vector3(0, 1, 0),
                Settings = default,
                TargetVelocity = default
            });
            handles.Hinge = simulation.Solver.Add(bodyHandle, handles.Wheel, hingeDescription);
            //The demos SubgroupCollisionFilter is pretty simple and only tests one direction, so we make the non-colliding relationship symmetric.
            ref var wheelProperties = ref properties.Allocate(handles.Wheel);
            wheelProperties = new CarBodyProperties { Filter = new SubgroupCollisionFilter(bodyHandle.Value, 1), Friction = wheelFriction };
            SubgroupCollisionFilter.DisableCollision(ref wheelProperties.Filter, ref bodyFilter);

            return handles;
        }

        public static SimpleCar Create(Simulation simulation, CollidableProperty<CarBodyProperties> properties, in RigidPose pose,
            TypedIndex bodyShape, BodyInertia bodyInertia, Number bodyFriction, TypedIndex wheelShape, BodyInertia wheelInertia, Number wheelFriction,
            Vector3 bodyToFrontLeftSuspension, Vector3 bodyToFrontRightSuspension, Vector3 bodyToBackLeftSuspension, Vector3 bodyToBackRightSuspension,
            Vector3 suspensionDirection, Number suspensionLength, in SpringSettings suspensionSettings, Quaternion localWheelOrientation)
        {
            SimpleCar car;
            car.Body = simulation.Bodies.Add(BodyDescription.CreateDynamic(pose, bodyInertia, new(bodyShape, Constants.C0p5), Constants.C0p01));
            ref var bodyProperties = ref properties.Allocate(car.Body);
            bodyProperties = new CarBodyProperties { Friction = bodyFriction, Filter = new SubgroupCollisionFilter(car.Body.Value, 0) };
            QuaternionEx.TransformUnitY(localWheelOrientation, out var wheelAxis);
            car.hingeDescription = new AngularHinge
            {
                LocalHingeAxisA = wheelAxis,
                LocalHingeAxisB = new Vector3(0, 1, 0),
                SpringSettings = new SpringSettings(30, 1)
            };
            car.suspensionDirection = suspensionDirection;
            car.BackLeftWheel = CreateWheel(simulation, properties, pose, wheelShape, wheelInertia, wheelFriction, car.Body, ref bodyProperties.Filter, bodyToBackLeftSuspension, suspensionDirection, suspensionLength, car.hingeDescription, suspensionSettings, localWheelOrientation);
            car.BackRightWheel = CreateWheel(simulation, properties, pose, wheelShape, wheelInertia, wheelFriction, car.Body, ref bodyProperties.Filter, bodyToBackRightSuspension, suspensionDirection, suspensionLength, car.hingeDescription, suspensionSettings, localWheelOrientation);
            car.FrontLeftWheel = CreateWheel(simulation, properties, pose, wheelShape, wheelInertia, wheelFriction, car.Body, ref bodyProperties.Filter, bodyToFrontLeftSuspension, suspensionDirection, suspensionLength, car.hingeDescription, suspensionSettings, localWheelOrientation);
            car.FrontRightWheel = CreateWheel(simulation, properties, pose, wheelShape, wheelInertia, wheelFriction, car.Body, ref bodyProperties.Filter, bodyToFrontRightSuspension, suspensionDirection, suspensionLength, car.hingeDescription, suspensionSettings, localWheelOrientation);
            return car;
        }

    }
}