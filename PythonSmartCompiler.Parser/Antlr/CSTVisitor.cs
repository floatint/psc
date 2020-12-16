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

        public override ASTNode VisitExpr_stmt([NotNull] Python3Parser.Expr_stmtContext context)
        {
            return base.VisitExpr_stmt(context);
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

        private IList<IASTStatement> _statements { set; get; }
    }
}
