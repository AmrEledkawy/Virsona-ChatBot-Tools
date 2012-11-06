using System;
using System.Collections.Generic;
using System.Text;
using DataTemple.Codeland;
using PluggerBase.ActionReaction.Evaluations;

namespace DataTemple.AgentEvaluate
{
    // Calls success continue
    public class ContinueCodelet : Codelet, IFailure
    {
        protected Context context;
        protected IContinuation succ;
        protected IFailure fail;

        public ContinueCodelet(double salience, Context context, IContinuation succ, IFailure fail)
            : base(context.Coderack, salience, 4 * 4, 5)
        {
            this.context = context;
            this.succ = succ;
            this.fail = fail;
        }

        public override int Evaluate()
        {
            succ.Continue(context, fail);

            return time;
        }

        #region IFailure Members

        public int Fail(string reason, IContinuation succ)
        {
            coderack.AddCodelet((Codelet) this.Clone());
            return 1;
        }

        #endregion
    }
}
