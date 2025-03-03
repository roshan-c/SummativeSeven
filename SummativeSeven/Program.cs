using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Mini Heroes Quest!");
        
        // Initialize list of characters
        List<Character> characters = new List<Character>();
        Character playerCharacter = null;
        
        // Let the user select their character class
        Console.WriteLine("Select your character class:");
        Console.WriteLine("1. Ranger");
        Console.WriteLine("2. Barbarian");
        Console.WriteLine("3. Mage");
        Console.WriteLine("4. Titan");
        
        int characterChoice;
        while (!int.TryParse(Console.ReadLine(), out characterChoice) || characterChoice < 1 || characterChoice > 4)
        {
            Console.WriteLine("Invalid choice. Please enter a number between 1 and 4:");
        }
        
        // Create player character based on selection with predefined names
        switch (characterChoice)
        {
            case 1:
                playerCharacter = new Ranger("John");
                Console.WriteLine("You've chosen to play as a Ranger named John");
                break;
            case 2:
                playerCharacter = new Barbarian("Susan");
                Console.WriteLine("You've chosen to play as a Barbarian named Susan");
                break;
            case 3:
                playerCharacter = new Mage("Richard");
                Console.WriteLine("You've chosen to play as a Mage named Richard");
                break;
            case 4:
                playerCharacter = new Titan("Crayon");
                Console.WriteLine("You've chosen to play as a Titan named Crayon");
                break;
        }
        
        characters.Add(playerCharacter);
        
        // Create CPU characters (one of each remaining class)
        if (!(playerCharacter is Ranger)) characters.Add(new Ranger("Robin"));
        if (!(playerCharacter is Barbarian)) characters.Add(new Barbarian("Bjorn"));
        if (!(playerCharacter is Mage)) characters.Add(new Mage("Merlin"));
        if (!(playerCharacter is Titan)) characters.Add(new Titan("Thanos"));
        
        // Game turn
        Console.WriteLine("\n--- BATTLE BEGINS ---\n");
        
        // Display status of all characters
        DisplayCharacterStatus(characters);
        
        // Player's turn
        Console.WriteLine($"\n--- {playerCharacter.Name}'s Turn (Player) ---");
        
        // Let player choose an action based on their class
        List<string> actionOptions = new List<string>();
        
        if (playerCharacter is Ranger)
        {
            actionOptions.Add("Fire Arrow");
            actionOptions.Add("Collect Arrows");
            actionOptions.Add("Rest");
        }
        else if (playerCharacter is Barbarian)
        {
            actionOptions.Add("Swing Axe");
            actionOptions.Add("Rest");
        }
        else if (playerCharacter is Mage)
        {
            actionOptions.Add("Throw Fireball");
            actionOptions.Add("Heal");
            actionOptions.Add("Rest");
        }
        else if (playerCharacter is Titan)
        {
            actionOptions.Add("Crush");
            actionOptions.Add("Ground Smash");
            actionOptions.Add("Regenerate");
            actionOptions.Add("Rest");
        }
        
        Console.WriteLine("Choose your action:");
        for (int i = 0; i < actionOptions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {actionOptions[i]}");
        }
        
        int actionChoice;
        while (!int.TryParse(Console.ReadLine(), out actionChoice) || actionChoice < 1 || actionChoice > actionOptions.Count)
        {
            Console.WriteLine($"Invalid choice. Please enter a number between 1 and {actionOptions.Count}:");
        }
        
        string selectedAction = actionOptions[actionChoice - 1];
        
        // For actions that need a target
        Character target = null;
        if (selectedAction == "Fire Arrow" || selectedAction == "Swing Axe" || 
            selectedAction == "Throw Fireball" || selectedAction == "Heal" || 
            selectedAction == "Crush")
        {
            // Get available targets (all characters except self for attacks, include self for heal)
            List<Character> targets = new List<Character>();
            
            foreach (Character c in characters)
            {
                if (selectedAction == "Heal")
                {
                    // For healing, include all characters (including self)
                    targets.Add(c);
                }
                else
                {
                    // For attacks, only include other characters
                    if (c != playerCharacter) targets.Add(c);
                }
            }
            
            Console.WriteLine("Choose your target:");
            for (int i = 0; i < targets.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {targets[i].Name} ({targets[i].GetType().Name}) - HP: {targets[i].HealthPoints}/{targets[i].MaxHealthPoints}");
            }
            
            int targetChoice;
            while (!int.TryParse(Console.ReadLine(), out targetChoice) || targetChoice < 1 || targetChoice > targets.Count)
            {
                Console.WriteLine($"Invalid choice. Please enter a number between 1 and {targets.Count}:");
            }
            
            target = targets[targetChoice - 1];
        }
        
        // Execute the player's action
        ExecutePlayerAction(playerCharacter, selectedAction, target, characters.ToArray());
        
        // CPU characters' turns
        foreach (Character cpu in characters)
        {
            if (cpu != playerCharacter && !cpu.IsKnockedOut)
            {
                Console.WriteLine($"\n--- {cpu.Name}'s Turn (CPU) ---");
                ExecuteCpuAction(cpu, characters);
            }
        }
        
        // Show final status
        Console.WriteLine("\n--- TURN COMPLETE ---");
        DisplayCharacterStatus(characters);
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
    
    static void DisplayCharacterStatus(List<Character> characters)
    {
        Console.WriteLine("\n--- CHARACTER STATUS ---");
        foreach (Character c in characters)
        {
            Console.Write($"{c.Name} ({c.GetType().Name}) - HP: {c.HealthPoints}/{c.MaxHealthPoints}, Energy: {c.EnergyPoints}/{c.MaxEnergyPoints}");
            
            if (c is Ranger ranger)
            {
                Console.Write($", Arrows: {ranger.NumberOfArrows}, Fired: {ranger.FiredArrows}");
            }
            
            if (c.IsKnockedOut)
            {
                Console.Write(" [KNOCKED OUT]");
            }
            
            Console.WriteLine();
        }
    }
    
    static void ExecutePlayerAction(Character player, string action, Character target, Character[] allCharacters)
    {
        switch (action)
        {
            case "Fire Arrow":
                if (player is Ranger ranger)
                {
                    ranger.FireArrow(target);
                }
                break;
                
            case "Collect Arrows":
                if (player is Ranger rangerCollect)
                {
                    rangerCollect.CollectArrows();
                    Console.WriteLine($"{player.Name} collected their arrows.");
                }
                break;
                
            case "Swing Axe":
                if (player is Barbarian barbarian)
                {
                    barbarian.SwingAxe(target);
                }
                break;
                
            case "Throw Fireball":
                if (player is Mage mageFireball)
                {
                    mageFireball.ThrowFireball(target);
                }
                break;
                
            case "Heal":
                if (player is Mage mageHeal)
                {
                    mageHeal.Heal(target);
                }
                break;
                
            case "Crush":
                if (player is Titan titanCrush)
                {
                    titanCrush.Crush(target);
                }
                break;
                
            case "Ground Smash":
                if (player is Titan titanSmash)
                {
                    titanSmash.GroundSmash(allCharacters);
                }
                break;
                
            case "Regenerate":
                if (player is Titan titanRegen)
                {
                    titanRegen.Regenerate();
                }
                break;
                
            case "Rest":
                player.Rest();
                Console.WriteLine($"{player.Name} rests and recovers their energy and health.");
                break;
        }
    }
    
    static void ExecuteCpuAction(Character cpu, List<Character> characters)
    {
        // Simple AI for CPU characters
        Random random = new Random();
        
        // Find characters that aren't knocked out (excluding self)
        List<Character> potentialTargets = new List<Character>();
        List<Character> alliesNeedingHealing = new List<Character>();
        
        foreach (Character c in characters)
        {
            if (c != cpu && !c.IsKnockedOut)
            {
                potentialTargets.Add(c);
            }
            
            // For mage healing - find allies (including self) below 50% health
            if (!c.IsKnockedOut && c.HealthPoints < c.MaxHealthPoints / 2)
            {
                alliesNeedingHealing.Add(c);
            }
        }
        
        // If no valid targets, rest
        if (potentialTargets.Count == 0 && (!(cpu is Mage) || alliesNeedingHealing.Count == 0))
        {
            cpu.Rest();
            Console.WriteLine($"{cpu.Name} rests and recovers their energy and health.");
            return;
        }
        
        // CPU actions based on character type
        if (cpu is Ranger ranger)
        {
            // If out of arrows, collect them
            if (ranger.NumberOfArrows == 0)
            {
                ranger.CollectArrows();
                Console.WriteLine($"{ranger.Name} collected their arrows.");
            }
            // If energy is low, rest
            else if (ranger.EnergyPoints < 1)
            {
                ranger.Rest();
                Console.WriteLine($"{ranger.Name} rests and recovers their energy and health.");
            }
            // Otherwise, attack a random target
            else if (potentialTargets.Count > 0)
            {
                Character target = potentialTargets[random.Next(potentialTargets.Count)];
                ranger.FireArrow(target);
            }
        }
        else if (cpu is Barbarian barbarian)
        {
            // If energy is too low, rest
            if (barbarian.EnergyPoints < 3)
            {
                barbarian.Rest();
                Console.WriteLine($"{barbarian.Name} rests and recovers their energy and health.");
            }
            // Otherwise, attack a random target
            else if (potentialTargets.Count > 0)
            {
                Character target = potentialTargets[random.Next(potentialTargets.Count)];
                barbarian.SwingAxe(target);
            }
        }
        else if (cpu is Mage mage)
        {
            // If there are allies needing healing and enough energy, heal
            if (alliesNeedingHealing.Count > 0 && mage.EnergyPoints >= 1)
            {
                Character healTarget = alliesNeedingHealing[random.Next(alliesNeedingHealing.Count)];
                mage.Heal(healTarget);
            }
            // If energy enough for fireball, attack
            else if (potentialTargets.Count > 0 && mage.EnergyPoints >= 2)
            {
                Character target = potentialTargets[random.Next(potentialTargets.Count)];
                mage.ThrowFireball(target);
            }
            // Otherwise, rest
            else
            {
                mage.Rest();
                Console.WriteLine($"{mage.Name} rests and recovers their energy and health.");
            }
        }
        else if (cpu is Titan titan)
        {
            // Randomly choose between available actions based on energy
            int choice = random.Next(3);
            
            // If multiple targets and enough energy, use ground smash
            if (choice == 0 && potentialTargets.Count > 1 && titan.EnergyPoints >= 10)
            {
                titan.GroundSmash(characters.ToArray());
            }
            // If health is below 50% and enough energy, regenerate
            else if (choice == 1 && titan.HealthPoints < titan.MaxHealthPoints / 2 && titan.EnergyPoints >= 5)
            {
                titan.Regenerate();
            }
            // If energy enough for crush, attack
            else if (potentialTargets.Count > 0 && titan.EnergyPoints >= 6)
            {
                Character target = potentialTargets[random.Next(potentialTargets.Count)];
                titan.Crush(target);
            }
            // Otherwise, rest
            else
            {
                titan.Rest();
                Console.WriteLine($"{titan.Name} rests and recovers their energy and health.");
            }
        }
    }
}

