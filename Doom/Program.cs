class Program
{
  private static void Main(string[] args)
  {
    Mesh cube = new([new(0, 0, 0), new(1, 0, 0), new(1, 1, 0), new(0, 1, 0), new(0, 0, 1), new(1, 0, 1), new(1, 1, 1), new(0, 1, 1)],
                    [new(0, 1), new(1, 2), new(2, 3), new(3, 0), new(4, 5), new(5, 6), new(6, 7), new(7, 4), new(0, 4), new(1, 5), new(2, 6), new(3, 7)],
                    [new(0, 1, 2), new(0, 2, 3), new(4, 5, 6), new(4, 6, 7), new(0, 1, 5), new(0, 5, 4), new(2, 3, 7), new(2, 7, 6), new(1, 2, 6), new(1, 6, 5), new(0, 3, 7), new(0, 7, 4)]);
    Camera camera = new(new(0, 0, -5), new(0, 0, 0), [cube]);


    // Console.WriteLine(camera.Render());
    float[,] depths = camera.Render();
    Viewport.Draw(depths);
  }
}