using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;
using BepuUtilities.Memory;
using ConsoleRenderer;

namespace term3d;

public class Render
{
  Simulation Sim = Simulation.Create(new BufferPool(), new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new Vector3(0, -10, 0)), new SolveDescription(8, 1));
  ConsoleCanvas Canvas = new ConsoleCanvas();

  BodyHandle bh;
  StaticHandle sh;

  RayHitHandler hitHandler;
  public void Init()
  {
    hitHandler = default(RayHitHandler);
    hitHandler.T = float.MaxValue;
    InitSimulation();
  }

  // NOTE: I'm basically sifting through RayCastingDemo.cs from Bepu Demos to cobble something together
  private void InitSimulation()
  {
    //Drop a ball on a big static box.
    var unitCube = new Box(1,1,1);
    var cubeInertia = unitCube.ComputeInertia(1);
    bh = Sim.Bodies.Add(BodyDescription.CreateDynamic(new Vector3(0, 1, 0), cubeInertia, Sim.Shapes.Add(unitCube), 0.01f));

    // sh = Sim.Statics.Add(new StaticDescription(new Vector3(5, 5, 0), Sim.Shapes.Add(new Box(5, 1, 5))));
  }

  // Most of this is copied from BepuPhysics' source code
  public void CameraRaycast((int x, int y, int z) loc)
  {
    var origin = new Vector3(loc.x, loc.y, loc.z);

    Canvas.Clear();
    int hits = 0;
    int maxhits = Canvas.Width * Canvas.Height;
    for (int row = 1; row < Canvas.Width - 1; row++)
    {
      for (int col = 1; col < Canvas.Height - 1; col++)
      {
        hitHandler.T = float.MaxValue; // TODO: make a better reset
        var pixelDir = new Vector3(row - (Canvas.Width / 2f), col - (Canvas.Height / 2f), 0f);
        Sim.RayCast(origin, Vector3.Normalize(pixelDir), 100, ref hitHandler); // NOTE: this is the only line AI actually helped me with
        
        Canvas.Text(0, 0, $"XYZ: {loc.x} {loc.y} {loc.z}");
        Canvas.Text(0, 1, $"Origin: {origin}  |  directionRay: {pixelDir}");
        Canvas.Text(0, 2, $"{hits} / {maxhits} rays hit object located at {Sim.Bodies[hitHandler.HitCollidable.BodyHandle].BoundingBox} (last distance {hitHandler.T})");
        if (hitHandler.T < float.MaxValue)
        {
          hits++;
          Canvas.Set(row, col, '#');
        }
      }
    }
    Canvas.Render();
    Task.Delay(100); // TODO: move this - this is not where framerate/timing code belongs.
  }
}

struct RayHitHandler : IRayHitHandler
{
  public float T;
  public CollidableReference HitCollidable;
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool AllowTest(CollidableReference collidable)
  {
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool AllowTest(CollidableReference collidable, int childIndex)
  {
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void OnRayHit(in RayData ray, ref float maximumT, float t, Vector3 normal, CollidableReference collidable, int childIndex)
  {
    //We are only interested in the earliest hit. This callback is executing within the traversal, so modifying maximumT informs the traversal
    //that it can skip any AABBs which are more distant than the new maximumT.
    maximumT = t;
    //Cache the earliest impact.
    T = t;
    HitCollidable = collidable;
  }

  public void OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex)
  {
    //We are only interested in the earliest hit. This callback is executing within the traversal, so modifying maximumT informs the traversal
    //that it can skip any AABBs which are more distant than the new maximumT.
    maximumT = t;
    //Cache the earliest impact.
    T = t;
    HitCollidable = collidable;
  }
}

