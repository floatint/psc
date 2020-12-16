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
            var child = context.GetChild(0);

            switch (child)
            {
                
            }

            return base.VisitSimple_stmt(context);
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
                ASTFuncParameter funcParam = null;
                for (int i = 0; i < paramRoot.ChildCount; i++)
                {
                    var paramCtx = paramRoot.GetChild(i);
                    if (paramCtx is Python3Parser.TfpdefContext pCtx)
                    {
                        // get parameter name
                        // TODO: не будет работать в случае с *,**
                        funcParam = new ASTFuncParameter(pCtx.GetChild(0).GetText());
                        // get constraint
                        funcParam.Constraint = pCtx.test() != null ? (IASTExpression)VisitTest(pCtx.test()) : null;
                    }
                    else
                    {
                        if ("=".Equals(paramCtx.GetText()))
                        {
                            funcParam.DefaultValue = (IASTExpression)VisitTest((Python3Parser.TestContext)paramRoot.GetChild(++i));
                        }
                    }
                    var y = 9;
                }
                foreach (var p in paramRoot.children)
                {
                    if (p is Python3Parser.TfpdefContext pCtx)
                    {
                        // get parameter name
                        // TODO: не будет работать в случае с *,**
                        funcParam = new ASTFuncParameter(pCtx.GetChild(0).GetText());
                        // get constraint
                        funcParam.Constraint = pCtx.test() != null ? (IASTExpression)VisitTest(pCtx.test()) : null;
                    }
                    else
                    {
                        //var defValCtx = p.GetChild(0);
                        // TODO: проработать
                        if ("=".Equals(p.GetText()))
                        {
                            funcParam.DefaultValue = (IASTExpression)VisitTest((Python3Parser.TestContext)p.GetChild(1));
                        }
                    }
                }
            }


            // get body context
            var suite = context.suite();

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

        private IList<IASTStatement> _statements { set; get; }
    }
}
