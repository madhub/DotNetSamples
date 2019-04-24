using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Linq;

namespace RoslynDemo
{
    public class AttributeValueRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitAttribute(AttributeSyntax attribute)
        {
            if (attribute.Name.GetText().ToString() == "InternalsVisibleTo")
            {
                var value = attribute.ArgumentList.Arguments.Select(argument => argument.Expression).OfType<LiteralExpressionSyntax>()
                    .Select(a =>   a.Token.ValueText).FirstOrDefault();

                var newArgumentList = SyntaxFactory.AttributeArgumentList(
                        SyntaxFactory.SeparatedList(new[]
                        {
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal($"{value},myupdated value"))
                                        )
                        }));
                return attribute.ReplaceNode(attribute.ArgumentList, newArgumentList);

            }
            return attribute;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {

            var assemblyInfoFileText = @"using System.Reflection;
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;
            [assembly: AssemblyCopyright(""Copyright © Maarten Balliauw 2014"")]
            [assembly: ComVisible(false)]
            [assembly: Guid(""958e6d8b-7693-46c4-a8f3-8eb2cc2cd684"")]
            [assembly: InternalsVisibleTo(""madhusudana"")]
            [assembly: InternalsVisibleTo(""abc"")]";

            var tree = CSharpSyntaxTree.ParseText(assemblyInfoFileText);
            var rewriter = new AttributeValueRewriter();

            var rewrittenRoot = rewriter.Visit(tree.GetRoot());

            Console.WriteLine(rewrittenRoot.GetText().ToString());
        }
    }
}
