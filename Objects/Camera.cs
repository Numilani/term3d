using System.Numerics;
using BepuUtilities;

namespace term3d.Objects;

public class Camera
{
  public string Id { get; set; }

  public Vector3 Location { get; set; }

  public Vector3 Facing { get; set; }

  float yaw;
  public float Yaw
  {
    get => yaw;
    set
    {
      if (value > 180) yaw = value - 360f;
      if (value < -180) yaw = value + 360f;
    }
  }

  float pitch;
  public float Pitch
  {
    get => pitch;
    set
    {
      pitch = Math.Clamp(value, -90f, 90f);
    }
  }


  //All of this could be quite a bit faster, but wasting a few thousand cycles per frame isn't exactly a concern.
  /// <summary>
  /// Gets the orientation quaternion of the camera.
  /// </summary>
  public Quaternion OrientationQuaternion
  {
    get
    {
      QuaternionEx.CreateFromYawPitchRoll(-yaw, -pitch, 0, out var orientationQuaternion);
      return orientationQuaternion;
    }
  }

  /// <summary>
  /// Gets the orientation transform of the camera.
  /// </summary>
  public Matrix Orientation => Matrix.CreateFromQuaternion(OrientationQuaternion);

  /// <summary>
  /// Gets the right direction of the camera. Equivalent to transforming (1,0,0) by Orientation.
  /// </summary>
  public Vector3 Right
  {
    get
    {
      var orientation = OrientationQuaternion;
      QuaternionEx.TransformUnitX(orientation, out var right);
      return right;
    }
  }
  /// <summary>
  /// Gets the left direction of the camera. Equivalent to transforming (-1,0,0) by Orientation.
  /// </summary>
  public Vector3 Left => -Right;
  /// <summary>
  /// Gets the up direction of the camera. Equivalent to transforming (0,1,0) by Orientation.
  /// </summary>
  public Vector3 Up
  {
    get
    {
      var orientation = OrientationQuaternion;
      QuaternionEx.TransformUnitY(orientation, out var up);
      return up;
    }
  }
  /// <summary>
  /// Gets the down direction of the camera. Equivalent to transforming (0,-1,0) by Orientation.
  /// </summary>
  public Vector3 Down => -Up;
  /// <summary>
  /// Gets the backward direction of the camera. Equivalent to transforming (0,0,1) by Orientation.
  /// </summary>
  public Vector3 Backward
  {
    get
    {
      var orientation = OrientationQuaternion;
      QuaternionEx.TransformUnitZ(orientation, out var backward);
      return backward;
    }
  }
  /// <summary>
  /// Gets the forward direction of the camera. Equivalent to transforming (0,0,-1) by Orientation.
  /// </summary>
  public Vector3 Forward => -Backward;



}
