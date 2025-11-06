
using System.Numerics;

class Program
{
  private static void Main(string[] args)
  {
    Mesh cube = Mesh.CreateCube(new Vector3(0, 0, 0), 1.0f);
    Mesh sphere = Mesh.CreateSphere(new Vector3(0, 0, 0), 0.80f, 8, 8);

    cube.Translate(new Vector3(-1f, 0, 0));
    sphere.Translate(new Vector3(1f, 0, 0));

    Camera camera = new(new(0, 0, -4), new(0, 0, 0), [cube, sphere]);

    cube.RotateAroundCenter(new Vector3(-0.30f, 0, -0.30f));

    float[,] depths = camera.Render();
    Viewport.Draw(depths);

    while (true)
    {
      // Rotate cube around vertical axis
      cube.RotateAroundCenter(new Vector3(0, 0.05f, 0));
      sphere.RotateAroundCenter(new Vector3(0.03f, 0.02f, 0));

      depths = camera.Render();
      Viewport.Draw(depths);
      Thread.Sleep(100);
    }
  }

  public static void WriteFrameBuffer(float[,] message)
  {
    for (int i = 0; i < message.GetLength(0); i++)
    {
      for (int j = 0; j < message.GetLength(1); j++)
      {
        System.Console.Write(message[i, j] + ", ");
      }
      System.Console.WriteLine();
    }
  }
}