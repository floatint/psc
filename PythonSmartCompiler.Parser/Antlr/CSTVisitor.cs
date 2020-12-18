using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PythonSmartCompiler.Parser.AST;

namespace PythonSmartCompiler.Parser.Antlr
{
    class CSTVisitor : Python3BaseVisitor<ASTNode>
    {
        public override ASTNode Visit([NotNull] IParseTree tree)
        {
            if (tree is Python3Parser.Single_inputContext sInCtx)
                return base.VisitSingle_input(sInCtx);
            else
                return VisitFile_input((Python3Parser.File_inputContext)tree);
            //return base.Visit(tree);
        }

        public override ASTNode VisitFile_input([NotNull] Python3Parser.File_inputContext context)
        {
            ASTCompilationUnit compUnit = new ASTCompilationUnit();
            foreach (var c in context.children)
            {
                if (c is Python3Parser.StmtContext stmtCtx)
                {
                    compUnit.Statements.Add((IASTStatement)VisitStmt(stmtCtx));
                }
            }
            return compUnit;
            //return VisitChildren(context);
        }

        public override ASTNode VisitStmt([NotNull] Python3Parser.StmtContext context)
        {
            // get child context
            var child = context.GetChild(0);

            // dispatch
            if (child is Python3Parser.Simple_stmtContext s)
                return VisitSimple_stmt(s);
            else if (child is Python3Parser.Compound_stmtContext c)
                return VisitCompound_stmt(c);
            throw new ArgumentException("Child CST node is unknown");
        }

        public override ASTNode VisitSimple_stmt([NotNull] Python3Parser.Simple_stmtContext context)
        {
            // TODO: список из small_stmt
            var child = context.GetChild(0);

            switch (child)
            {
                case Python3Parser.Small_stmtContext smallStmtCtx:
                    return VisitSmall_stmt(smallStmtCtx);
            }

            return base.VisitSimple_stmt(context);
        }

        public override ASTNode VisitSmall_stmt([NotNull] Python3Parser.Small_stmtContext context)
        {
            var child = context.GetChild(0);

            switch (child)
            {
                case Python3Parser.Expr_stmtContext exprStmtCtx:
                    return VisitExpr_stmt(exprStmtCtx);
                case Python3Parser.Del_stmtContext delStmtCtx:
                    return VisitDel_stmt(delStmtCtx);
                case Python3Parser.Pass_stmtContext passStmtCtx:
                    return VisitPass_stmt(passStmtCtx);
                case Python3Parser.Flow_stmtContext flowStmtCtx:
                    return VisitFlow_stmt(flowStmtCtx);
                case Python3Parser.Import_stmtContext impStmtCtx:
                    return VisitImport_stmt(impStmtCtx);
                case Python3Parser.Global_stmtContext globStmtCtx:
                    return VisitGlobal_stmt(globStmtCtx);
                case Python3Parser.Nonlocal_stmtContext nlocStmtCtx:
                    return VisitNonlocal_stmt(nlocStmtCtx);
                case Python3Parser.Assert_stmtContext asStmtCtx:
                    return VisitAssert_stmt(asStmtCtx);
                default:
                    return ThrowDispatchException(context);
            }
        }

        // TODO: выражения вида a = 4, a,b,*c *= 3,3,[0,0] 
        public override ASTNode VisitExpr_stmt([NotNull] Python3Parser.Expr_stmtContext context)
        {
            VisitTestlist_star_expr(context.testlist_star_expr(0));
            IList<IASTExpression> leftSide = new List<IASTExpression>();

            foreach (var n in _nodes)
                leftSide.Add((IASTExpression)n);

            _nodes.Clear();

            if (context.ChildCount > 1)
            {
                //TODO: work
                // check assign op
                var assignOpCtx = context.GetChild(1);
                if (assignOpCtx is Python3Parser.AugassignContext augassignCtx)
                {

                }
            }
            else
            {

            }
            //var leftListRoot = 
            var children = context.children;
            var child = context.GetChild(0);

            switch (child)
            {
                case Python3Parser.Testlist_star_exprContext tlsExprCtx:
                    return VisitTestlist_star_expr(tlsExprCtx);
            }

            return base.VisitExpr_stmt(context);
        }

