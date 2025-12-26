using System.Numerics;

namespace term3d.Extensions;

public static class Vector3Extensions
{
    extension(Vector3 vector)
    {
        public static Vector3 Round(Vector3 vec, int precision)
        {
            float multiplier = (float)Math.Pow(10, precision);

            return new Vector3(
                MathF.Round(vec.X * multiplier) / multiplier,
                MathF.Round(vec.Y * multiplier) / multiplier,
                MathF.Round(vec.Z * multiplier) / multiplier
                );
        }
    }
}