// Base Character class for common functionality
public abstract class Character
{
    // Private backing fields for HealthPoints and EnergyPoints
    private int _healthPoints;
    private int _energyPoints;
    
    // Public property for the character's name (read-only after initialization)
    public string Name { get; private set; }
    
    // Protected property for the maximum health points (accessible by derived classes)
    public int MaxHealthPoints { get; protected set; }
    
    // Protected property for the maximum energy points (accessible by derived classes)
    public int MaxEnergyPoints { get; protected set; }
    
    // Public property for the character's current health points
    public int HealthPoints {
        get { return _healthPoints; }
        // Protected setter to allow derived classes to modify health, but with validation
        protected set
        {
            // Ensure health doesn't drop below 0
            if (value <= 0) {
                _healthPoints = 0;
            } 
            // Ensure health doesn't exceed the maximum
            else if (value > MaxHealthPoints) { 
                _healthPoints = MaxHealthPoints; 
            } 
            // Otherwise, set the health to the new value
            else {
                _healthPoints = value;
            }
        }
    }
 
    // Public property for the character's current energy points
    public int EnergyPoints
    {
        get { return _energyPoints; }
        // Protected setter to allow derived classes to modify energy, but with validation
        protected set
        {
            // Ensure energy doesn't drop below 0
            if (value <= 0) {
                _energyPoints = 0;
            } 
            // Ensure energy doesn't exceed the maximum
            else if (value > MaxEnergyPoints) { 
                _energyPoints = MaxEnergyPoints; 
            } 
            // Otherwise, set the energy to the new value
            else {
                _energyPoints = value;
            }
        }
    }
    
