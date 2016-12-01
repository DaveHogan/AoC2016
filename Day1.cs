using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016
{
    public class Day1
    {
        public class Coords
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        internal static void Execute()
        {
            var d1 = new Day1();
            var steps = d1.ParseInput("R5, R4, R2, L3, R1, R1, L4, L5, R3, L1, L1, R4, L2, R1, R4, R4, L2, L2, R4, L4, R1, R3, L3, L1, L2, R1, R5, L5, L1, L1, R3, R5, L1, R4, L5, R5, R1, L185, R4, L1, R51, R3, L2, R78, R1, L4, R188, R1, L5, R5, R2, R3, L5, R3, R4, L1, R2, R2, L4, L4, L5, R5, R4, L4, R2, L5, R2, L1, L4, R4, L4, R2, L3, L4, R2, L3, R3, R2, L2, L3, R4, R3, R1, L4, L2, L5, R4, R4, L1, R1, L5, L1, R3, R1, L2, R1, R1, R3, L4, L1, L3, R2, R4, R2, L2, R1, L5, R3, L3, R3, L1, R4, L3, L3, R4, L2, L1, L3, R2, R3, L2, L1, R4, L3, L5, L2, L4, R1, L4, L4, R3, R5, L4, L1, L1, R4, L2, R5, R1, R1, R2, R1, R5, L1, L3, L5, R2");

            var result = d1.CalculateBlocksAway(steps);
            Console.WriteLine($"That's {result} blocks away");
        }

        private readonly Coords _pos = new Coords();
        private char _facing = 'N';

        private readonly List<Coords> _moveHistory = new List<Coords>();

        public int CalculateBlocksAway(string[] input)
        {
            foreach (var i in input)
            {
                PerformMove(i);
            }

            int distanceAway = Math.Abs(_pos.X) + Math.Abs(_pos.Y);
            return distanceAway;
        }

        private void PerformMove(string move)
        {
            move = move.Trim();
            var stepCount = int.Parse(move.Substring(1));
            if (stepCount < 1)
                return;

            // face the correct way
            if (move.StartsWith("R"))
            {
                switch (_facing)
                {
                    case 'N':
                        _facing = 'E';
                        break;
                    case 'E':
                        _facing = 'S';
                        break;
                    case 'S':
                        _facing = 'W';
                        break;
                    case 'W':
                        _facing = 'N';
                        break;
                }
            }
            else if (move.StartsWith("L"))
            {
                switch (_facing)
                {
                    case 'N':
                        _facing = 'W';
                        break;
                    case 'E':
                        _facing = 'N';
                        break;
                    case 'S':
                        _facing = 'E';
                        break;
                    case 'W':
                        _facing = 'S';
                        break;
                }
            }

            // move x times, step by step to capture history and when we visit the same block more than once.
            for (int i = 0; i < stepCount; i++)
            {
         
                switch (_facing)
                {
                    case 'N':
                        _pos.Y = _pos.Y - 1;
                        break;
                    case 'E':
                        _pos.X = _pos.X + 1;
                        break;
                    case 'S':
                        _pos.Y = _pos.Y + 1;
                        break;
                    case 'W':
                        _pos.X = _pos.X - 1;
                        break;
                }

                // Record where you been
                _moveHistory.Add(new Coords{X = _pos.X,Y = _pos.Y});

                // Shout if you've been here before
                if (_moveHistory.Count(c => c.X == _pos.X && c.Y == _pos.Y) > 1)
                {
                    Console.WriteLine($"Woooo. We found we've been here before {_pos.X}-{_pos.Y}. We're {Math.Abs(_pos.X) + Math.Abs(_pos.Y)} blocks away");
                }
            }
        }

        public string[] ParseInput(string rawInput)
        {
            return rawInput.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
