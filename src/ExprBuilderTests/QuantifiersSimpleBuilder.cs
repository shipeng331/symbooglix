﻿using System;
using Symbooglix;
using NUnit.Framework;
using Microsoft.Boogie;
using System.Collections.Generic;

namespace ExprBuilderTests
{
    [TestFixture()]
    public class QuantifiersSimpleBuilder : SimpleExprBuilderTestBase
    {
        // FIXME: Use GetVarAndIdExpr instead
        private Variable GetVariable(string name, Microsoft.Boogie.Type type)
        {
            var typedIdent = new TypedIdent(Token.NoToken, name, type);
            var gv = new GlobalVariable(Token.NoToken, typedIdent);
            return gv;
        }

        [Test()]
        public void SimpleForAll()
        {
            var builder = GetBuilder();
            var freeVarX = GetVariable("x", BasicType.Int);
            var xid = new IdentifierExpr(Token.NoToken, freeVarX);
            var freeVarY = GetVariable("y", BasicType.Int);
            var yid = new IdentifierExpr(Token.NoToken, freeVarY);
            var body = builder.Eq(xid, yid);
            var result = builder.ForAll(new List<Variable>() { freeVarX, freeVarY }, body);
            Assert.AreEqual("(forall x: int, y: int :: x == y)", result.ToString());
            CheckIsBoolType(result);
        }

        [Test(), ExpectedException(typeof(ExprTypeCheckException))]
        public void SimpleForAllWrongBodyType()
        {
            var builder = GetBuilder();
            var freeVarX = GetVariable("x", BasicType.Int);
            var xid = new IdentifierExpr(Token.NoToken, freeVarX);
            var freeVarY = GetVariable("y", BasicType.Int);
            var yid = new IdentifierExpr(Token.NoToken, freeVarY);
            var body = builder.Add(xid, yid); // Wrong body type, should be bool
            builder.ForAll(new List<Variable>() { freeVarX, freeVarY }, body);
        }

        [Test()]
        public void SimpleExists()
        {
            var builder = GetBuilder();
            var freeVarX = GetVariable("x", BasicType.Int);
            var xid = new IdentifierExpr(Token.NoToken, freeVarX);
            var freeVarY = GetVariable("y", BasicType.Int);
            var yid = new IdentifierExpr(Token.NoToken, freeVarY);
            var body = builder.Eq(xid, yid);
            var result = builder.Exists(new List<Variable>() { freeVarX, freeVarY }, body);
            Assert.AreEqual("(exists x: int, y: int :: x == y)", result.ToString());
            CheckIsBoolType(result);
        }

        [Test(), ExpectedException(typeof(ExprTypeCheckException))]
        public void SimpleExistsAllWrongBodyType()
        {
            var builder = GetBuilder();
            var freeVarX = GetVariable("x", BasicType.Int);
            var xid = new IdentifierExpr(Token.NoToken, freeVarX);
            var freeVarY = GetVariable("y", BasicType.Int);
            var yid = new IdentifierExpr(Token.NoToken, freeVarY);
            var body = builder.Add(xid, yid); // Wrong body type, should be bool
            builder.Exists(new List<Variable>() { freeVarX, freeVarY }, body);
        }
    }
}

