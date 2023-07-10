﻿using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Numerics;
using BepuUtilities.Utils;
using DemoContentLoader;
using DemoRenderer;
using DemoUtilities;



namespace Demos.SpecializedTests.Media
{
    /// <summary>
    /// A colosseum made out of boxes that is sometimes hit by large purple hail.
    /// </summary>
    public class Colosseum24VideoDemo : Demo
    {
        public static void CreateRingWall(Simulation simulation, Vector3 position, Box ringBoxShape, BodyDescription bodyDescription, int height, Number radius)
        {
            var circumference = MathF.PI * 2 * radius;
            var boxCountPerRing = (int)(0.9f * circumference / ringBoxShape.Length);
            Number increment = MathHelper.TwoPi / boxCountPerRing;
            for (int ringIndex = 0; ringIndex < height; ringIndex++)
            {
                for (int i = 0; i < boxCountPerRing; i++)
                {
                    var angle = ((ringIndex & 1) == 0 ? i + 0.5f : i) * increment;
                    bodyDescription.Pose = (position + new Vector3(-MathF.Cos(angle) * radius, (ringIndex + 0.5f) * ringBoxShape.Height, MathF.Sin(angle) * radius), QuaternionEx.CreateFromAxisAngle(Vector3.UnitY, angle));
                    simulation.Bodies.Add(bodyDescription);
                }
            }
        }

        public static void CreateRingPlatform(Simulation simulation, Vector3 position, Box ringBoxShape, BodyDescription bodyDescription, Number radius)
        {
            var innerCircumference = MathF.PI * 2 * (radius - ringBoxShape.HalfLength);
            var boxCount = (int)(0.95f * innerCircumference / ringBoxShape.Height);
            Number increment = MathHelper.TwoPi / boxCount;
            for (int i = 0; i < boxCount; i++)
            {
                var angle = i * increment;
                bodyDescription.Pose = (position + new Vector3(-MathF.Cos(angle) * radius, ringBoxShape.HalfWidth, MathF.Sin(angle) * radius),
                    QuaternionEx.Concatenate(QuaternionEx.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI * 0.5f), QuaternionEx.CreateFromAxisAngle(Vector3.UnitY, angle + MathF.PI * 0.5f)));
                simulation.Bodies.Add(bodyDescription);
            }
        }

        public static Vector3 CreateRing(Simulation simulation, Vector3 position, Box ringBoxShape, BodyDescription bodyDescription, Number radius, int heightPerPlatformLevel, int platformLevels)
        {
            for (int platformIndex = 0; platformIndex < platformLevels; ++platformIndex)
            {
                var wallOffset = ringBoxShape.HalfLength - ringBoxShape.HalfWidth;
                CreateRingWall(simulation, position, ringBoxShape, bodyDescription, heightPerPlatformLevel, radius + wallOffset);
                CreateRingWall(simulation, position, ringBoxShape, bodyDescription, heightPerPlatformLevel, radius - wallOffset);
                CreateRingPlatform(simulation, position + new Vector3(0, heightPerPlatformLevel * ringBoxShape.Height, 0), ringBoxShape, bodyDescription, radius);
                position.Y += heightPerPlatformLevel * ringBoxShape.Height + ringBoxShape.Width;
            }
            return position;
        }

        public unsafe override void Initialize(ContentArchive content, Camera camera)
        {
            camera.Position = new Vector3(-30, 40, -30);
            camera.Yaw = MathHelper.Pi * Constants.C3 / 4;
            camera.Pitch = MathHelper.Pi * 0.2f;

            Simulation = Simulation.Create(BufferPool, new DemoNarrowPhaseCallbacks(new SpringSettings(90, 1), maximumRecoveryVelocity: 20), new DemoPoseIntegratorCallbacks(new Vector3(0, -10, 0)), new SolveDescription(2, 7));

            var ringBoxShape = new Box(0.5f, 1, 3);
            var boxDescription = BodyDescription.CreateDynamic(new Vector3(), ringBoxShape.ComputeInertia(1), new(Simulation.Shapes.Add(ringBoxShape), 0.1f), -Constants.C0p01);

            //CreateRingWall(Simulation, default, ringBoxShape, boxDescription, 400, 45);
            var layerPosition = new Vector3();
            const int layerCount = 1;
            var innerRadius = 20f;
            var heightPerPlatform = 10;
            var platformsPerLayer = 30;
            var ringSpacing = 0.5f;
            for (int layerIndex = 0; layerIndex < layerCount; ++layerIndex)
            {
                var ringCount = layerCount - layerIndex;
                for (int ringIndex = 0; ringIndex < ringCount; ++ringIndex)
                {
                    CreateRing(Simulation, layerPosition, ringBoxShape, boxDescription, innerRadius + ringIndex * (ringBoxShape.Length + ringSpacing) + layerIndex * (ringBoxShape.Length - ringBoxShape.Width), heightPerPlatform, platformsPerLayer);
                }
                layerPosition.Y += platformsPerLayer * (ringBoxShape.Height * heightPerPlatform + ringBoxShape.Width);
            }

            System.Console.WriteLine($"box count: {Simulation.Bodies.ActiveSet.Count}");
            Simulation.Statics.Add(new StaticDescription(new Vector3(0, -0.5f, 0), Simulation.Shapes.Add(new Box(500, 1, 500))));

            var bulletShape = new Sphere(0.5f);
            bulletDescription = BodyDescription.CreateDynamic(new Vector3(), bulletShape.ComputeInertia(.1f), Simulation.Shapes.Add(bulletShape), Constants.C0p01);

            var shootiePatootieShape = new Sphere(Constants.C3);
            shootiePatootieDescription = BodyDescription.CreateDynamic(new Vector3(), shootiePatootieShape.ComputeInertia(100), new(Simulation.Shapes.Add(shootiePatootieShape), 0.1f), Constants.C0p01);
        }

        BodyDescription bulletDescription;
        BodyDescription shootiePatootieDescription;
        public override void Update(Window window, Camera camera, Input input, Number dt)
        {
            if (input != null)
            {
                if (input.WasPushed(OpenTK.Input.Key.Z))
                {
                    bulletDescription.Pose.Position = camera.Position;
                    var pos = window.GetNormalizedMousePosition(input.MousePosition);
                    bulletDescription.Velocity.Linear = camera.GetRayDirection(input.MouseLocked, new Vector2(pos.X, pos.Y)) * 400;
                    Simulation.Bodies.Add(bulletDescription);
                }
                else if (input.WasPushed(OpenTK.Input.Key.X))
                {
                    shootiePatootieDescription.Pose.Position = camera.Position;
                    var pos = window.GetNormalizedMousePosition(input.MousePosition);
                    shootiePatootieDescription.Velocity.Linear = camera.GetRayDirection(input.MouseLocked, new Vector2(pos.X, pos.Y)) * 100;
                    Simulation.Bodies.Add(shootiePatootieDescription);
                }
            }
            base.Update(window, camera, input, dt);
        }

    }
}
