using System.Numerics;
using BepuUtilities;

namespace term3d.Objects;

public class Camera
{
  public string Id { get; set; }

  public Vector3 Location { get; set; }

  public Vector3 Facing { get; set; }

  float yaw;
  /// <summary>
  /// Gets or sets the yaw of the camera as a value from -PI to PI. At 0, Forward is aligned with -z. At PI/2, Forward is aligned with +x. In other words, higher values turn right.
  /// </summary>
  public float Yaw
  {
    get { return yaw; }
    set
    {
      var revolution = (value + Math.PI) / (2 * Math.PI);
      revolution -= Math.Floor(revolution);
      yaw = (float)(revolution * (Math.PI * 2) - Math.PI);
    }
  }
  float pitch;
  /// <summary>
  /// Gets or sets the pitch of the camera, clamped to a value from -MaximumPitch to MaximumPitch. Higher values look downward, lower values look upward.
  /// </summary>
  public float Pitch
  {
    get { return pitch; }
    set { pitch = Math.Clamp(value, -maximumPitch, maximumPitch); }
  }

  float maximumPitch = (float)Math.PI / 2;
  /// <summary>
  /// Gets or sets the maximum pitch of the camera, a value from 0 to PI / 2.
  /// </summary>
  public float MaximumPitch
  {
    get { return maximumPitch; }
    set { maximumPitch = (float)Math.Clamp(value, 0, Math.PI / 2); }
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
