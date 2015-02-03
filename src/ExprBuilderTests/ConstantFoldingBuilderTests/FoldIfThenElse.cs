﻿using System;
using NUnit.Framework;
using Symbooglix;
using Microsoft.Boogie;

namespace ExprBuilderTests.ConstantFoldingTests
{
    public class FoldIfThenElse : ConstantFoldingExprBuilderTests
    {
        [Test()]
        public void IfTrue()
        {
            var constantBuilder = GetConstantFoldingBuilder();
            var id0 = GetVarAndIdExpr("foo", BasicType.GetBvType(8)).Item2;
            var id1 = GetVarAndIdExpr("bar", BasicType.GetBvType(8)).Item2;
            var result = constantBuilder.IfThenElse(constantBuilder.True, id0, id1);
            Assert.AreEqual("foo", result.ToString());
            CheckIsBvType(result, 8);
            Assert.AreSame(result, id0);
        }

        [Test()]
        public void IfFalse()
        {
            var cfb = GetConstantFoldingBuilder();
            var id0 = GetVarAndIdExpr("foo", BasicType.GetBvType(8)).Item2;
            var id1 = GetVarAndIdExpr("bar", BasicType.GetBvType(8)).Item2;
            Expr result = cfb.IfThenElse(Expr.False, id0, id1);
            Assert.AreEqual("bar", result.ToString());
            CheckIsBvType(result, 8);
            Assert.AreSame(result, id1);
        }

        [Test()]
        public void IfConditionThenTrueElseFalse()
        {
            var builder = GetConstantFoldingBuilder();
            var id0 = GetVarAndIdExpr("foo", BasicType.GetBvType(8)).Item2;
            var id1 = GetVarAndIdExpr("bar", BasicType.GetBvType(8)).Item2;
            var condition = builder.Eq(id0, id1);
            var result = builder.IfThenElse(condition, builder.True, builder.False);
            Assert.IsNull(ExprUtil.AsLiteral(result));
            Assert.AreSame(condition, result);
        }

        [Test()]
        public void IfConditionThenFalseElseTrue()
        {
            var builder = GetConstantFoldingBuilder();
            var id0 = GetVarAndIdExpr("foo", BasicType.GetBvType(8)).Item2;
            var id1 = GetVarAndIdExpr("bar", BasicType.GetBvType(8)).Item2;
            var condition = builder.Eq(id0, id1);
            var result = builder.IfThenElse(condition, builder.False, builder.True);
            Assert.IsNull(ExprUtil.AsLiteral(result));
            var expected = builder.Not(condition);
            Assert.IsNull(ExprUtil.AsLiteral(expected));
            Assert.AreEqual(expected, result);
        }


        [Test()]
        public void IfConditionThenConditionAndElseFalse()
        {
            var builder = GetConstantFoldingBuilder();
            var v = GetVarAndIdExpr("v", BasicType.Bool).Item2;
            var result = builder.IfThenElse(v, v, builder.False);
            Assert.AreEqual("v", result.ToString());
            Assert.AreSame(v, result);
            CheckIsBoolType(result);
        }

        [Test()]
        public void IfNotConditionThenConditionElseTrue()
        {
            var v = GetVarAndIdExpr("v", BasicType.Bool).Item2;
            var builder = GetConstantFoldingBuilder();
            var result = builder.IfThenElse(builder.Not(v), v, builder.True);
            Assert.AreEqual("v", result.ToString());
            Assert.AreSame(v, result);
            CheckIsBoolType(result);
        }

        [Test()]
        public void IfConditionThenTrueElseCondition()
        {
            var builder = GetConstantFoldingBuilder();
            var v = GetVarAndIdExpr("v", BasicType.Bool).Item2;
            var y = GetVarAndIdExpr("y", BasicType.Bool).Item2;
            var vNotEqy = builder.NotEq(v, y);

            var result = builder.IfThenElse(vNotEqy, builder.True, vNotEqy);
            Assert.AreEqual("v != y", result.ToString());
            Assert.AreSame(vNotEqy, result);
            CheckIsBoolType(result);
        }

        [Test()]
        public void IfConditionThenElseStructuallySame()
        {
            var builder = GetConstantFoldingBuilder();
            var v = GetVarAndIdExpr("v", BasicType.Int).Item2;
            var y = GetVarAndIdExpr("y", BasicType.Int).Item2;
            var vNotEqy = builder.NotEq(v, y);
            var vPlusOne = builder.Add(v, builder.ConstantInt(1));

            var result = builder.IfThenElse(vNotEqy, vPlusOne, vPlusOne);
            Assert.AreSame(vPlusOne, result);
            CheckIsInt(result);
        }



        // (if group_size_y == 1bv32 then 1bv1 else 0bv1) != 0bv1;
        //
        // This is collaboration between NotEq() and IfThenElse()
        [Test()]
        public void MergeNotEqToCondition()
        {
            var builder = GetConstantFoldingBuilder();
            var v = GetVarAndIdExpr("group_size_y", BasicType.GetBvType(32)).Item2;
            var condition = builder.Eq(v, builder.ConstantBV(1, 32));
            var ite = builder.IfThenElse(condition, builder.ConstantBV(1, 1), builder.ConstantBV(0, 1));
            Assert.IsNull(ExprUtil.AsLiteral(ite));
            var result = builder.NotEq(ite, builder.ConstantBV(0, 1));
            Assert.AreSame(condition, result);
        }

        // (if group_size_y == 1bv32 then 0bv1 else 1bv1) != 0bv1;
        // fold to
        // ! (group_size_y == 1bv32 )
        [Test()]
        public void MergeNotEqToNotCondition()
        {
            var builder = GetConstantFoldingBuilder();
            var v = GetVarAndIdExpr("group_size_y", BasicType.GetBvType(32)).Item2;
            var condition = builder.Eq(v, builder.ConstantBV(1, 32));
            var ite = builder.IfThenElse(condition, builder.ConstantBV(0, 1), builder.ConstantBV(1, 1));
            Assert.IsNull(ExprUtil.AsLiteral(ite));
            var result = builder.NotEq(ite, builder.ConstantBV(0, 1));
            var expected = builder.Not(condition);
            Assert.IsNotInstanceOf<LiteralExpr>(expected);
            Assert.IsTrue(expected.Equals(result));
        }
    }
}
