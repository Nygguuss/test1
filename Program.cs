using System;

namespace ConsoleMazeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //definicja labiryntu jako dwuwymiarowej tablicy
            int[,] maze = GenerateMaze(9, 9);

            //wyświetlenie labiryntu
            Console.WriteLine("Labirynt:");
            PrintMaze(maze);

            //wybór punktu początkowego i końcowego
            Console.WriteLine("Podaj punkt początkowy (wiersz, kolumna):");
            int[] start = ParseCoordinates(Console.ReadLine());

            Console.WriteLine("Podaj punkt końcowy (wiersz, kolumna):");
            int[] end = ParseCoordinates(Console.ReadLine());

            //wyszukiwanie ścieżki za pomocą DFS
            bool[,] visited = new bool[maze.GetLength(0), maze.GetLength(1)];
            int[,] parent = new int[maze.GetLength(0), maze.GetLength(1)];

            bool foundPath = FindPath(maze, visited, parent, start[0], start[1], end[0], end[1]);

            //wyświetlenie ścieżki
            if (!foundPath)
            {
                Console.WriteLine("Nie znaleziono ścieżki!");
            }
            else
            {
                int[] currentNode = end;

                while (!IsEqual(currentNode, start))
                {
                    maze[currentNode[0], currentNode[1]] = 2; //oznaczenie ścieżki na labiryncie
                    currentNode = GetParent(parent, currentNode[0], currentNode[1]);
                }

                maze[start[0], start[1]] = 2; //oznaczenie punktu początkowego na labiryncie

                Console.WriteLine("Znaleziona ścieżka:");
                PrintMaze(maze);
            }

            Console.ReadLine();
            Console.ReadLine();
        }

        static int[,] GenerateMaze(int rows, int cols)
        {
            int[,] maze = new int[rows, cols];
            Random rand = new Random();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    maze[row, col] = rand.Next(2); //1 - ściana, 0 - wolne pole
                }
            }

            return maze;
        }

        static void PrintMaze(int[,] maze)
        {
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    if (maze[row, col] == 0)
                    {
                        Console.Write("  ");
                    }
                    else if (maze[row, col] == 1)
                    {
                        Console.Write("██");
                    }
                    else if (maze[row, col] == 2)
                    {
                        Console.Write("oo");
                    }
                }

                Console.WriteLine();
            }
        }

        static int[] ParseCoordinates(string input)
        {
            string[] tokens = input.Split(',');
            int[] coordinates = new int[2];

            coordinates[0] = int.Parse(tokens[0]) - 1; //kolejność wiersz, kolumna
            coordinates[1] = int.Parse(tokens[1]) - 1;

            return coordinates;
        }

        static bool FindPath(int[,] maze, bool[,] visited, int[,] parent, int row, int col, int endRow, int endCol)
        {
            if (row < 0 || col < 0 || row >= maze.GetLength(0) || col >= maze.GetLength(1))
            {
                return false; //przekroczenie granic labiryntu
            }

            if (maze[row, col] == 1 || visited[row, col])
            {
                return false; //ściana lub już odwiedzone pole
            }

            visited[row, col] = true;

            if (IsEqual(new int[] { row, col }, new int[] { endRow, endCol }))
            {
                return true; //znaleziono punkt końcowy
            }

            parent[row, col] = GetParentIndex(row, col, maze.GetLength(1));

            //rekurencyjne wywołanie dla sąsiednich pól
            if (FindPath(maze, visited, parent, row - 1, col, endRow, endCol) ||
                FindPath(maze, visited, parent, row, col - 1, endRow, endCol) ||
                FindPath(maze, visited, parent, row + 1, col, endRow, endCol) ||
                FindPath(maze, visited, parent, row, col + 1, endRow, endCol))
            {
                return true; //znaleziono ścieżkę
            }

            return false; //nie znaleziono ścieżki
        }

        static int GetParentIndex(int row, int col, int numCols)
        {
            return row * numCols + col;
        }

        static int[] GetParent(int[,] parent, int row, int col)
        {
            int parentIndex = parent[row, col];
            int numRows = parent.GetLength(0);
            int numCols = parent.GetLength(1);

            int parentRow = parentIndex / numCols;
            int parentCol = parentIndex % numCols;

            return new int[] { parentRow, parentCol };
        }

        static bool IsEqual(int[] a, int[] b)
        {
            return a[0] == b[0] && a[1] == b[1];
        }
    }
}
