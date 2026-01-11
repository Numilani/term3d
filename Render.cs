using System.Numerics;
using BepuPhysics;
using BepuUtilities;
using ConsoleRenderer;
using term3d.Objects;

namespace term3d;

public class Renderer(Simulation Sim)
{
  readonly ConsoleCanvas Canvas = new();

  // NOTE: this could be fine for some sort of 2D view, i.e. a minimap?
  public void RenderOrtho(Camera player, bool debug = false)
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
          if (!debug) Canvas.Set(sx, sy, '#');
          counter++;
        }

      }
    }
    if (debug) Console.WriteLine($"hits: {counter} / {Canvas.Height * Canvas.Width}");
    if (!debug) Canvas.Render();
  }

  public void RenderPerspective(Camera player)
  {
    Canvas.Clear();

    RayHitHandler hitHandler;
    hitHandler = default;
    hitHandler.T = float.MaxValue;

    int counter = 0;

    // START PERSPECTIVE LOGIC
    for (int sy = 0; sy < Canvas.Height; sy++)
    {
      for (int sx = 0; sx < Canvas.Width; sx++)
      {
        hitHandler.T = float.MaxValue;

        var dirX = (float)sx / Canvas.Width * 2 - 1;
        var dirY = (float)sy / Canvas.Height * 2 - 1;

        // dirX *= (float)Canvas.Width / Canvas.Height;

        var direction = new Vector3(
            dirX,
            -dirY,
            0.5f // the focus distance - how far away from "the camera" is the "imaginary screen"
            );

        direction = Vector3.Transform(direction, Matrix4x4.CreateFromQuaternion(player.OrientationQuaternion));

        Sim.RayCast(
            player.Location, // From the player point
            direction, // spreading out from the origin like a big cone
            float.MaxValue, // as far as a ray can go
            ref hitHandler
            );

        if (hitHandler.T < float.MaxValue)
        {
          Canvas.Set(sx, sy, '#');
          counter++;
        }
      }
    }
    // END PERSPECTIVE LOGIC

    Canvas.Text(0,0, $"Location: {player.Location}");
    Canvas.Render();
  }

  public void Cleanup()
  {
    Sim.Dispose();
    Canvas.Clear();
  }
}
