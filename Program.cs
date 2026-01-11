using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using term3d;
using term3d.Objects;

internal class Program
{

  // public List<MapStaticsData> StaticsData {get;set;}

  private static void Main(string[] args)
  {
    var r = new Renderer(InitSimulation());
    var player = new Camera()
    {
      Location = new Vector3(0, 0, 0)
    };

    bool keepRunning = true;
    // main loop
    while (keepRunning)
    {
      if (Console.KeyAvailable)
      {
        ReadKey();
      }
      else
      {
        // r.RenderOrtho(player);
        r.RenderPerspective(player);
      }
    }

    r.Cleanup();
    Thread.Sleep(100);

    void ReadKey()
    {
      var keyInfo = Console.ReadKey(true);
      switch (keyInfo.Key)
      {
        case ConsoleKey.W:
          player.Location = Vector3.Round(Vector3.Add(player.Location, player.Forward));
          break;
        case ConsoleKey.S:
          player.Location = Vector3.Round(Vector3.Add(player.Location, player.Backward));
          break;
        case ConsoleKey.A:
          player.Location = Vector3.Round(Vector3.Add(player.Location, player.Left));
          break;
        case ConsoleKey.D:
          player.Location = Vector3.Round(Vector3.Add(player.Location, player.Right));
          break;
        case ConsoleKey.R:
          player.Location = Vector3.Round(Vector3.Add(player.Location, player.Up));
          break;
        case ConsoleKey.F:
          player.Location = Vector3.Round(Vector3.Add(player.Location, player.Down));
          break;

        case ConsoleKey.I:
          player.Pitch = player.Pitch + (float)Math.PI / 8 * 10000f / 10000f;
          break;
        case ConsoleKey.K:
          player.Pitch = player.Pitch - (float)Math.PI / 8 * 10000f / 10000f;
          break;
        case ConsoleKey.J:
          player.Yaw = MathF.Round((player.Yaw + ((float)Math.PI / 4)) * 10000f) / 10000f;
          break;
        case ConsoleKey.L:
          player.Yaw = MathF.Round((player.Yaw - ((float)Math.PI / 4)) * 10000f) / 10000f;
          break;
        case ConsoleKey.X:
          keepRunning = false;
          break;

      }
    }
  }

  private static Simulation InitSimulation()
  {
    Simulation Sim = Simulation.Create(new BufferPool(), new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new Vector3(0, -10, 0)), new SolveDescription(8, 1));
    var unitCube = new Box(3, 3, 3);
    var cubeInertia = unitCube.ComputeInertia(1);
    // bh = Sim.Bodies.Add(BodyDescription.CreateDynamic(new Vector3(0, 1, 0), cubeInertia, Sim.Shapes.Add(unitCube), 0.01f));
    Sim.Bodies.Add(BodyDescription.CreateDynamic(new Vector3(0, 0, 10), cubeInertia, Sim.Shapes.Add(unitCube), 0.01f));

    // sh = Sim.Statics.Add(new StaticDescription(new Vector3(5, 5, 0), Sim.Shapes.Add(new Box(5, 1, 5))));

    return Sim;
  }
}
