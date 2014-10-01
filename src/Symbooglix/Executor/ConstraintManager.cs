using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Boogie;
using System.IO;

namespace Symbooglix
{

    public class ConstraintManager : Util.IDeepClone<ConstraintManager>
    {
        // The implementation is deliberatly hidden from users
        // because we might later change to container. E.g. perhaps
        // we might move to a set rather than a list.
        private List<Constraint> InternalConstraints;

        public int Count
        {
            get { return InternalConstraints.Count; }
        }


        public IEnumerable<Expr> ConstraintExprs
        {
            get { return InternalConstraints.Select(c => c.Condition); }
        }

        public IEnumerable<Constraint> Constraints
        {
            get { return InternalConstraints; }
        }

        public ConstraintManager()
        {
            InternalConstraints = new List<Constraint>();
        }

        public ConstraintManager DeepClone()
        {
            ConstraintManager other = (ConstraintManager) this.MemberwiseClone();
            other.InternalConstraints = new List<Constraint>();

            // FIXME: Cloning constraints is probably wasteful. They shouldn't
            // really be changing.
            foreach (var c in this.InternalConstraints)
            {
                other.InternalConstraints.Add(c.DeepClone());
            }

            return other;
        }

        public void AddConstraint(Expr e, ProgramLocation location)
        {
            InternalConstraints.Add(new Constraint(e, location));
        }

        public void Dump(TextWriter TW, int indent, bool showConstraints=false)
        {
            string indentStr = new string(' ', indent);
            TW.WriteLine("[Constraints]");
            TW.WriteLine(indentStr + InternalConstraints.Count + " constraint(s)");

            if (showConstraints)
            {
                foreach (var e in InternalConstraints)
                {
                    TW.WriteLine(indentStr + "Origin:" + e.Origin);
                    TW.WriteLine(indentStr + "Expr:" + e.Condition);
                }
            }
        }

        public override string ToString()
        {
            string result = null;
            using (var SW = new StringWriter())
            {
                Dump(SW, 4);
                result = SW.ToString();
            }
            return result;
        }
    }

    public class Constraint : Util.IDeepClone<Constraint>
    {
        public Expr Condition { get; private set;}
        public ProgramLocation Origin { get; private set;}

        public Constraint(Expr condition)
        {
            Condition = condition;
            Debug.Assert(condition.Type.IsBool, "Constraint must be a boolean expression!");
            Origin = null;
        }

        public Constraint(Expr condition, ProgramLocation location) : this(condition)
        {
            Debug.Assert(location != null);
            Origin = location;
        }

        public Constraint DeepClone()
        {
            var duplicator = new NonSymbolicDuplicator();
            Constraint other = (Constraint) this.MemberwiseClone();
            other.Condition = (Expr) duplicator.Visit(this.Condition);

            // There isn't a need to deep clone the origin
            other.Origin = this.Origin;
            return other;
        }

    }
}