    // Public property to check if the character is knocked out (health <= 0)
    public bool IsKnockedOut => HealthPoints <= 0;
    
    // Constructor for the Character class
    protected Character(string name)
    {
        Name = name;
        InitializeStats(); // Call the InitializeStats method to set up character-specific stats
    }
    
    // Virtual method to initialize character-specific stats (to be overridden by derived classes)
    protected virtual void InitializeStats()
    {
        // Base initialization to be overridden by derived classes
    }
    
    // Public method to restore the character's health and energy to their maximum values
    public void Rest()
    {
        if (!IsKnockedOut)
        {
            EnergyPoints = MaxEnergyPoints;
            HealthPoints = MaxHealthPoints;
        }
    }
    
    // Public method to reduce the character's health by a given amount
    public void TakeDamage(int amount)
    {
        if (!IsKnockedOut)
        {
            HealthPoints -= amount;
            if (HealthPoints == 0)
            {
                Console.WriteLine($"{Name} has been knocked out!");
            }
        }
    }
    
    // Public method to increase the character's health by a given amount
    public void ReceiveHealing(int amount)
    {
        if (!IsKnockedOut)
        {
            int previousHealth = HealthPoints;
            HealthPoints += amount;
            int actualHealing = HealthPoints - previousHealth;
            Console.WriteLine($"{Name} recovered {actualHealing} health points.");
        }
        else
        {
            Console.WriteLine($"{Name} is knocked out and cannot be healed.");
        }
    }
}

