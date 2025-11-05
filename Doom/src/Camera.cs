
using System.Numerics;

class Camera(Vector3 position, Vector3 rotation, List<Mesh> scene)
{
  public Vector3 Position = position;
  public Vector3 Rotation = rotation;

  private List<Mesh> scene = scene;

  private float aspectRatio = 3.0f / 1.0f;
  private int width = 180;
  private int height = 60;
  private float fov = 90.0f;
  private float nearClip = 0.1f;
  private float farClip = 1000.0f;

  /// <summary>
  /// Renders the scene from the camera's perspective as a 2D array of pixel depths.
  /// </summary>
  /// <returns></returns>
  public float[,] Render()
  {
    float[,] pixelDepths = new float[(int)width, (int)height];

    Matrix4x4 viewMatrix = GetViewMatrix();
    Matrix4x4 projectionMatrix = GetProjectionMatrix();

    foreach (var mesh in scene)
    {
      foreach (var face in mesh.Faces)
      {
        Vector3 v1 = mesh.Vertices[face.Item1];
        Vector3 v2 = mesh.Vertices[face.Item2];
        Vector3 v3 = mesh.Vertices[face.Item3];

        Vector4 tv1 = Vector4.Transform(new Vector4(v1, 1), viewMatrix);
        Vector4 tv2 = Vector4.Transform(new Vector4(v2, 1), viewMatrix);
        Vector4 tv3 = Vector4.Transform(new Vector4(v3, 1), viewMatrix);

        Vector4 pv1 = Vector4.Transform(tv1, projectionMatrix);
        Vector4 pv2 = Vector4.Transform(tv2, projectionMatrix);
        Vector4 pv3 = Vector4.Transform(tv3, projectionMatrix);

        // Perspective divide
        pv1 /= pv1.W;
        pv2 /= pv2.W;
        pv3 /= pv3.W;

        // Convert to screen space
        int sx1 = (int)((pv1.X + 1) * 0.5f * width);
        int sy1 = (int)((1 - (pv1.Y + 1) * 0.5f) * height);
        int sx2 = (int)((pv2.X + 1) * 0.5f * width);
        int sy2 = (int)((1 - (pv2.Y + 1) * 0.5f) * height);
        int sx3 = (int)((pv3.X + 1) * 0.5f * width);
        int sy3 = (int)((1 - (pv3.Y + 1) * 0.5f) * height);

        pixelDepths[sx1, sy1] = pv1.Z;
        pixelDepths[sx2, sy2] = pv2.Z;
        pixelDepths[sx3, sy3] = pv3.Z;

        // Rasterization and depth buffering would go here
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

    return new Matrix4x4(
      fovRad / aspectRatio, 0, 0, 0,
      0, fovRad, 0, 0,
      0, 0, farClip / (farClip - nearClip), 1,
      0, 0, (-farClip * nearClip) / (farClip - nearClip), 0
    );
  }
}