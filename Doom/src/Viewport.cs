
class Viewport
{
  public static char GetDepthChar(float depth)
  {
    // Convert depth to 0.0 - 1.0 range
    float normalizedDepth = (depth - 0.97509754f) / (0.9800981f - 0.97509754f);

    if (normalizedDepth < 0.1f) return '@';
    else if (normalizedDepth < 0.2f) return '%';
    else if (normalizedDepth < 0.3f) return '#';
    else if (normalizedDepth < 0.4f) return '*';
    else if (normalizedDepth < 0.5f) return '+';
    else if (normalizedDepth < 0.6f) return '=';
    else if (normalizedDepth < 0.7f) return '-';
    else if (normalizedDepth < 0.8f) return ':';
    else if (normalizedDepth < 0.9f) return '.';
    else return ' ';
  }

  public static void Draw(float[,] pixelDepths)
  {
    int width = pixelDepths.GetLength(0);
    int height = pixelDepths.GetLength(1);

    string buffer = "";

    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        float depth = pixelDepths[x, y];
        char depthChar = GetDepthChar(depth);
        buffer += depthChar;
      }
      buffer += "\n";
    }
    Console.Clear();
    Console.Write(buffer);
  }
}