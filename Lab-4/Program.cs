internal class Program
{
  private static void Main(string[] args)
  {
    Rectangle rect = new(4.0, 5.0) { Name = "Rect1" };
    rect.Print();

    rect.Scale(2.0);
    rect.Print();

    rect.Resize(3.0, 6.0);
    rect.Print();

    rect.SetArea(50.0);
    rect.Print();

    Rectangle rect2 = new(rect) { Name = "Rect2" };
    rect2.Print();

    rect2.Scale(0.5, 1.5);
    rect2.Print();

    rect.CopyArea(rect2);
    rect.Print();
    rect2.Print();
  }
}

class Rectangle
{
  public string? Name { get; set; }

  private double width;
  private double height;

  public Rectangle(double width, double height)
  {
    this.width = width;
    this.height = height;
  }
  public Rectangle(double size) : this(size, size) { }
  public Rectangle(Rectangle other) : this(other.width, other.height) { }

  public void Scale(double widthFactor, double heightFactor)
  {
    width *= widthFactor;
    height *= heightFactor;
  }

  public void Scale(double factor) =>
    Scale(factor, factor);

  public void Resize(double newWidth, double newHeight)
  {
    width = newWidth;
    height = newHeight;
  }
  public void Resize(double newSize) =>
    Resize(newSize, newSize);
  public void Resize(Rectangle other) =>
    Resize(other.width, other.height);

  public double GetArea() =>
    width * height;

  public void SetArea(double area)
  {
    double aspectRatio = GetAspectRatio();
    height = Math.Sqrt(area / aspectRatio);
    width = area / height;
  }
  public void SetArea(Rectangle other)
  {
    SetArea(other.GetArea());
  }

  public double GetAspectRatio() =>
    width / height;


  override public string ToString() =>
    Name ?? "Rectangle";

  public void Print() =>
    Console.WriteLine($"{this.ToString()} ({width} x {height}) Area: {GetArea()}");
}