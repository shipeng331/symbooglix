﻿using System;
using Microsoft.Boogie;

namespace Symbooglix
{
    // Should we split this into an abstract ProgramLocation and then have subclasses
    // for the different locations?
    public class ProgramLocation
    {
        private Object Location;

        // The location is where this variable is declared
        public ProgramLocation(Variable V)
        {
            Location = (Object) V;
        }

        // The location is where this cmd is executed
        public ProgramLocation(Cmd cmd)
        {
            Location = (Object) cmd;
        }

        // The location is where this cmd is executed
        public ProgramLocation(TransferCmd cmd)
        {
            Location = (Object) cmd;
        }

        public ProgramLocation(Requires requires)
        { 
            Location = (Object) requires;
        }

        public ProgramLocation(Ensures ensures)
        { 
            Location = (Object) ensures;
        }

        public bool IsVariable
        {
            get { return Location is Variable; }
        }

        public Variable AsVariable
        {
            get
            {
                if (IsVariable)
                    return Location as Variable;
                else
                    return null;
            }
        }

        public bool IsCmd
        {
            get { return Location is Cmd; }
        }

        public Cmd AsCmd
        {
            get
            {
                if (IsCmd)
                    return Location as Cmd;
                else
                    return null;
            }
        }

        public bool IsTransferCmd
        {
            get { return Location is TransferCmd; }
        }

        public TransferCmd AsTransferCmd
        {
            get
            {
                if (IsTransferCmd)
                    return Location as TransferCmd;
                else
                    return null;
            }
        }

        public bool IsRequires
        {
            get { return Location is Requires; }
        }

        public Requires AsRequires
        {
            get
            {
                if (IsRequires)
                    return Location as Requires;
                else
                    return null;
            }
        }

        public bool IsEnsures
        {
            get { return Location is Ensures; }
        }

        public Ensures AsEnsures
        {
            get
            {
                if (IsEnsures)
                    return Location as Ensures;
                else
                    return null;
            }
        }

        public override string ToString()
        {
            if (IsVariable)
                return "[Variable] " + AsVariable.ToString();
            else if (IsCmd)
                return "[Cmd] " + AsCmd.ToString();
            else if (IsTransferCmd)
                return "[TransferCmd] " + AsTransferCmd.ToString();
            else
                return "unknown";
        }
    }
}