﻿using NUnit.Framework;
using System;
using Symbooglix;
using System.Linq;

namespace SymbooglixLibTests
{
    [TestFixture()]
    public class OldExprCall : SymbooglixTest
    {
        [Test()]
        public void Impl()
        {
            p = loadProgram("programs/OldExprCallImpl.bpl");
            e = getExecutor(p, new DFSStateScheduler(), GetSolver());

            var TSC = new TerminationCounter();
            TSC.Connect(e);

            int bpCount = 0;
            e.BreakPointReached += delegate(object executor, Executor.BreakPointEventArgs data)
            {
                Assert.AreEqual("check_old_g", data.Name);

                // Make sure the old global has been recorded
                Assert.AreEqual(1, e.CurrentState.GetCurrentStackFrame().OldGlobals.Where( kv => kv.Key.Name == "g").Count());

                ++bpCount;
            };

            e.Run(getMain(p));

            Assert.AreEqual(1, TSC.Sucesses);
            Assert.AreEqual(0, TSC.NumberOfFailures);
            Assert.AreEqual(2, bpCount);
             
        }

        [Test()]
        public void Proc()
        {
            p = loadProgram("programs/OldExprCallProc.bpl");
            e = getExecutor(p, new DFSStateScheduler(), GetSolver());

            var TSC = new TerminationCounter();
            TSC.Connect(e);

            e.Run(getMain(p));

            Assert.AreEqual(0, TSC.Sucesses);
            Assert.AreEqual(1, TSC.NumberOfFailures);
            Assert.AreEqual(1, TSC.UnsatisfiableEnsures);

        }
    }
}
