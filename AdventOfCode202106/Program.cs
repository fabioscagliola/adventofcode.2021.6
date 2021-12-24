using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace com.fabioscagliola.AdventOfCode202106
{
    class Program
    {
        class Lanternfish
        {
            public event EventHandler LanternfishBorn;

            protected int internalTimer;

            public Lanternfish(int internalTimer)
            {
                this.internalTimer = internalTimer;
            }

            public void LiveOneDay()
            {
                internalTimer--;

                if (internalTimer == -1)
                {
                    internalTimer = 6;
                    LanternfishBorn?.Invoke(this, new EventArgs());
                }
            }
        }

        class LanternfishList : List<Lanternfish>
        {
            protected List<Lanternfish> babyLanternfishList;

            public LanternfishList(int[] initialStateList)
            {
                foreach (int initialState in initialStateList)
                {
                    Lanternfish lanternfish = new Lanternfish(initialState);
                    lanternfish.LanternfishBorn += LanternfishBornEventListener;
                    Add(lanternfish);
                }
            }

            protected void LanternfishBornEventListener(object sender, EventArgs e)
            {
                Lanternfish lanternfish = new Lanternfish(8);
                lanternfish.LanternfishBorn += LanternfishBornEventListener;
                babyLanternfishList.Add(lanternfish);
            }

            public int CountAfterDays(int days)
            {
                for (int i = 0; i < days; i++)
                {
                    babyLanternfishList = new List<Lanternfish>();

                    foreach (Lanternfish lanternfish in this)
                    {
                        lanternfish.LiveOneDay();
                    }

                    this.AddRange(babyLanternfishList);

                    Console.WriteLine($"[{DateTime.Now:s}] After day {i} there are {Count} lanternfish");
                }

                return Count;
            }
        }

        static void Main()
        {
            int[] initialStateList = File.ReadAllText("Input1.txt")
                .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList().Select(x => int.Parse(x)).ToArray();

            LanternfishList lanternfishList = new LanternfishList(initialStateList);

            Console.WriteLine($"After {Properties.Settings.Default.Days} days there would be {lanternfishList.CountAfterDays(Properties.Settings.Default.Days)} lanternfish");
        }

    }
}

