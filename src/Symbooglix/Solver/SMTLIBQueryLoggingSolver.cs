using System;
using System.Collections.Generic;
using Microsoft.Boogie;
using Symbooglix;
using System.IO;


namespace Symbooglix
{
    namespace Solver
    {
        // SMTLIBQueryLoggingSolver and SMTLIBQueryLoggingSolverImpl
        // share quite a bit of code because they used to be the same
        // class which ended up being quite messy so they were separated

        public class SMTLIBQueryLoggingSolverImpl : ISolverImpl
        {
            private ISolverImpl UnderlyingImpl;
            private SMTLIBQueryPrinter Printer;
            private int UseCounter=0;
            public SMTLIBQueryLoggingSolverImpl(ISolverImpl underlyingImplementation, TextWriter TW, bool namedAttributeBindings, bool humanReadable)
            {
                this.UnderlyingImpl = underlyingImplementation;
                Printer = new SMTLIBQueryPrinter(TW, namedAttributeBindings, humanReadable);
            }

            private void PrintDeclarationsAndConstraints(IConstraintManager cm)
            {
                Printer.PrintSortDeclarations();
                Printer.PrintVariableDeclarations();
                Printer.PrintFunctionDeclarations();
                Printer.PrintCommentLine(cm.Count.ToString() +  " Constraints");
                foreach (var constraint in cm.Constraints)
                {
                    Printer.PrintCommentLine("Origin : " + constraint.Origin.ToString());
                    Printer.PrintAssert(constraint.Condition);
                }
            }

            public Tuple<Result, IAssignment> ComputeSatisfiability(Query query, bool computeAssignment)
            {
                // FIXME: Optimise the case where only the query expression has changed
                Printer.ClearDeclarations();
                foreach (var constraint in query.Constraints.Constraints)
                {
                    Printer.AddDeclarations(constraint);
                }
                Printer.AddDeclarations(query.QueryExpr);


                Printer.PrintCommentLine("Query " + UseCounter + " Begin");
                PrintDeclarationsAndConstraints(query.Constraints);
                Printer.PrintCommentLine("Query Expr:" + query.QueryExpr.Origin.ToString());
                Printer.PrintAssert(query.QueryExpr.Condition);
                Printer.PrintCheckSat();

                var result = UnderlyingImpl.ComputeSatisfiability(query, computeAssignment);

                Printer.PrintCommentLine("Result : " + result.Item1);

                if (computeAssignment)
                {
                    // FIXME: Support this for query logging
                    Printer.PrintCommentLine(" WARNING: computeAssignment requested, but the printer doesn't support this!");
                }

                Printer.PrintExit();
                Printer.PrintCommentLine("End of Query " + (UseCounter));
                ++UseCounter;
                return result;
            }

            public void SetTimeout(int seconds)
            {
                UnderlyingImpl.SetTimeout(seconds);
            }

            public void Dispose()
            {
                UnderlyingImpl.Dispose();
            }

            public void Interrupt()
            {
                UnderlyingImpl.Interrupt();
            }

            public ISolverImplStatistics Statistics
            {
                get
                {
                    return UnderlyingImpl.Statistics;
                }
            }
        }
    }
}

