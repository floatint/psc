using System;
using System.Collections.Generic;
using System.Text;
using PythonSmartCompiler.Parser.AST;
using Antlr4.Runtime;

namespace PythonSmartCompiler.Parser
{
    public class AntlrParser : IParser
    {
        public ASTNode Parse(string sourceCode)
        {
            try
            {
                // get input source code
                var sourceInput = Utils.SourceProvider.FromString(sourceCode);
                // get token source
                ITokenSource tokenSource = new Parser.Python3Lexer(sourceInput);
                // get token stream
                ITokenStream tokenStream = new CommonTokenStream(tokenSource);
                // get python parser
                Parser.Python3Parser pythonParser = new Parser.Python3Parser(tokenStream);

                // get CST visitor
                Parser.IPython3Visitor<Parser.AST.ASTNode> cstVisitor = new Parser.Antlr.CSTVisitor();

                return cstVisitor.Visit(pythonParser.file_input());

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
