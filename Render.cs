using System.Numerics;
using BepuPhysics;
using ConsoleRenderer;
using term3d.Objects;

namespace term3d;

public class Renderer(Simulation Sim)
{
  readonly ConsoleCanvas Canvas = new();

  // Most of this is copied from BepuPhysics' source code
  public void CameraRaycast(Camera player)
  {
    Canvas.Clear();
    RayHitHandler hitHandler;
    hitHandler = default;
    hitHandler.T = float.MaxValue;

    float aspect = (float)Canvas.Width / Canvas.Height;

    Canvas.Clear();

    int hits = 0;
    int maxhits = Canvas.Width * Canvas.Height;

    for (int row = 1; row < Canvas.Height - 1; row++)
    {
      for (int col = 1; col < Canvas.Width - 1; col++)
      {
        // reset hit handler
        hitHandler.T = float.MaxValue;

        var location = new Vector3(row - (Canvas.Width / 2f), col - (Canvas.Height / 2f), 0f);
        location = Vector3.Transform(location, player.OrientationQuaternion);
        var worldRay = Vector3.Normalize(location);
        float u = (float)col / (Canvas.Width - 1);
        float v = (float)row / (Canvas.Height - 1);

        float px = (2f * u - 1f) * aspect;
        float py = 1f - 2f * v;

        // var location = new Vector3(px, py, 1f);

        location = Vector3.Normalize(location);
        // location = Vector3.TransformNormal(location, player.OrientationMatrix);

        Sim.RayCast(player.Location, Vector3.Normalize(location), float.MaxValue, ref hitHandler); // NOTE: this is the only line AI actually helped me with because I didn't know that vector normalization was needeed

        Canvas.Text(0, 0, $"XYZ: {player.Location.X} {player.Location.Y} {player.Location.Z} | Pitch: {player.Pitch} | Yaw: {player.Yaw}");
        Canvas.Text(0, 2, $"{hits} / {maxhits} rays hit object located at {Sim.Bodies[hitHandler.HitCollidable.BodyHandle].BoundingBox} (last distance {hitHandler.T})");
        if (hitHandler.T < float.MaxValue)
        {
          hits++;
          Canvas.Set(row, col, '#');
        }
        if (row == (Canvas.Width - 1) / 2 && col == (Canvas.Height - 1) / 2)
        {
          Canvas.Text(0, 1, $"Origin: {player.Location}  |  directionRay: {location}");
          Canvas.Set(row, col, 'X', ConsoleColor.DarkRed);
        }
      }
    }
    Canvas.Render();
  }


  public void RenderFrame(Camera player, bool debug = false)
  {
    if (!debug) Canvas.Clear();
    RayHitHandler hitHandler;
    hitHandler = default;
    hitHandler.T = float.MaxValue;

    int counter = 0;
    for (int sy = 0; sy < Canvas.Height; sy++)
    {
      for (int sx = 0; sx < Canvas.Width; sx++)
      {
        hitHandler.T = float.MaxValue;

        var origin = new Vector3(sx + player.Location.X, sy + player.Location.Y, player.Location.Z); // NOTE: EUREKA
        Sim.RayCast(
            origin, // From the camera grid
            new Vector3(0f, 0f, 1f), // towards +Z
            float.MaxValue, // as far as a ray can go
            ref hitHandler
            );

        if (hitHandler.T < float.MaxValue)
        {
          if (debug) Console.WriteLine($"ray originating from {player.Location} through pixel [{sx}, {sy}] hit object at {Sim.Bodies[hitHandler.HitCollidable.BodyHandle].BoundingBox}");
          if (!debug) Canvas.Set(sx,sy,'#');
          counter++;
        }

      }
    }
    if (debug) Console.WriteLine($"hits: {counter} / {Canvas.Height * Canvas.Width}");
    if (!debug) Canvas.Render();
  }

  public void Cleanup()
  {
    Sim.Dispose();
    Canvas.Clear();
  }
}
