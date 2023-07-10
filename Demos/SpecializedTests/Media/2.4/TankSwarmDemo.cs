﻿

using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Collections;
using BepuUtilities.Numerics;
using DemoContentLoader;
using DemoRenderer;
using DemoRenderer.UI;
using Demos.Demos.Tanks;
using DemoUtilities;
using OpenTK.Input;
using System;
using Math = BepuUtilities.Utils.Math;
using MathF = BepuUtilities.Utils.MathF;

namespace Demos.SpecializedTests.Media
{
    public class TankSwarmDemo : Demo
    {
        CollidableProperty<TankDemoBodyProperties> bodyProperties;
        TankController playerController;

        QuickList<AITank> aiTanks;
        Random random;
        Vector2 playAreaMin, playAreaMax;

        //We want to create a little graphical explosion at projectile impact points. Since it's not an instant thing, we'll have to track it over a period of time.
        struct Explosion
        {
            public Vector3 Position;
            public Number Scale;
            public Vector3 Color;
            public int Age;
        }
        QuickList<Explosion> explosions;

        static MouseButton Fire = MouseButton.Left;
        static Key Forward = Key.W;
        static Key Backward = Key.S;
        static Key Right = Key.D;
        static Key Left = Key.A;
        static Key Zoom = Key.LShift;
        static Key Brake = Key.Space;
        static Key BrakeAlternate = Key.BackSpace; //I have a weird keyboard.
        static Key ToggleTank = Key.C;
        public override void Initialize(ContentArchive content, Camera camera)
        {
            camera.Position = new Vector3(0, 5, 10);
            camera.Yaw = 0;
            camera.Pitch = 0;

            bodyProperties = new CollidableProperty<TankDemoBodyProperties>();
            //Note that this demo uses only 1 substep and 6 velocity iterations.
            //That's partly to show that you can do such a thing, and partly because of (as of 2.4's initial release), there are situations where
            //contact data can become a little out of date during substepping, since the contact data is only updated once per frame rather than substep (apart from the depths, which are incrementally updated every substep).
            //In this demo, when using substepping, a wheel resting on another wheel from a destroyed tank can keep rocking back and forth for a long time as the error in contact offsets over substeps can introduce energy.
            //(I'd like to address this issue more directly to make substepping an unconditional win.)
            Simulation = Simulation.Create(BufferPool, new TankCallbacks() { Properties = bodyProperties }, new DemoPoseIntegratorCallbacks(new Vector3(0, -10, 0)), new SolveDescription(6, 1));

            var builder = new CompoundBuilder(BufferPool, Simulation.Shapes, 2);
            builder.Add(new Box(1.85f, 0.7f, 4.73f), RigidPose.Identity, 10);
            builder.Add(new Box(1.85f, 0.6f, 2.5f), new Vector3(0, 0.65f, -0.35f), 0.5f);
            builder.BuildDynamicCompound(out var children, out var bodyInertia, out _);
            builder.Dispose();
            var bodyShape = new Compound(children);
            var bodyShapeIndex = Simulation.Shapes.Add(bodyShape);
            var wheelShape = new Cylinder(0.4f, .18f);
            var wheelInertia = wheelShape.ComputeInertia(0.25f);
            var wheelShapeIndex = Simulation.Shapes.Add(wheelShape);

            var projectileShape = new Sphere(0.1f);
            var projectileInertia = projectileShape.ComputeInertia(0.2f);
            var tankDescription = new TankDescription
            {
                Body = TankPartDescription.Create(10, new Box(4f, 1, 5), RigidPose.Identity, 0.5f, Simulation.Shapes),
                Turret = TankPartDescription.Create(1, new Box(1.5f, 0.7f, 2f), new Vector3(0, 0.85f, 0.4f), 0.5f, Simulation.Shapes),
                Barrel = TankPartDescription.Create(0.5f, new Box(0.2f, 0.2f, Constants.C3), new Vector3(0, 0.85f, 0.4f - 1f - 1.5f), 0.5f, Simulation.Shapes),
                TurretAnchor = new Vector3(Constants.C0, 0.5f, 0.4f),
                BarrelAnchor = new Vector3(0, 0.5f + 0.35f, 0.4f - 1f),
                TurretBasis = Quaternion.Identity,
                TurretServo = new ServoSettings(1f, Constants.C0, 40f),
                TurretSpring = new SpringSettings(10f, 1f),
                BarrelServo = new ServoSettings(1f, Constants.C0, 40f),
                BarrelSpring = new SpringSettings(10f, 1f),

                ProjectileShape = Simulation.Shapes.Add(projectileShape),
                ProjectileSpeed = 100f,
                BarrelLocalProjectileSpawn = new Vector3(0, 0, -1.5f),
                ProjectileInertia = projectileInertia,

                LeftTreadOffset = new Vector3(-1.9f, Constants.C0, 0),
                RightTreadOffset = new Vector3(1.9f, Constants.C0, 0),
                SuspensionLength = 1f,
                SuspensionSettings = new SpringSettings(2.5f, 1.5f),
                WheelShape = wheelShapeIndex,
                WheelInertia = wheelInertia,
                WheelFriction = 2f,
                TreadSpacing = 1f,
                WheelCountPerTread = 5,
                WheelOrientation = QuaternionEx.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI * -0.5f),
            };