        public override ASTNode VisitTestlist_star_expr([NotNull] Python3Parser.Testlist_star_exprContext context)
        {
            foreach(var c in context.children)
            {
                switch (c)
                {
                    case Python3Parser.TestContext testCtx:
                        _nodes.Add(VisitTest(testCtx));
                        break;
                    case Python3Parser.Star_exprContext sExprCtx:
                        _nodes.Add(VisitStar_expr(sExprCtx));
                        break;
                    default:
                        return ThrowDispatchException(context);
                }
            }
            return DefaultResult;
        }

        public override ASTNode VisitTest([NotNull] Python3Parser.TestContext context)
        {
            // lambda def
            if (context.lambdef() != null)
            {
                return VisitLambdef(context.lambdef());
            }
            else if (context.or_test(1) != null) // ternary op
            {
                var ternaryOperator = new ASTTestExpression();
                ternaryOperator.TrueValue = (IASTExpression)VisitOr_test(context.or_test(0));
                ternaryOperator.Condition = (IASTExpression)VisitOr_test(context.or_test(1));
                ternaryOperator.FalseValue = (IASTExpression)VisitTest(context.test());
                return ternaryOperator;
            }
            else // other expr
            {
                return VisitOr_test(context.or_test(0));
            }
        }

        public override ASTNode VisitLambdef([NotNull] Python3Parser.LambdefContext context)
        {
            return base.VisitLambdef(context);
        }

        public override ASTNode VisitOr_test([NotNull] Python3Parser.Or_testContext context)
        {
            ASTLogicOrExpression orExpr = new ASTLogicOrExpression();
            orExpr.Left = (IASTExpression)VisitAnd_test(context.and_test(0));
            if (context.and_test(1) != null)
                orExpr.Right = (IASTExpression)VisitAnd_test(context.and_test(1));
            return orExpr;
        }

        public override ASTNode VisitAnd_test([NotNull] Python3Parser.And_testContext context)
        {
            ASTLogicAndExpression andExpr = new ASTLogicAndExpression();
            andExpr.Left = (IASTExpression)VisitNot_test(context.not_test(0));
            if (context.not_test(1) != null)
                andExpr.Right = (IASTExpression)VisitNot_test(context.not_test(1));
            return andExpr;
        }

        public override ASTNode VisitNot_test([NotNull] Python3Parser.Not_testContext context)
        {
            ASTLogicNotExpression notExpr = new ASTLogicNotExpression();

            if (context.not_test() != null)
                notExpr.Expression =  (IASTExpression)VisitNot_test(context.not_test());
            else
                notExpr.Expression = (IASTExpression)VisitComparison(context.comparison());

            return notExpr;
        }

        // TODO: сделано правоассоциативно, а надо лево
        public override ASTNode VisitComparison([NotNull] Python3Parser.ComparisonContext context)
        {
            // get all expr rules
            var exprs = context.expr();
            // if comparison chain
            if (exprs.Length > 1)
            {
                // get all comparison ops
                var cmpOps = context.comp_op();
                //
                AST.Comparison.IASTComparison tmpNode = null;

                // iterate through ops
                for (int i = 0; i < cmpOps.Length; i++)
                {
                    AST.Comparison.IASTComparison tmp = null;

                    switch (cmpOps[i].GetText())
                    {
                        case "<":
                            tmp = new AST.Comparison.ASTLessComparison();
                            break;
                        case ">":
                            tmp = new AST.Comparison.ASTGreaterComparison();
                            break;
                        default:
                            throw new ArgumentException(string.Format("Couldn't dispatch {0}", cmpOps));
                    }

                    if (tmpNode == null)
                    {
                        tmp.Left = (IASTExpression)VisitExpr(exprs[i]);
                        tmp.Right = (IASTExpression)VisitExpr(exprs[i + 1]);
                    }
                    else
                    {
                        tmp.Left = tmpNode;
                        tmp.Right = (IASTExpression)VisitExpr(exprs[i]);
                    }

                    tmpNode = tmp;
                }

                return (ASTNode)tmpNode;

            }
            else // single expr
            {
                return VisitExpr(exprs[0]);
            }
        }

