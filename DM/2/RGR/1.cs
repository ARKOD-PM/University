using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("input.txt");
        string[] nm = lines[0].Split();
        int N = int.Parse(nm[0]);
        int M = int.Parse(nm[1]);

        char[][] grid = new char[N][];
        int startX = -1, startY = -1;
        int endX = -1, endY = -1;
        List<Tuple<int, int>> minotaurs = new List<Tuple<int, int>>();

        for (int i = 0; i < N; i++)
        {
            grid[i] = lines[i + 1].ToCharArray();
            for (int j = 0; j < M; j++)
            {
                if (grid[i][j] == 'S')
                {
                    startX = i;
                    startY = j;
                }
                else if (grid[i][j] == 'F')
                {
                    endX = i;
                    endY = j;
                }
                else if (grid[i][j] == 'M')
                {
                    minotaurs.Add(Tuple.Create(i, j));
                }
            }
        }

        bool[,] blocked = new bool[N, M];
        foreach (var m in minotaurs)
        {
            int x = m.Item1, y = m.Item2;
            blocked[x, y] = true;
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            for (int d = 0; d < 4; d++)
            {
                int nx = x + dx[d], ny = y + dy[d];
                if (nx >= 0 && nx < N && ny >= 0 && ny < M)
                    blocked[nx, ny] = true;
            }
        }

        for (int i = 0; i < N; i++)
            for (int j = 0; j < M; j++)
                if (grid[i][j] == 'X')
                    blocked[i, j] = true;

        if (blocked[startX, startY] || blocked[endX, endY])
        {
            File.WriteAllText("output.txt", "Пути нет");
            return;
        }

        Queue<Tuple<int, int, int>> queue = new Queue<Tuple<int, int, int>>();
        bool[,] visited = new bool[N, M];
        queue.Enqueue(Tuple.Create(startX, startY, 0));
        visited[startX, startY] = true;

        int[] dirX = { -1, 1, 0, 0 };
        int[] dirY = { 0, 0, -1, 1 };
        int result = -1;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int x = current.Item1, y = current.Item2, dist = current.Item3;

            if (x == endX && y == endY)
            {
                result = dist;
                break;
            }

            for (int d = 0; d < 4; d++)
            {
                int nx = x + dirX[d], ny = y + dirY[d];
                if (nx >= 0 && nx < N && ny >= 0 && ny < M && !blocked[nx, ny] && !visited[nx, ny])
                {
                    visited[nx, ny] = true;
                    queue.Enqueue(Tuple.Create(nx, ny, dist + 1));
                }
            }
        }

        File.WriteAllText("output.txt", result == -1 ? "Пути нет" : result.ToString());
    }
}
