﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    public class ASTList : ASTNode, IASTExpression
    {
        public IList<IASTExpression> Items { set; get; } = new List<IASTExpression>();
    }
}
