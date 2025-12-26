using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using term3d;
using term3d.Objects;
using term3d.Extensions;

internal class Program
{
  private static void Main(string[] args)
  {
    var r = new Render(InitSimulation());
    var player = new Camera()
    {
      Location = new Vector3(0, 0, 0)
    };

    // main loop
    while (true)
    {
      if (Console.KeyAvailable)
      {
        ReadKey();
      }
      else
      {
        r.CameraRaycast(player);
      }
    }

    void ReadKey()
    {
      var keyInfo = Console.ReadKey(true);
      switch (keyInfo.Key)
      {
        case ConsoleKey.W:
          player.Location = Vector3.Round(player.Location + new Vector3(0, 0.10f, 0), 2);
          break;
        case ConsoleKey.S:
          player.Location = Vector3.Round(player.Location - new Vector3(0, 0.10f, 0), 2);
          break;
        case ConsoleKey.A:
          player.Location = Vector3.Round(player.Location - new Vector3(0.10f, 0, 0), 2);
          break;
        case ConsoleKey.D:
          player.Location = Vector3.Round(player.Location + new Vector3(0.10f, 0, 0), 2);
          break;
        case ConsoleKey.R:
          player.Location = Vector3.Round(player.Location + new Vector3(0, 0, 0.10f), 2);
          break;
        case ConsoleKey.F:
          player.Location = Vector3.Round(player.Location - new Vector3(0, 0, 0.10f), 2);
          break;
        case ConsoleKey.Q:
          player.Yaw += 5f;
          break;
      }
    }
  }

  private static Simulation InitSimulation()
  {
    Simulation Sim = Simulation.Create(new BufferPool(), new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new Vector3(0, -10, 0)), new SolveDescription(8, 1));
    var unitCube = new Box(1, 1, 1);
    var cubeInertia = unitCube.ComputeInertia(1);
    // bh = Sim.Bodies.Add(BodyDescription.CreateDynamic(new Vector3(0, 1, 0), cubeInertia, Sim.Shapes.Add(unitCube), 0.01f));
    Sim.Bodies.Add(BodyDescription.CreateDynamic(new Vector3(0, 1, 0), cubeInertia, Sim.Shapes.Add(unitCube), 0.01f));

    // sh = Sim.Statics.Add(new StaticDescription(new Vector3(5, 5, 0), Sim.Shapes.Add(new Box(5, 1, 5))));

    return Sim;
  }
}
