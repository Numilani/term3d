using System.Numerics;
using BepuPhysics;
using ConsoleRenderer;
using term3d.Objects;

namespace term3d;

public class Render(Simulation Sim)
{
  readonly ConsoleCanvas Canvas = new();

    // Most of this is copied from BepuPhysics' source code
    public void CameraRaycast(Player player)
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

        var visionRay = new Vector3(
            player.Location.X + (row - (Canvas.Width / 2f)),
            player.Location.Y + (col - (Canvas.Height / 2f)),
            player.Location.Z + 0f);
        Sim.RayCast(player.Location, Vector3.Normalize(visionRay), float.MaxValue, ref hitHandler); // NOTE: this is the only line AI actually helped me with because I didn't know that vector normalization was needeed

        Canvas.Text(0, 0, $"XYZ: {player.Location.X} {player.Location.Y} {player.Location.Z}");
        Canvas.Text(0, 2, $"{hits} / {maxhits} rays hit object located at {Sim.Bodies[hitHandler.HitCollidable.BodyHandle].BoundingBox} (last distance {hitHandler.T})");
        if (hitHandler.T < float.MaxValue)
        {
          hits++;
          Canvas.Set(row, col, '#');
        }
        if (row == (Canvas.Width - 1) / 2 && col == (Canvas.Height - 1) / 2){
          Canvas.Text(0, 1, $"Origin: {player.Location}  |  directionRay: {visionRay}");
          Canvas.Set(row, col, 'X', ConsoleColor.DarkRed);
        }
      }
    }
    Canvas.Render();
  }
}



