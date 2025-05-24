using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int N = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(CountReconGroups(N));
    }

    static int CountReconGroups(int N)
    {
        List<int> queues = new List<int> { N };
        int groupsOfThree = 0;

        while (queues.Exists(queue => queue > 3))
        {
            List<int> newQueues = new List<int>();
            foreach (int queue in queues)
            {
                if (queue > 3)
                {
                    int even = (queue + 1) / 2;
                    int odd = queue / 2;
                    newQueues.Add(even);
                    newQueues.Add(odd);
                }
                else
                {
                    newQueues.Add(queue);
                }
            }
            queues = newQueues;
        }

        groupsOfThree = queues.FindAll(queue => queue == 3).Count;

        return groupsOfThree;
    }
}