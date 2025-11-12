using System.Text.RegularExpressions;

internal class Program
{
  private static void Main(string[] args)
  {
    Weapon sword = new("Excalibur", 50, 0.9);
    Player hero = new("Arthur", 100, sword);

    Weapon lance = new("Bone Lance", 40, 0.6);
    Player villain = new("Orc", 90, lance);

    hero.PrintStatus();
    villain.PrintStatus();

    // Reflection:
    //  - What does it mean that the player has a weapon?
    //    - It has a holds it via dependency injection.
    //  - Why is the player’s health private?
    //    - To encapsulate it so it can only be modified through methods with additional logic.
    //  - Can two players share the same weapon object? What might that imply?
    //    - They would hold the same handle which will surely lead to issues.

    hero.Attack(villain);
    villain.Attack(hero);
    hero.PrintStatus();
    villain.PrintStatus();

    hero.Attack(villain);
    villain.Attack(hero);
    hero.PrintStatus();
    villain.PrintStatus();

    sword.Tick();
    lance.Tick();

    hero.Attack(villain);
    villain.Attack(hero);
    hero.PrintStatus();
    villain.PrintStatus();
  }
}

class Weapon
{
  private string name;
  private int damage;

  private double accuracy;
  private int cooldown;
  private int cooldownTimer;

  public Weapon(string name, int damage, double accuracy = 1, int cooldown = 1)
  {
    this.name = name;
    this.damage = damage;
    this.accuracy = Math.Clamp(accuracy, 0, 1);
    this.cooldown = Math.Clamp(cooldown, 0, int.MaxValue);
    this.cooldownTimer = this.cooldown;
  }

  public void Attack(Player target)
  {
    Random rand = new();

    if (cooldown - cooldownTimer > 0)
    {
      Console.WriteLine($"{name} is not ready yet");

      if (rand.NextDouble() > 0.95)
      {
        Console.WriteLine($"When trying to use {name} prematurely, it tired the wielder. Cooldown increased");
        cooldownTimer = Math.Clamp(cooldownTimer - 1, -10, cooldown);
      }
      return;
    }
    cooldownTimer = 0;

    if (rand.NextDouble() > accuracy) // When 1.0 means always hits since NextDouble is 1.0 exclusive
    {
      Console.WriteLine($"{name} missed!");
      return;
    }

    Console.WriteLine($"{name} hits for {damage} damage!");
    target.TakeDamage(damage);
  }

  public void Tick() => cooldownTimer = Math.Clamp(cooldownTimer + 1, -10, cooldown);
}

class Player
{
  private string name;
  private int health;
  private Weapon weapon;

  public Player(string name, int health, Weapon weapon)
  {
    this.name = name;
    this.health = health;
    this.weapon = weapon;
  }

  public void TakeDamage(int amount)
  {
    int newHealth = health - amount;
    if (newHealth < 0) newHealth = 0;
    health = newHealth;
  }

  public void Attack(Player target) => weapon.Attack(target);

  public void PrintStatus() =>
    Console.WriteLine($"{name,-10} {health,3}❤️");
}