using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;
using ConsoleRenderer;
using term3d.Objects.simulation;

namespace term3d;

public class Render
{
  Simulation Sim;
  ConsoleCanvas Canvas;

  public void Init()
  {
    Canvas = new ConsoleCanvas().CreateBorder();
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

    // no idea
    var r = new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle());
    var location = spacing * (new Vector3(5, 5, 5) + new Vector3(-width, -height, -length) * 0.5f) + randomizationBase + r * randomizationSpan;

    var sphere = new Sphere(0.5f);
    var sphereIndex = Sim.Shapes.Add(sphere);

    // I guess I need to study up on my physics and higher maths <.< 
    Quaternion orientation;
    orientation.X = -1 + 2 * random.NextSingle();
    orientation.Y = -1 + 2 * random.NextSingle();
    orientation.Z = -1 + 2 * random.NextSingle();
    orientation.W = 0.01f + random.NextSingle();
    QuaternionEx.Normalize(ref orientation);

    Sim.Statics.Add(new StaticDescription(location, orientation, sphereIndex));
  }

  public void RenderFrame()
  {
    Canvas.Render();
  }

  public void CameraRaycast()
  {

  }
}
