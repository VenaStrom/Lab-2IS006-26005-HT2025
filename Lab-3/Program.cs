using System.Xml.Serialization;

internal class Program
{
  private static void Main(string[] args)
  {
    Player hero = new("Hero", 100, 50, 50);
    Player ally = new("Sidekick", 80, 30, 25);
    Enemy goblin = new("Goblin", 80, 15, 20);
    Enemy orc = new("Orc", 120, 25, 30);
  }
}

class Character(string name, int health, int armor = 0, int damage = 0)
{
  private string name = name;
  private int maxHealth = health;
  private int currentHealth = health;
  private int armor = armor;
  private int damage = damage;

  public void Print() => Console.WriteLine($"{name,8}: {currentHealth + "/" + maxHealth,7}❤️ {armor,4}🛡️ {damage,4}⚔️");

  public bool IsAlive()
  {
    return currentHealth > 0;
  }

  public void Heal(int amount)
  {
    int newHealth = currentHealth + amount;
    if (newHealth > maxHealth) currentHealth = maxHealth;
    else currentHealth = newHealth;

    int healthGained = currentHealth - (newHealth - amount);
    Console.WriteLine($"{name} healed for {healthGained} health!");
  }

  public void TakeDamage(int amount)
  {
    double damageAfterArmor = amount * armor / 100;
    if (damageAfterArmor < 0) damageAfterArmor = 0;

    currentHealth -= Convert.ToInt32(Math.Round(damageAfterArmor));
    if (currentHealth < 0) currentHealth = 0;
  }

  public void Attack(Character target)
  {
    target.TakeDamage(damage);

    if (!target.IsAlive())
    {
      Console.WriteLine($"{target.name} has been defeated by {name}!");
    }
  }

  public void Revive(Player ally)
  {
    if (ally.IsAlive())
    {
      Console.WriteLine($"{ally.name} is already alive and cannot be revived.");
      return;
    }

    ally.currentHealth = ally.maxHealth / 2;
    Console.WriteLine($"{name} has revived {ally.name} to {ally.currentHealth} health!");
  }
}

class Player(string name, int health, int armor = 0, int damage = 0) : Character(name, health, armor, damage)
{
  
}

class Enemy(string name, int health, int armor = 0, int damage = 0) : Character(name, health, armor, damage)
{
}