// Ranger class, derived from Character
public class Ranger : Character
{
    // Public property for the number of arrows the ranger has
    public int NumberOfArrows { get; private set; }
    
    // Public property for the number of arrows the ranger has fired
    public int FiredArrows { get; private set; }
    
    // Constructor for the Ranger class
    public Ranger(string name) : base(name)
    {
    }
    
    // Override the InitializeStats method to set up ranger-specific stats
    protected override void InitializeStats()
    {
        MaxHealthPoints = 10;
        MaxEnergyPoints = 8;
        HealthPoints = MaxHealthPoints;
        EnergyPoints = MaxEnergyPoints;
        NumberOfArrows = 10;
        FiredArrows = 0;
    }
    
    // Public method for the ranger to collect fired arrows
    public void CollectArrows()
    {
        if (!IsKnockedOut)
        {
            NumberOfArrows += FiredArrows;
            FiredArrows = 0;
        }
    }
    
    // Public method for the ranger to fire an arrow at a target
    public void FireArrow(Character target)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot fire arrows.");
            return;
        }
        
        if (NumberOfArrows <= 0)
        {
            Console.WriteLine($"{Name} has no arrows left to fire.");
            return;
        }
        
        if (EnergyPoints < 1)
        {
            Console.WriteLine($"{Name} doesn't have enough energy to fire an arrow.");
            return;
        }
        
        EnergyPoints -= 1;
        NumberOfArrows--;
        FiredArrows++;
        Console.WriteLine($"{Name} the ranger shot an arrow at {target.Name}.");
        target.TakeDamage(1);
    }
}

// Barbarian class, derived from Character
public class Barbarian : Character
{
    // Constructor for the Barbarian class
    public Barbarian(string name) : base(name)
    {
    }
    
    // Override the InitializeStats method to set up barbarian-specific stats
    protected override void InitializeStats()
    {
        MaxHealthPoints = 18;
        MaxEnergyPoints = 12;
        HealthPoints = MaxHealthPoints;
        EnergyPoints = MaxEnergyPoints;
    }
    
