using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Atom
{
    public class ASTAtomName : ASTNode, IASTAtom
    {
        public ASTAtomName(string v) => Value = v;

        public string Value { set; get; }

    }
}
