using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;


namespace Arena
{
    internal class Program
    {
        // defouit warriors
        static List<Participant> Warriors = new List<Participant>() { new Knight("Oleg"), new Tank("Gregory"), new Theaf("'Niger'"), new Berserk("Olaf") };
        // defouit warriors

        // menu options
        static List<string> Options = new List<string>() { "FIGHT", "CREATE A NEW WARRIOR", "MY WARRIORS", "EXIT" };
        // menu options

        static Type typeOfParticipant = typeof(Participant);

        static Random rand = new Random();
        //--------------------------------------------------------------------------------------------------------------------
        static void Main(string[] args)
        {
    

            Console.CursorVisible = false;

            //main Thread 
            Thread secondThread = new Thread(MainMenu);
            secondThread.Start();
            //main Thread 
       
        }
        abstract public class Participant // abstract_class_parent
        {

            public string Name { get; protected set; }
            public int Health { get; protected set; }
            public int Damage { get; protected set; }
            public int DefoultHealth { get;protected set; }
            public int DefoultDamage { get; protected set; }
            public double AttackSpeed { get;protected set; }
            public double Armor  { get;protected set; }
            public virtual bool CheckHealth()
            {
                    return Health > 0;
            }
            public virtual bool TryToBolck() { return false; }
            public virtual double FinalDamage()
            {
                double damage= Damage* AttackSpeed;
                return damage;
            }
            public virtual void GettingDamage(double damage)
            {
                Health -= (int) Math.Round( damage* Armor, 0);
                if (Health < 0) { Health = 0; }
            }

            public virtual void Reset()
            {
                Health = DefoultHealth;
            }
        }

        //------------------------------------------description of warrior classes----------------------------------------------------
        public class Knight : Participant //BlockChance
        { 
            public double BlockChance;
            public Knight(string name,int damage = 18, int health = 100, double attackSpeed = 1, double armor = 0.3,double blockChance=0.3)
            {
                Name = name;
                DefoultDamage  = damage; 
                Damage = damage;
                DefoultHealth = health;
                Health = health;
                AttackSpeed = attackSpeed;
                BlockChance = blockChance;
                Armor = armor;
            }
            public override bool TryToBolck()
            {
                if (BlockChance <= rand.NextDouble())
                {
                    return true;
                }
                return false;
            }


        }
        class Tank : Participant            // increaseArmor + BlockChance
        {
            public double BlockChance;
            public Tank(string name, int damage = 12, int health = 150, double attackSpeed = 0.7, double armor = 0.5, double blockChance = 0.5)
            {
                Name = name;
                DefoultDamage = damage;
                Damage = damage;
                DefoultHealth = health;
                Health = health;
                AttackSpeed = attackSpeed;
                BlockChance = blockChance;
                Armor = armor;
            }
            public override bool TryToBolck()
            {
                if (BlockChance <= rand.NextDouble())
                {
                    return true;
                }
                return false;
            }

        }
        class Theaf : Participant //DodgeChance
        {
            public double DodgeChance;
            public Theaf(string name, int damage = 10, int health = 75, double attackSpeed = 1.4, double armor = 0.1, double dodgeChance = 0.6)
            {
                Name = name;
                DefoultDamage = damage;
                Damage = damage;
                DefoultHealth = health;
                Health = health;
                AttackSpeed = attackSpeed;
                DodgeChance = dodgeChance;
                Armor = armor;
            }
            public override bool TryToBolck()
            {
                if (DodgeChance <= rand.NextDouble())
                {
                    return true;
                }
                return false;
            }

        }
        class Berserk : Participant //damage increases when HP decreases
        {
            double IncreaseDamage;
            public Berserk(string name, int damage = 17, int health = 125, double attackSpeed = 1, double armor = 0.3, double increaseDamage=0.5)
            {
                Name = name;
                DefoultDamage = damage;
                Damage = damage;
                DefoultHealth = health;
                Health = health;
                AttackSpeed = attackSpeed;
                Armor = armor;
                IncreaseDamage=increaseDamage;  
            }
            override public double FinalDamage() 
            {
                double damage = Damage * AttackSpeed + (IncreaseDamage * (DefoultHealth - Health));
                return damage;
            }
            override public void GettingDamage(double damage)
            {
                Health -= (int)Math.Round(damage * Armor, 0);
                if (Health < 0) { Health = 0; }
            }
        }
        //-------------------------------------end of the description of warrior classes----------------------------------------------------

