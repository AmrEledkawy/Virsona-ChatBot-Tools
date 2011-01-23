/******************************************************************\
 *      Class Name:     PronounPossessive
 *      Written By:     James Rising
 *      Copyright:      2009, Virsona, Inc.
 *                      GNU Lesser General Public License, Ver. 3
 *                      (see license.txt and license.lesser.txt)
 *
 *                      Originally written by Jerome Scheuring
 *                      translated to C# and extended
 *      -----------------------------------------------------------
 * Encapsulates a possessive pronoun (like our)
\******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageNet.AgentParser
{
    public class PronounPossessive : POSPhrase
    {
        public PronounPossessive(string word)
            : base("PRP$", word)
        {
        }
    }
}
