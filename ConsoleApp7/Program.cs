using System;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
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
            new BankCard("Kapital Bank", "Emin Agayev", "1112223334445555", "1234", "123", new DateTime(12 / 26), 1500));
            Client client4 = new Client(4, "Nihad", "Huseynli", 40, 4500,
            new BankCard("Kapital Bank", "Nihad Huseynli", "5554443332221111", "4321", "421", new DateTime(01 / 26), 3000));
            Client client5 = new Client(5, "Leyla", "Agayeva", 35, 2500,
            new BankCard("Kapital Bank", "Leyla Agayeva", "9876598765987659", "9876", "976", new DateTime(03 / 28), 1200));

            Bank bank = new Bank();
            bank.Clients = new Client[] { client1, client2, client3, client4, client5 };
            //bank.ShowAllClients();

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
                4.History
                5.Exit");
                        string choice = Console.ReadLine();
                        if (choice == "1")
                        {
                            System.Console.WriteLine($"Your balance: {currentUser.BankAccount.Balance}AZN");
                        }
                        else if (choice == "2")
                        {
                            
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

    }

    class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public BankCard BankAccount { get; set; }

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
        public BankCard[] Cards { get; set; }

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


    }



}