        //---------------------------------------description of the buttons in the menu-----------------------------------------------------
        public static void Fight()
        {
            int diceResultOne = rand.Next(1, 6);
            int diceResultTwo = rand.Next(1,6);
            while (diceResultOne == diceResultTwo)
            {
                diceResultOne = rand.Next(1, 6);
            }
            int startpositionVertical = 1;
            int positionVertical = startpositionVertical;
            Console.Clear();
            Console.WriteLine("---------------Coose a fighters!-------------");
            foreach (var warrior in Warriors)
            {
                Console.SetCursorPosition(2, positionVertical++);
                Console.WriteLine( warrior.Name + " (" + warrior.GetType().Name + ")");
            }
            Console.SetCursorPosition(1, positionVertical);
            Console.Write("They're going to fight today: ...");
            int firstFighter = Checker(startpositionVertical, Warriors.Count);
            Console.SetCursorPosition("They're going to fight today: ...".Length -3, positionVertical);

            Console.Write(Warriors[firstFighter].Name.ToUpper() +" and ");
            int secondFighter = Checker(startpositionVertical, Warriors.Count);
            Console.SetCursorPosition("They're going to fight today: ".Length + Warriors[firstFighter].Name.Length + " and ".Length, positionVertical);
            Console.WriteLine(Warriors[secondFighter].Name.ToUpper() + "!");
            Console.WriteLine("In order for the battle to be fair, we will throw a dice to find out who will be the first to beat: ");
            Thread.Sleep(3000);
            Console.WriteLine(Warriors[firstFighter].Name + " has "+ diceResultOne);
            Console.Write(Warriors[secondFighter].Name + " has ");
            Thread.Sleep(5000);
            Console.WriteLine(diceResultTwo);
            if (diceResultTwo > diceResultOne)
            {
                int temp = firstFighter;
                firstFighter = secondFighter;
                secondFighter = temp;

            }
            Console.WriteLine("-----------------------BATLE BEGIN-----------------------");
            while (true)
            {
                bool isBlocked=false;
                if (Warriors[secondFighter].TryToBolck()) 
                {
                    Console.WriteLine(Warriors[secondFighter].Name+ " has avoided damage!");
                    Console.WriteLine("==========");
                    isBlocked =true;
                    Thread.Sleep(200);
                }
                if (!isBlocked)
                {
                    Warriors[secondFighter].GettingDamage(Warriors[firstFighter].FinalDamage());
                    Console.WriteLine(Warriors[secondFighter].Name +" getting hit. He has "+ Warriors[secondFighter].Health + " HP.");
                    Console.WriteLine("==========");
                    Thread.Sleep(200);
                }
                isBlocked = false;

                if (!Warriors[secondFighter].CheckHealth())
                {
                    Console.WriteLine(Warriors[firstFighter].Name+" is WINNER!!!");
                    break;
                }

                if (Warriors[firstFighter].TryToBolck())
                {
                    Console.WriteLine(Warriors[firstFighter].Name + " has avoided damage!");
                    Console.WriteLine("==========");
                    Thread.Sleep(200);
                    isBlocked = true;
                }
                if (!isBlocked)
                {
                    Warriors[firstFighter].GettingDamage(Warriors[secondFighter].FinalDamage());
                    Console.WriteLine(Warriors[firstFighter].Name + " getting hit. He has " + Warriors[firstFighter].Health + " HP.");
                    Console.WriteLine("==========");
                    Thread.Sleep(200);
                }
                isBlocked = false;

                if (!Warriors[firstFighter].CheckHealth())
                {
                    Console.WriteLine(Warriors[secondFighter].Name + " is WINNER!!!");
                    break;
                }

            }
            Warriors[firstFighter].Reset();
            Warriors[secondFighter].Reset();
            Console.WriteLine("press any key to continue...");
            Console.ReadLine();
            Console.Clear() ;
        }
        public static void CharacterCreation()
        {
            IEnumerable<Type> ListOfClass = Assembly.GetAssembly(typeOfParticipant).GetTypes().Where(type => type.IsSubclassOf(typeOfParticipant));
            List<Type> children= ListOfClass.ToList();
            int playerChouse = 0;
            string empty;
            ConsoleKeyInfo key;


            Console.Clear();
            Console.WriteLine("------------Сhoose your warrior class!----------");
            Console.Write("class: ");
            Console.Write("<");
            Console.SetCursorPosition(8, 1);
            Console.Write(ListOfClass.ElementAt(playerChouse).Name);
            Console.Write(">");
            while (true)
            {
                empty = new string(' ', ListOfClass.ElementAt(playerChouse).Name.Length + 1);
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (playerChouse == ListOfClass.Count() - 1)
                        {
                            Console.SetCursorPosition(8, 1);
                            Console.Write(empty);
                            playerChouse = 0;
                            Console.SetCursorPosition(8, 1);
                            Console.Write(ListOfClass.ElementAt(playerChouse).Name);
                            Console.Write(">");
                            continue;
                        }
                        Console.SetCursorPosition(8, 1);
                        Console.Write(empty);
                        playerChouse++;
                        Console.SetCursorPosition(8, 1);
                        Console.Write(ListOfClass.ElementAt(playerChouse).Name);
                        Console.Write(">");
                        continue;
                    case ConsoleKey.LeftArrow:
                        if (playerChouse == 0)
                        {
                            Console.SetCursorPosition(8, 1);
                            Console.Write(empty);
                            playerChouse = ListOfClass.Count() - 1;
                            Console.SetCursorPosition(8, 1);
                            Console.Write(ListOfClass.ElementAt(playerChouse).Name);
                            Console.Write(">");
                            continue;
                        }
                        Console.SetCursorPosition(8, 1);
                        Console.Write(empty);
                        playerChouse--;
                        Console.SetCursorPosition(8, 1);
                        Console.Write(ListOfClass.ElementAt(playerChouse).Name);
                        Console.Write(">");
                        continue;
                    case ConsoleKey.Enter:
                        empty = new string(' ', ListOfClass.ElementAt(playerChouse).Name.Length + 2);
                        Console.SetCursorPosition(7, 1);
                        Console.Write(empty);
                        Console.SetCursorPosition(7, 1);
                        Console.WriteLine("-//_" + ListOfClass.ElementAt(playerChouse).Name + "_//-");
                        break;
                    default: continue;
                }
                Console.Write("what is the name of your fighter? ");
                string newName = "";
                newName = Console.ReadLine();
                while (newName.Length == 0)
                {
                    Console.SetCursorPosition("what is the name of your fighter? ".Length, 2);
                    newName = Console.ReadLine();
                }
                switch (ListOfClass.ElementAt(playerChouse).Name)
                {
                    case "Knight":
                        Warriors.Add(new Knight(newName));
                        break;
                    case "Tank":
                        Warriors.Add(new Tank(newName));
                        break;
                    case "Theaf":
                        Warriors.Add(new Theaf(newName));
                        break;
                    case "Berserk":
                        Warriors.Add(new Berserk(newName));
                        break;
                    default:
                        Console.WriteLine("ERROR! Wrong class!");
                        continue;
                }
                Console.Clear();
                break;
            }


        }
        public static void ShowWarriors()
        {
            int positionVertical = 1;
            Console.Clear();
            foreach (var warrior in Warriors)
            {
                Console.SetCursorPosition(1, positionVertical++);
                Console.WriteLine(" > " + warrior.Name + " is " + warrior.GetType().Name);
            }
            Console.WriteLine("press any key to return to the menu...");
            Console.ReadKey();
            Console.Clear();

        }
        public static void Exit()
        {
            System.Environment.Exit(1);
        }
        //----------------------------------end of the description of the buttons in the menu-----------------------------------------------------

