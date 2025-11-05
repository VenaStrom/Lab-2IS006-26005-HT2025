using System.Numerics;

class Camera(Vector3 position, Vector3 rotation, List<Mesh> scene, float charAspect = 0.5f)
{
  public Vector3 Position = position;
  public Vector3 Rotation = rotation;

  private List<Mesh> scene = scene;

  // toggles: set these at runtime to control rendering mode
  public static bool RenderFaces = true; // when false, draw wireframe (edges only)
  public static bool BackfaceCulling = false; // when false, don't cull backfaces

  private int width = Console.WindowWidth;
  private int height = Math.Max(1, Console.WindowHeight);
  private float charAspect = charAspect;
  private float fov = 40.0f;
  private float nearClip = 0.1f;
  private float farClip = 1000.0f;

  /// <summary>
  /// Renders the scene from the camera's perspective as a 2D array of pixel depths.
  /// </summary>
  /// <returns></returns>
  public float[,] Render()
  {
    // Refresh terminal size in case the user resized the window
    width = Console.WindowWidth;
    height = Math.Max(1, Console.WindowHeight);

    float[,] pixelDepths = new float[width, height];

    // initialize depth buffer to +inf (far away)
    for (int ix = 0; ix < width; ix++)
      for (int iy = 0; iy < height; iy++)
        pixelDepths[ix, iy] = float.PositiveInfinity;

    Matrix4x4 viewMatrix = GetViewMatrix();
    Matrix4x4 projectionMatrix = GetProjectionMatrix();

    foreach (var mesh in scene)
    {
      foreach (var face in mesh.Faces)
      {
        Vector3 v1 = mesh.Vertices[face.Item1];
        Vector3 v2 = mesh.Vertices[face.Item2];
        Vector3 v3 = mesh.Vertices[face.Item3];

        // Backface culling (optional). Use a small epsilon so culling isn't overly aggressive.
        if (BackfaceCulling)
        {
          Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1);
          Vector3 toCamera = Position - v1;
          if (Vector3.Dot(normal, toCamera) <= 1e-6f)
            continue;
        }

        Vector4 tv1 = Vector4.Transform(new Vector4(v1, 1), viewMatrix);
        Vector4 tv2 = Vector4.Transform(new Vector4(v2, 1), viewMatrix);
        Vector4 tv3 = Vector4.Transform(new Vector4(v3, 1), viewMatrix);

        // Skip triangles with any vertex behind the camera (W <= 0) to avoid invalid projection
        if (tv1.W <= 0 || tv2.W <= 0 || tv3.W <= 0)
          continue;

        Vector4 pv1 = Vector4.Transform(tv1, projectionMatrix);
        Vector4 pv2 = Vector4.Transform(tv2, projectionMatrix);
        Vector4 pv3 = Vector4.Transform(tv3, projectionMatrix);

        // Perspective divide
        pv1 /= pv1.W;
        pv2 /= pv2.W;
        pv3 /= pv3.W;

        // skip invalid projections
        if (float.IsNaN(pv1.X) || float.IsNaN(pv2.X) || float.IsNaN(pv3.X))
          continue;

        // Convert to screen space
        int sx1 = (int)((pv1.X + 1) * 0.5f * width);
        int sy1 = (int)((1 - (pv1.Y + 1) * 0.5f) * height);
        int sx2 = (int)((pv2.X + 1) * 0.5f * width);
        int sy2 = (int)((1 - (pv2.Y + 1) * 0.5f) * height);
        int sx3 = (int)((pv3.X + 1) * 0.5f * width);
        int sy3 = (int)((1 - (pv3.Y + 1) * 0.5f) * height);

        // clamp coordinates to buffer
        sx1 = Math.Clamp(sx1, 0, width - 1);
        sx2 = Math.Clamp(sx2, 0, width - 1);
        sx3 = Math.Clamp(sx3, 0, width - 1);
        sy1 = Math.Clamp(sy1, 0, height - 1);
        sy2 = Math.Clamp(sy2, 0, height - 1);
        sy3 = Math.Clamp(sy3, 0, height - 1);

        // Simple bounding-box rasterization with barycentric coordinates
        int minX = Math.Max(0, Math.Min(sx1, Math.Min(sx2, sx3)));
        int maxX = Math.Min(width - 1, Math.Max(sx1, Math.Max(sx2, sx3)));
        int minY = Math.Max(0, Math.Min(sy1, Math.Min(sy2, sy3)));
        int maxY = Math.Min(height - 1, Math.Max(sy1, Math.Max(sy2, sy3)));

        // Precompute for barycentric
        Vector2 p1s = new Vector2(sx1, sy1);
        Vector2 p2s = new Vector2(sx2, sy2);
        Vector2 p3s = new Vector2(sx3, sy3);

        float denom = (p2s.Y - p3s.Y) * (p1s.X - p3s.X) + (p3s.X - p2s.X) * (p1s.Y - p3s.Y);

        // helper to draw an edge with linear depth interpolation
        void DrawEdge(int x0, int y0, float d0, int x1, int y1, float d1)
        {
          int dx = Math.Abs(x1 - x0);
          int dy = Math.Abs(y1 - y0);
          int steps = Math.Max(1, Math.Max(dx, dy));
          for (int i = 0; i <= steps; i++)
          {
            float t = (float)i / steps;
            int x = x0 + (int)Math.Round((x1 - x0) * t);
            int y = y0 + (int)Math.Round((y1 - y0) * t);
            if (x < 0 || x >= width || y < 0 || y >= height) continue;
            float depth = d0 * (1 - t) + d1 * t;
            if (depth < pixelDepths[x, y]) pixelDepths[x, y] = depth;
          }
        }

        if (!RenderFaces)
        {
          // wireframe mode: just draw triangle edges
          DrawEdge(sx1, sy1, pv1.Z, sx2, sy2, pv2.Z);
          DrawEdge(sx2, sy2, pv2.Z, sx3, sy3, pv3.Z);
          DrawEdge(sx3, sy3, pv3.Z, sx1, sy1, pv1.Z);
        }
        else if (Math.Abs(denom) < 1e-6f)
        {
          // degenerate triangle (collinear): draw edges so at least lines are visible
          DrawEdge(sx1, sy1, pv1.Z, sx2, sy2, pv2.Z);
          DrawEdge(sx2, sy2, pv2.Z, sx3, sy3, pv3.Z);
          DrawEdge(sx3, sy3, pv3.Z, sx1, sy1, pv1.Z);
        }
        else
        {
          for (int x = minX; x <= maxX; x++)
          {
            for (int y = minY; y <= maxY; y++)
            {
              float a = ((p2s.Y - p3s.Y) * (x - p3s.X) + (p3s.X - p2s.X) * (y - p3s.Y)) / denom;
              float b = ((p3s.Y - p1s.Y) * (x - p3s.X) + (p1s.X - p3s.X) * (y - p3s.Y)) / denom;
              float c = 1 - a - b;

              if (a >= 0 && b >= 0 && c >= 0)
              {
                float depth = a * pv1.Z + b * pv2.Z + c * pv3.Z;
                if (depth < pixelDepths[x, y])
                  pixelDepths[x, y] = depth;
              }
            }
          }
        }
      }
    }

    return pixelDepths;
  }

  public Matrix4x4 GetViewMatrix()
  {
    Matrix4x4 translation = Matrix4x4.CreateTranslation(-Position);
    Matrix4x4 rotationX = Matrix4x4.CreateRotationX(-Rotation.X);
    Matrix4x4 rotationY = Matrix4x4.CreateRotationY(-Rotation.Y);
    Matrix4x4 rotationZ = Matrix4x4.CreateRotationZ(-Rotation.Z);
    return rotationZ * rotationY * rotationX * translation;
  }

  public Matrix4x4 GetProjectionMatrix()
  {
    float fovRad = 1.0f / (float)Math.Tan((fov * 0.5f) * (Math.PI / 180.0f));
    // account for terminal character aspect ratio (char width / char height)
    float aspectRatio = ((float)width / (float)height) * charAspect;

    return new Matrix4x4(
      fovRad / aspectRatio, 0, 0, 0,
      0, fovRad, 0, 0,
      0, 0, farClip / (farClip - nearClip), 1,
      0, 0, (-farClip * nearClip) / (farClip - nearClip), 0
    );
  }
}