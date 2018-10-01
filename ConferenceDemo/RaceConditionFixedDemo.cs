using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConferenceDemo_RaceConditionFixed
{
    public class BankAccount
    {
        private int _balance;
        public int Balance
        {
            get
            {
                return _balance;
            }
            private set { _balance = value; }
        }

        public void Deposit(int amount)
        {
            Interlocked.Add(ref _balance, amount);
            
        }
        public void WithDraw(int amount)
        {
            Interlocked.Add(ref _balance, -amount);
        }
    }
    class RaceConditionFixedDemo
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