    // Public method for the barbarian to swing their axe at a target
    public void SwingAxe(Character target)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot swing their axe.");
            return;
        }
        
        if (EnergyPoints < 3)
        {
            Console.WriteLine($"{Name} doesn't have enough energy to swing their axe.");
            return;
        }
        
        EnergyPoints -= 3;
        Console.WriteLine($"{Name} the barbarian swung their mighty axe at {target.Name}.");
        target.TakeDamage(3);
    }
}

// Titan class, derived from Character
public class Titan : Character
{
    // Constructor for the Titan class
    public Titan(string name) : base(name)
    {
    }
    
    // Override the InitializeStats method to set up titan-specific stats
    protected override void InitializeStats()
    {
        MaxHealthPoints = 50;
        MaxEnergyPoints = 50;
        HealthPoints = MaxHealthPoints;
        EnergyPoints = MaxEnergyPoints;
    }
    
    // Public method for the titan to crush a target
    public void Crush(Character target)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot crush anyone.");
            return;
        }
        
        if (EnergyPoints < 6)
        {
            Console.WriteLine($"{Name} doesn't have enough energy to crush.");
            return;
        }
        
        EnergyPoints -= 6;
        Console.WriteLine($"{Name} the titan crushes {target.Name} with enormous strength.");
        target.TakeDamage(6);
    }
    
    // Public method for the titan to unleash a ground smash (area effect)
    public void GroundSmash(Character[] targets)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot perform a ground smash.");
            return;
        }
        
        if (EnergyPoints < 10)
        {
            Console.WriteLine($"{Name} doesn't have enough energy to perform a ground smash.");
            return;
        }
        
        EnergyPoints -= 10;
        Console.WriteLine($"{Name} the titan slams the ground with incredible force, affecting all nearby enemies!");
        
        foreach (Character target in targets)
        {
            if (target != this && !target.IsKnockedOut) // Don't damage self or knocked out characters
            {
                Console.WriteLine($"{target.Name} is caught in the shockwave!");
                target.TakeDamage(4);
            }
        }
    }
    
    // Public method for the titan to regenerate health
    public void Regenerate()
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot regenerate.");
            return;
        }
        
        if (EnergyPoints < 5)
        {
            Console.WriteLine($"{Name} doesn't have enough energy to regenerate.");
            return;
        }
        
        int healAmount = 5; // Fixed healing amount
        
        EnergyPoints -= 5;
        Console.WriteLine($"{Name} the titan regenerates through sheer willpower!");
        ReceiveHealing(healAmount);
    }
}

// Mage class, derived from Character
public class Mage : Character
{
    // Constructor for the Mage class
    public Mage(string name) : base(name)
    {
    }
    
    // Override the InitializeStats method to set up mage-specific stats
    protected override void InitializeStats()
    {
        MaxHealthPoints = 8;
        MaxEnergyPoints = 8;
        HealthPoints = MaxHealthPoints;
        EnergyPoints = MaxEnergyPoints;
    }
    
    // Public method for the mage to throw a fireball at a target
    public void ThrowFireball(Character target)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot throw a fireball.");
            return;
        }
        
        if (EnergyPoints < 2)
        {
            Console.WriteLine($"{Name} doesn't have enough energy to throw a fireball.");
            return;
        }
        
        EnergyPoints -= 2;
        Console.WriteLine($"{Name} the mage threw a fireball at {target.Name}.");
        target.TakeDamage(2);
    }
    
    // Public method for the mage to heal a target
    public void Heal(Character target)
    {
        if (IsKnockedOut)
        {
            Console.WriteLine($"{Name} is knocked out and cannot heal.");
            return;
        }
        
        if (EnergyPoints < 1)
        {
            Console.WriteLine($"{Name} doesn't have enough energy to heal.");
            return;
        }
        
        if (target.IsKnockedOut)
        {
            Console.WriteLine($"{target.Name} is knocked out and cannot be healed.");
            return;
        }
        
        EnergyPoints -= 1;
        Console.WriteLine($"{Name} casts a healing spell on {target.Name}.");
        target.ReceiveHealing(5);
    }
}
