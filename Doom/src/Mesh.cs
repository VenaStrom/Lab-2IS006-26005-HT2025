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

  public void Translate(Vector3 translation)
  {
    for (int i = 0; i < Vertices.Count; i++)
    {
      Vertices[i] += translation;
    }
  }

  public static Mesh CreateSphere(Vector3 center, float radius, int latitudeSegments, int longitudeSegments)
  {
    List<Vector3> vertices = new();
    List<(int, int)> edges = new();
    List<(int, int, int)> faces = new();

    for (int lat = 0; lat <= latitudeSegments; lat++)
    {
      float theta = lat * MathF.PI / latitudeSegments;
      float sinTheta = MathF.Sin(theta);
      float cosTheta = MathF.Cos(theta);

      for (int lon = 0; lon <= longitudeSegments; lon++)
      {
        float phi = lon * 2 * MathF.PI / longitudeSegments;
        float sinPhi = MathF.Sin(phi);
        float cosPhi = MathF.Cos(phi);

        Vector3 vertex = new(
          center.X + radius * cosPhi * sinTheta,
          center.Y + radius * cosTheta,
          center.Z + radius * sinPhi * sinTheta
        );
        vertices.Add(vertex);
      }
    }

    // Generate edges
    for (int i = 0; i < vertices.Count; i++)
    {
      int nextLon = (i + 1) % (longitudeSegments + 1) + (i / (longitudeSegments + 1)) * (longitudeSegments + 1);
      int nextLat = i + (longitudeSegments + 1);
      if (nextLat < vertices.Count)
      {
        edges.Add((i, nextLat));
      }
      edges.Add((i, nextLon));
    }

    // Generate faces
    for (int lat = 0; lat < latitudeSegments; lat++)
    {
      for (int lon = 0; lon < longitudeSegments; lon++)
      {
        int first = (lat * (longitudeSegments + 1)) + lon;
        int second = first + longitudeSegments + 1;

        faces.Add((first, second, first + 1));
        faces.Add((second, second + 1, first + 1));
      }
    }

    return new Mesh(vertices, edges, faces);
  }

  public static Mesh CreateCube(Vector3 center, float size)
  {
    float halfSize = size / 2;
    List<Vector3> vertices = new()
    {
      new Vector3(center.X - halfSize, center.Y - halfSize, center.Z - halfSize),
      new Vector3(center.X + halfSize, center.Y - halfSize, center.Z - halfSize),
      new Vector3(center.X + halfSize, center.Y + halfSize, center.Z - halfSize),
      new Vector3(center.X - halfSize, center.Y + halfSize, center.Z - halfSize),
      new Vector3(center.X - halfSize, center.Y - halfSize, center.Z + halfSize),
      new Vector3(center.X + halfSize, center.Y - halfSize, center.Z + halfSize),
      new Vector3(center.X + halfSize, center.Y + halfSize, center.Z + halfSize),
      new Vector3(center.X - halfSize, center.Y + halfSize, center.Z + halfSize)
    };

    List<(int, int)> edges = new()
    {
      (0, 1), (1, 2), (2, 3), (3, 0),
      (4, 5), (5, 6), (6, 7), (7, 4),
      (0, 4), (1, 5), (2, 6), (3, 7)
    };

    List<(int, int, int)> faces = new()
    {
      (0, 1, 2), (0, 2, 3),
      (4, 5, 6), (4, 6, 7),
      (0, 1, 5), (0, 5, 4),
      (2, 3, 7), (2, 7, 6),
      (1, 2, 6), (1, 6, 5),
      (0, 3, 7), (0, 7, 4)
    };

    return new Mesh(vertices, edges, faces);
  }
}