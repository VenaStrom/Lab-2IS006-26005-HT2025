internal class Program
{
  private static void Main(string[] args)
  {
    Car myCar = new();
    myCar.Speed = 50;
    Console.WriteLine("Initial Speed: " + myCar.Speed);

    myCar.Accelerate(30);
    Console.WriteLine("After Accelerating by 30: " + myCar.Speed);

    myCar.Brake(60);
    Console.WriteLine("After Braking by 60: " + myCar.Speed);

    myCar.Accelerate(250);
    Console.WriteLine("After Accelerating by 250: " + myCar.Speed);
  }
}

class Car
{
  private double speed;
  private double topSpeed = 200;

  public double Speed
  {
    get { return speed; }
    set { speed = value; }
  }

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
}