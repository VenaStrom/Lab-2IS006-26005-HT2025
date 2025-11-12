internal class Program
{
  private static void Main(string[] args)
  {
    // Rectangle r1 = new(20, 10);
    // Rectangle r2 = new(5, 5);
    // Rectangle r3 = new(10, 8);

    // Console.WriteLine(Rectangle.Count);

    // Rectangle phi2 = Rectangle.MakePhiSquareByWidth(1);
    // Rectangle phi1 = Rectangle.MakePhiSquareByHeight(1);
    // phi2.Print();
    // phi1.Print();

    Temperature t1 = new();
    t1.Celsius = 100;
    Console.WriteLine($"t1: {t1.Celsius} C, {t1.Fahrenheit} F, {t1.Kelvin} K");

    Temperature t2 = Temperature.FromFahrenheit(32);
    Console.WriteLine($"t2: {t2.Celsius} C, {t2.Fahrenheit} F, {t2.Kelvin} K");
  }
}

class Temperature
{
  private double kelvin;
  public double Celsius
  {
    get { return kelvin - 273.15; }
    set { kelvin = value + 273.15; }
  }
  public double Fahrenheit
  {
    get { return (kelvin - 273.15) * 9 / 5 + 32; }
    set { kelvin = (value - 32) * 5 / 9 + 273.15; }
  }
  public double Kelvin
  {
    get { return kelvin; }
    set { kelvin = value; }
  }

  public static string[] SupportedUnits = ["Kelvin", "Celsius", "Fahrenheit"];

  public Temperature() => kelvin = 0;

  public static Temperature FromCelsius(double c)
    => new()
    {
      Celsius = c
    };

  public static Temperature FromFahrenheit(double f)
    => new()
    {
      Fahrenheit = f
    };

  public static Temperature FromKelvin(double k)
    => new()
    {
      Kelvin = k
    };
}

class Rectangle
{
  public double Width { get; set; }
  public double Height { get; set; }

  public double Area => Width * Height;

  public void Print()
  {
    Console.WriteLine($"Rectangle: w={Width:F3} h={Height:F3} A={Area:F3}");
  }

  public Rectangle(double width, double height)
  {
    Rectangle.Count++;
    Width = width;
    Height = height;
  }

  static Rectangle()
  {
    Console.WriteLine("This is the static constructor of Rectangle");
  }

  public static int Count;

  public static Rectangle MakeUnitSquare() => new(1, 1);
  public static Rectangle MakePhiSquareByWidth(double width)
  {
    double phi = (1 + Math.Sqrt(5)) / 2;

    double height = width * phi;
    return new(width, height);
  }
  public static Rectangle MakePhiSquareByHeight(double height)
  {
    double phi = (1 + Math.Sqrt(5)) / 2;

    double width = height / phi;
    return new(width, height);
  }
}