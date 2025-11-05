using System.Numerics;

class Mesh(List<Vector3> vertices, List<(int, int)> edges, List<(int, int, int)> faces)
{
  public List<Vector3> Vertices = vertices;
  public List<(int, int)> Edges = edges;
  public List<(int, int, int)> Faces = faces;
}