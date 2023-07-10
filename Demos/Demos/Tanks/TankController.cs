

using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Numerics;

namespace Demos.Demos.Tanks
{
    /// <summary>
    /// Applies control inputs to a tank instance.
    /// </summary>
    public struct TankController
    {
        public Tank Tank;

        //While the Tank instance contains references to all the simulation-contained stuff, none of it actually defines how fast or strong the tank is.
        //We store that here in the controller so it can be modified on the fly.
        public Number Speed;
        public Number Force;
        public Number ZoomMultiplier;
        public Number IdleForce;
        public Number BrakeForce;
        
        //Track the previous state to force wakeups if the constraint targets have changed.
        private Number previousLeftTargetSpeed;
        private Number previousLeftForce;
        private Number previousRightTargetSpeed;
        private Number previousRightForce;
        private Number previousTurretSwivel;
        private Number previousBarrelPitch;

        public TankController(Tank tank,
            Number speed, Number force, Number zoomMultiplier, Number idleForce, Number brakeForce) : this()
        {
            Tank = tank;
            Speed = speed;
            Force = force;
            ZoomMultiplier = zoomMultiplier;
            IdleForce = idleForce;
            BrakeForce = brakeForce;
        }

        /// <summary>
        /// Updates constraint targets for an input state.
        /// </summary>
        /// <param name="simulation">Simulation containing the tank.</param>
        /// <param name="leftTargetSpeedFraction">Target speed fraction of the maximum speed for the left tread.</param>
        /// <param name="rightTargetSpeedFraction">Target speed fraction of the maximum speed for the right tread.</param>
        /// <param name="zoom">Whether or not to use the boost mulitplier.</param>
        /// <param name="brakeLeft">Whether the left tread should brake.</param>
        /// <param name="brakeRight">Whether the right tread should brake.</param>
        /// <param name="aimDirection">Direction that the tank's barrel should point.</param>
        public void UpdateMovementAndAim(Simulation simulation, Number leftTargetSpeedFraction, Number rightTargetSpeedFraction, bool zoom, bool brakeLeft, bool brakeRight, Vector3 aimDirection)
        {
            var leftTargetSpeed = brakeLeft ? 0 : leftTargetSpeedFraction * Speed;
            var rightTargetSpeed = brakeRight ? 0 : rightTargetSpeedFraction * Speed;
            if (zoom)
            {
                leftTargetSpeed *= ZoomMultiplier;
                rightTargetSpeed *= ZoomMultiplier;
            }
            var leftForce = brakeLeft ? BrakeForce : leftTargetSpeedFraction == 0 ? IdleForce : Force;
            var rightForce = brakeRight ? BrakeForce : rightTargetSpeedFraction == 0 ? IdleForce : Force;

            var (targetSwivelAngle, targetPitchAngle) = Tank.ComputeTurretAngles(simulation, aimDirection);

            if (leftTargetSpeed != previousLeftTargetSpeed || rightTargetSpeed != previousRightTargetSpeed ||
                leftForce != previousLeftForce || rightForce != previousRightForce ||
                targetSwivelAngle != previousTurretSwivel || targetPitchAngle != previousBarrelPitch)
            {
                //By guarding the constraint modifications behind a state test, we avoid waking up the tank every single frame.
                //(We could have also used the ApplyDescriptionWithoutWaking function and then explicitly woke the tank up when changes occur.)
                Tank.SetSpeed(simulation, Tank.LeftMotors, leftTargetSpeed, leftForce);
                Tank.SetSpeed(simulation, Tank.RightMotors, rightTargetSpeed, rightForce);
                previousLeftTargetSpeed = leftTargetSpeed;
                previousRightTargetSpeed = rightTargetSpeed;
                previousLeftForce = leftForce;
                previousRightForce = rightForce;
                Tank.SetAim(simulation, targetSwivelAngle, targetPitchAngle);
                previousTurretSwivel = targetSwivelAngle;
                previousBarrelPitch = targetPitchAngle;
            }


        }
    }
}