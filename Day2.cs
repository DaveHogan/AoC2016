﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
    public class Day2
    {
        public interface IKeypadNumber
        {
            int Number { get; }
            List<AvailableMove> AvailableMoves { get; }
        }

        public class AvailableMove
        {
            public Char Direction { get; set; }
            public IKeypadNumber NewNumber { get; set; }
        }
        public class NumberOne : IKeypadNumber
        {
            public int Number => 1;

            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberTwo() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberFour() }
                };
        }

        public class NumberTwo : IKeypadNumber
        {
            public int Number => 2;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberOne() },
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberThree() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberFive() }
                };
        }
    public class NumberThree : IKeypadNumber
        {
            public int Number => 3;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberTwo() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberSix() }
                };
        }
        public class NumberFour : IKeypadNumber
        {
            public int Number => 4;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberFive() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberSeven() },
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberOne() },
                };
        }
        public class NumberFive : IKeypadNumber
        {
            public int Number => 5;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberFour() },
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberSix() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberEight()},
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberTwo()  }
                };
        }
        public class NumberSix : IKeypadNumber
        {
            public int Number => 6;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberFive() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberNine()},
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberThree()  }
                };
        }

        public class NumberSeven : IKeypadNumber
        {
            public int Number => 7;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberEight() },
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberFour()  }
                };
        }

        public class NumberEight : IKeypadNumber
        {
            public int Number => 8;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberNine() },
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberSeven() },
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberFive()  }
                };
        }
        public class NumberNine : IKeypadNumber
        {
            public int Number => 9;
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() { 
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberEight() },
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberSix()  }
                };
        }

        public void Calculate(string[] inputs)
        {
            IKeypadNumber number = new NumberFive();
            
            foreach (var input in inputs)
            {
                number = CalculateNextNumber(number, input);
                Console.Write(number.Number);
            }
            

        }

        private IKeypadNumber CalculateNextNumber(IKeypadNumber number, string input)
        {
            foreach (var move in input.ToCharArray())
            {
                var nextPos = number.AvailableMoves.FirstOrDefault(c => c.Direction == move);
                if (nextPos != null)
                    number = nextPos.NewNumber;
            }
            return number;
        }

        internal static void Execute()
        {
            
            var input =
                @"LURLLLLLDUULRDDDRLRDDDUDDUULLRLULRURLRRDULUUURDUURLRDRRURUURUDDRDLRRLDDDDLLDURLDUUUDRDDDLULLDDLRLRRRLDLDDDDDLUUUDLUULRDUDLDRRRUDUDDRULURULDRUDLDUUUDLUDURUURRUUDRLDURRULURRURUUDDLRLDDDDRDRLDDLURLRDDLUDRLLRURRURRRURURRLLRLDRDLULLUDLUDRURDLRDUUDDUUDRLUDDLRLUDLLURDRUDDLRURDULLLUDDURULDRLUDLUDLULRRUUDDLDRLLUULDDURLURRRRUUDRUDLLDRUDLRRDUDUUURRULLDLDDRLUURLDUDDRLDRLDULDDURDLUUDRRLDRLLLRRRDLLLLURDLLLUDRUULUULLRLRDLULRLURLURRRDRLLDLDRLLRLULRDDDLUDDLLLRRLLLUURLDRULLDURDLULUDLRLDLUDURLLLURUUUDRRRULRDURLLURRLDLRLDLDRRUUDRDDDDDRDUUDULUL
RRURLURRULLUDUULUUURURULLDLRLRRULRUDUDDLLLRRRRLRUDUUUUDULUDRULDDUDLURLRRLLDLURLRDLDUULRDLLLDLLULLURLLURURULUDLDUDLUULDDLDRLRRUURRRLLRRLRULRRLDLDLRDULDLLDRRULRDRDUDUUUDUUDDRUUUDDLRDULLULDULUUUDDUULRLDLRLUUUUURDLULDLUUUULLLLRRRLDLLDLUDDULRULLRDURDRDRRRDDDLRDDULDLURLDLUDRRLDDDLULLRULDRULRURDURRUDUUULDRLRRUDDLULDLUULULRDRDULLLDULULDUDLDRLLLRLRURUDLUDDDURDUDDDULDRLUDRDRDRLRDDDDRLDRULLURUDRLLUDRLDDDLRLRDLDDUULRUDRLUULRULRLDLRLLULLUDULRLDRURDD
UUUUUURRDLLRUDUDURLRDDDURRRRULRLRUURLLLUULRUDLLRUUDURURUDRDLDLDRDUDUDRLUUDUUUDDURRRDRUDDUURDLRDRLDRRULULLLUDRDLLUULURULRULDRDRRLURULLDURUURDDRDLLDDDDULDULUULLRULRLDURLDDLULRLRRRLLURRLDLLULLDULRULLDLRULDDLUDDDLDDURUUUURDLLRURDURDUUDRULDUULLUUULLULLURLRDRLLRULLLLRRRRULDRULLUURLDRLRRDLDDRLRDURDRRDDDRRUDRLUULLLULRDDLDRRLRUDLRRLDULULRRDDURULLRULDUDRLRUUUULURLRLRDDDUUDDULLULLDDUDRLRDDRDRLDUURLRUULUULDUDDURDDLLLURUULLRDLRRDRDDDUDDRDLRRDDUURDUULUDDDDUUDDLULLDRDDLULLUDLDDURRULDUDRRUURRDLRLLDDRRLUUUDDUUDUDDDDDDDLULURRUULURLLUURUDUDDULURDDLRDDRRULLLDRRDLURURLRRRDDLDUUDR
URLLRULULULULDUULDLLRDUDDRRLRLLLULUDDUDLLLRURLLLLURRLRRDLULRUDDRLRRLLRDLRRULDLULRRRRUUDDRURLRUUDLRRULDDDLRULDURLDURLRLDDULURDDDDULDRLLUDRULRDDLUUUDUDUDDRRUDUURUURLUUULRLULUURURRLRUUULDDLURULRRRRDULUDLDRLLUURRRLLURDLDLLDUDRDRLLUDLDDLRLDLRUDUULDRRLLULDRRULLULURRLDLUUDLUDDRLURDDUDRDUDDDULLDRUDLRDLRDURUULRRDRUUULRUURDURLDUDRDLLRUULUULRDDUDLRDUUUUULDDDDDRRULRURLLRLLUUDLUDDUULDRULDLDUURUDUDLRULULUULLLLRLULUDDDRRLLDRUUDRLDDDRDDURRDDDULURDLDLUDDUULUUURDULDLLULRRUURDDUDRUULDLRLURUDLRDLLLDRLDUURUDUDRLLLDDDULLUDUUULLUUUDLRRRURRRRRDUULLUURRDUU
UDULUUDLDURRUDDUDRDDRRUULRRULULURRDDRUULDRLDUDDRRRRDLRURLLLRLRRLLLULDURRDLLDUDDULDLURLURUURLLLDUURRUUDLLLUDRUDLDDRLRRDLRLDDDULLRUURUUUDRRDLLLRRULDRURLRDLLUDRLLULRDLDDLLRRUDURULRLRLDRUDDLUUDRLDDRUDULLLURLRDLRUUDRRUUDUDRDDRDRDDLRULULURLRULDRURLURLRDRDUUDUDUULDDRLUUURULRDUDRUDRULUDDULLRDDRRUULRLDDLUUUUDUDLLLDULRRLRDDDLULRDUDRLDLURRUUDULUDRURUDDLUUUDDRLRLRLURDLDDRLRURRLLLRDRLRUUDRRRLUDLDLDDDLDULDRLURDURULURUDDDUDUULRLLDRLDDDDRULRDRLUUURD".Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Day2 d = new Day2();
            d.Calculate(input);
        }
    }
}
