using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceDemo_RaceCondition
{
    public class BankAccount
    {
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            Balance += amount;
        }
        public void WithDraw(int amount)
        {
            Balance -= amount;
        }
    }

    public class RaceConditionDemo
    {
        public void Run()
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Deposit(100);
                    }

                }));
            }

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.WithDraw(100);
                    }

                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Account Balance : {ba.Balance}");

        }
    }
}