        //-------------------------------------------------description of the main menu-----------------------------------------------------
        public static void MainMenu() 
            {

            int startPos = 1;

            while (true)
            {

                DrawMainMenu(startPos);
                int j = Checker(startPos,Options.Count());
                switch (j)
                {
                    case 0:
                        Fight();
                        break;
                    case 1:
                        CharacterCreation();
                        break;
                    case 2:

                        ShowWarriors();
                        break;
                    case 3:
                        Exit();
                        break;
                        default: continue;
                }
            }
            }
        public static void DrawMainMenu(int startPos)//decoration
        {
            Console.SetCursorPosition(0, 0); Console.WriteLine("-----------------ARENA----------------");
            foreach (string option in Options)
            {
                Console.SetCursorPosition(2, startPos + Options.IndexOf(option));
                Console.WriteLine(option);
            }
        }
        //--------------------------------------------end of the description of the main menu-----------------------------------------------------


        //----------------------------------------------auxiliary method for drawing arrows-----------------------------------------------------
        public static int Checker(int startPos = 0, int count = 4, string arrow = "->")
        {
            string empty = new string(' ', arrow.Length);
            int i = startPos;
            Console.SetCursorPosition(0, startPos);
            Console.Write(arrow);
            ConsoleKeyInfo key;
            for (; ; )
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (i == count + startPos - 1)
                            continue;
                        Console.SetCursorPosition(0, i);
                        Console.Write(empty);
                        Console.SetCursorPosition(0, ++i);
                        Console.Write(arrow);
                        break;
                    case ConsoleKey.UpArrow:
                        if (i == startPos)
                            continue;
                        Console.SetCursorPosition(0, i);
                        Console.Write(empty);
                        Console.SetCursorPosition(0, --i);
                        Console.Write(arrow);
                        break;
                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, i);
                        Console.Write(empty);
                        return i - startPos;
                    default: continue; 
                }
            }
        }
        //----------------------------------------end of the auxiliary method for drawing arrows-----------------------------------------------------

    }
}
