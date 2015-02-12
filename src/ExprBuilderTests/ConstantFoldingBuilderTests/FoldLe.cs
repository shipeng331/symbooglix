﻿using System;
using NUnit.Framework;
using Symbooglix;
using Microsoft.Boogie;

namespace ExprBuilderTests.ConstantFoldingTests
{
    [TestFixture()]
    public class FoldLe : ConstantFoldingExprBuilderTests
    {
        [Test()]
        public void simpleConstantIntEqual()
        {
            var cfb = GetConstantFoldingBuilder();
            var constant0 = cfb.ConstantInt(0);
            var constant1 = cfb.ConstantInt(0);
            var result = cfb.Le(constant0, constant1);
            CheckIsBoolType(result);
            Assert.IsTrue(ExprUtil.IsTrue(result));
        }

        [Test()]
        public void simpleConstantIntLt()
        {
            var cfb = GetConstantFoldingBuilder();
            var constant0 = cfb.ConstantInt(0);
            var constant1 = cfb.ConstantInt(1);
            var result = cfb.Le(constant0, constant1);
            CheckIsBoolType(result);
            Assert.IsTrue(ExprUtil.IsTrue(result));
        }

        [Test()]
        public void simpleConstantRealEqual()
        {
            var cfb = GetConstantFoldingBuilder();
            var constant0 = cfb.ConstantReal("0.0");
            var constant1 = cfb.ConstantReal("0.0");
            var result = cfb.Le(constant0, constant1);
            CheckIsBoolType(result);
            Assert.IsTrue(ExprUtil.IsTrue(result));
        }

        [Test()]
        public void simpleConstantRealLt()
        {
            var cfb = GetConstantFoldingBuilder();
            var constant0 = cfb.ConstantReal("0.0");
            var constant1 = cfb.ConstantReal("0.1");
            var result = cfb.Le(constant0, constant1);
            CheckIsBoolType(result);
            Assert.IsTrue(ExprUtil.IsTrue(result));
        }

        [Test()]
        public void foldSameExpr()
        {
            var builderPair = GetSimpleAndConstantFoldingBuilder();
            var cfb = builderPair.Item2;
            var v0 = GetVarAndIdExpr("x", BasicType.Int);
            var side = cfb.Add(v0.Item2, v0.Item2);
            var foldedResult = cfb.Le(side, side);
            CheckType(foldedResult, p => p.IsBool);
            Assert.IsTrue(ExprUtil.IsTrue(foldedResult));
        }

        [Test()]
        public void NoFold()
        {
            var builderPair = GetSimpleAndConstantFoldingBuilder();
            var sfb = builderPair.Item1;
            var cfb = builderPair.Item2;
            var v0 = GetVarAndIdExpr("x", BasicType.Int);
            var v1 = GetVarAndIdExpr("y", BasicType.Int);
            var foldedResult = cfb.Le(v0.Item2, v1.Item2);
            var simpleResult = sfb.Le(v0.Item2, v1.Item2);
            CheckType(foldedResult, p => p.IsBool);
            Assert.AreEqual(simpleResult, foldedResult);
        }
    }
}
