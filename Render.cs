using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuPhysics.Trees;
using BepuUtilities;
using BepuUtilities.Memory;
using ConsoleRenderer;
using term3d.Objects.simulation;

namespace term3d;

public class Render
{
  Simulation Sim;
  ConsoleCanvas Canvas;

  StaticHandle sh;

  public void Init()
  {
    Canvas = new ConsoleCanvas();
    InitSimulation();
  }

  // NOTE: I'm basically sifting through RayCastingDemo.cs from Bepu Demos to cobble something together
  private void InitSimulation()
  {
    Sim = Simulation.Create(new BufferPool(), new DemoNarrowPhaseCallbacks(new SpringSettings(30, 1)), new DemoPoseIntegratorCallbacks(new Vector3(0, -10, 0)), new SolveDescription(8, 1));

    // idk what half of this does half as well as I should
    var random = new Random(5);
    const int width = 16;
    const int height = 16;
    const int length = 16;
    var spacing = new Vector3(2.01f);
    var halfSpacing = spacing / 2;
    float randomizationSubset = 0.9f;
    var randomizationSpan = (spacing - new Vector3(1)) * randomizationSubset;
    var randomizationBase = randomizationSpan * -0.5f;

    var sphere = new Sphere(0.5f);
    var sphereIndex = Sim.Shapes.Add(sphere);

    // I guess I need to study up on my physics and higher maths <.< 
    Quaternion orientation;
    orientation.X = -1 + 2 * random.NextSingle();
    orientation.Y = -1 + 2 * random.NextSingle();
    orientation.Z = -1 + 2 * random.NextSingle();
    orientation.W = 0.01f + random.NextSingle();
    QuaternionEx.Normalize(ref orientation);

    sh = Sim.Statics.Add(new StaticDescription(new Vector3(500,500,0), orientation, sphereIndex));
  }

  public void Test_Checkerboard()
  {
    for (int row = 1; row < Canvas.Width - 1; row++)
    {
      for (int col = 1; col < Canvas.Height - 1; col++)
      {
        var bg = ConsoleColor.Black;
        var fg = ConsoleColor.White;
        if (row % 2 == 0)
        {
          bg = ConsoleColor.White;
          fg = ConsoleColor.Black;
        }
        Canvas.Set(row, col, '#', fg, bg);
      }
    }
    Canvas.Render();
  }

  // Most of this is copied from BepuPhysics' source code
  public void CameraRaycast()
  {
    var hitHandler = default(RayHitHandler);
    hitHandler.T = float.MaxValue;
    var origin = new Vector3(0, 0, 0);

    Console.WriteLine($"statichandle: object at {Sim.Statics[sh].BoundingBox}");

    for (int row = 1; row < Canvas.Width - 1; row++)
    {
      for (int col = 1; col < Canvas.Height - 1; col++)
      {
        var directionRay = new Vector3(row - (Canvas.Width / 2), col - (Canvas.Height / 2), origin.Z);
        Sim.RayCast(origin, directionRay, float.MaxValue, ref hitHandler);
        // Console.WriteLine($"ray cast from {origin} towards {directionRay}");
        if (hitHandler.T < float.MaxValue)
        {
          Console.WriteLine($"Ray for cell {row}|{col} hit something (distance {hitHandler.T})");
          // Canvas.Set(row, col, '#');
        }
      }
    }
    // Canvas.Render();
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

