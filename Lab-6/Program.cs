internal class Program
{
  private static void Main(string[] args)
  {
    Car myCar = new() { TopSpeed = 200 };

    myCar.Accelerate(30);
    Console.WriteLine("After Accelerating by 30: " + myCar.Speed);

    myCar.Brake(60);
    Console.WriteLine("After Braking by 60: " + myCar.Speed);

    myCar.Accelerate(250);
    Console.WriteLine("After Accelerating by 250: " + myCar.Speed);

    myCar.Speed = -20;
    Console.WriteLine("After setting Speed to -20: " + myCar.Speed);
  }
}

class Car
{
  private double speed;
  private double topSpeed;

  public double Speed
  {
    get { return speed; }
    set { speed = Math.Clamp(value, 0, TopSpeed); } // Use public TopSpeed property here to ensure EcoMode is considered
  }

  required public double TopSpeed
  {
    get { return ApplyEcoMode(topSpeed); }
    init { topSpeed = Math.Max(0, value); }
  }

  public bool EcoMode { get; set; } = false;

  public void Accelerate(double amount)
  {
    double newSpeed = speed + amount;
    if (newSpeed < 0) speed = 0;
    else if (newSpeed > topSpeed) speed = topSpeed;
    else speed = newSpeed;
  }

  public void Brake(double amount)
  {
    double newSpeed = speed - amount;
    if (newSpeed < 0) speed = 0;
    else if (newSpeed > topSpeed) speed = topSpeed;
    else speed = newSpeed;
  }

  public bool IsStopped() => speed == 0;
  public bool IsAtTopSpeed() => speed == TopSpeed;
  private double ApplyEcoMode(double speed) => EcoMode ? speed * 0.8 : speed;
}