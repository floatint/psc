using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Atom
{
    public class ASTAtomInteger : ASTNode, IASTAtom
    {
        public ASTAtomInteger(int v) => Value = v;
        public int Value { set; get; }
    }
}
