using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;

namespace Demos.Demos.Cars
{
    struct CarBodyProperties
    {
        public SubgroupCollisionFilter Filter;
        public Number Friction;
    }

    /// <summary>
    /// For the car demo, we want both wheel-body collision filtering and different friction for wheels versus the car body.
    /// </summary>
    struct CarCallbacks : INarrowPhaseCallbacks
    {
        public CollidableProperty<CarBodyProperties> Properties;
        public void Initialize(Simulation simulation)
        {
            Properties.Initialize(simulation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref Number speculativeMargin)
        {
            //It's impossible for two statics to collide, and pairs are sorted such that bodies always come before statics.
            if (b.Mobility != CollidableMobility.Static)
            {
                return SubgroupCollisionFilter.AllowCollision(Properties[a.BodyHandle].Filter, Properties[b.BodyHandle].Filter);
            }
            return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
        {
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
        {
            pairMaterial.FrictionCoefficient = Properties[pair.A.BodyHandle].Friction;
            if (pair.B.Mobility != CollidableMobility.Static)
            {
                //If two bodies collide, just average the friction.
                pairMaterial.FrictionCoefficient = (pairMaterial.FrictionCoefficient + Properties[pair.B.BodyHandle].Friction) * Constants.C0p5;
            }
            pairMaterial.MaximumRecoveryVelocity = Constants.C0p3;
            pairMaterial.SpringSettings = new SpringSettings(30, 1);
            return true;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
        {
            return true;
        }

        public void Dispose()
        {
            Properties.Dispose();
        }
    }
}