﻿using System;

namespace Symbooglix
{
    namespace Solver
    {
        // This interface is designed around the needs of the underlying solver
        // as opposed to the Executor.
        public interface ISolverImpl : IDisposable
        {
            void SetConstraints(ConstraintManager constraints);
            Tuple<Solver.Result, IAssignment> ComputeSatisfiability(Microsoft.Boogie.Expr queryExpr, bool computeAssignment);
            void SetTimeout(int seconds);
        }
    }
}

