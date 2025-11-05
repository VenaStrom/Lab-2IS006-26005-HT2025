
using System.Numerics;

class Program
{
  private static void Main(string[] args)
  {
    Mesh cube = new([new(0, 0, 0), new(1, 0, 0), new(1, 1, 0), new(0, 1, 0), new(0, 0, 1), new(1, 0, 1), new(1, 1, 1), new(0, 1, 1)],
                    [new(0, 1), new(1, 2), new(2, 3), new(3, 0), new(4, 5), new(5, 6), new(6, 7), new(7, 4), new(0, 4), new(1, 5), new(2, 6), new(3, 7)],
                    [new(0, 1, 2), new(0, 2, 3), new(4, 5, 6), new(4, 6, 7), new(0, 1, 5), new(0, 5, 4), new(2, 3, 7), new(2, 7, 6), new(1, 2, 6), new(1, 6, 5), new(0, 3, 7), new(0, 7, 4)]);
    Camera camera = new(new(0.5f, 0.5f, -4), new(0, 0, 0), [cube]);

    cube.RotateAroundCenter(new Vector3(-0.30f, 0, -0.30f));

    float[,] depths = camera.Render();
    Viewport.Draw(depths);

    while (true)
    {
      // Rotate cube around vertical axis
      cube.RotateAroundCenter(new Vector3(0, 0.05f, 0));
      // Random rng = new Random();
      // cube.RotateAroundCenter(new Vector3((float)(rng.NextDouble() - 0.5) * 0.02f, (float)(rng.NextDouble() - 0.5) * 0.02f, (float)(rng.NextDouble() - 0.5) * 0.02f));

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