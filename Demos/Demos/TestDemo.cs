using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Numerics;
using DemoContentLoader;
using DemoRenderer;

namespace Demos.Demos
{

    /// <summary>
    /// Shows how to use custom velocity integration to implement planetary gravity.
    /// </summary>
    public class TestDemo : Demo
    {
        public unsafe override void Initialize(ContentArchive content, Camera camera)
        {
            camera.Position = new Vector3(0, 8, -20);
            camera.Yaw = MathHelper.Pi;

            Simulation = Simulation.Create(BufferPool, new DemoNarrowPhaseCallbacks(
                new SpringSettings(30, 1)), 
                new DemoPoseIntegratorCallbacks(new Vector3(0 , -9.81f, 0)),
                new SolveDescription(4, 1));

            //Console.WriteLine(Vector<int>.Count);
            Simulation.Statics.Add(new StaticDescription(new Vector3(0, -0.5f, 0), Simulation.Shapes.Add(new Box(30, 1, 30))));
            Simulation.Bodies.Add(BodyDescription.CreateConvexDynamic(new Vector3(0, 5, 0), 1f, Simulation.Shapes, new Capsule(1, 1)));
            Simulation.Bodies.Add(BodyDescription.CreateConvexDynamic(new Vector3(3, 5, 0), 1f, Simulation.Shapes, new Capsule(1, 1)));
        }

        //public override void Update(Window window, Camera camera, Input input, Number dt)
        //{
        //    base.Update(window, camera, input, dt);
        //}

        //public override void Render(Renderer renderer, Camera camera, Input input, TextBuilder text, Font font)
        //{
        //    var bottomY = renderer.Surface.Resolution.Y;
        //    renderer.TextBatcher.Write(text.Clear().Append("The library does not prescribe any particular kind of gravity."), new Vector2(16, bottomY - 48), 16, Vector3.One, font);
        //    renderer.TextBatcher.Write(text.Clear().Append("The IPoseIntegratorCallbacks provided to the simulation is responsible for telling the simulation how to integrate."), new Vector2(16, bottomY - 32), 16, Vector3.One, font);
        //    renderer.TextBatcher.Write(text.Clear().Append("In this demo, all bodies are pulled towards the center of the planet."), new Vector2(16, bottomY - 16), 16, Vector3.One, font);
        //    base.Render(renderer, camera, input, text, font);
        //}
    }
}
