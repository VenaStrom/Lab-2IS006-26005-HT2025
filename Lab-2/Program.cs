using System.Xml.Serialization;

internal class Program
{
  private static void Main(string[] args)
  {
    Player player = new("Hero", 100);
    player.Armor = 50;
    player.Damage = 50;

    Player ally = new("Sidekick", 80);
    ally.Armor = 30;
    ally.Damage = 25;

    Enemy goblin = new("Goblin", 80);
    goblin.Armor = 15;
    goblin.Damage = 20;

    Enemy orc = new("Orc", 120);
    orc.Armor = 25;
    orc.Damage = 30;

    Console.Clear();
    while (player.IsAlive() && (goblin.IsAlive() || orc.IsAlive()))
    {
      // Print status
      Console.WriteLine("Current Status:");
      player.Print();
      ally.Print();
      goblin.Print();
      orc.Print();
      Console.WriteLine();
      Thread.Sleep(1000);

      Console.WriteLine("Your turn!");
      Console.WriteLine("Commanding " + player.Name);
      Console.WriteLine("A1-A9: attack enemy");
      Console.WriteLine("R1-R9: revive ally");
      string command = (Console.ReadLine() ?? "").Trim().ToUpper();

      // Player input
      if (command.StartsWith("A"))
      {
        int enemyIndex = int.Parse(command[1..]) - 1;
        if (enemyIndex == 0 && goblin.IsAlive())
        {
          player.Attack(goblin);
        }
        else if (enemyIndex == 1 && orc.IsAlive())
        {
          player.Attack(orc);
        }
        else
        {
          Console.WriteLine("Invalid enemy selection.");
          continue;
        }
      }
      else if (command.StartsWith("R"))
      {
        int allyIndex = int.Parse(command[1..]) - 1;
        if (allyIndex == 0)
        {
          player.Revive(ally);
        }
        else
        {
          Console.WriteLine("Invalid ally selection.");
          continue;
        }
      }
      else
      {
        Console.WriteLine("Invalid command.");
        continue;
      }

      // Enemies' turn
      if (goblin.IsAlive())
      {
        Console.WriteLine("Goblin's turn!");
        // Random attack against player or ally
        Random rand = new();
        if (ally.IsAlive() && rand.Next(2) == 0)
        {
          goblin.Attack(ally);
        }
        else
        {
          goblin.Attack(player);
        }
      }
      if (orc.IsAlive())
      {
        Console.WriteLine("Orc's turn!");
        // Random attack against player or ally
        Random rand = new();
        if (ally.IsAlive() && rand.Next(2) == 0)
        {
          orc.Attack(ally);
        }
        else
        {
          orc.Attack(player);
        }
      }

      // Ally attacks random enemy if alive
      if (ally.IsAlive())
      {
        Console.WriteLine("Sidekick's turn!");
        Random rand = new();
        if (goblin.IsAlive() && orc.IsAlive())
        {
          if (rand.Next(2) == 0)
          {
            ally.Attack(goblin);
          }
          else
          {
            ally.Attack(orc);
          }
        }
      }
      else
      {
        Console.WriteLine("Sidekick is down and cannot attack.");
      }
    }
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

    int healthGained = CurrentHealth - (newHealth - amount);
    Console.WriteLine($"{Name} healed for {healthGained} health!");
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

    if (!target.IsAlive())
    {
      Console.WriteLine($"{target.Name} has been defeated by {Name}!");
    }
  }
}

class Player(string name, int health) : Character(name, health)
{
  public void Revive(Player ally)
  {
    if (ally.IsAlive())
    {
      Console.WriteLine($"{ally.Name} is already alive and cannot be revived.");
      return;
    }

    ally.CurrentHealth = ally.MaxHealth / 2;
    Console.WriteLine($"{Name} has revived {ally.Name} to {ally.CurrentHealth} health!");
  }
}

class Enemy(string name, int health) : Character(name, health)
{
}