using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Atom
{
    public class ASTAtomFloat : ASTNode, IASTAtom
    {
        public ASTAtomFloat(double v) => Value = v;
        public double Value { set; get; }
    }
}