            playerController = new TankController(Tank.Create(Simulation, bodyProperties, BufferPool, (new Vector3(0, 10, 0), Quaternion.Identity), tankDescription), 20, 5, 2, 1, 3.5f);


            const int planeWidth = 257;
            Number terrainScale = 3;
            Number inverseTerrainScale = 1f / terrainScale;
            var terrainPosition = new Vector2(1 - planeWidth, 1 - planeWidth) * terrainScale * 0.5f;
            random = new Random(5);

            //Add some building-ish landmarks.
            var landmarkMin = new Vector3(planeWidth * terrainScale * -0.45f, 0, planeWidth * terrainScale * -0.45f);
            var landmarkMax = new Vector3(planeWidth * terrainScale * 0.45f, 0, planeWidth * terrainScale * 0.45f);
            var landmarkSpan = landmarkMax - landmarkMin;
            for (int j = 0; j < 25; ++j)
            {
                var buildingShape = new Box(10 + random.NextSingle() * 10, 20 + random.NextSingle() * 20, 10 + random.NextSingle() * 10);
                var position = landmarkMin + landmarkSpan * new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle());
                Simulation.Statics.Add(new StaticDescription(
                    new Vector3(0, buildingShape.HalfHeight - 4f + GetHeightForPosition(position.X, position.Z, planeWidth, inverseTerrainScale, terrainPosition), 0) + position,
                    QuaternionEx.CreateFromAxisAngle(Vector3.UnitY, random.NextSingle() * MathF.PI),
                    Simulation.Shapes.Add(buildingShape)));
            }

            var planeMesh = DemoMeshHelper.CreateDeformedPlane(planeWidth, planeWidth,
                (int vX, int vY) =>
                    {
                        var position2D = new Vector2(vX, vY) * terrainScale + terrainPosition;
                        return new Vector3(position2D.X, GetHeightForPosition(position2D.X, position2D.Y, planeWidth, inverseTerrainScale, terrainPosition), position2D.Y);
                    }, new Vector3(1, 1, 1), BufferPool);
            Simulation.Statics.Add(new StaticDescription(new Vector3(0, 0, 0), Simulation.Shapes.Add(planeMesh)));

            explosions = new QuickList<Explosion>(32, BufferPool);