        public override ASTNode VisitExpr([NotNull] Python3Parser.ExprContext context)
        {
            // get all xor exprs
            var xorExprs = context.xor_expr();

            if (xorExprs.Length > 1)
            {
                AST.Bitwise.IASTBitwiseExpression tmpNode = null;

                for (int i = 0; i < xorExprs.Length; i++)
                {
                    var tmp = new AST.Bitwise.ASTBitwiseOrExpression();

                    if (tmpNode == null)
                    {
                        tmp.Left = (IASTExpression)VisitXor_expr(xorExprs[i]);
                        tmp.Right = (IASTExpression)VisitXor_expr(xorExprs[i + 1]);
                    }
                    else
                    {
                        tmp.Left = tmpNode;
                        tmp.Right = (IASTExpression)VisitXor_expr(xorExprs[i]);
                    }

                    tmpNode = tmp;
                }

                return (ASTNode)tmpNode;
            }
            else
            {
                return VisitXor_expr(xorExprs[0]);
            }
        }

        public override ASTNode VisitXor_expr([NotNull] Python3Parser.Xor_exprContext context)
        {
            var andExprs = context.and_expr();

            if (andExprs.Length > 1)
            {
                AST.Bitwise.IASTBitwiseExpression tmpNode = null;

                for (int i = 0; i < andExprs.Length; i++)
                {
                    var tmp = new AST.Bitwise.ASTBitwiseXorExpression();

                    if (tmpNode == null)
                    {
                        tmp.Left = (IASTExpression)VisitAnd_expr(andExprs[i]);
                        tmp.Right = (IASTExpression)VisitAnd_expr(andExprs[i + 1]);
                    }
                    else
                    {
                        tmp.Left = tmpNode;
                        tmp.Right = (IASTExpression)VisitAnd_expr(andExprs[i]);
                    }

                    tmpNode = tmp;
                }

                return (ASTNode)tmpNode;
            }
            else
            {
                return VisitAnd_expr(andExprs[0]);
            }
        }

        public override ASTNode VisitAnd_expr([NotNull] Python3Parser.And_exprContext context)
        {
            var shiftExprs = context.shift_expr();

            if (shiftExprs.Length > 1)
            {
                AST.Bitwise.IASTBitwiseExpression tmpNode = null;

                for (int i = 0; i < shiftExprs.Length; i++)
                {
                    var tmp = new AST.Bitwise.ASTBitwiseAndExpression();

                    if (tmpNode == null)
                    {
                        tmp.Left = (IASTExpression)VisitShift_expr(shiftExprs[i]);
                        tmp.Right = (IASTExpression)VisitShift_expr(shiftExprs[i + 1]);
                    }
                    else
                    {
                        tmp.Left = tmpNode;
                        tmp.Right = (IASTExpression)VisitShift_expr(shiftExprs[i]);
                    }

                    tmpNode = tmp;
                }

                return (ASTNode)tmpNode;
            }
            else
            {
                return VisitShift_expr(shiftExprs[0]);
            }
        }

