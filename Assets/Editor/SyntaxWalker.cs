using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Scripting;

/// <summary>
/// </summary>
public class SyntaxWalker : Editor {
    [MenuItem("ShortCutCommand/Syntax")]
    private static void Syntax()
    {
        //var fileName = "Resources/GameScene.txt";

        string code =
            @"using System;
using System.Collections;
using System.Linq;
using System.Text;
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var firstMember = root.Members[0];
        var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;
        var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
        var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
        var argsParameter = mainDeclaration.ParameterList.Parameters[0];
        var firstParameters = from methodDeclaration in root.DescendantNodes()
                        .OfType<MethodDeclarationSyntax>()
                              where methodDeclaration.Identifier.ValueText == "Main"
                              select methodDeclaration.ParameterList.Parameters.First();
        var argsParameter2 = firstParameters.Single();
        Debug.Log(argsParameter2);
    }
    
	[MenuItem("ShortCutCommand/Syntax2")]
    private static void Syntax2()
    {
        var fileName = "Resources/GameScene.txt";

		StreamReader sr = new StreamReader(@"./Assets/Resources/GameScene.txt", Encoding.GetEncoding("UTF-8"));
 
		string code = sr.ReadToEnd();
		Debug.Log(code);
 
		sr.Close();

        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();
		// TOPレベルのUsing
		SyntaxList<UsingDirectiveSyntax> usingDirectives = root.Usings;

		// 全Using走査
		for (int i = 0; i < usingDirectives.Count; i++) {
			Debug.Log(usingDirectives[i]);
		}

		// ネームスペース（次の階層まとまり）を取得
		Debug.Log(root.Members.Count);
		MemberDeclarationSyntax firstMember = root.Members[0];
		Debug.Log(firstMember.Kind());
		var nameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;

		// ネームスペース内を走査
		Debug.Log(nameSpaceDeclaration.Members.Count);
		var classDeclaration = (ClassDeclarationSyntax)nameSpaceDeclaration.Members[0];
		Debug.Log(classDeclaration.Members.Count);
		for (int i = 0; i < classDeclaration.Members.Count; i++) {
			Debug.Log(classDeclaration.Members[i].Kind());
		}

		Debug.Log("=====methodList=====");
        var methodList = classDeclaration.DescendantNodes()
							.OfType<MethodDeclarationSyntax>().ToList();
		for (int i = 0; i < methodList.Count; i++) {
			Debug.Log(methodList[i].Identifier.ValueText);
		}
        
		Debug.Log("=====propertyList=====");
		var propertyList = classDeclaration.DescendantNodes()
							.OfType<PropertyDeclarationSyntax>().ToList();
		for (int i = 0; i < propertyList.Count; i++) {
			Debug.Log(propertyList[i].Identifier.ValueText);
		}
		
		Debug.Log("=====typeList=====");
		var typeList = classDeclaration.DescendantNodes()
							.OfType<TypeDeclarationSyntax>().ToList();
		for (int i = 0; i < typeList.Count; i++) {
			Debug.Log(typeList[i].Identifier.ValueText);
		}
		
		Debug.Log("=====fieldList=====");
		var fieldList = classDeclaration.DescendantNodes()
							.OfType<FieldDeclarationSyntax>().ToList();
		for (int i = 0; i < fieldList.Count; i++) {
			Debug.Log(fieldList[i]);

			// ※これでコメント取れる
			if (fieldList[i].HasLeadingTrivia == true)
			{
				var trivia = fieldList[i].GetLeadingTrivia();
				Debug.Log("=====true=====");
				Debug.Log(trivia);
			}

			// ※これで変数名取れる
			var syntaxList = fieldList[i].DescendantNodes().ToList();
			for (int i2 = 0; i2 < syntaxList.Count; i2++) {
				Debug.Log(syntaxList[i2]);
			}
		}
    }
}
