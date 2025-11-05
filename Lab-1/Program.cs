using System.Xml.Serialization;

internal class Program
{
  private static void Main(string[] args)
  {
    Player player1 = new("Hero", 100);
    player1.Armor = 50;
    player1.Damage = 30;

    Enemy enemy1 = new("Goblin", 80);
    enemy1.Armor = 20;
    enemy1.Damage = 15;
  }

  static void SimpleFight()
  {
    Player player1 = new("Hero", 100);
    player1.Armor = 50;
    player1.Damage = 30;

    Enemy enemy1 = new("Goblin", 80);
    enemy1.Armor = 20;
    enemy1.Damage = 15;

    Console.WriteLine("Enter the field:");
    player1.Print();
    enemy1.Print();
    Thread.Sleep(1000);

    Console.WriteLine("Round 1!");
    player1.Attack(enemy1);
    enemy1.Print();
    Thread.Sleep(1000);
    enemy1.Attack(player1);
    player1.Print();
    Thread.Sleep(1000);

    Console.WriteLine("Round 2!");
    player1.Attack(enemy1);
    enemy1.Print();
    Thread.Sleep(1000);
    enemy1.Attack(player1);
    player1.Print();
    Thread.Sleep(1000);
  }
}

class Character(string name, int health)
{
  public string Name = name;

  public int MaxHealth = health;
  public int CurrentHealth = health;
  public int Armor;
  public int Damage;

  public void Print()
  {
    Console.WriteLine($"{Name,8}: {CurrentHealth + "/" + MaxHealth,7}❤️ {Armor,4}🛡️ {Damage,4}⚔️");
  }

  public bool IsAlive()
  {
    return CurrentHealth > 0;
  }

  public void Heal(int amount)
  {
    int newHealth = CurrentHealth + amount;
    if (newHealth > MaxHealth) CurrentHealth = MaxHealth;
    else CurrentHealth = newHealth;
  }

  public void TakeDamage(int amount)
  {
    double damageAfterArmor = amount * Armor / 100;
    if (damageAfterArmor < 0) damageAfterArmor = 0;

    CurrentHealth -= Convert.ToInt32(Math.Round(damageAfterArmor));
    if (CurrentHealth < 0) CurrentHealth = 0;
  }

  public void Attack(Character target)
  {
    target.TakeDamage(Damage);
  }
}

class Player(string name, int health) : Character(name, health)
{
}

class Enemy(string name, int health) : Character(name, health)
{
}