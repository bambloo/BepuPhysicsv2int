﻿using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Numerics;
using DemoContentLoader;
using DemoRenderer;
using System;
using System.Diagnostics;

namespace Demos.SpecializedTests
{
    public class MeshSerializationTestDemo : Demo
    {
        public override void Initialize(ContentArchive content, Camera camera)
        {
            camera.Position = new Vector3(-30, 8, -60);
            camera.Yaw = MathHelper.Pi * Constants.C3 / 4;
            camera.Pitch = 0;

            Simulation = Simulation.Create(BufferPool, new DemoNarrowPhaseCallbacks(new SpringSettings(30, 1)), new DemoPoseIntegratorCallbacks(new Vector3(0, -10, 0)), new SolveDescription(8, 1));

            var startTime = Stopwatch.GetTimestamp();
            var originalMesh = DemoMeshHelper.CreateDeformedPlane(1025, 1025, (x, y) => new Vector3(x * 0.125f, MathF.Sin(x) + MathF.Sin(y), y * 0.125f), Vector3.One, BufferPool);
            Simulation.Statics.Add(new StaticDescription(new Vector3(0, 0, 0), Simulation.Shapes.Add(originalMesh)));
            var endTime = Stopwatch.GetTimestamp();
            var freshConstructionTime = (endTime - startTime) / (Number)Stopwatch.Frequency;
            Console.WriteLine($"Fresh construction time (ms): {freshConstructionTime * 1e3}");

            BufferPool.Take<byte>(originalMesh.GetSerializedByteCount(), out var serializedMeshBytes);
            originalMesh.Serialize(serializedMeshBytes);
            startTime = Stopwatch.GetTimestamp();
            var loadedMesh = new Mesh(serializedMeshBytes, BufferPool);
            endTime = Stopwatch.GetTimestamp();
            var loadTime = (endTime - startTime) / (Number)Stopwatch.Frequency;
            Console.WriteLine($"Load time (ms): {(endTime - startTime) * 1e3 / Stopwatch.Frequency}");
            Console.WriteLine($"Relative speedup: {freshConstructionTime / loadTime}");
            Simulation.Statics.Add(new StaticDescription(new Vector3(128, 0, 0), Simulation.Shapes.Add(loadedMesh)));


            BufferPool.Return(ref serializedMeshBytes);

            var random = new Random(5);
            var shapeToDrop = new Box(1, 1, 1);
            var descriptionToDrop = BodyDescription.CreateDynamic(new Vector3(), shapeToDrop.ComputeInertia(1), Simulation.Shapes.Add(shapeToDrop), Constants.C0p01);
            for (int i = 0; i < 1024; ++i)
            {
                descriptionToDrop.Pose.Position = new Vector3(8 + 240 * random.NextSingle(), 10 + 10 * random.NextSingle(), 8 + 112 * random.NextSingle());
                Simulation.Bodies.Add(descriptionToDrop);
            }

        }
    }
}
