using System;
using System.Collections.Generic;
using System.Text;

namespace Resurrect.Us.Data.Services
{
    public class BaseXHashGenerationStrategy : IHashStrategy
    {
        public string EncodeHash(long input)
        {
            return this.Encode(input);
        }

        public long DecodeHash(string input)
        {
            return this.Decode(input);
        }

        public string Encode(long number)
        {
            string symbol = "";
            try
            {
                number = Math.Abs(number);
                int symbolLength = GetSymbolLength(number);
                char[] symbolArray = new char[symbolLength];
                //start with the highest symbol digit, and proceed down
                for (int i = symbolLength; i > 0; i--)
                {
                    //get the positional multiplier: 
                    //(e.g. in base10, the number 3 in 321 has a positional multiplier of 10^(3-1) = 100)
                    long positionalMultiplier = GetPositionalMultiplier(i);
                    //get the symbol digit by Div'ing the number by the positional base
                    //(e.g. in 321(base10), the first digit is 321 Div 100 = 3
                    var quotient = number / positionalMultiplier;
                    number = number % positionalMultiplier;
                    int symbolDigit = Convert.ToInt32(quotient);
                    //then convert to HumanBase32 symbol
                    symbolArray[symbolLength - i] = _encodingSymbols[symbolDigit];
                }
                symbol = new String(symbolArray);
            }
            catch
            {
                symbol = "(NAN)";
            }
            return symbol;
        }

        public long Decode(string symbol)
        {
            long number = 0;
            try
            {
                symbol = symbol.Trim().ToLower();
                int symbolLength = symbol.Length;
                //loop through the characters from left to right
                for (int i = symbolLength; i > 0; i--)
                {
                    //Get the character
                    char symbolChar = symbol[symbolLength - i];
                    //Decode the digit
                    int symbolDigit = _decodingSymbols[symbolChar];
                    //Get the positional multiplier for the digit
                    long positionalMultiplier = GetPositionalMultiplier(i);
                    //Add to the result
                    number += symbolDigit * positionalMultiplier;
                }
            }
            catch
            {
                number = -1;
            }
            return number;
        }

        public const int Base = 32;
        private static Dictionary<char, int> _decodingSymbols = new Dictionary<char, int>(Base);
        private static Dictionary<int, char> _encodingSymbols = new Dictionary<int, char>(Base);

        static BaseXHashGenerationStrategy()
        {
            FillDecodingAlphabet();
            FillEncodingAlphabet();
        }

        private static void FillEncodingAlphabet()
        {
            _encodingSymbols.Add(0, '0');
            _encodingSymbols.Add(1, '1');
            _encodingSymbols.Add(2, '2');
            _encodingSymbols.Add(3, '3');
            _encodingSymbols.Add(4, '4');
            _encodingSymbols.Add(5, '5');
            _encodingSymbols.Add(6, '6');
            _encodingSymbols.Add(7, '7');
            _encodingSymbols.Add(8, '8');
            _encodingSymbols.Add(9, '9');
            _encodingSymbols.Add(10, 'a');
            _encodingSymbols.Add(11, 'b');
            _encodingSymbols.Add(12, 'c');
            _encodingSymbols.Add(13, 'd');
            _encodingSymbols.Add(14, 'e');
            _encodingSymbols.Add(15, 'f');
            _encodingSymbols.Add(16, 'g');
            _encodingSymbols.Add(17, 'h');
            _encodingSymbols.Add(18, 'j');
            _encodingSymbols.Add(19, 'k');
            _encodingSymbols.Add(20, 'm');
            _encodingSymbols.Add(21, 'n');
            _encodingSymbols.Add(22, 'p');
            _encodingSymbols.Add(23, 'q');
            _encodingSymbols.Add(24, 'r');
            _encodingSymbols.Add(25, 's');
            _encodingSymbols.Add(26, 't');
            _encodingSymbols.Add(27, 'v');
            _encodingSymbols.Add(28, 'w');
            _encodingSymbols.Add(29, 'x');
            _encodingSymbols.Add(30, 'y');
            _encodingSymbols.Add(31, 'z');
        }

        private static void FillDecodingAlphabet()
        {
            _decodingSymbols.Add('0', 0);
            _decodingSymbols.Add('o', 0);
            _decodingSymbols.Add('1', 1);
            _decodingSymbols.Add('i', 1);
            _decodingSymbols.Add('l', 1);
            _decodingSymbols.Add('2', 2);
            _decodingSymbols.Add('3', 3);
            _decodingSymbols.Add('4', 4);
            _decodingSymbols.Add('5', 5);
            _decodingSymbols.Add('6', 6);
            _decodingSymbols.Add('7', 7);
            _decodingSymbols.Add('8', 8);
            _decodingSymbols.Add('9', 9);
            _decodingSymbols.Add('a', 10);
            _decodingSymbols.Add('b', 11);
            _decodingSymbols.Add('c', 12);
            _decodingSymbols.Add('d', 13);
            _decodingSymbols.Add('e', 14);
            _decodingSymbols.Add('f', 15);
            _decodingSymbols.Add('g', 16);
            _decodingSymbols.Add('h', 17);
            _decodingSymbols.Add('j', 18);
            _decodingSymbols.Add('k', 19);
            _decodingSymbols.Add('m', 20);
            _decodingSymbols.Add('n', 21);
            _decodingSymbols.Add('p', 22);
            _decodingSymbols.Add('q', 23);
            _decodingSymbols.Add('r', 24);
            _decodingSymbols.Add('s', 25);
            _decodingSymbols.Add('t', 26);
            _decodingSymbols.Add('v', 27);
            _decodingSymbols.Add('w', 28);
            _decodingSymbols.Add('x', 29);
            _decodingSymbols.Add('y', 30);
            _decodingSymbols.Add('z', 31);
        }

        private static int GetSymbolLength(long number)
        {
            return Convert.ToInt32(Math.Floor(Convert.ToDecimal(Math.Log(number, Base))) + 1);
        }

        private static long GetPositionalMultiplier(int i)
        {
            return Convert.ToInt64(Math.Pow(Base, i - 1));
        }
    }
}
