using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
    public class Day2Part2
    {
        public interface IKeypadNumber
        {
            Char Number { get; }
            List<AvailableMove> AvailableMoves { get; }
        }

        public class AvailableMove
        {
            public Char Direction { get; set; }
            public IKeypadNumber NewNumber { get; set; }
        }
        public class NumberOne : IKeypadNumber
        {
            public Char Number => '1';

            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberThree() }
                };
        }

        public class NumberTwo : IKeypadNumber
        {
            public Char Number => '2';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberThree() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberSix() }
                };
        }

        public class NumberThree : IKeypadNumber
        {
            public Char Number => '3';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberOne() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberSeven() },
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberTwo() },
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberFour() }
                };
        }
        public class NumberFour : IKeypadNumber
        {
            public Char Number => '4';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberThree() },
                    new AvailableMove() { Direction = 'D', NewNumber = new NumberEight() },
                };
        }
        public class NumberFive : IKeypadNumber
        {
            public Char Number => '5';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberSix() },
                };
        }
        public class NumberSix : IKeypadNumber
        {
            public Char Number => '6';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberFive() },
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberSeven() },
                    new AvailableMove() { Direction = 'D', NewNumber = new LetterA()},
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberTwo()  }
                };
        }

        public class NumberSeven : IKeypadNumber
        {
            public Char Number => '7';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberSix() },
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberEight() },
                    new AvailableMove() { Direction = 'D', NewNumber = new LetterB()},
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberThree()  }
            };
        }

        public class NumberEight : IKeypadNumber
        {
            public Char Number => '8';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberSeven() },
                    new AvailableMove() { Direction = 'R', NewNumber = new NumberNine() },
                    new AvailableMove() { Direction = 'D', NewNumber = new LetterC()},
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberFour()  }
                };
        }
        public class NumberNine : IKeypadNumber
        {
            public Char Number => '9';
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new NumberEight() },
                };
        }

        public class LetterA : IKeypadNumber
        {
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'R', NewNumber = new LetterB() },
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberSix() }
                };

            public Char Number => 'A';

        }
        public class LetterB : IKeypadNumber
        {
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new LetterA() },
                    new AvailableMove() { Direction = 'R', NewNumber = new LetterC() },
                    new AvailableMove() { Direction = 'D', NewNumber = new LetterD() },
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberSeven() },

                };

            public Char Number => 'B';

        }
        public class LetterC : IKeypadNumber
        {
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'L', NewNumber = new LetterB() },
                    new AvailableMove() { Direction = 'U', NewNumber = new NumberEight() }
                };

            public Char Number => 'C';

        }
        public class LetterD : IKeypadNumber
        {
            public List<AvailableMove> AvailableMoves => new List<AvailableMove>() {
                    new AvailableMove() { Direction = 'U', NewNumber = new LetterB() }
                };

            public Char Number => 'D';

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

            Day2Part2 d = new Day2Part2();
            d.Calculate(input);
        }
    }
}
