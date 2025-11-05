
class Viewport
{
  public static readonly Dictionary<char, ConsoleColor> DepthColors = new Dictionary<char, ConsoleColor>
  {
    {' ', ConsoleColor.Black},
    {'.', ConsoleColor.DarkBlue},
    {':', ConsoleColor.Blue},
    {'-', ConsoleColor.Cyan},
    {'=', ConsoleColor.Green},
    {'+', ConsoleColor.Yellow},
    {'*', ConsoleColor.Magenta},
    {'#', ConsoleColor.Red},
    {'%', ConsoleColor.White},
    {'@', ConsoleColor.Gray}
  };

  public static char GetDepthChar(float depth)
  {
    if (depth < 0.1f) return ' ';
    else if (depth < 0.2f) return '.';
    else if (depth < 0.3f) return ':';
    else if (depth < 0.4f) return '-';
    else if (depth < 0.5f) return '=';
    else if (depth < 0.6f) return '+';
    else if (depth < 0.7f) return '*';
    else if (depth < 0.8f) return '#';
    else if (depth < 0.9f) return '%';
    else return '@';
  }

  public static void Draw(float[,] pixelDepths)
  {
    int width = pixelDepths.GetLength(0);
    int height = pixelDepths.GetLength(1);

    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        float depth = pixelDepths[x, y];
        char depthChar = GetDepthChar(depth);
        ConsoleColor color = DepthColors[depthChar];
        System.Console.ForegroundColor = color;
        System.Console.Write(depthChar);
      }
      System.Console.WriteLine();
    }
    System.Console.ResetColor();
  }
}