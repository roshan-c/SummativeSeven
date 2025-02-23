using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Mini Heroes Quest!");

        // Create our heroes
        Ranger ranger = new Ranger("John");
        Barbarian barbarian = new Barbarian("Susan");
        Mage mage = new Mage("Richard");

        // Demonstrate some combat scenarios
        Console.WriteLine("\n=== Combat Demonstration ===");
        
        // Barbarian attacks ranger
        barbarian.SwingAxe(ranger);
        
        // Ranger retaliates
        ranger.FireArrows(barbarian);
        
        // Mage heals ranger
        mage.Heal(ranger);
        
        // Mage attacks with fireball
        mage.ThrowFireball(barbarian);

        // Demonstrate arrow collection
        Console.WriteLine($"\n=== Arrow Management ===");
        Console.WriteLine($"Ranger arrows before collecting: {ranger.NumberOfArrows}");
        ranger.CollectArrows();
        Console.WriteLine($"Ranger arrows after collecting: {ranger.NumberOfArrows}");

        // Demonstrate rest
        Console.WriteLine("\n=== Rest Demonstration ===");
        Console.WriteLine($"{barbarian.Name}'s health before rest: {barbarian.HealthPoints}");
        barbarian.Rest();
        Console.WriteLine($"{barbarian.Name}'s health after rest: {barbarian.HealthPoints}");
    }
}

public abstract class Character
{
    private int _healthPoints;
    private int _energyPoints;

    public string Name { get; private set; }
    public int MaxHealthPoints { get; protected set; }
    public int MaxEnergyPoints { get; protected set; }

    // Health Points property with validation
    public int HealthPoints
    {
        get { return _healthPoints; }
        protected set
        {
            _healthPoints = Math.Min(Math.Max(0, value), MaxHealthPoints);
        }
    }

    // Energy Points property with validation
    public int EnergyPoints
    {
        get { return _energyPoints; }
        protected set
        {
            _energyPoints = Math.Min(Math.Max(0, value), MaxEnergyPoints);
        }
    }

    // Constructor
    protected Character(string name)
    {
        Name = name;
    }

    // Common methods
    public bool IsKnockedOut => HealthPoints <= 0;

    public virtual void Rest()
    {
        if (!IsKnockedOut)
        {
            EnergyPoints = MaxEnergyPoints;
            HealthPoints = MaxHealthPoints;
            Console.WriteLine($"{Name} rested and recovered to full health and energy.");
        }
        else
        {
            Console.WriteLine($"{Name} cannot rest while knocked out!");
        }
    }

    // Protected helper method for checking if action can be performed
    protected bool CanPerformAction(int energyCost)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot perform actions!");
            return false;
        }
        if (EnergyPoints < energyCost)
        {
            Console.WriteLine($"{Name} doesn't have enough energy!");
            return false;
        }
        return true;
    }

    // Methods to handle damage and healing
    public virtual void TakeDamage(int amount)
    {
        if (IsKnockedOut) return;
        
        HealthPoints -= amount;
        Console.WriteLine($"{Name} took {amount} damage. Health: {HealthPoints}/{MaxHealthPoints}");
        
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} has been knocked out!");
        }
    }

    public virtual void ReceiveHealing(int amount)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"Cannot heal {Name} while knocked out!");
            return;
        }
        
        int healingDone = MaxHealthPoints - HealthPoints;
        HealthPoints += amount;
        healingDone = Math.Min(amount, healingDone);
        
        Console.WriteLine($"{Name} received {healingDone} healing. Health: {HealthPoints}/{MaxHealthPoints}");
    }
}

public class Ranger : Character
{
    public int NumberOfArrows { get; private set; }
    public int FiredArrows { get; private set; }

    public Ranger(string name) : base(name)
    {
        MaxHealthPoints = 10;
        MaxEnergyPoints = 8;
        HealthPoints = MaxHealthPoints;
        EnergyPoints = MaxEnergyPoints;
        NumberOfArrows = 10;
        FiredArrows = 0;
    }

    public void FireArrows(Character target)
    {
        if (!CanPerformAction(1) || NumberOfArrows <= 0)
        {
            if (NumberOfArrows <= 0)
            {
                Console.WriteLine($"{Name} has no arrows left!");
            }
            return;
        }

        EnergyPoints -= 1;
        NumberOfArrows--;
        FiredArrows++;
        Console.WriteLine($"{Name} the ranger shot an arrow at {target.Name}.");
        target.TakeDamage(1);
    }

    public void CollectArrows()
    {
        if (FiredArrows > 0)
        {
            NumberOfArrows += FiredArrows;
            Console.WriteLine($"{Name} collected {FiredArrows} arrows.");
            FiredArrows = 0;
        }
        else
        {
            Console.WriteLine($"{Name} has no arrows to collect.");
        }
    }
}

public class Barbarian : Character
{
    public Barbarian(string name) : base(name)
    {
        MaxHealthPoints = 18;
        MaxEnergyPoints = 12;
        HealthPoints = MaxHealthPoints;
        EnergyPoints = MaxEnergyPoints;
    }

    public void SwingAxe(Character target)
    {
        if (!CanPerformAction(3)) return;

        EnergyPoints -= 3;
        Console.WriteLine($"{Name} the barbarian swung their mighty axe at {target.Name}.");
        target.TakeDamage(3);
    }
}

public class Mage : Character
{
    public Mage(string name) : base(name)
    {
        MaxHealthPoints = 8;
        MaxEnergyPoints = 8;
        HealthPoints = MaxHealthPoints;
        EnergyPoints = MaxEnergyPoints;
    }

    public void ThrowFireball(Character target)
    {
        if (!CanPerformAction(2)) return;

        EnergyPoints -= 2;
        Console.WriteLine($"{Name} the mage threw a fireball at {target.Name}.");
        target.TakeDamage(2);
    }

    public void Heal(Character target)
    {
        if (!CanPerformAction(1)) return;

        EnergyPoints -= 1;
        Console.WriteLine($"{Name} the mage cast a healing spell on {target.Name}.");
        target.ReceiveHealing(5);
    }
}
