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
        r.RenderFrame(player);
      }
      else
      {
        // r.CameraRaycast(player);
      }
    }

    r.Cleanup();
    Thread.Sleep(100);

    void ReadKey()
    {
      var keyInfo = Console.ReadKey(true);
      switch (keyInfo.Key)
      {
        case ConsoleKey.R:
          player.Location = Vector3.Round(player.Location + new Vector3(0, 1f, 0), 2);
          break;
        case ConsoleKey.F:
          player.Location = Vector3.Round(player.Location - new Vector3(0, 1f, 0), 2);
          break;
        case ConsoleKey.A:
          player.Location = Vector3.Round(player.Location - new Vector3(1f, 0, 0), 2);
          break;
        case ConsoleKey.D:
          player.Location = Vector3.Round(player.Location + new Vector3(1f, 0, 0), 2);
          break;
        case ConsoleKey.W:
          player.Location = Vector3.Round(player.Location + new Vector3(0, 0, 1f), 2);
          break;
        case ConsoleKey.S:
          player.Location = Vector3.Round(player.Location - new Vector3(0, 0, 1f), 2);
          break;
        case ConsoleKey.I:
          // player.Pitch = MathF.Round((player.Pitch + 0.0157f) * 10000f) / 10000f;
          player.Pitch = player.Pitch + 0.0157f;
          break;
        case ConsoleKey.K:
          // player.Pitch = MathF.Round((player.Pitch - 0.0157f) * 10000f) / 10000f;
          player.Pitch = player.Pitch - 0.0157f;
          break;
        case ConsoleKey.J:
          player.Yaw = MathF.Round((player.Yaw - 0.314f) * 10000f) / 10000f;
          break;
        case ConsoleKey.L:
          player.Yaw = MathF.Round((player.Yaw + 0.314f) * 10000f) / 10000f;
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
    Sim.Bodies.Add(BodyDescription.CreateDynamic(new Vector3(0, 0, 20), cubeInertia, Sim.Shapes.Add(unitCube), 0.01f));

    // sh = Sim.Statics.Add(new StaticDescription(new Vector3(5, 5, 0), Sim.Shapes.Add(new Box(5, 1, 5))));

    return Sim;
  }
}
