/******************************************************************\
 *      Class Name:     Evaluator
 *      Written By:     James.R
 *      Copyright:      Virsona Inc
 *      -----------------------------------------------------------
 * Evaluates a string, passing any number of argument to a value
 *   succ and the remaining arguments to an after succ
\******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using PluggerBase.ActionReaction.Evaluations;
using DataTemple.Codeland;
using InOutTools;

namespace DataTemple.AgentEvaluate
{
    public class Evaluator : ContinueAgentCodelet
    {
        protected ArgumentMode argumentMode;
        protected IContinuation aftersucc;
		protected bool isUserInput;
        
        public Evaluator(double salience, ArgumentMode argumentMode, IContinuation valuesucc, IContinuation aftersucc, bool isUserInput)
            : base(salience, 2 * 4, 100, valuesucc)
        {
            this.argumentMode = argumentMode;
            this.aftersucc = aftersucc;
			this.isUserInput = isUserInput;
        }

        public override bool Evaluate()
        {
            List<IContent> contents = context.Contents;

            if (contents.Count == 0) {
                // nothing to do!
                succ.Continue(context, fail);
                aftersucc.Continue(new Context(context, null), fail);
                return true;
            }

            // Look ahead until first recursion
            int ii;
            for (ii = 0; ii < contents.Count; ii++)
                if ((contents[ii] is Special) && !(contents[ii].Name.StartsWith("*") || contents[ii].Name.StartsWith("_")))
                    break;

            if (ii > 0 && argumentMode == ArgumentMode.SingleArgument) {
                succ.Continue(new Context(context, contents.GetRange(0, 1)), fail);
                aftersucc.Continue(context.ChildRange(1), fail);
                return true;
            }

            if (ii == contents.Count)
            {
                // everything was done
                succ.Continue(context, fail);
                aftersucc.Continue(new Context(context, null), fail);
                return true;
            }

            if (contents[ii] == Special.EndDelimSpecial)
            {
                // everything to the # is done
                Context value = new Context(context, contents.GetRange(0, ii));
                Context after = new Context(context, contents.GetRange(ii + 1, contents.Count - ii - 1));
                succ.Continue(value, fail);
                aftersucc.Continue(after, fail);
                return true;
            }

            IContent content = contents[ii];
			
			object element = null;
			try {
	            element = context.Lookup(content.Name);
			} catch (Exception ex) {
				if (isUserInput)
					throw new UserException("The variable '" + content.Name + "' is unknown", ex);
				else
					throw ex;
			}
            if (element is CallAgent)
            {
                IContinuation mysucc = succ;
                if (ii > 0)
                    mysucc = new ContextAppender(salience, context, ii, succ);
                List<IContent> sublst = context.Contents.GetRange(ii + 1, context.Contents.Count - ii - 1);

                if (((CallAgent)element).ArgumentOptions == ArgumentMode.NoArugments)
                {
                    if (argumentMode == ArgumentMode.SingleArgument)
                    {
                        ContinueToCallAgent.Instantiate((CallAgent)element, new Context(context, null), mysucc, fail);
                        aftersucc.Continue(new Context(context, sublst), fail);
                        return true;
                    }
                    else
                    {
                        ContinuationAppender evalappend = new ContinuationAppender(context, mysucc);

                        ContinueToCallAgent.Instantiate((CallAgent)element, new Context(context, null), evalappend.AsIndex(0), fail);

                        Evaluator eval = new Evaluator(salience, ArgumentMode.ManyArguments, evalappend.AsIndex(1), aftersucc, isUserInput);
                        eval.Continue(new Context(context, sublst), fail);

                        return true;
                    }
                }
                else if (((CallAgent)element).ArgumentOptions == ArgumentMode.RemainderUnevaluated)
                {
                    ContinueToCallAgent.Instantiate((CallAgent) element, new Context(context, sublst), mysucc, fail);
                    aftersucc.Continue(new Context(context, new List<IContent>()), fail);
                    return true;
				} else if (((CallAgent)element).ArgumentOptions == ArgumentMode.DelimitedUnevaluated) {
					int jj;
		            for (jj = 0; jj < contents.Count; jj++)
        		        if (contents[jj] == Special.EndDelimSpecial)
                		    break;
					
					List<IContent> before = context.Contents.GetRange(ii + 1, jj - 1);
					List<IContent> after;
					if (jj < context.Contents.Count - 1)
						after = context.Contents.GetRange(jj + 1, context.Contents.Count - jj - 1);
					else
						after = new List<IContent>();
					
                    ContinuationAppender evalappend = new ContinuationAppender(context, mysucc);
					
                    ContinueToCallAgent.Instantiate((CallAgent) element, new Context(context, before), evalappend.AsIndex(0), fail);
                    Evaluator aftereval = new Evaluator(salience, ArgumentMode.ManyArguments, evalappend.AsIndex(1), aftersucc, isUserInput);
					aftereval.Continue(new Context(context, after), fail);
                    return true;
                } else {
                    if (argumentMode == ArgumentMode.SingleArgument)
                    {
                        ContinueToCallAgent callagent = new ContinueToCallAgent((CallAgent)element, mysucc);
                        Evaluator eval = new Evaluator(salience, ((CallAgent) element).ArgumentOptions, callagent, aftersucc, isUserInput);
                        eval.Continue(new Context(context, sublst), fail);
                        return true;
                    }
                    else
                    {
                        ContinuationAppender evalappend = new ContinuationAppender(context, mysucc);

                        ContinueToCallAgent callagent = new ContinueToCallAgent((CallAgent)element, evalappend.AsIndex(0));

                        Evaluator aftereval = new Evaluator(salience, ArgumentMode.ManyArguments, evalappend.AsIndex(1), aftersucc, isUserInput);

                        Evaluator eval = new Evaluator(salience, ((CallAgent) element).ArgumentOptions, callagent, aftereval, isUserInput);
                        eval.Continue(new Context(context, sublst), fail);
                        return true;
                    }
                }
            } else if (element is Codelet) {
                IContinuation mysucc = succ;
                if (ii > 0)
                    mysucc = new ContextAppender(salience, context, ii, succ);
                List<IContent> sublst = context.Contents.GetRange(ii + 1, context.Contents.Count - ii - 1);

                coderack.AddCodelet((Codelet) element, "Codelet in content");

                Evaluator eval = new Evaluator(salience, argumentMode, mysucc, aftersucc, isUserInput);
                eval.Continue(new Context(context, sublst), fail);
                return true;
            } else {
                Context result = new Context(context, new List<IContent>(context.Contents.GetRange(0, ii)));
                if (element is IContent)
                    result.Contents.Add((IContent) element);
                else if (element is List<IContent>)
                    result.Contents.AddRange((List<IContent>) element);
                else
                    result.Contents.Add(new Value(element));

                if (argumentMode == ArgumentMode.SingleArgument)
                {
                    succ.Continue(result, fail);
                    aftersucc.Continue(context.ChildRange(1), fail);
                    return true;
                }
                else
                {
                    IContinuation appender = new ContextAppender(salience, result, result.Contents.Count, succ);
                    List<IContent> sublst = context.Contents.GetRange(ii + 1, context.Contents.Count - ii - 1);

                    Evaluator eval = new Evaluator(salience, argumentMode, appender, aftersucc, isUserInput);
                    eval.Continue(new Context(context, sublst), fail);
                    return true;
                }
            }
		}
		
		public override string TraceTitle {
			get {
				return "Evaluator";
			}
		}
    }
}
