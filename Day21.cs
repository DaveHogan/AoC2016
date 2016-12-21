using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2016
{

    /*
        --- Day 21: Scrambled Letters and Hash ---

        The computer system you're breaking into uses a weird scrambling function to store its passwords. 
        It shouldn't be much trouble to create your own scrambled password so you can add it to the system; you just have to implement the scrambler.

        The scrambling function is a series of operations (the exact list is provided in your puzzle input). 
        Starting with the password to be scrambled, apply each operation in succession to the string. The individual operations behave as follows:

        swap position X with position Y means that the letters at indexes X and Y (counting from 0) should be swapped.
        swap letter X with letter Y means that the letters X and Y should be swapped (regardless of where they appear in the string).
        rotate left/right X steps means that the whole string should be rotated; for example, one right rotation would turn abcd into dabc.
        rotate based on position of letter X means that the whole string should be rotated to the right based on the index of 
        letter X (counting from 0) as determined before this instruction does any rotations. Once the index is determined, rotate the 
        string to the right one time, plus a number of times equal to that index, plus one additional time if the index was at least 4.
        reverse positions X through Y means that the span of letters at indexes X through Y (including the letters at X and Y) should be reversed in order.
        move position X to position Y means that the letter which is at index X should be removed from the string, then inserted such that it ends up at index Y.
        For example, suppose you start with abcde and perform the following operations:

        swap position 4 with position 0 swaps the first and last letters, producing the input for the next step, ebcda.
        swap letter d with letter b swaps the positions of d and b: edcba.
        reverse positions 0 through 4 causes the entire string to be reversed, producing abcde.
        rotate left 1 step shifts all letters left one position, causing the first letter to wrap to the end of the string: bcdea.
        move position 1 to position 4 removes the letter at position 1 (c), then inserts it at position 4 (the end of the string): bdeac.
        move position 3 to position 0 removes the letter at position 3 (a), then inserts it at position 0 (the front of the string): abdec.
        rotate based on position of letter b finds the index of letter b (1), then rotates the string right once plus a number of times equal to that index (2): ecabd.
        rotate based on position of letter d finds the index of letter d (4), then rotates the string right once, plus a number of times equal to that index, plus an additional time because the index was at least 4, for a total of 6 right rotations: decab.
        After these steps, the resulting scrambled password is decab.

        Now, you just need to generate a new scrambled password and you can access the system. Given the list of scrambling operations in your puzzle input, what is the result of scrambling abcdefgh?
    */
    public class Day21
    {
        public class Command
        {
            public string CommandType { get; private set; }
            public string LetterA { get; private set; }
            public string LetterB { get; private set; }
            public int NumberA { get; private set; }
            public int NumberB { get; private set; }
            public string Raw { get; private set; }

            public static Command Parse(string input)
            {
                var command = new Command();
                var commandParts = input.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                command.CommandType = commandParts[0] + commandParts[1];
                command.Raw = input;
                switch (command.CommandType)
                {
                    case "swapletter":
                        command.LetterA = commandParts[2];
                        command.LetterB = commandParts[5];
                        break;
                    case "swapposition":
                        command.NumberA = int.Parse(commandParts[2]);
                        command.NumberB = int.Parse(commandParts[5]);
                        break;
                    case "reversepositions":
                        command.NumberA = int.Parse(commandParts[2]);
                        command.NumberB = int.Parse(commandParts[4]);
                        break;
                    case "rotateleft":
                    case "rotateright":
                        command.NumberA = int.Parse(commandParts[2]);
                        break;
                    case "moveposition":
                        command.NumberA = int.Parse(commandParts[2]);
                        command.NumberB = int.Parse(commandParts[5]);
                        break;
                    case "rotatebased":
                        command.LetterA = commandParts[6];
                        break;
                    default:
                        throw new NotImplementedException();
                }

                return command;
            }

            internal string Execute(string output)
            {
                switch (CommandType)
                {
                    case "swapletter":
                        var letterAPos = output.IndexOf(LetterA, StringComparison.Ordinal);
                        var letterBPos = output.IndexOf(LetterB, StringComparison.Ordinal);
                        output = output.Remove(letterBPos, 1);
                        output = output.Insert(letterBPos, LetterA);
                        output = output.Remove(letterAPos, 1);
                        return output.Insert(letterAPos, LetterB);
                    case "swapposition":
                        var letterAtPosA = output.Substring(NumberA, 1);
                        var letterAtPosB = output.Substring(NumberB, 1);
                        output = output.Remove(NumberA, 1);
                        output = output.Insert(NumberA, letterAtPosB);
                        output = output.Remove(NumberB, 1);
                        return output.Insert(NumberB, letterAtPosA);
                    case "reversepositions":
                        var length = (NumberB - NumberA) + 1;
                        var toReverse = output.Substring(NumberA, length);
                        var reverseString = string.Join("", toReverse.Reverse());
                        output = output.Remove(NumberA, length);
                        return output.Insert(NumberA, reverseString);
                    case "rotateleft":
                        var chars = output.ToCharArray().Take(NumberA);
                        output = output.Remove(0, NumberA);
                        output = output + string.Join("", chars);
                        return output;
                    case "rotateright":
                        for (int i = 0; i < NumberA; i++)
                        {
                            output = RotateRight(output);
                        }
                        return output;
                    case "moveposition":
                        var charToMove = output.Substring(NumberA, 1);
                        output = output.Remove(NumberA, 1);
                        output = output.Insert(NumberB, charToMove);
                        return output;
                    case "rotatebased":
                        var pos = output.IndexOf(LetterA, StringComparison.Ordinal);
                        var timesToRotate = 1 + pos;
                        if (pos >= 4)
                        {
                            timesToRotate++;
                        }
                        for (int i = 0; i < timesToRotate; i++)
                        {
                            output = RotateRight(output);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return output;
            }

            private string RotateRight(string output)
            {
                var endChars = output.ToCharArray().Skip(output.Length - 1);
                output = output.Remove(output.Length - 1, 1);
                output = string.Join("", endChars) + output;
                return output;
            }
        }

        internal static void Execute()
        {
            var d = new Day21();

            var demoResult = d.Calculate("abcde", DemoInput);
            Console.WriteLine($"DEMO OUTPUT: {demoResult}");

            var realResult = d.Calculate("abcdefgh", RealInput);
            Console.WriteLine($"REAL OUTPUT: {realResult}");
        }

        private string Calculate(string input, string commands)
        {

            List<Command> _commands = new List<Command>();

            var lines = commands.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                _commands.Add(Command.Parse(line));
            }
            var output = input;
            foreach (var command in _commands)
            {
                var old = output;
                output = command.Execute(output);
                
                Console.WriteLine($"{command.Raw}: {old} = {output}");
            }

            return output;
        }


        public static string DemoInput = @"swap position 4 with position 0
swap letter d with letter b
reverse positions 0 through 4
rotate left 1
move position 1 to position 4
move position 3 to position 0
rotate based on position of letter b
rotate based on position of letter d";


        public static String RealInput = @"rotate left 2 steps
rotate right 0 steps
rotate based on position of letter a
rotate based on position of letter f
swap letter g with letter b
rotate left 4 steps
swap letter e with letter f
reverse positions 1 through 6
swap letter b with letter d
swap letter b with letter c
move position 7 to position 5
rotate based on position of letter h
swap position 6 with position 5
reverse positions 2 through 7
move position 5 to position 0
rotate based on position of letter e
rotate based on position of letter c
rotate right 4 steps
reverse positions 3 through 7
rotate left 4 steps
rotate based on position of letter f
rotate left 3 steps
swap letter d with letter a
swap position 0 with position 1
rotate based on position of letter a
move position 3 to position 6
swap letter e with letter g
move position 6 to position 2
reverse positions 1 through 2
rotate right 1 step
reverse positions 0 through 6
swap letter e with letter h
swap letter f with letter a
rotate based on position of letter a
swap position 7 with position 4
reverse positions 2 through 5
swap position 1 with position 2
rotate right 0 steps
reverse positions 5 through 7
rotate based on position of letter a
swap letter f with letter h
swap letter a with letter f
rotate right 4 steps
move position 7 to position 5
rotate based on position of letter a
reverse positions 0 through 6
swap letter g with letter c
reverse positions 5 through 6
reverse positions 3 through 5
reverse positions 4 through 6
swap position 3 with position 4
move position 4 to position 2
reverse positions 3 through 4
rotate left 0 steps
reverse positions 3 through 6
swap position 6 with position 7
reverse positions 2 through 5
swap position 2 with position 0
reverse positions 0 through 3
reverse positions 3 through 5
rotate based on position of letter d
move position 1 to position 2
rotate based on position of letter c
swap letter e with letter a
move position 4 to position 1
reverse positions 5 through 7
rotate left 1 step
rotate based on position of letter h
reverse positions 1 through 7
rotate based on position of letter f
move position 1 to position 5
reverse positions 1 through 4
rotate based on position of letter a
swap letter b with letter c
rotate based on position of letter g
swap letter a with letter g
swap position 1 with position 0
rotate right 2 steps
rotate based on position of letter f
swap position 5 with position 4
move position 1 to position 0
swap letter f with letter b
swap letter f with letter h
move position 1 to position 7
swap letter c with letter b
reverse positions 5 through 7
rotate left 6 steps
swap letter d with letter b
rotate left 3 steps
swap position 1 with position 4
rotate based on position of letter a
rotate based on position of letter a
swap letter b with letter c
swap letter e with letter f
reverse positions 4 through 7
rotate right 0 steps
reverse positions 2 through 3
rotate based on position of letter a
reverse positions 1 through 4
rotate right 1 step";


    }
}
