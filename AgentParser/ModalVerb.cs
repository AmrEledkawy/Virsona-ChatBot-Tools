/******************************************************************\
 *      Class Name:     ModalVerb
 *      Written By:     James Rising
 *      Copyright:      2009, Virsona, Inc.
 *                      GNU Lesser General Public License, Ver. 3
 *                      (see license.txt and license.lesser.txt)
 *
 *                      Originally written by Jerome Scheuring
 *                      translated to C# and extended
 *      -----------------------------------------------------------
 * Encapsulates a modal verb (like would).
\******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageNet.AgentParser
{
    public class ModalVerb : POSPhrase
    {
        public ModalVerb(string word)
            : base("MD", word)
        {
        }

        public override bool Transform(Sentence sentence)
        {
            List<Phrase> phrases = new List<Phrase>();
            phrases.Add(this);
            sentence.Combine(phrases, new VerbPhrase());
            return true;
        }
    }
}
