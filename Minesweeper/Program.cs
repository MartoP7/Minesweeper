using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    class Program
    {   
        //global variables
        public static bool isRunning = true;
        public static bool isPlaying = false;
        public static int selection = 0;

        static void Main()
        {
            //2d arrays

            string[,] board = new string[8, 8];
            string[,] boardBombs = new string[8, 8];
            string[,] boardBombCount = new string[8, 8];
            string[,] board3x3 = new string[3, 3];

            //defaults

            string defaultBlock = "///";
            string testBlock = "tst";
            string flaggedBlock = "| # ";

            //bombs

            Random bomb = new Random();
            int bombsCount = 0;
            int bombCount3x3 = 0;

            //main menu

            MainMenu(bombsCount,board,boardBombCount,boardBombs,bomb,bombCount3x3,defaultBlock,flaggedBlock);

            //commands input

            while (isRunning)
            {
                Console.WriteLine();

                //command input
                string input = Console.ReadLine();
                string[] arr = input.Split();
                int selRow, selCol;

                //game commands
                if (arr[0] == "break")
                {
                    selRow = Convert.ToInt32(arr[1]) - 1;
                    selCol = Convert.ToInt32(arr[2]) - 1;

                    //update
                    if (board[selRow,selCol] == "#")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Cannot break flagged block!!!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (boardBombs[selRow, selCol] == "x")
                    { 
                        PrintBombs(boardBombs, defaultBlock);
                        Console.WriteLine("You lost!!!");
                        isRunning = false;             
                    }
                    else
                    {

                        Console.Beep(900, 3);
                        Console.Beep(700, 2);

                        Break(board, boardBombs, selRow, selCol, defaultBlock, flaggedBlock, testBlock, boardBombCount, bombCount3x3, board3x3);
                    }
                }
                else if (arr[0] == "flag")
                {
                    selRow = Convert.ToInt32(arr[1]) - 1;
                    selCol = Convert.ToInt32(arr[2]) - 1;

                    //set flagged box
                    if (board[selRow, selCol] != "#")
                    {
                        Console.Beep(4000, 3);
                        Console.Beep(2800, 2);
                        board[selRow, selCol] = "#";
                        Update(board, boardBombCount, bombCount3x3, defaultBlock, flaggedBlock);
                    }
                    //unset flagged box
                    else if (board[selRow, selCol] == "#")
                    {   
                        Console.Beep(2800, 2);
                        Console.Beep(4000, 3);
                        board[selRow, selCol] = "";
                        Update(board, boardBombCount, bombCount3x3, defaultBlock, flaggedBlock);
                    }
                    //flag error
                    else if (board[selRow, selCol] == "|1|" || board[selRow, selCol] == "|2|" || board[selRow, selCol] == "|3|" || board[selRow, selCol] == "|0|")
                    {
                        Console.WriteLine("Can't flag breaked block!");
                    }

                }
                else if (arr[0] == "end")
                {
                    isRunning = false;
                }
                else if (arr[0] == "restart")
                {
                    Restart(board,boardBombs,boardBombCount,board3x3,defaultBlock,flaggedBlock,bombCount3x3);
                }
                else if (arr[0] == "commands")
                {
                    PrintCommands();
                }
                else if (arr[0] == "menu")
                {
                    isPlaying = false;
                    selection = 0;
                    Console.Clear();
                    MainMenu(bombsCount, board, boardBombCount, boardBombs, bomb, bombCount3x3, defaultBlock, flaggedBlock);
                }

                //developer commands
                else if (arr[0] == "dev")
                {
                    if (arr[1] == "bombs")
                    {
                        PrintBombs(boardBombs,defaultBlock);
                    }
                    else if (arr[1] == "print")
                    {
                        selRow = Convert.ToInt32(arr[3]) - 1;
                        selCol = Convert.ToInt32(arr[4]) - 1;

                        Console.WriteLine($"board[{selRow},{selCol}] = '{board[selRow,selCol]}'");
                    }
                }

                //not exist
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Command does not exist! Use 'commands' to check available commands");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        private static void MainMenu(int bombsCount, string[,] board, string[,] boardBombCount, string[,] boardBombs,Random bomb, int bombCount3x3, string defaultBlock, string flaggedBlock)
        {
            //main menu
            while (!isPlaying && selection != 5)
            {
                PrintMainMenu(selection);

                Console.Write(" - ");
                selection = int.Parse(Console.ReadLine());

                PrintMainMenu(selection);
                if (selection > 0 && selection <= 4)
                {
                    for (int i = 0; i < new Random().Next(2, 6); i++)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\n               Generating");
                        for (int j = 0; j < 3; j++)
                        {
                            Thread.Sleep(200);
                            Console.Write(".");
                        }
                        PrintMainMenu(selection);
                    }
                    isPlaying = true;
                    
                }
                if (selection == 5)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n (?) Minesweeper is single-player logic-based computer game played on rectangular board whose object is to locate \n a " +
                        "predetermined number of randomly-placed mines in the shortest possible time by clicking on safe squares \n while avoiding the " +
                        "squares with mines. If the player clicks on a mine, the game ends.");
                    Console.WriteLine("\nUse 'menu'");
                    Console.ForegroundColor = ConsoleColor.White;
                    //Thread.Sleep(8000);
                }
            }
            if (isPlaying)
            {
                switch (selection)
                {
                    case 1: bombsCount = 6; break;
                    case 2: bombsCount = 12; break;
                    case 3: bombsCount = 18; break;
                    case 4: bombsCount = 20; break;
                }
                SpawnBombs(bombsCount, boardBombs, bomb);
                Update(board, boardBombCount, bombCount3x3, defaultBlock, flaggedBlock);
                //Console.WriteLine(bombsCount);
            }
        }
        private static void PrintMainMenu(int selection)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n          x ► MINESWEEPER ◄ x           ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("         ► BY MARTIN PAPAZOV ◄             ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(" ______________________________________ ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n       Select difficulty to start:          \n");
            Console.WriteLine("          ---------------------                 ");
            if (selection == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("           (1)     Easy          ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("          ---------------------                 ");
            if (selection == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("           (2)    Normal          ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("          ---------------------                 ");
            if (selection == 3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("           (3)     Hard          ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("          ---------------------                 ");
            if (selection == 4)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("           (4)   Hardcore          ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("          ---------------------                 ");
            if (selection == 5)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("           (5)  How to play          ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("          ---------------------                 ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(" ______________________________________ ");
            Console.ForegroundColor = ConsoleColor.White;

            //Console.WriteLine(@"                      __    __           __   __   __    __   __      ");
            //Console.WriteLine(@"    |\  /|  |  |\ |  |__   |__  \    /  |__  |__  |__|  |__  |__|     ");
            //Console.WriteLine(@"    | \/ |  |  | \|  |__    __|  \/\/   |__  |__  |     |__  | \      ");


        }
        private static void PrintCommands()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nbreak (row) (col) - breaks the entered block");
            Console.WriteLine("flag (row) (col) - puts a flag on the entered block");
            Console.WriteLine("restart - restarts the game");
            Console.WriteLine("commands - shows the list of commands");
            Console.WriteLine("menu - goes to the main menu");
            Console.WriteLine("end - ends the game");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("dev - parent command for developers (cheating!)");
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void Restart(string[,] board, string[,] boardBombs, string[,] boardBombCount, string[,] board3x3, string defaultBlock, string flaggedBlock, int bombCount3x3)
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = "";
                }
            }
            for (int row = 0; row < boardBombs.GetLength(0); row++)
            {
                for (int col = 0; col < boardBombs.GetLength(1); col++)
                {
                    boardBombs[row, col] = "";
                }
            }
            for (int row = 0; row < boardBombCount.GetLength(0); row++)
            {
                for (int col = 0; col < boardBombCount.GetLength(1); col++)
                {
                    boardBombCount[row, col] = "";
                }
            }
            for (int row = 0; row < board3x3.GetLength(0); row++)
            {
                for (int col = 0; col < board3x3.GetLength(1); col++)
                {
                    board3x3[row, col] = "";
                }
            }

            Update(board, boardBombCount, bombCount3x3, defaultBlock, flaggedBlock);
        }
        private static void SpawnBombs(int bombsCount, string[,] boardBombs, Random bomb)
        {
            for (int i = 0; i < bombsCount; i++)
            {
                int bombRow = bomb.Next(0, 8);
                int bombColumn = bomb.Next(0, 8);
                boardBombs[bombRow, bombColumn] = "x";
            }
        }
        static void Break(string[,] board, string[,] boardBombs, int selRow, int selCol, string defaultBlock, string flaggedBlock, string testBlock, string[,] boardBombCount, int bombCount3x3, string[,] board3x3)
        {
            AssingLocalBoard(board, boardBombs, bombCount3x3, board3x3, selRow, selCol);
            if (bombCount3x3 <= 3)
            {
                boardBombCount[selRow, selCol] = bombCount3x3.ToString();
                Update(board, boardBombCount, bombCount3x3, defaultBlock, flaggedBlock);

              /*Console.Clear();
                for (int row = 0; row < board.GetLength(0); row++)
                {
                    //print rows
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{row + 1} ");

                    for (int col = 0; col < board.GetLength(1); col++)
                    {
                        //boardBombCount[selRow, selCol] = bombCount3x3.ToString();

                        if (boardBombCount[row, col] != "1" && boardBombCount[row, col] != "2" && boardBombCount[row, col] != "3" && boardBombCount[row, col] != "0")
                        {
                            //board[selRow, selCol] = "";
                            if (board[row,col] != "#")
                            {
                                Console.Write(board[row, col] + "|");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(defaultBlock);
                            }
                            else
                            {
                                Console.Write(board[row, col] + "|");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(flaggedBlock);
                            }
                            

                        }
                        else if (boardBombCount[row, col] == "1" || boardBombCount[row, col] == "2" || boardBombCount[row, col] == "3" || boardBombCount[row, col] == "0")
                        {
                            //board[selRow, selCol] = "";

                            
                            if (board[row, col] != "#")
                            {
                                Console.Write(board[row, col] + "|");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($" {bombCount3x3.ToString()} ");
                            }
                            else
                            {
                                Console.Write(board[row, col] + "|");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(flaggedBlock);
                            }
                        }
                    }
                    Console.WriteLine("|");
                }
                Console.WriteLine("    1   2   3   4   5   6   7   8");*/
                
            }

        }
        private static void Update(string[,] board, string[,] boardBombCount, int bombCount3x3, string defaultBlock, string flaggedBlock)
        {
            Console.Clear();
            for (int row = 0; row < board.GetLength(0); row++)
            {
                //print rows
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{row + 1} ");

                for (int col = 0; col < board.GetLength(1); col++)
                {
                    //print default
                    if (board[row, col] != "#" && boardBombCount[row, col] != "1" && boardBombCount[row, col] != "2" && boardBombCount[row, col] != "3" && boardBombCount[row, col] != "0")
                    {

                        Console.Write(board[row, col] + "|");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(defaultBlock);

                    }
                    //print flag
                    else if (board[row, col] == "#")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(flaggedBlock);
                    }
                    //print break
                    else if (boardBombCount[row, col] == "1" || boardBombCount[row, col] == "2" || boardBombCount[row, col] == "3" || boardBombCount[row, col] == "0")
                    {
                        Console.Write(board[row, col] + "|");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($" {bombCount3x3.ToString()} ");
                    }
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("    1   2   3   4   5   6   7   8");
            Console.WriteLine(bombCount3x3);
        }
        private static void AssingLocalBoard(string[,] board, string[,] boardBombs, int bombCount3x3, string[,] board3x3, int selRow, int selCol)
        {
            //assing 3x3 board
            if (selRow > 0 && selCol > 0)
            {
                board3x3[0, 0] = boardBombs[selRow - 1, selCol - 1];
            }
            if (selCol > 0)
            {
                board3x3[1, 0] = boardBombs[selRow, selCol - 1];
            }

            if (selRow > 0 && selCol < 7)
            {
                board3x3[2, 0] = boardBombs[selRow - 1, selCol + 1];
            }
            if (selCol > 0)
            {
                board3x3[0, 1] = boardBombs[selRow, selCol - 1];
            }

            board3x3[1, 1] = "";

            if (selCol < 7)
            {
                board3x3[2, 1] = boardBombs[selRow, selCol + 1];
            }
            if (selRow < 7 && selCol > 0)
            {
                board3x3[0, 2] = boardBombs[selRow + 1, selCol - 1];
            }

            if (selCol < 7)
            {
                board3x3[1, 2] = boardBombs[selRow, selCol + 1];
            }
            if (selRow < 7 && selCol < 7)
            {
                board3x3[2, 2] = boardBombs[selRow + 1, selCol + 1];
            }

            //calculate 3x3 board bombs
            foreach (var box in board3x3)
            {
                if (box == "x")
                {
                    bombCount3x3++;
                }
            }

            //if (board[selRow,selCol] == "#")
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Can't break flagged block!!!");
            //    Console.ForegroundColor = ConsoleColor.White;
            //}
            //if (boardBombs[selRow, selCol] == "x")
            //{
            //    //if (board[selRow, selCol] != "#")
            //    //{
            //        PrintBombs(boardBombs,defaultBlock);
            //        Console.WriteLine("You lost!!!");
            //        isRunning = false;
            //    //}                
            //}
        }
        private static void PrintBombs(string[,] boardBombs, string defaultBlock)
        {
            Console.Clear();
            //print
            for (int row = 0; row < boardBombs.GetLength(0); row++)
            {
                //print rows
                Console.Write($"{row + 1} ");
                
                for (int col = 0; col < boardBombs.GetLength(1); col++)
                {
                    if (boardBombs[row, col] != "x")
                    {

                        Console.Write(boardBombs[row, col] + "|");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(defaultBlock);
                    }
                    else
                    {
                        Console.Write("|");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($" x ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("    1   2   3   4   5   6   7   8");
        }
    }
}
