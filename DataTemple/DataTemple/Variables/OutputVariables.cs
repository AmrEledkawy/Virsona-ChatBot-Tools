using System;
using PluggerBase;
using PluggerBase.ActionReaction.Evaluations;
using LanguageNet.Grammarian;
using DataTemple.AgentEvaluate;
using DataTemple.Matching;

namespace DataTemple
{
	public class OutputVariables
	{
        public static void LoadVariables(Context env, double basesal, PluginEnvironment plugenv)
        {			
            env.Map.Add("@print", new CallAgentWrapper(PrintContents, ArgumentMode.ManyArguments, basesal, 4, 10, plugenv));
		}

        public static int PrintContents(Context context, IContinuation succ, IFailure fail, params object[] args)
        {
			PluginEnvironment plugenv = (PluginEnvironment) args[0];
			POSTagger tagger = new POSTagger(plugenv);
			GrammarParser parser = new GrammarParser(plugenv);
			
			Console.WriteLine(StarUtilities.ProducedCode(context, tagger, parser));
            return 10;
		}
	}
}

