using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class IncrementingBT
    {
        public void Run()
        {
            int size = 10;

            int max = 20;

            var arr = new int[size];

            // Assume 0 means None
            for (int i = 0; i < size; i++)
            {
                arr[i] = 0;
            }

            bool stop = false;

            do
            {
                stop = Increment(arr, max);

                /*
                for (int i = 0; i < size; i++)
                {
                    Console.Write(arr[i] + " ");
                }
                Console.WriteLine();
                */
            }
            while (!stop); // Add check for last

            Console.WriteLine("end!");
        }

        public bool Increment(int[] arr, int max)
        {
            for (int i = arr.Length-1; i >= 0; i--)
            {
                if (arr[i] == max)
                {
                    arr[i] = 0;
                }
                else
                {
                    arr[i]++;

                    if (i == 0 && arr[i] == max)
                    {
                        return true;
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