        public override ASTNode VisitShift_expr([NotNull] Python3Parser.Shift_exprContext context)
        {

            List<Python3Parser.Arith_exprContext> arithExprs = new List<Python3Parser.Arith_exprContext>();
            List<ITerminalNode> opsExpr = new List<ITerminalNode>();

            foreach (var c in context.children)
            {
                if (c is Python3Parser.Arith_exprContext aExpr)
                    arithExprs.Add(aExpr);
                else if (c is ITerminalNode tNode)
                    opsExpr.Add(tNode);
            }

            if (arithExprs.Count > 1)
            {
                AST.Bitwise.IASTBitwiseExpression tmpNode = null;

                for (int i = 0; i < opsExpr.Count; i++)
                {
                    AST.Bitwise.IASTBitwiseExpression tmp = null;

                    switch (opsExpr[i].GetText())
                    {
                        case "<<":
                            tmp = new AST.Bitwise.ASTBitwiseLshExpression();
                            break;
                        case ">>":
                            tmp = new AST.Bitwise.ASTBitwiseRshExpression();
                            break;
                    }

                    if (tmpNode == null)
                    {
                        tmp.Left = (IASTExpression)VisitArith_expr(arithExprs[i]);
                        tmp.Right = (IASTExpression)VisitArith_expr(arithExprs[i + 1]);
                    }
                    else
                    {
                        tmp.Left = tmpNode;
                        tmp.Right = (IASTExpression)VisitArith_expr(arithExprs[i]);
                    }

                    tmpNode = tmp;
                }

                return (ASTNode)tmpNode;

            }
            else
            {
                return VisitArith_expr(arithExprs[0]);
            }
        }

        public override ASTNode VisitArith_expr([NotNull] Python3Parser.Arith_exprContext context)
        {
            List<Python3Parser.TermContext> terms = new List<Python3Parser.TermContext>();
            List<ITerminalNode> opsExpr = new List<ITerminalNode>();

            foreach (var c in context.children)
            {
                if (c is Python3Parser.TermContext termExpr)
                    terms.Add(termExpr);
                else if (c is ITerminalNode tNode)
                    opsExpr.Add(tNode);
            }

            if (terms.Count > 1)
            {
                AST.Arithmetic.IASTArithExpression tmpNode = null;

                for (int i = 0; i < opsExpr.Count; i++)
                {
                    AST.Arithmetic.IASTArithExpression tmp = null;

                    switch (opsExpr[i].GetText())
                    {
                        case "+":
                            tmp = new AST.Arithmetic.ASTArithAddExpression();
                            break;
                        case "-":
                            tmp = new AST.Arithmetic.ASTArithSubExpression();
                            break;
                    }

                    if (tmpNode == null)
                    {
                        tmp.Left = (IASTExpression)VisitTerm(terms[i]);
                        tmp.Right = (IASTExpression)VisitTerm(terms[i + 1]);
                    }
                    else
                    {
                        tmp.Left = tmpNode;
                        tmp.Right = (IASTExpression)VisitTerm(terms[i]);
                    }

                    tmpNode = tmp;
                }

                return (ASTNode)tmpNode;

            }
            else
            {
                return VisitTerm(terms[0]);
            }
        }

        public override ASTNode VisitTerm([NotNull] Python3Parser.TermContext context)
        {

            List<Python3Parser.FactorContext> factors = new List<Python3Parser.FactorContext>();
            List<ITerminalNode> opsExpr = new List<ITerminalNode>();

            foreach (var c in context.children)
            {
                if (c is Python3Parser.FactorContext fExpr)
                    factors.Add(fExpr);
                else if (c is ITerminalNode tNode)
                    opsExpr.Add(tNode);
            }

            if (factors.Count > 1)
            {
                AST.Arithmetic.IASTArithExpression tmpNode = null;

                for (int i = 0; i < opsExpr.Count; i++)
                {
                    AST.Arithmetic.IASTArithExpression tmp = null;

                    switch (opsExpr[i].GetText())
                    {
                        case "*":
                            tmp = new AST.Arithmetic.ASTArithMulExpression();
                            break;
                        case "@":
                            tmp = new AST.Arithmetic.ASTArithMatMulExpression();
                            break;
                        case "/":
                            tmp = new AST.Arithmetic.ASTArithDivExpression();
                            break;
                        case "%":
                            tmp = new AST.Arithmetic.ASTArithModExpression();
                            break;
                        case "//":
                            tmp = new AST.Arithmetic.ASTArithFloorDivExpression();
                            break;
                            // TODO: dispatch exception
                    }

                    if (tmpNode == null)
                    {
                        tmp.Left = (IASTExpression)VisitFactor(factors[i]);
                        tmp.Right = (IASTExpression)VisitFactor(factors[i + 1]);
                    }
                    else
                    {
                        tmp.Left = tmpNode;
                        tmp.Right = (IASTExpression)VisitFactor(factors[i]);
                    }

                    tmpNode = tmp;
                }

                return (ASTNode)tmpNode;

            }
            else
            {
                return VisitFactor(factors[0]);
            }
        }

