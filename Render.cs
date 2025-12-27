using System.Numerics;
using BepuPhysics;
using ConsoleRenderer;
using term3d.Objects;

namespace term3d;

public class Render(Simulation Sim)
{
  readonly ConsoleCanvas Canvas = new();

  // Most of this is copied from BepuPhysics' source code
  public void CameraRaycast(Camera player)
  {
    RayHitHandler hitHandler;
    hitHandler = default;
    hitHandler.T = float.MaxValue;

    Canvas.Clear();

    int hits = 0;
    int maxhits = Canvas.Width * Canvas.Height;

    for (int row = 1; row < Canvas.Width - 1; row++)
    {
      for (int col = 1; col < Canvas.Height - 1; col++)
      {
        // reset hit handler
        hitHandler.T = float.MaxValue;

        var location = new Vector3(row - (Canvas.Width / 2f), col - (Canvas.Height / 2f), 0f);
        location = Vector3.Normalize(location);

        var worldRay = Vector3.Transform(location, player.OrientationQuaternion);


        Sim.RayCast(player.Location, worldRay, float.MaxValue, ref hitHandler); // NOTE: this is the only line AI actually helped me with because I didn't know that vector normalization was needeed

        Canvas.Text(0, 0, $"XYZ: {player.Location.X} {player.Location.Y} {player.Location.Z} | Pitch: {player.Pitch} | Yaw: {player.Yaw}");
        Canvas.Text(0, 2, $"{hits} / {maxhits} rays hit object located at {Sim.Bodies[hitHandler.HitCollidable.BodyHandle].BoundingBox} (last distance {hitHandler.T})");
        if (hitHandler.T < float.MaxValue)
        {
          hits++;
          Canvas.Set(row, col, '#');
        }
        if (row == (Canvas.Width - 1) / 2 && col == (Canvas.Height - 1) / 2)
        {
          Canvas.Text(0, 1, $"Origin: {player.Location}  |  directionRay: {worldRay}");
          Canvas.Set(row, col, 'X', ConsoleColor.DarkRed);
        }
      }
    }
    Canvas.Render();
  }

  public void Cleanup()
  {
    Sim.Dispose();
    Canvas.Clear();
  }
}
