internal class Program
{
  private static void Main(string[] args)
  {
    IShape rect = new Rectangle(20, 10);
    IShape ellipse = new Ellipse(10, 5);

    Console.WriteLine(rect.Area);
    Console.WriteLine(ellipse.Area);

    rect.Scale(9);
    ellipse.Scale(0.3);
    
    Console.WriteLine(rect.Area);
    Console.WriteLine(ellipse.Area);
  }
}

interface IShape
{
  public double Width { get; }
  public double Height { get; }
  public double Area { get; }
  public void Scale(double factor);
}

class Rectangle(int width, int height) : IShape
{
  public double Width { get; private set; } = width;
  public double Height { get; private set; } = height;

  public double Area => Width * Height;

  public void Scale(double factor)
  {
    Width *= factor;
    Height *= factor;
  }
}

class Ellipse(int width, int height) : IShape
{
  public double Width { get; private set; } = width;
  public double Height { get; private set; } = height;

  public double Area => Math.PI * Width * Height;

  public Ellipse(double radius) : this((int)(radius * 2d), (int)(radius * 2d)) { }

  public void Scale(double factor)
  {
    Width *= factor;
    Height *= factor;
  }
}