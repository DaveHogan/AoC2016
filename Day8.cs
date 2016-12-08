using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
    /*
     
        --- Day 8: Two-Factor Authentication ---

        You come across a door implementing what you can only assume is an implementation of two-factor
        authentication after a long game of requirements telephone.

        To get past the door, you first swipe a keycard (no problem; there was one on a nearby desk). 
        Then, it displays a code on a little screen, and you type that code on a keypad. Then, presumably, the door unlocks.

        Unfortunately, the screen has been smashed. After a few minutes, you've taken everything apart and 
        figured out how it works. Now you just have to work out what the screen would have displayed.

        The magnetic strip on the card you swiped encodes a series of instructions for the screen; these 
        instructions are your puzzle input. The screen is 50 pixels wide and 6 pixels tall, all of which 
        start off, and is capable of three somewhat peculiar operations:

        rect AxB turns on all of the pixels in a rectangle at the top-left of the screen which is A wide and B tall.
        rotate row y=A by B shifts all of the pixels in row A (0 is the top row) right by B pixels. Pixels 
        that would fall off the right end appear at the left end of the row.
        rotate column x=A by B shifts all of the pixels in column A (0 is the left column) down by B pixels. Pixels 
        that would fall off the bottom appear at the top of the column.
        For example, here is a simple sequence on a smaller screen:

        rect 3x2 creates a small rectangle in the top-left corner:

        ###....
        ###....
        .......
        rotate column x=1 by 1 rotates the second column down by one pixel:

        #.#....
        ###....
        .#.....
        rotate row y=0 by 4 rotates the top row right by four pixels:

        ....#.#
        ###....
        .#.....
        rotate column x=1 by 1 again rotates the second column down by one pixel, causing the bottom pixel to wrap back to the top:

        .#..#.#
        #.#....
        .#.....
        As you can see, this display technology is extremely powerful, and will soon dominate the tiny-code-displaying-screen market. That's what the advertisement on the back of the display tries to convince you, anyway.

        There seems to be an intermediate check of the voltage used by the display: after you swipe your card, if the screen did work, how many pixels should be lit?

    */
    public class Day8
    {
        public enum CommandType
        {
            Rect,
            Rotatecolumn,
            Rotaterow
        };

        public class Command
        {
            public CommandType Type { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int By { get; set; }

            public static Command Parse(string input)
            {
                var command = new Command();
                var commandParts = input.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                switch (commandParts[0])
                {
                    case "rect":
                        command.Type = CommandType.Rect;
                        command.X =
                            int.Parse(commandParts[1].Split("x".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0]);
                        command.Y = int.Parse(commandParts[1].Split("x".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
                        break;
                    case "rotate":
                        switch (commandParts[1])
                        {
                            case "column": // rotate column x = 11 by 2
                                command.Type = CommandType.Rotatecolumn;
                                command.X = int.Parse(commandParts[2].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
                                command.By = int.Parse(commandParts[4]);
                                break;
                            case "row": // rotate row y = 2 by 39
                                command.Type = CommandType.Rotaterow;
                                command.Y = int.Parse(commandParts[2].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
                                command.By = int.Parse(commandParts[4]);
                                break;
                            default:
                                throw new Exception("Unknown command");
                        }
                        break;
                    default:
                        throw new Exception("Unknown command");
                }
                return command;
            }

            public override string ToString()
            {
                return $"{this.Type}:{this.X}:{this.Y}:{this.By}";
            }
        }

        internal static void Execute()
        {
            var d = new Day8();
            d.Calculate();
        }

        private void Calculate()
        {
            var grid = new Boolean[50, 6];
            //var grid = new Boolean[7, 3];

            int x = grid.GetUpperBound(0);
            int y = grid.GetUpperBound(1);

            var commands =
                RealInput.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(Command.Parse);
            foreach (var command in commands)
            {
                Console.WriteLine("Applying command " + command.ToString());
                var newValues = new Dictionary<KeyValuePair<int, int>, bool>();

                if (command.Type == CommandType.Rect)
                {
                    for (var yi = 0; yi < command.Y; yi++)
                    {
                        for (var xi = 0; xi < command.X; xi++)
                        {
                            grid[xi, yi] = (grid[xi, yi] ? grid[xi, yi] == false : grid[xi, yi] = true);
                        }
                    }
                }
                else if (command.Type == CommandType.Rotatecolumn)
                {
                    /*
                    ###....            #.#....
                    ###....            ###....
                    .......            .#.....
                     */
                    var columnIndex = command.X;
                    var by = command.By;

                    for (var i = 0; i < by; i++)// Loop over x times (BY)
                    {
                        for (var yi = 0; yi <= y; yi++)
                        {

                            for (var xi = 0; xi <= x; xi++) // loop over item column starting from Command.X
                            {
                                if (xi == columnIndex)
                                {
                                    var pos = new KeyValuePair<int, int>(xi, yi);

                                    var nextyi = yi-1;
                                    if (nextyi < 0)
                                    {
                                        nextyi = y;
                                    }

                                    newValues.Add(pos, grid[xi, nextyi]);
                                }
                            }
                        }

                        ApplyMoves(ref newValues, ref grid);
                    }
                }
                else if (command.Type == CommandType.Rotaterow)
                {

                    var rowIndex = command.Y;
                    var by = command.By;

                    for (var i = 0; i < by; i++)// Loop over x times (BY)
                    {
                        for (var yi = 0; yi <= y; yi++)
                        {

                            for (var xi = 0; xi <= x; xi++) // loop over item column starting from Command.X
                            {
                                if (yi == rowIndex)
                                {
                                    var pos = new KeyValuePair<int, int>(xi, yi);

                                    var nextxi = xi - 1;
                                    if (nextxi < 0)
                                    {
                                        nextxi = x;
                                    }

                                    newValues.Add(pos, grid[nextxi, yi]);
                                }
                            }
                        }

                        ApplyMoves(ref newValues, ref grid);
                    }
                }

            }

            displayGrid(grid);

        }

        private static void ApplyMoves(ref Dictionary<KeyValuePair<int, int>, bool> newValues, ref bool[,] grid)
        {
            // apply all new moves
            foreach (var newValue in newValues)
            {
                var pos = newValue.Key;
                var posX = pos.Key;
                var posY = pos.Value;

                grid[posX, posY] = newValue.Value;
            }

            newValues = new Dictionary<KeyValuePair<int, int>, bool>();
        }

        private void displayGrid(bool[,] grid)
        {
            int x = grid.GetUpperBound(0);
            int y = grid.GetUpperBound(1);

            var total = 0;
            for (int yi = 0; yi <= y; yi++)
            {
                for (int xi = 0; xi <= x; xi++)
                {
                    if (grid[xi, yi]) // If pixel true
                        total++;
                    Console.Write(grid[xi, yi] ? "#" : ".");
                }
                Console.WriteLine();
            }


            Console.WriteLine($"Total of {total} pixels are lit");
        }

        public string DemoInput => @"rect 3x2
rotate column x=1 by 1
rotate row y=0 by 4
rotate column x=1 by 1";

        public string RealInput => @"rect 1x1
rotate row y=0 by 6
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 4
rect 2x1
rotate row y=0 by 5
rect 2x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 5
rect 4x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 6
rect 4x1
rotate row y=0 by 4
rotate column x=0 by 1
rect 3x1
rotate row y=0 by 6
rotate column x=0 by 1
rect 4x1
rotate column x=10 by 1
rotate row y=2 by 16
rotate row y=0 by 8
rotate column x=5 by 1
rotate column x=0 by 1
rect 7x1
rotate column x=37 by 1
rotate column x=21 by 2
rotate column x=15 by 1
rotate column x=11 by 2
rotate row y=2 by 39
rotate row y=0 by 36
rotate column x=33 by 2
rotate column x=32 by 1
rotate column x=28 by 2
rotate column x=27 by 1
rotate column x=25 by 1
rotate column x=22 by 1
rotate column x=21 by 2
rotate column x=20 by 3
rotate column x=18 by 1
rotate column x=15 by 2
rotate column x=12 by 1
rotate column x=10 by 1
rotate column x=6 by 2
rotate column x=5 by 1
rotate column x=2 by 1
rotate column x=0 by 1
rect 35x1
rotate column x=45 by 1
rotate row y=1 by 28
rotate column x=38 by 2
rotate column x=33 by 1
rotate column x=28 by 1
rotate column x=23 by 1
rotate column x=18 by 1
rotate column x=13 by 2
rotate column x=8 by 1
rotate column x=3 by 1
rotate row y=3 by 2
rotate row y=2 by 2
rotate row y=1 by 5
rotate row y=0 by 1
rect 1x5
rotate column x=43 by 1
rotate column x=31 by 1
rotate row y=4 by 35
rotate row y=3 by 20
rotate row y=1 by 27
rotate row y=0 by 20
rotate column x=17 by 1
rotate column x=15 by 1
rotate column x=12 by 1
rotate column x=11 by 2
rotate column x=10 by 1
rotate column x=8 by 1
rotate column x=7 by 1
rotate column x=5 by 1
rotate column x=3 by 2
rotate column x=2 by 1
rotate column x=0 by 1
rect 19x1
rotate column x=20 by 3
rotate column x=14 by 1
rotate column x=9 by 1
rotate row y=4 by 15
rotate row y=3 by 13
rotate row y=2 by 15
rotate row y=1 by 18
rotate row y=0 by 15
rotate column x=13 by 1
rotate column x=12 by 1
rotate column x=11 by 3
rotate column x=10 by 1
rotate column x=8 by 1
rotate column x=7 by 1
rotate column x=6 by 1
rotate column x=5 by 1
rotate column x=3 by 2
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 14x1
rotate row y=3 by 47
rotate column x=19 by 3
rotate column x=9 by 3
rotate column x=4 by 3
rotate row y=5 by 5
rotate row y=4 by 5
rotate row y=3 by 8
rotate row y=1 by 5
rotate column x=3 by 2
rotate column x=2 by 3
rotate column x=1 by 2
rotate column x=0 by 2
rect 4x2
rotate column x=35 by 5
rotate column x=20 by 3
rotate column x=10 by 5
rotate column x=3 by 2
rotate row y=5 by 20
rotate row y=3 by 30
rotate row y=2 by 45
rotate row y=1 by 30
rotate column x=48 by 5
rotate column x=47 by 5
rotate column x=46 by 3
rotate column x=45 by 4
rotate column x=43 by 5
rotate column x=42 by 5
rotate column x=41 by 5
rotate column x=38 by 1
rotate column x=37 by 5
rotate column x=36 by 5
rotate column x=35 by 1
rotate column x=33 by 1
rotate column x=32 by 5
rotate column x=31 by 5
rotate column x=28 by 5
rotate column x=27 by 5
rotate column x=26 by 5
rotate column x=17 by 5
rotate column x=16 by 5
rotate column x=15 by 4
rotate column x=13 by 1
rotate column x=12 by 5
rotate column x=11 by 5
rotate column x=10 by 1
rotate column x=8 by 1
rotate column x=2 by 5
rotate column x=1 by 5";
    }
}
