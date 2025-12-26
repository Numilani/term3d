using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;

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
