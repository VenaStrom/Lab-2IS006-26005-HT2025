using System.Numerics;

class Mesh(List<Vector3> vertices, List<(int, int)> edges, List<(int, int, int)> faces)
{
  public List<Vector3> Vertices = vertices;
  public List<(int, int)> Edges = edges;
  public List<(int, int, int)> Faces = faces;

  public void Rotate(Vector3 rotation)
  {
    Matrix4x4 rotationX = Matrix4x4.CreateRotationX(rotation.X);
    Matrix4x4 rotationY = Matrix4x4.CreateRotationY(rotation.Y);
    Matrix4x4 rotationZ = Matrix4x4.CreateRotationZ(rotation.Z);
    Matrix4x4 rotationMatrix = rotationZ * rotationY * rotationX;

    for (int i = 0; i < Vertices.Count; i++)
    {
      Vector4 v = new(Vertices[i], 1);
      v = Vector4.Transform(v, rotationMatrix);
      Vertices[i] = new Vector3(v.X, v.Y, v.Z);
    }
  }

  public void RotateAroundCenter(Vector3 rotation)
  {
    // Calculate center
    Vector3 center = new(0, 0, 0);
    foreach (var v in Vertices)
    {
      center += v;
    }
    center /= Vertices.Count;

    // Translate to origin
    for (int i = 0; i < Vertices.Count; i++)
    {
      Vertices[i] -= center;
    }

    // Rotate
    Rotate(rotation);

    // Translate back
    for (int i = 0; i < Vertices.Count; i++)
    {
      Vertices[i] += center;
    }
  }
}