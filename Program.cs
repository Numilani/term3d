using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using term3d;
using term3d.Objects;

internal class Program
{

  private static void Main(string[] args)
  {
    var state = new SessionState();
    state.LoadedLevel = InitSimulation();
    state.Player = new Camera(){Location = new Vector3(0,0,0)};
    state.Status = RunState.RUNNING;

    var r = new Renderer(state.LoadedLevel);

    bool keepRunning = true;
    // main loop
    while (state.Status == RunState.RUNNING)
    {
      if (Console.KeyAvailable)
      {
        ReadKey(state, Console.ReadKey(true));
      }
      else
      {
        // r.RenderOrtho(player);
        r.RenderPerspective(state.Player);
      }
    }

    r.Cleanup();
    Thread.Sleep(100);

  }

  static void ReadKey(SessionState state, ConsoleKeyInfo key)
  {
    switch (key.Key)
    {
      case ConsoleKey.W:
        state.Player.Location = Vector3.Round(Vector3.Add(state.Player.Location, state.Player.Forward));
        break;
      case ConsoleKey.S:
        state.Player.Location = Vector3.Round(Vector3.Add(state.Player.Location, state.Player.Backward));
        break;
      case ConsoleKey.A:
        state.Player.Location = Vector3.Round(Vector3.Add(state.Player.Location, state.Player.Left));
        break;
      case ConsoleKey.D:
        state.Player.Location = Vector3.Round(Vector3.Add(state.Player.Location, state.Player.Right));
        break;
      case ConsoleKey.R:
        state.Player.Location = Vector3.Round(Vector3.Add(state.Player.Location, state.Player.Up));
        break;
      case ConsoleKey.F:
        state.Player.Location = Vector3.Round(Vector3.Add(state.Player.Location, state.Player.Down));
        break;

      case ConsoleKey.I:
        state.Player.Pitch = state.Player.Pitch + (float)Math.PI / 8 * 10000f / 10000f;
        break;
      case ConsoleKey.K:
        state.Player.Pitch = state.Player.Pitch - (float)Math.PI / 8 * 10000f / 10000f;
        break;
      case ConsoleKey.J:
        state.Player.Yaw = MathF.Round((state.Player.Yaw + ((float)Math.PI / 4)) * 10000f) / 10000f;
        break;
      case ConsoleKey.L:
        state.Player.Yaw = MathF.Round((state.Player.Yaw - ((float)Math.PI / 4)) * 10000f) / 10000f;
        break;
      case ConsoleKey.X:
        state.Status = RunState.EXIT_REQUESTED;
        break;

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

public enum RunState
{
  RUNNING,
  PAUSED,
  EXIT_REQUESTED
}
