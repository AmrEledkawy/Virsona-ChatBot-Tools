using System;
using System.Collections.Generic;
using System.Text;
using DataTemple.Codeland;
using PluggerBase.ActionReaction.Evaluations;
using GenericTools;

namespace DataTemple.AgentEvaluate
{
    public class ContinueAgentCodelet : RecipientCodelet<TwoTuple<Context, IFailure>>, IContinuation
    {
        protected IContinuation succ;

        // Filled in when continued to
        protected Context context;
        protected IFailure fail;

        protected string lineage;

        public ContinueAgentCodelet(double salience, int space, int time, IContinuation succ)
            : base(null, salience, space + 4 * 4, time + 1)
        {
            this.succ = succ;
        }

        public IContinuation Success
        {
            get
            {
                return succ;
            }
            set
            {
                succ = value;
            }
        }

        public Context Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
            }
        }

        public IFailure Failure
        {
            get
            {
                return fail;
            }
        }

        public string Lineage
        {
            get
            {
                return lineage;
            }
            set
            {
                lineage = value;
            }
        }

        public static string NewLineage() {
            return randgen.Next().ToString();
        }

        public virtual int Continue(object value, IFailure fail)
        {
			Context context = (Context) value;
            ContinueAgentCodelet clone = (ContinueAgentCodelet)Clone();
            context.AddToSequence(clone);
            clone.SetResult(new TwoTuple<Context, IFailure>(context, fail), context.Weight);
            return 1;
        }

        public override void SetResult(TwoTuple<Context, IFailure> result, double weight)
        {
            context = result.one;
            fail = result.two;

            coderack = context.Coderack;

            salience *= weight;

            base.SetResult(result, weight);
        }
    }
}
