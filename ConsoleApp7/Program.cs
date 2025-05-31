using System;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Swift;
using System.Security.Cryptography.X509Certificates;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            Client client1 = new Client(1, "Aysel", "Aliyeva", 25, 2000,
            new BankCard("Kapital Bank", "Aysel Aliyeva", "1234561234561234", "2520", "252", new DateTime(10 / 27), 1500));
            Client client2 = new Client(2, "Nezrin", "Xelilova", 30, 4000,
            new BankCard("Kapital Bank", "Nezrin Xelilova", "6543216543216543", "1309", "139", new DateTime(08 / 27), 2500));
            Client client3 = new Client(3, "Emin", "Agayev", 28, 3500,
            new BankCard("Leo Bank", "Emin Agayev", "1112223334445555", "1234", "123", new DateTime(12 / 26), 1500));
            Client client4 = new Client(4, "Nihad", "Huseynli", 40, 4500,
            new BankCard("Access Bank", "Nihad Huseynli", "5554443332221111", "4321", "421", new DateTime(01 / 26), 3000));
            Client client5 = new Client(5, "Leyla", "Agayeva", 35, 2500,
            new BankCard("Leo Bank", "Leyla Agayeva", "9876598765987659", "9876", "976", new DateTime(03 / 28), 1200));

            Bank bank = new Bank();
            bank.Clients = new Client[] { client1, client2, client3, client4, client5 };

            Client currentUser = null;

            while (true)
            {
                System.Console.WriteLine("Enter card PIN: ");
                string pin = Console.ReadLine();
                currentUser = bank.FindUserWithPin(pin);

                if (currentUser == null)
                {
                    System.Console.WriteLine("This user not found. Please try again...");
                }
                else
                {
                    System.Console.WriteLine($"Welcome: {currentUser.Name} {currentUser.Surname}!");
                    while (true)
                    {
                        System.Console.WriteLine(@$"Enter your choice:
                1.Show balance
                2.Withdraw
                3.Transfer money
                4.Show all users
                5.History
                6.Exit");
                        string choice = Console.ReadLine();
                        if (choice == "1")
                        {
                            System.Console.WriteLine($"Your balance: {currentUser.BankAccount.Balance}AZN");
                        }
                        else if (choice == "2")
                        {
                            System.Console.WriteLine(@"Choose an amount: 
                            1. 10 AZN
                            2. 20 AZN
                            3. 50 AZN
                            4. 100 AZN
                            5. Other");

                            string withdrawChoice = Console.ReadLine();
                            float amount = 0;
                            switch (withdrawChoice)
                            {
                                case "1":
                                    amount = 10;
                                    break;

                                case "2":
                                    amount = 20;
                                    break;

                                case "3":
                                    amount = 50;
                                    break;

                                case "4":
                                    amount = 100;
                                    break;

                                case "5":
                                    System.Console.WriteLine("Enter amount:");

                                    if (!float.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                                    {
                                        Console.WriteLine("Wrong input,try again..");
                                        continue;
                                    }
                                    break;
                                default:
                                    System.Console.WriteLine("This choice is wrong..");
                                    continue;
                            }
                            try
                            {

                                currentUser.BankAccount.Withdraw(amount);
                                string historyEntry = $"{DateTime.Now}: Withdrawn {amount} AZN";
                                currentUser.History.Add(historyEntry);
                                Console.WriteLine(historyEntry);
                                System.Console.WriteLine($"{amount} AZN was successfully withdrawn from the card");
                                System.Console.WriteLine($"Current balance: {currentUser.BankAccount.Balance} AZN");
                            }

                            catch (Exception ex)
                            {
                                System.Console.WriteLine($"Error: {ex.Message}");
                            }
                        }

                        else if (choice == "3")
                        {
                            System.Console.WriteLine("Enter cart pin to which you want to transfer money: ");
                            string CartPin = Console.ReadLine();
                            Client receiver = bank.FindUserWithPin(CartPin);

                            if (receiver == null || receiver == currentUser)
                            {
                                System.Console.WriteLine("Wrong input,you can't tarnsfer money to your cart");
                                continue;
                            }

                            System.Console.WriteLine("Enter amount of money which you want to transfer: ");
                            if (!double.TryParse(Console.ReadLine(), out double transferAmount) || transferAmount <= 0)
                            {
                                Console.WriteLine("This amount is wrong..");
                                continue;
                            }
                            if (transferAmount > currentUser.BankAccount.Balance)
                            {
                                System.Console.WriteLine("You don't have enough money to reveice");
                            }
                            else
                            {
                                currentUser.BankAccount.Balance -= transferAmount;
                                receiver.BankAccount.Balance += transferAmount;

                                string sendEntry = $"{DateTime.Now}: Sent {transferAmount} AZN to {receiver.Name} {receiver.Surname}'s new balance: {receiver.BankAccount.Balance}";
                                string receiveEntry = $"{DateTime.Now}: Received {transferAmount} AZN from {currentUser.Name} {currentUser.Surname}'s new balance: {currentUser.BankAccount.Balance}";

                                currentUser.History.Add(sendEntry);
                                receiver.History.Add(receiveEntry);

                                Console.WriteLine(sendEntry);
                                System.Console.WriteLine(receiveEntry);


                            }
                        }

                        else if (choice == "4")
                        {
                            bank.ShowClientsWithCursor();
                        }

                        else if (choice == "5")
                        {
                            if (currentUser.History.Count == 0)
                            {
                                Console.WriteLine("No operations yet.");
                            }
                            else
                            {
                                Console.WriteLine("Your operation history:");
                                foreach (var entry in currentUser.History)
                                {
                                    Console.WriteLine(entry);
                                }
                            }
                        }

                        else if (choice == "6")
                        {
                            System.Console.WriteLine("Log out from bank system");
                            return;
                        }
                        else
                        {
                            System.Console.WriteLine("This choice does not exit..");
                        }

                    }
                }

            }

        }
    }

    class BankCard
    {
        public string BankName { get; set; }
        public string FullName { get; set; }
        public string PAN { get; set; }
        public string PIN { get; set; }
        public string CVC { get; set; }
        public DateTime ExpireDate { get; set; }
        public double Balance { get; set; }

        public BankCard(string bankname, string fullname, string pan, string pin, string cvc, DateTime expiredate, double balance)
        {
            BankName = bankname;
            FullName = fullname;
            PAN = pan;
            PIN = pin;
            CVC = cvc;
            ExpireDate = expiredate;
            Balance = balance;
        }

        public void Withdraw(double amount)
        {
            if (amount > Balance)
            {
                throw new ArgumentOutOfRangeException("Not enough balance in your card..");
            }

            Balance -= amount;
        }

    }

    class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public BankCard BankAccount { get; set; }

public List<string> History { get; set; } = new List<string>();

        public Client(int id, string name, string surname, int age, double salary, BankCard bankaccount)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Age = age;
            Salary = salary;
            BankAccount = bankaccount;
        }

        public override string ToString()
        {
            return $@"Fullname: {Name} {Surname}
Age: {Age}
Salary: {Salary}
Balance: {BankAccount.Balance} AZN";
        }

    }

    class Bank
    {
        public Client[] Clients { get; set; }

        public void ShowAllClients()
        {
            for (int i = 0; i < Clients.Length; i++)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(Clients[i]);
                System.Console.WriteLine("__________________________________");

            }
        }

        public Client FindUserWithPin(string pin)
        {
            for (int i = 0; i < Clients.Length; i++)
            {
                if (Clients[i].BankAccount.PIN == pin)
                {
                    return Clients[i];
                }
            }
            return null;
        }


public void ShowClientsWithCursor()
        {
            if (Clients == null || Clients.Length == 0)
            {
                Console.WriteLine("No clients available.");
                return;
            }

            int cursorPos = 0;
            ConsoleKeyInfo keyInfo;

            do
            {
                Console.Clear();
                Console.WriteLine("Use Up/Down arrows to move, ESC to exit:");

                for (int i = 0; i < Clients.Length; i++)
                {
                    if (i == cursorPos)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {Clients[i].Name} {Clients[i].Surname}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {Clients[i].Name} {Clients[i].Surname}");
                    }
                }

                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    cursorPos--;
                    if (cursorPos < 0) cursorPos = Clients.Length - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    cursorPos++;
                    if (cursorPos >= Clients.Length) cursorPos = 0;
                }

            } while (keyInfo.Key != ConsoleKey.Escape);

            Console.Clear();
            Console.WriteLine("Selected client:");
            Console.WriteLine(Clients[cursorPos]);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

    }

}
