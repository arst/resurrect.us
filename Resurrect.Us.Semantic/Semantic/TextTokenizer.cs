﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Resurrect.Us.Semantic.Semantic
{
    class TextTokenizer
    {
        private static char[] delimiters = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', ' ', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t'};
        private static int tokenizingMinThreshold = 3;

        public List<string> Tokenize(string tokenizeTarget)
        {
            string[] tokens = tokenizeTarget.Split(delimiters,
                                    StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (token.Length > tokenizingMinThreshold)
                {
                    if (token.StartsWith("'") && token.EndsWith("'"))
                        tokens[i] = token.Substring(1, token.Length - 2);

                    else if (token.StartsWith("'"))
                        tokens[i] = token.Substring(1);

                    else if (token.EndsWith("'"))
                        tokens[i] = token.Substring(0, token.Length - 1);
                }
            }

            return tokens.ToList();
        }
    }
}
