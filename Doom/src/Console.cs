
class Console
{
  public static void WriteLine(Array message)
  {
    for (int i = 0; i < message.GetLength(0); i++)
    {
      for (int j = 0; j < message.GetLength(1); j++)
      {
        System.Console.Write(message.GetValue(i, j) + " ");
      }
      System.Console.WriteLine();
    }
  }

  public static void WriteLine(float[,] message)
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