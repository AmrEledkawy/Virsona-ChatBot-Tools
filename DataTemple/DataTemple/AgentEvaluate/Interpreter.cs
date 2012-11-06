using System;
using System.Collections.Generic;
using System.Text;
using LanguageNet.Grammarian;

namespace DataTemple.AgentEvaluate
{
    public class Interpreter
    {
        public static bool IsSpecial(char c)
        {
            return (c == '*' || c == '_' || c == '%' || c == '@' || c == '$' || c == '/' || c == '#');
        }

        public static Context ParseCommands(Context parent, string commands)
        {
            List<string> tokens = StringUtilities.SplitWords(commands, true);
            List<IContent> contents = new List<IContent>();

            for (int ii = 0; ii < tokens.Count; ii++)
            {
                int jj = 0;
                if (tokens[ii][0] == ' ')
                    jj = 1;

                if (IsSpecial(tokens[ii][jj]))
                {
                    if (tokens.Count > ii + 1 && tokens[ii + 1][0] == ' ')
                    {
                        // runs straight into something else!
                        if (tokens[ii + 1].Length > 1 && tokens[ii + 1][1] == tokens[ii][jj])
                        {
                            // It's just the character!
                            contents.Add(new Word(tokens[ii]));
                            // skip the duplicate
                            ii++;
                        }
                        else if (tokens[ii + 1].Length == 1 && tokens.Count > ii + 2 && char.IsLetterOrDigit(tokens[ii + 2][0]))
                        {
                            if (jj == 1 && ii != 0)
                                contents.Add(new Word(" "));  // no space!
                            // This is part of our name!
                            contents.Add(new Special(tokens[ii].Substring(jj) + tokens[ii + 2]));
                            // skip next two
                            ii += 2;
                        }
                        else
                            throw new Exception("Could not parse at token " + ii + ": " + commands);
                    }
                    else
                    {
                        string token = tokens[ii].Substring(jj);
                        if (jj == 1 && ii != 0)
                            contents.Add(new Word(" "));  // no space!
                        if (token == "/")
                            contents.Add(Special.ArgDelimSpecial);
                        else if (token == "#")
                            contents.Add(Special.EndDelimSpecial);
                        else
                            contents.Add(new Special(token));
                    }
                }
                else
                    contents.Add(new Word(tokens[ii]));
            }

            return new Context(parent, contents);
        }
    }
}
