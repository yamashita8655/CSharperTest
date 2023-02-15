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
	
	[MenuItem("ShortCutCommand/Syntax3")]
    private static void Syntax3()
    {
        var fileName = "Resources/GameScene.txt";

		StreamReader sr = new StreamReader(@"./Assets/Resources/GameScene.txt", Encoding.GetEncoding("UTF-8"));
 
		string code = sr.ReadToEnd();
		Debug.Log(code);
 
		sr.Close();

        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();
		
		//// TOPレベルのUsing
		//SyntaxList<UsingDirectiveSyntax> usingDirectives = root.Usings;

		//// 全Using走査
		//for (int i = 0; i < usingDirectives.Count; i++) {
		//	Debug.Log(usingDirectives[i]);
		//}

		// ネームスペース（次の階層まとまり）を取得
		//Debug.Log(root.Members.Count);
		MemberDeclarationSyntax firstMember = root.Members[0];
		//Debug.Log(firstMember.Kind());
		var nameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;

		// ネームスペース内を走査
		//Debug.Log(nameSpaceDeclaration.Members.Count);
		var classDeclaration = (ClassDeclarationSyntax)nameSpaceDeclaration.Members[0];
		//Debug.Log(classDeclaration.Members.Count);
		//for (int i = 0; i < classDeclaration.Members.Count; i++) {
		//	Debug.Log(classDeclaration.Members[i].Kind());
		//}

		Debug.Log("=====methodList=====");

		// ※調査の際に、型タイプ調べる際の方法
		//Debug.Log(methodList[i].Identifier.GetType());
        
		var methodList = classDeclaration.DescendantNodes()
							.OfType<MethodDeclarationSyntax>().ToList();

		string methodLogString = string.Empty;
		// コメント取得
		for (int i = 0; i < methodList.Count; i++) {

			Debug.Log(methodList[i].Identifier.GetType());
			// メソッド名
			Debug.Log(methodList[i].Identifier.ValueText);

			if (methodList[i].HasLeadingTrivia == true)
			{
				var triviaList = methodList[i].GetLeadingTrivia();

                var commentArray = triviaList.Where(trivia => (!trivia.IsKind(SyntaxKind.WhitespaceTrivia)) && (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))).ToArray();
				for (int i2 = 0; i2 < commentArray.Length; i2++) {
					//Debug.Log(commentArray[i2]);
				}

				var commentSyntaxTriviaArray = GetMethodComments(methodList[i]);
				for (int i2 = 0; i2 < commentSyntaxTriviaArray.Length; i2++) {
					Debug.Log(commentSyntaxTriviaArray[i2]);
				}
			}
			
			// private,public,protected/async,virtualの確認
			var modifierTexts = methodList[i].Modifiers.Select(x => x.Text).ToArray();
			for (int i2 = 0; i2 < modifierTexts.Length; i2++) {
				Debug.Log(modifierTexts[i2]);
			}
			
			// 引数調べたい
			var parameterTexts = methodList[i].ParameterList.Parameters.Select(x => x).ToArray();
			for (int i2 = 0; i2 < parameterTexts.Length; i2++) {
				Debug.Log(parameterTexts[i2].Identifier.ValueText);
				//Debug.Log(parameterTexts[i2].);// int aaa とか全文
			}
		}

        
		//Debug.Log("=====propertyList=====");
		//var propertyList = classDeclaration.DescendantNodes()
		//					.OfType<PropertyDeclarationSyntax>().ToList();
		//for (int i = 0; i < propertyList.Count; i++) {
		//	Debug.Log(propertyList[i].Identifier.ValueText);
		//}
		//
		//Debug.Log("=====typeList=====");
		//var typeList = classDeclaration.DescendantNodes()
		//					.OfType<TypeDeclarationSyntax>().ToList();
		//for (int i = 0; i < typeList.Count; i++) {
		//	Debug.Log(typeList[i].Identifier.ValueText);
		//}
		//
		//Debug.Log("=====fieldList=====");
		//var fieldList = classDeclaration.DescendantNodes()
		//					.OfType<FieldDeclarationSyntax>().ToList();
		//for (int i = 0; i < fieldList.Count; i++) {
		//	Debug.Log(fieldList[i]);

		//	// ※これでコメント取れる
		//	if (fieldList[i].HasLeadingTrivia == true)
		//	{
		//		var trivia = fieldList[i].GetLeadingTrivia();
		//		Debug.Log("=====true=====");
		//		Debug.Log(trivia);
		//	}

		//	// ※これで変数名取れる
		//	var syntaxList = fieldList[i].DescendantNodes().ToList();
		//	for (int i2 = 0; i2 < syntaxList.Count; i2++) {
		//		Debug.Log(syntaxList[i2]);
		//	}
		//}
    }
	
	[MenuItem("ShortCutCommand/Syntax4")]
    private static void Syntax4()
    {
        var fileName = "Resources/GameScene.txt";

		StreamReader sr = new StreamReader(@"./Assets/Resources/GameScene.txt", Encoding.GetEncoding("UTF-8"));
 
		string code = sr.ReadToEnd();
 
		sr.Close();

        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();
		
		//// TOPレベルのUsing
		//SyntaxList<UsingDirectiveSyntax> usingDirectives = root.Usings;

		//// 全Using走査
		//for (int i = 0; i < usingDirectives.Count; i++) {
		//	Debug.Log(usingDirectives[i]);
		//}

		// ネームスペース（次の階層まとまり）を取得
		MemberDeclarationSyntax firstMember = root.Members[0];
		var nameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;

		// ネームスペース内を走査
		var classDeclaration = (ClassDeclarationSyntax)nameSpaceDeclaration.Members[0];

		Debug.Log("=====methodList=====");

		// ※調査の際に、型タイプ調べる際の方法
		//Debug.Log(methodList[i].Identifier.GetType());
        
		var methodList = classDeclaration.DescendantNodes()
							.OfType<MethodDeclarationSyntax>().ToList();

		string methodLogString = string.Empty;
		// コメント取得
		for (int i = 0; i < methodList.Count; i++) {

			methodLogString += "メソッド名：" + methodList[i].Identifier.ValueText + "\n";

			string methodName = methodList[i].Identifier.ValueText;
			
			// private,public,protected/async,virtualの確認
			var modifierTexts = GetMethodModifiers(methodList[i]);
			if (modifierTexts.Contains("public"))
			{
				char c1 = methodName[0];
				if (char.IsUpper(c1) != true)
				{
					methodLogString += "public属性のメソッド名は、先頭大文字にしてください\n";
				}
			}
			else if (modifierTexts.Contains("protected"))
			{
				char c1 = methodName[0];
				if (char.IsLower(c1) != true)
				{
					methodLogString += "protected属性のメソッド名は、先頭小文字にしてください\n";
				}
			}
			else if (modifierTexts.Contains("private"))
			{
				char c1 = methodName[0];
				if (char.IsLower(c1) != true)
				{
					methodLogString += "private属性のメソッド名は、先頭小文字にしてください\n";
				}
			}
			else
			{
				// privateと同じ扱い
				char c1 = methodName[0];
				if (char.IsLower(c1) != true)
				{
					methodLogString += "private属性のメソッド名は、先頭小文字にしてください\n";
				}
			}

			if (modifierTexts.Contains("async"))
			{
				if (!methodName.EndsWith("Async"))
				{
					methodLogString += "async属性のメソッド名には、末尾にAsyncをつけてください\n";
				}
			}

			if (methodList[i].HasLeadingTrivia == true)
			{
				//var triviaList = methodList[i].GetLeadingTrivia();

                //var commentArray = triviaList.Where(trivia => (!trivia.IsKind(SyntaxKind.WhitespaceTrivia)) && (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))).ToArray();
				//for (int i2 = 0; i2 < commentArray.Length; i2++) {
				//	//Debug.Log(commentArray[i2]);
				//}

				//var commentSyntaxTriviaArray = GetMethodComments(methodList[i]);
				//for (int i2 = 0; i2 < commentSyntaxTriviaArray.Length; i2++) {
				//	Debug.Log(commentSyntaxTriviaArray[i2]);
				//}
			}
			else
			{
				methodLogString += "コメントが無いので、記載してください\n";
			}
			
			// 引数調べたい
			var parameterTexts = GetMethodParameters(methodList[i]);
			for (int i2 = 0; i2 < parameterTexts.Length; i2++) {
				string name = parameterTexts[i2].Identifier.ValueText;
				string fullName = parameterTexts[i2].ToString();
				char c1 = name[0];
				if (char.IsLower(c1) != true)
				{
					methodLogString += "引数" + fullName + "の先頭文字は、小文字にしてください\n";
				}
			}
		}

		Debug.Log(methodLogString);
        
		//Debug.Log("=====propertyList=====");
		//var propertyList = classDeclaration.DescendantNodes()
		//					.OfType<PropertyDeclarationSyntax>().ToList();
		//for (int i = 0; i < propertyList.Count; i++) {
		//	Debug.Log(propertyList[i].Identifier.ValueText);
		//}
		//
		//Debug.Log("=====typeList=====");
		//var typeList = classDeclaration.DescendantNodes()
		//					.OfType<TypeDeclarationSyntax>().ToList();
		//for (int i = 0; i < typeList.Count; i++) {
		//	Debug.Log(typeList[i].Identifier.ValueText);
		//}
		//
		//Debug.Log("=====fieldList=====");
		//var fieldList = classDeclaration.DescendantNodes()
		//					.OfType<FieldDeclarationSyntax>().ToList();
		//for (int i = 0; i < fieldList.Count; i++) {
		//	Debug.Log(fieldList[i]);

		//	// ※これでコメント取れる
		//	if (fieldList[i].HasLeadingTrivia == true)
		//	{
		//		var trivia = fieldList[i].GetLeadingTrivia();
		//		Debug.Log("=====true=====");
		//		Debug.Log(trivia);
		//	}

		//	// ※これで変数名取れる
		//	var syntaxList = fieldList[i].DescendantNodes().ToList();
		//	for (int i2 = 0; i2 < syntaxList.Count; i2++) {
		//		Debug.Log(syntaxList[i2]);
		//	}
		//}
    }
    
	// コメントの取得
	public static SyntaxTrivia[] GetMethodComments(SyntaxNode methodNode)
    {
		var triviaList = methodNode.GetLeadingTrivia();
		var commentSyntaxTriviaArray = triviaList.Where(trivia => (!trivia.IsKind(SyntaxKind.WhitespaceTrivia)) && (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))).ToArray();

		return commentSyntaxTriviaArray;
	}
	
	// private/punlic等の修飾子取得
	public static string[] GetMethodModifiers(BaseMethodDeclarationSyntax methodNode)
    {
		var modifierTexts = methodNode.Modifiers.Select(x => x.Text).ToArray();
		return modifierTexts;
	}
	
	// 引数要素取得
	public static ParameterSyntax[] GetMethodParameters(BaseMethodDeclarationSyntax methodNode)
    {
		var parameterTexts = methodNode.ParameterList.Parameters.Select(x => x).ToArray();
		return parameterTexts;
	}
}
