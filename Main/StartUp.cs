using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static List<Position> snakeBody;

    static Position[] directions = new Position[]
        {
            // Up
            new Position(-1, 0),
            // Right
            new Position(0, 1),
            // Down
            new Position(+1, 0),
            // Left
            new Position(0, -1)
        };

    static Position applePosition;

    static bool isAppleEaten = false;

    static int snakeLenght = 2;

    static bool isGameOver = false;

    static void Main()
    {
        Console.WriteLine("CHANGE CONSOLE FONT TO 'RASTER FONTS'");
        Console.WriteLine("PRESS ANY KEY TO START THE GAME");
        Console.ReadKey(true);

        while (true)
        {
            Console.ResetColor();
            Console.Clear();
            snakeLenght = 2;
            Console.Title = $"Snake Lenght: --- {snakeLenght} ---";
            snakeBody = new List<Position>();
            snakeBody.Add(new Position(0, 0));
            snakeBody.Add(new Position(0, 1));
            int movingDirection = 1;
            int delay = 300;

            Console.CursorVisible = false;
            Console.WindowHeight = 30;
            Console.WindowWidth = 70;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            Random random = new Random();
            applePosition.row = random.Next(Console.WindowHeight);
            applePosition.col = random.Next(Console.BufferWidth);

            while (isGameOver == false)
            {
                if (Console.KeyAvailable)
                {
                    int input = ReadUserInput();

                    // Clear input buffer
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                    // Check is it move backwards
                    if (movingDirection != Math.Abs(input - 2) && movingDirection != Math.Abs(input + 2))
                    {
                        movingDirection = input;
                    }
                }

                if (isAppleEaten)
                {
                    snakeLenght++;
                    Console.Title = $"Snake Lenght: --- {snakeLenght} ---";

                    if (delay > 70)
                    {
                        delay -= 10;
                    }
                    else if (delay > 45)
                    {
                        delay -= 1;
                    }
                    applePosition.row = random.Next(Console.WindowHeight);
                    applePosition.col = random.Next(Console.BufferWidth);
                    isAppleEaten = false;
                }

                Console.Clear();
                Console.SetCursorPosition(applePosition.col, applePosition.row);
                Console.Write('@');


                MoveSnake(movingDirection);
                Print(movingDirection);

                Thread.Sleep(delay);
            }

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition((Console.WindowWidth - 12) / 2, 0);
            Console.WriteLine("YOU ARE DEAD");
            Console.SetCursorPosition((Console.WindowWidth - 32) / 2, 0);
            Console.WriteLine("Press 'r' to restart 'q' to quit");

            var userInput = Console.ReadKey(true);

            if (userInput.Key == ConsoleKey.R)
            {
                isGameOver = false;
            }
            else if (userInput.Key == ConsoleKey.Q)
            {
                return;
            }
        }
    }

    static int ReadUserInput()
    {
        ConsoleKeyInfo keyPressed = Console.ReadKey(true);
        int movingDirection = -1;

        if (keyPressed.Key == ConsoleKey.W)
        {
            movingDirection = 0;
        }
        else if (keyPressed.Key == ConsoleKey.D)
        {
            movingDirection = 1;
        }
        else if (keyPressed.Key == ConsoleKey.S)
        {
            movingDirection = 2;
        }
        else if (keyPressed.Key == ConsoleKey.A)
        {
            movingDirection = 3;
        }

        return movingDirection;
    }

    static void Print(int direction)
    {
        for (int a = 0; a < snakeBody.Count - 1; a++)
        {
            Console.SetCursorPosition(snakeBody[a].col, snakeBody[a].row);
            Console.Write('*');
        }

        Console.SetCursorPosition(snakeBody[snakeBody.Count - 1].col, snakeBody[snakeBody.Count - 1].row);

        switch (direction)
        {
            case 0:
                Console.Write('^');
                break;
            case 1:
                Console.Write('>');
                break;
            case 2:
                Console.Write('v');
                break;
            case 3:
                Console.Write('<');
                break;
        }
    }

    static void MoveSnake(int direction)
    {
        Position nextPosition = new Position(snakeBody[snakeBody.Count - 1].row + directions[direction].row,
            snakeBody[snakeBody.Count - 1].col + directions[direction].col);

        if (CheckIsSnakeColide(nextPosition))
        {
            isGameOver = true;
            return;
        }
        else if (nextPosition.row == applePosition.row && nextPosition.col == applePosition.col)
        {
            isAppleEaten = true;
        }
        else
        {
            snakeBody.RemoveAt(0);
        }

        snakeBody.Add(nextPosition);
    }

    static bool CheckIsSnakeColide(Position nextPosition)
    {
        // Check is it colide with itself
        for (int a = 0; a < snakeBody.Count; a++)
        {
            if (snakeBody[a].row == nextPosition.row && snakeBody[a].col == nextPosition.col)
            {
                return true;
            }
        }

        if (nextPosition.row < 0 || nextPosition.col < 0 || nextPosition.col >= Console.BufferWidth || nextPosition.row >= Console.BufferHeight)
        {
            return true;
        }

        return false;
    }
}