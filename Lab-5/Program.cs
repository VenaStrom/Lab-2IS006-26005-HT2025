internal class Program
{
  private static void Main(string[] args)
  {
    Rectangle rect = new("43 23");
    rect.Print();
  }
}
class Rectangle
{
  public Rectangle(double height, double width)
  {
    this.height = height;
    this.width = width;
  }
  public Rectangle(double size) : this(size, size) { } // Square
  public Rectangle() : this(1.0, 1.0) { } // Default to unit square
  public Rectangle(Rectangle other) : this(other.height, other.width) { } // Copy constructor
  public Rectangle(string dimensions)
  {
    char[] separators = ['x', 'X', ' ', ','];
    string[] parts = dimensions.Split(separators, StringSplitOptions.RemoveEmptyEntries);

    if (parts.Length == 2 &&
      double.TryParse(parts[0], out double h) &&
      double.TryParse(parts[1], out double w))
    {
      height = h;
      width = w;
    }
    else
    {
      throw new ArgumentException("Invalid dimensions format. Use 'height x width'.");
    }
  }

  private double width;
  private double height;

  public void Print() =>
    Console.WriteLine($"({height} x {width})");
}