        // TODO: в процессе
        public override ASTNode VisitFactor([NotNull] Python3Parser.FactorContext context)
        {
            return base.VisitFactor(context);
        }

        public override ASTNode VisitCompound_stmt([NotNull] Python3Parser.Compound_stmtContext context)
        {
            // get child context
            var child = context.GetChild(0);

            switch (child)
            {
                case Python3Parser.If_stmtContext ifStmtCtx:
                    return base.VisitIf_stmt(ifStmtCtx);
                case Python3Parser.While_stmtContext wStmtCtx:
                    return base.VisitWhile_stmt(wStmtCtx);
                case Python3Parser.For_stmtContext fStmtCtx:
                    return base.VisitFor_stmt(fStmtCtx);
                case Python3Parser.FuncdefContext fDefCtx:
                    return VisitFuncdef(fDefCtx);
            }
            return base.VisitCompound_stmt(context);
        }

        public override ASTNode VisitIf_stmt([NotNull] Python3Parser.If_stmtContext context)
        {
            return base.VisitIf_stmt(context);
        }

        public override ASTNode VisitFuncdef([NotNull] Python3Parser.FuncdefContext context)
        {
            // get func name
            ASTFuncDefStatement funcDefStmt = new ASTFuncDefStatement(context.NAME().GetText());

            // get parameters root
            var paramRoot = context.parameters().typedargslist();

            if (paramRoot != null)
            {
                FuncParameterTypeEnum paramType = FuncParameterTypeEnum.Value;
                ASTFuncParameter funcParam = null;
                for (int i = 0; i < paramRoot.ChildCount; i++)
                {
                    var paramCtx = paramRoot.GetChild(i);
                    // try get parameter info
                    if (paramCtx is Python3Parser.TfpdefContext pCtx)
                    {
                        // get parameter name
                        funcParam = new ASTFuncParameter(pCtx.GetChild(0).GetText());
                        // get constraint
                        funcParam.Constraint = pCtx.test() != null ? (IASTExpression)VisitTest(pCtx.test()) : null;
                        // set type
                        funcParam.Type = paramType;
                        // add to func def node
                        funcDefStmt.Parameters.Add(funcParam);
                    }
                    else
                    {
                        // if set value
                        if ("=".Equals(paramCtx.GetText()))
                        {
                            funcParam.DefaultValue = (IASTExpression)VisitTest((Python3Parser.TestContext)paramRoot.GetChild(++i));
                        }
                        
                        // list param def
                        if ("*".Equals(paramCtx.GetText()))
                        {
                            paramType = FuncParameterTypeEnum.List;
                        }

                        // dict param def
                        if ("**".Equals(paramCtx.GetText()))
                        {
                            paramType = FuncParameterTypeEnum.Dictionary;
                        }


                    }
                }
            }


            // get body context
            var suite = context.suite();

            // visit body statements
            foreach (var s in suite.children)
            {
                switch (s)
                {
                    case Python3Parser.Simple_stmtContext sStmtCtx:
                        funcDefStmt.Statements.Add((IASTStatement)VisitSimple_stmt(sStmtCtx));
                        break;
                    case Python3Parser.StmtContext stmtCtx:
                        funcDefStmt.Statements.Add((IASTStatement)VisitStmt(stmtCtx));
                        break;
                }
            }
           
            return funcDefStmt;
        }


        private ASTNode ThrowDispatchException(IParseTree pt)
        {
            throw new ArgumentException(string.Format("Couldn't dispatch {0}", pt.ToString()));
        }

        //private Stack<ASTNode> _nodes { set; get; } = new Stack<ASTNode>();
        private IList<ASTNode> _nodes { set; get; } = new List<ASTNode>();
    }
}