            //Create the AI tanks.
            const int aiTankCount = 3072;
            aiTanks = new QuickList<AITank>(aiTankCount, BufferPool);
            playAreaMin = new Vector2(landmarkMin.X, landmarkMin.Z);
            playAreaMax = new Vector2(landmarkMax.X, landmarkMax.Z);
            var playAreaSpan = playAreaMax - playAreaMin;
            for (int i = 0; i < aiTankCount; ++i)
            {
                var horizontalPosition = playAreaMin + new Vector2(random.NextSingle(), random.NextSingle()) * playAreaSpan;
                aiTanks.AllocateUnsafely() = new AITank
                {
                    Controller = new TankController(
                        Tank.Create(Simulation, bodyProperties, BufferPool,
                            (new Vector3(horizontalPosition.X, 10, horizontalPosition.Y), QuaternionEx.CreateFromAxisAngle(new Vector3(0, 1, 0), random.NextSingle() * 0.1f)),
                            tankDescription), 20, 5, 2, 1, 3.5f),
                    HitPoints = 5
                };
            }
            Console.WriteLine($"body count: {Simulation.Bodies.ActiveSet.Count}");
        }

        Number GetHeightForPosition(Number x, Number y, int planeWidth, Number inverseTerrainScale, in Vector2 terrainPosition)
        {
            var normalizedX = (x - terrainPosition.X) * inverseTerrainScale;
            var normalizedY = (y - terrainPosition.Y) * inverseTerrainScale;
            var octave0 = (MathF.Sin((normalizedX + 5f) * 0.05f) + MathF.Sin((normalizedY + 11) * 0.05f)) * 3.8f;
            var octave1 = (MathF.Sin((normalizedX + 17) * 0.15f) + MathF.Sin((normalizedY + 47) * 0.15f)) * 1.5f;
            var octave2 = (MathF.Sin((normalizedX + 37) * 0.35f) + MathF.Sin((normalizedY + 93) * 0.35f)) * 0.5f;
            var octave3 = (MathF.Sin((normalizedX + 53) * 0.65f) + MathF.Sin((normalizedY + 131) * 0.65f)) * 0.3f;
            var octave4 = (MathF.Sin((normalizedX + 67) * 1.50f) + MathF.Sin((normalizedY + 13) * 1.5f)) * 0.1525f;
            var distanceToEdge = planeWidth / 2 - Math.Max(Math.Abs(normalizedX - planeWidth / 2), Math.Abs(normalizedY - planeWidth / 2));
            //Flatten an area in the middle.
            var offsetX = planeWidth * 0.5f - normalizedX;
            var offsetY = planeWidth * 0.5f - normalizedY;
            var distanceToCenterSquared = offsetX * offsetX + offsetY * offsetY;
            Number centerCircleSize = 30f;
            Number fadeoutBoundary = 50f;
            var outsideWeight = MathF.Min(1f, MathF.Max(0, distanceToCenterSquared - centerCircleSize * centerCircleSize) / (fadeoutBoundary * fadeoutBoundary - centerCircleSize * centerCircleSize));
            var edgeRamp = 25f / (5 * distanceToEdge + 1);
            return outsideWeight * (octave0 + octave1 + octave2 + octave3 + octave4 + edgeRamp);
        }

        bool playerControlActive = true;
        long frameIndex;
        long lastPlayerShotFrameIndex;
        int projectileCount;
        public override void Update(Window window, Camera camera, Input input, Number dt)
        {
            if (input.WasPushed(ToggleTank))
                playerControlActive = !playerControlActive;
            if (playerControlActive)
            {
                Number leftTargetSpeedFraction = 0;
                Number rightTargetSpeedFraction = 0;
                var left = input.IsDown(Left);
                var right = input.IsDown(Right);
                var forward = input.IsDown(Forward);
                var backward = input.IsDown(Backward);
                if (forward)
                {
                    if ((left && right) || (!left && !right))
                    {
                        leftTargetSpeedFraction = 1f;
                        rightTargetSpeedFraction = 1f;
                    }
                    //Note turns require a bit of help from the opposing track to overcome friction.
                    else if (left)
                    {
                        leftTargetSpeedFraction = 0.5f;
                        rightTargetSpeedFraction = 1f;
                    }
                    else if (right)
                    {
                        leftTargetSpeedFraction = 1f;
                        rightTargetSpeedFraction = 0.5f;
                    }
                }
                else if (backward)
                {
                    if ((left && right) || (!left && !right))
                    {
                        leftTargetSpeedFraction = -1f;
                        rightTargetSpeedFraction = -1f;
                    }
                    else if (left)
                    {
                        leftTargetSpeedFraction = -0.5f;
                        rightTargetSpeedFraction = -1f;
                    }
                    else if (right)
                    {
                        leftTargetSpeedFraction = -1f;
                        rightTargetSpeedFraction = -0.5f;
                    }
                }
                else
                {
                    //Not trying to move. Turn?
                    if (left && !right)
                    {
                        leftTargetSpeedFraction = -1f;
                        rightTargetSpeedFraction = 1f;
                    }
                    else if (right && !left)
                    {
                        leftTargetSpeedFraction = 1f;
                        rightTargetSpeedFraction = -1f;
                    }
                }

                var zoom = input.IsDown(Zoom);
                var brake = input.IsDown(Brake) || input.IsDown(BrakeAlternate);
                playerController.UpdateMovementAndAim(Simulation, leftTargetSpeedFraction, rightTargetSpeedFraction, zoom, brake, brake, camera.Forward);

                if (input.WasPushed(Fire) && frameIndex > lastPlayerShotFrameIndex + 60)
                {
                    playerController.Tank.Fire(Simulation, bodyProperties);
                    lastPlayerShotFrameIndex = frameIndex;
                    ++projectileCount;
                }
            }

            for (int i = 0; i < aiTanks.Count; ++i)
            {
                aiTanks[i].Update(Simulation, bodyProperties, random, frameIndex, playAreaMin, playAreaMax, i, ref aiTanks, ref projectileCount);
            }


            frameIndex++;
            //Ensure that the callbacks list of exploding projectiles can contain all projectiles that exist.
            //(We cast the narrowphase to the generic subtype so that we can grab the callbacks. This isn't the only way-
            //notice that we cached the bodyProperties reference outside of the callbacks for direct access.
            //The exploding projectiles list, however, is a QuickList<int> value type. If we tried to cache it outside we'd only have a copy of it.
            //So, rather than trying to set up some pinned memory or replacing it with a reference type, we just cast our way in.)
            ref var projectileImpacts = ref ((NarrowPhase<TankCallbacks>)Simulation.NarrowPhase).Callbacks.ProjectileImpacts;
            projectileImpacts.EnsureCapacity(projectileCount, BufferPool);
            base.Update(window, camera, input, dt);
            //Remove any projectile that hit something.
            for (int i = 0; i < projectileImpacts.Count; ++i)
            {
                ref var impact = ref projectileImpacts[i];
                ref var explosion = ref explosions.Allocate(BufferPool);
                explosion.Age = 0;
                explosion.Position = Simulation.Bodies[impact.ProjectileHandle].Pose.Position;
                explosion.Scale = 1f;
                explosion.Color = new Vector3(1f, 0.5f, 0);
                Simulation.Bodies.Remove(impact.ProjectileHandle);
                if (impact.ImpactedTankBodyHandle.Value >= 0)
                {
                    //The projectile hit a tank. Hurt it!
                    for (int aiIndex = 0; aiIndex < aiTanks.Count; ++aiIndex)
                    {
                        ref var aiTank = ref aiTanks[aiIndex];
                        if (aiTank.Controller.Tank.Body.Value == impact.ImpactedTankBodyHandle.Value)
                        {
                            --aiTank.HitPoints;
                            if (aiTank.HitPoints == 0)
                            {
                                ref var deathExplosion = ref explosions.Allocate(BufferPool);
                                deathExplosion.Position = Simulation.Bodies[aiTank.Controller.Tank.Turret].Pose.Position;
                                deathExplosion.Scale = 3;
                                deathExplosion.Age = 0;
                                deathExplosion.Color = new Vector3(1, 0, 0);
                                aiTank.Controller.Tank.Explode(Simulation, bodyProperties, BufferPool);
                                aiTanks.FastRemoveAt(aiIndex);
                            }
                            break;
                        }
                    }
                    //This loop might actually fail to find the tank- if a tank gets hit by more than one projectile in a frame, or if the player tank is hit.
                    //(The player tank cheats and isn't in the aiTanks list.)
                    //That's fine, though.
                }
            }
            projectileImpacts.Count = 0;
        }


        void RenderControl(ref Vector2 position, Number textHeight, string controlName, string controlValue, TextBuilder text, TextBatcher textBatcher, Font font)
        {
            text.Clear().Append(controlName).Append(": ").Append(controlValue);
            textBatcher.Write(text, position, textHeight, new Vector3(1), font);
            position.Y += textHeight * 1.1f;
        }

        public override void Render(Renderer renderer, Camera camera, Input input, TextBuilder text, Font font)
        {
            if (playerControlActive)
            {
                var tankBody = new BodyReference(playerController.Tank.Body, Simulation.Bodies);
                QuaternionEx.TransformUnitY(tankBody.Pose.Orientation, out var tankUp);
                QuaternionEx.TransformUnitZ(tankBody.Pose.Orientation, out var tankBackward);
                var backwardDirection = camera.Backward;
                backwardDirection.Y = MathF.Max(backwardDirection.Y, -0.2f);
                camera.Position = tankBody.Pose.Position + tankUp * Constants.C3 + tankBackward * 0.4f + backwardDirection * 8;
            }

            //Draw explosions and remove old ones.
            for (int i = explosions.Count - 1; i >= 0; --i)
            {
                ref var explosion = ref explosions[i];
                var pose = new RigidPose(explosion.Position);
                //The age is measured in frames, so it's not framerate independent. That's fine for a demo.
                renderer.Shapes.AddShape(new Sphere(explosion.Scale * (0.25f + MathF.Sqrt(explosion.Age))), Simulation.Shapes, pose, explosion.Color);
                if (explosion.Age > 5)
                {
                    explosions.FastRemoveAt(i);
                }
                ++explosion.Age;
            }

            var textHeight = 16;
            var position = new Vector2(32, renderer.Surface.Resolution.Y - 144);
            RenderControl(ref position, textHeight, nameof(Fire), ControlStrings.GetName(Fire), text, renderer.TextBatcher, font);
            RenderControl(ref position, textHeight, nameof(Forward), ControlStrings.GetName(Forward), text, renderer.TextBatcher, font);
            RenderControl(ref position, textHeight, nameof(Backward), ControlStrings.GetName(Backward), text, renderer.TextBatcher, font);
            RenderControl(ref position, textHeight, nameof(Right), ControlStrings.GetName(Right), text, renderer.TextBatcher, font);
            RenderControl(ref position, textHeight, nameof(Left), ControlStrings.GetName(Left), text, renderer.TextBatcher, font);
            RenderControl(ref position, textHeight, nameof(Zoom), ControlStrings.GetName(Zoom), text, renderer.TextBatcher, font);
            RenderControl(ref position, textHeight, nameof(Brake), ControlStrings.GetName(Brake), text, renderer.TextBatcher, font);
            RenderControl(ref position, textHeight, nameof(ToggleTank), ControlStrings.GetName(ToggleTank), text, renderer.TextBatcher, font);

            if (aiTanks.Count > 0)
                renderer.TextBatcher.Write(text.Clear().Append("Enemy tanks remaining: ").Append(aiTanks.Count), new Vector2(32, renderer.Surface.Resolution.Y - 172), 24, new Vector3(1, 1, 1), font);
            else
                renderer.TextBatcher.Write(text.Clear().Append("ya did it!"), new Vector2(32, renderer.Surface.Resolution.Y - 172), 24, new Vector3(0.3f, 1, 0.3f), font);

            base.Render(renderer, camera, input, text, font);
        }
    }
}