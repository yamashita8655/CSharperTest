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
using System.Text.RegularExpressions;

/// <summary>
/// </summary>
public class SyntaxWalker : Editor {
    [MenuItem("ShortCutCommand/RegexTest")]
    private static void RegexTest()
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


		// ※調査の際に、型タイプ調べる際の方法
		//Debug.Log(methodList[i].Identifier.GetType());
        
		// 関数部分のチェック
		Debug.Log("=====methodList=====");
		
		// フィールド情報の取得
		var fieldList = classDeclaration.DescendantNodes()
							.OfType<FieldDeclarationSyntax>().ToList();
		
		for (int i = 0; i < fieldList.Count; i++) {

			// 1 フィールド名
			string fieldName = fieldList[i].ToString();
	
			// 2 フィールド指定先の戻り値の型
			string returnType = fieldList[i].Declaration.Type.ToString();
			
			// 3 フィールド指定先のprivate,public,protected/async,virtual等の修飾子
			var modifierTexts = fieldList[i].Modifiers.Select(x => x.Text).ToArray();

			// 4 フィールド指定先変数名
			var variables = fieldList[i].Declaration.Variables.ToList();
			string variable = variables[0].Identifier.ToString();
			
			// 5 コメント
			if (fieldList[i].HasLeadingTrivia == true)
			{
				var triviaList = fieldList[i].GetLeadingTrivia();
				var commentSyntaxTriviaArray = triviaList.Where(trivia => (!trivia.IsKind(SyntaxKind.WhitespaceTrivia)) && (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))).ToArray();
				if (commentSyntaxTriviaArray.Length == 0)
				{
					// コメント無し
				}
				else
				{
					// コメントが行数分含まれる
					for (int i2 = 0; i2 < commentSyntaxTriviaArray.Length; i2++) {
						commentSyntaxTriviaArray[i2].ToString();
					}
				}
			}
			else
			{
				// コメント無し
			}
		}
    }

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
			//Debug.Log(usingDirectives[i]);
		}

		// ネームスペース（次の階層まとまり）を取得
		//Debug.Log(root.Members.Count);
		MemberDeclarationSyntax firstMember = root.Members[0];
		//Debug.Log(firstMember.Kind());
		var nameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;

		// ネームスペース内を走査
		//Debug.Log(nameSpaceDeclaration.Members.Count);
		var classDeclaration = (ClassDeclarationSyntax)nameSpaceDeclaration.Members[0];
		//Debug.Log(classDeclaration.Members.Count);
		for (int i = 0; i < classDeclaration.Members.Count; i++) {
			//Debug.Log(classDeclaration.Members[i].Kind());
		}

		//Debug.Log("=====methodList=====");
        //var methodList = classDeclaration.DescendantNodes()
		//					.OfType<MethodDeclarationSyntax>().ToList();
		//for (int i = 0; i < methodList.Count; i++) {
		//	Debug.Log(methodList[i].Identifier.ValueText);
		//}
        //
		//Debug.Log("=====propertyList=====");
		//var propertyList = classDeclaration.DescendantNodes()
		//					.OfType<PropertyDeclarationSyntax>().ToList();
		//for (int i = 0; i < propertyList.Count; i++) {
		//	Debug.Log(propertyList[i].Identifier.ValueText);
		//}
		
		//Debug.Log("=====typeList=====");
		//var typeList = classDeclaration.DescendantNodes()
		//					.OfType<TypeDeclarationSyntax>().ToList();
		//for (int i = 0; i < typeList.Count; i++) {
		//	Debug.Log(typeList[i].Identifier.ValueText);
		//}
		
		Debug.Log("=====fieldList=====");
		var fieldList = classDeclaration.DescendantNodes()
							.OfType<FieldDeclarationSyntax>().ToList();
		for (int i = 0; i < fieldList.Count; i++) {
			Debug.Log(fieldList[i]);
			var declaration = fieldList[i].Declaration;
			Debug.Log("===declaration");
			Debug.Log(declaration.Type);
			var v = declaration.Variables.ToList();
			for (int i2 = 0; i2 < v.Count; i2++) {
				Debug.Log(v[i2].Identifier);// フィールドの変数名
			}
			//Debug.Log(declaration.Member[0]);

			// ※これでコメント取れる
			if (fieldList[i].HasLeadingTrivia == true)
			{
				var trivia = fieldList[i].GetLeadingTrivia();
				//Debug.Log("=====true=====");
				//Debug.Log(trivia);
			}

			// ※これで変数名取れる
			var syntaxList = fieldList[i].DescendantNodes().ToList();
			for (int i2 = 0; i2 < syntaxList.Count; i2++) {
				//Debug.Log("====syntaxList");
				//Debug.Log(syntaxList[i2]);

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

			// 戻り値型名
			Debug.Log(methodList[i].ReturnType);

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


		// ※調査の際に、型タイプ調べる際の方法
		//Debug.Log(methodList[i].Identifier.GetType());
        
		// 関数部分のチェック
		Debug.Log("=====methodList=====");
		var methodList = classDeclaration.DescendantNodes()
							.OfType<MethodDeclarationSyntax>().ToList();
		
		string methodLogString = CheckMethod(methodList);
		Debug.Log(methodLogString);
        
		Debug.Log("=====propertyList=====");
		var propertyList = classDeclaration.DescendantNodes()
							.OfType<PropertyDeclarationSyntax>().ToList();
		
		string propertyLogString = CheckProperty(propertyList);
		Debug.Log(propertyLogString);

		Debug.Log("=====fieldList=====");
		var fieldList = classDeclaration.DescendantNodes()
							.OfType<FieldDeclarationSyntax>().ToList();
		string fieldLogString = CheckField(fieldList);
		Debug.Log(fieldLogString);
		//for (int i = 0; i < propertyList.Count; i++) {
		//CheckProperty
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
	
	[MenuItem("ShortCutCommand/Syntax5")]
    private static void Syntax5()
    {
		// 定数扱いの変数たち
		string path = "TargetCs";

		string outputPath = "./TargetCs/Result";
		Directory.CreateDirectory(outputPath);

		// まずは、フォルダリストを取得
		string[] folders = Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
        //var fileName = "Resources/GameScene.txt";
		for (int i = 0; i < folders.Length; i++) {
			string[] subFolders = Directory.GetDirectories(folders[i], "*", System.IO.SearchOption.AllDirectories);

			string[] files = System.IO.Directory.GetFiles(folders[i], "*", System.IO.SearchOption.TopDirectoryOnly);
			for (int j = 0; j < files.Length; j++) {
				string ext = System.IO.Path.GetExtension(files[j]);
				if (ext != ".cs") {
					continue;
				}

				string fileName = files[j];
				StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("UTF-8"));
				string code = sr.ReadToEnd();
				sr.Close();

				Debug.Log(fileName);

				string output = CheckCode(code);
				Debug.Log("=====Last");
				Debug.Log(output);
				if (string.IsNullOrEmpty(output))
				{
					output = "問題ありません";
				}
				string filePathAndName = files[j].Replace("\\", "/");
				string[] stringList = filePathAndName.Split('/');
				string tag = stringList[stringList.Length-1].Replace(".cs", "");
				string resultFile = "TargetCs/Result/" + tag + "Result.txt";
				File.WriteAllText(resultFile, output);
			}
		}

        //SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        //var root = tree.GetCompilationUnitRoot();
		//
		////// TOPレベルのUsing
		////SyntaxList<UsingDirectiveSyntax> usingDirectives = root.Usings;

		////// 全Using走査
		////for (int i = 0; i < usingDirectives.Count; i++) {
		////	Debug.Log(usingDirectives[i]);
		////}

		//// ネームスペース（次の階層まとまり）を取得
		//MemberDeclarationSyntax firstMember = root.Members[0];
		//var nameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;

		//// ネームスペース内を走査
		//var classDeclaration = (ClassDeclarationSyntax)nameSpaceDeclaration.Members[0];


		//// ※調査の際に、型タイプ調べる際の方法
		////Debug.Log(methodList[i].Identifier.GetType());
        //
		//// 関数部分のチェック
		//Debug.Log("=====methodList=====");
		//var methodList = classDeclaration.DescendantNodes()
		//					.OfType<MethodDeclarationSyntax>().ToList();
		//
		//string methodLogString = CheckMethod(methodList);
		//Debug.Log(methodLogString);
        //
		//Debug.Log("=====propertyList=====");
		//var propertyList = classDeclaration.DescendantNodes()
		//					.OfType<PropertyDeclarationSyntax>().ToList();
		//
		//string propertyLogString = CheckProperty(propertyList);
		//Debug.Log(propertyLogString);

		//Debug.Log("=====fieldList=====");
		//var fieldList = classDeclaration.DescendantNodes()
		//					.OfType<FieldDeclarationSyntax>().ToList();
		//string fieldLogString = CheckField(fieldList);
		//Debug.Log(fieldLogString);
		////for (int i = 0; i < propertyList.Count; i++) {
		////CheckProperty
		////	Debug.Log(propertyList[i].Identifier.ValueText);
		////}
		////
		////Debug.Log("=====typeList=====");
		////var typeList = classDeclaration.DescendantNodes()
		////					.OfType<TypeDeclarationSyntax>().ToList();
		////for (int i = 0; i < typeList.Count; i++) {
		////	Debug.Log(typeList[i].Identifier.ValueText);
		////}
		////
		////Debug.Log("=====fieldList=====");
		////var fieldList = classDeclaration.DescendantNodes()
		////					.OfType<FieldDeclarationSyntax>().ToList();
		////for (int i = 0; i < fieldList.Count; i++) {
		////	Debug.Log(fieldList[i]);

		////	// ※これでコメント取れる
		////	if (fieldList[i].HasLeadingTrivia == true)
		////	{
		////		var trivia = fieldList[i].GetLeadingTrivia();
		////		Debug.Log("=====true=====");
		////		Debug.Log(trivia);
		////	}

		////	// ※これで変数名取れる
		////	var syntaxList = fieldList[i].DescendantNodes().ToList();
		////	for (int i2 = 0; i2 < syntaxList.Count; i2++) {
		////		Debug.Log(syntaxList[i2]);
		////	}
		////}
    }
	
    private static string CheckCode(string code)
    {
		Debug.Log(code);
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();
		
		//// TOPレベルのUsing
		//SyntaxList<UsingDirectiveSyntax> usingDirectives = root.Usings;

		//// 全Using走査
		//for (int i = 0; i < usingDirectives.Count; i++) {
		//	Debug.Log(usingDirectives[i]);
		//}

		string output = string.Empty;

		// ネームスペース（次の階層まとまり）を取得
		Debug.Log(root.Members[0]);
		for (int i = 0; i < root.Members.Count; i++)
		{
			MemberDeclarationSyntax member = root.Members[i];
			if (member is NamespaceDeclarationSyntax)
			{
				var nameSpaceDeclaration = (NamespaceDeclarationSyntax)member;
				for (int i2 = 0; i2 < nameSpaceDeclaration.Members.Count; i2++)
				{
					// namespace階層は、一つまで
					var classDeclaration = (ClassDeclarationSyntax)nameSpaceDeclaration.Members[i2];
					output += CheckClass(classDeclaration);
				}
				Debug.Log("nameSpaceDeclaration");
			}
			
			if (member is ClassDeclarationSyntax)
			{
				var classDeclaration = (ClassDeclarationSyntax)member;
				output += CheckClass(classDeclaration);
				Debug.Log("classDeclaration");
			}
		}
		//MemberDeclarationSyntax firstMember = root.Members[0];
		//var nameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;

		//// ネームスペース内を走査
		//var classDeclaration = (ClassDeclarationSyntax)nameSpaceDeclaration.Members[0];


		//// ※調査の際に、型タイプ調べる際の方法
		////Debug.Log(methodList[i].Identifier.GetType());
        //
		//// 関数部分のチェック
		//Debug.Log("=====methodList=====");
		//output += "■関数定義部分\n";
		//var methodList = classDeclaration.DescendantNodes()
		//					.OfType<MethodDeclarationSyntax>().ToList();
		//
		//string methodLogString = CheckMethod(methodList);
		//output += methodLogString;
		//Debug.Log(methodLogString);
        //
		//Debug.Log("=====propertyList=====");
		//output += "■プロパティ定義部分\n";
		//var propertyList = classDeclaration.DescendantNodes()
		//					.OfType<PropertyDeclarationSyntax>().ToList();
		//
		//string propertyLogString = CheckProperty(propertyList);
		//output += propertyLogString;
		//Debug.Log(propertyLogString);

		//Debug.Log("=====fieldList=====");
		//output += "■フィールド定義部分\n";
		//var fieldList = classDeclaration.DescendantNodes()
		//					.OfType<FieldDeclarationSyntax>().ToList();
		//string fieldLogString = CheckField(fieldList);
		//output += fieldLogString;
		//Debug.Log(fieldLogString);

		return output;
    }
    
	private static string CheckClass(ClassDeclarationSyntax classDeclaration)
    {
		string output = string.Empty;

		// ※調査の際に、型タイプ調べる際の方法
		//Debug.Log(methodList[i].Identifier.GetType());
        

		Debug.Log("=====constructorList=====");
		output += "■コンストラクタ定義部分\n";
		var constructorList = classDeclaration.DescendantNodes()
							.OfType<ConstructorDeclarationSyntax>().ToList();

		string constructorLogString = CheckConstructor(constructorList);
		output += constructorLogString;
		Debug.Log(constructorLogString);

		// 関数部分のチェック
		Debug.Log("=====methodList=====");
		output += "■関数定義部分\n";
		var methodList = classDeclaration.DescendantNodes()
							.OfType<MethodDeclarationSyntax>().ToList();
		
		string methodLogString = CheckMethod(methodList);
		output += methodLogString;
		Debug.Log(methodLogString);
        
		Debug.Log("=====propertyList=====");
		output += "■プロパティ定義部分\n";
		var propertyList = classDeclaration.DescendantNodes()
							.OfType<PropertyDeclarationSyntax>().ToList();
		
		string propertyLogString = CheckProperty(propertyList);
		output += propertyLogString;
		Debug.Log(propertyLogString);

		Debug.Log("=====fieldList=====");
		output += "■フィールド定義部分\n";
		var fieldList = classDeclaration.DescendantNodes()
							.OfType<FieldDeclarationSyntax>().ToList();
		string fieldLogString = CheckField(fieldList);
		output += fieldLogString;
		Debug.Log(fieldLogString);

		return output;
    }
	
	// コンストラクタチェック
	public static string CheckConstructor(List<ConstructorDeclarationSyntax> constructorList)
    {
		string constructorLogString = string.Empty;
		// 関数部分のチェック
		for (int i = 0; i < constructorList.Count; i++) {

			constructorLogString += "メソッド名：" + constructorList[i].Identifier.ValueText + "\n";

			string constructorName = constructorList[i].Identifier.ValueText;
			
			// 引数調べたい
			var parameterTexts = GetMethodParameters(constructorList[i]);
			for (int i2 = 0; i2 < parameterTexts.Length; i2++) {
				string name = parameterTexts[i2].Identifier.ValueText;
				string fullName = parameterTexts[i2].ToString();
				char c1 = name[0];
				if (char.IsLower(c1) != true)
				{
					constructorLogString += "引数" + fullName + "の先頭文字は、小文字にしてください\n";
				}
			}
			
			// 引数コメント確認用書庫
			var parameterDict = new Dictionary<string, bool>();
			for (int i3 = 0; i3 < parameterTexts.Length; i3++) {
				parameterDict.Add(parameterTexts[i3].Identifier.ValueText, false);
			}

			// コメント調べたい
			if (constructorList[i].HasLeadingTrivia == true)
			{
				var commentSyntaxTriviaArray = GetConstructorComments(constructorList[i]);
				if (commentSyntaxTriviaArray.Length == 0)
				{
					constructorLogString += "コメントが無いので、記載してください\n";
				}
				else
				{
					string[] lineList = commentSyntaxTriviaArray[0].ToString().Split('\n');
					bool findSummary = false;
					bool findInheritdoc = false;
					for (int i2 = 0; i2 < lineList.Length; i2++)
					{
						if (lineList[i2].ToString().Contains("inheritdoc"))
						{
							// 継承元のコメントを参照するはずなので、他のコメントチェックルールは無視
							findInheritdoc = true;
							break;
						}
						else
						{
							if (lineList[i2].ToString().Contains("<summary>"))
							{
								findSummary = true;
							}
							
							if (lineList[i2].ToString().Contains("<param name"))
							{
								var parameterComments = Regex.Matches(lineList[i2].ToString(), "\"(.+?)\"");
								if (parameterComments.Count > 0)
								{
									string comment = parameterComments[0].ToString().Replace("\"", "");
									if (parameterDict.ContainsKey(comment))
									{
										parameterDict[comment] = true;
									}
								}
							}
						}
					}

					if (findInheritdoc == true)
					{
						// 継承元のコメントを参照するはずなので、他のコメントチェックルールは無視
					}
					else
					{
						if (findSummary == false)
						{
							constructorLogString += "doc形式のコメントが無いので、<summary>を記載してください\n";
						}
						
						foreach (var data in parameterDict)
						{
							if (data.Value == false)
							{
								constructorLogString += "引数" + data.Key + "のコメントが無いので、記載してください\n";
							}
						}
					}
				}
			}
			else
			{
				constructorLogString += "コメントが無いので、記載してください\n";
			}
			
		}
		return constructorLogString;
	}
	
	// メソッドチェック
	public static string CheckMethod(List<MethodDeclarationSyntax> methodList)
    {
		string methodLogString = string.Empty;
		// 関数部分のチェック
		for (int i = 0; i < methodList.Count; i++) {

			string returnType = methodList[i].ReturnType.ToString();

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
				if (char.IsUpper(c1) != true)
				{
					methodLogString += "protected属性のメソッド名は、先頭大文字にしてください\n";
				}
			}
			else if (modifierTexts.Contains("private"))
			{
				char c1 = methodName[0];
				if (char.IsUpper(c1) != true)
				{
					methodLogString += "private属性のメソッド名は、先頭大文字にしてください\n";
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

			// 引数コメント確認用書庫
			var parameterDict = new Dictionary<string, bool>();
			for (int i3 = 0; i3 < parameterTexts.Length; i3++) {
				parameterDict.Add(parameterTexts[i3].Identifier.ValueText, false);
			}

			// コメント調べたい
			if (methodList[i].HasLeadingTrivia == true)
			{
				var commentSyntaxTriviaArray = GetMethodComments(methodList[i]);
				if (commentSyntaxTriviaArray.Length == 0)
				{
					methodLogString += "コメントが無いので、記載してください\n";
				}
				else
				{
					string[] lineList = commentSyntaxTriviaArray[0].ToString().Split('\n');
					bool findSummary = false;
					bool findInheritdoc = false;
					for (int i2 = 0; i2 < lineList.Length; i2++)
					{
						if (lineList[i2].ToString().Contains("inheritdoc"))
						{
							// 継承元のコメントを参照するはずなので、他のコメントチェックルールは無視
							findInheritdoc = true;
							break;
						}
						else
						{
							if (lineList[i2].ToString().Contains("<summary>"))
							{
								findSummary = true;
							}
							
							if (lineList[i2].ToString().Contains("<param name"))
							{
								var parameterComments = Regex.Matches(lineList[i2].ToString(), "\"(.+?)\"");
								if (parameterComments.Count > 0)
								{
									string comment = parameterComments[0].ToString().Replace("\"", "");
									if (parameterDict.ContainsKey(comment))
									{
										parameterDict[comment] = true;
									}
								}
							}
						}
					}

					bool findReturnParam = false;

					if (returnType == "void")
					{
						// 戻り値無しの場合は、戻り値用コメントのチェックは不要
						findReturnParam = true;
					}

					if (findInheritdoc == true)
					{
						// 継承元のコメントを参照するはずなので、他のコメントチェックルールは無視
					}
					else
					{
						if (findSummary == false)
						{
							methodLogString += "doc形式のコメントが無いので、<summary>を記載してください\n";
						}
						
						if (findReturnParam == false)
						{
							methodLogString += "戻り値のコメントが無いので、<returns>を記載してください\n";
						}
						
						foreach (var data in parameterDict)
						{
							if (data.Value == false)
							{
								methodLogString += "引数" + data.Key + "のコメントが無いので、記載してください\n";
							}
						}
					}
				}
			}
			else
			{
				methodLogString += "コメントが無いので、記載してください\n";
			}
		}
		return methodLogString;
	}
	
	// プロパティチェック
	public static string CheckProperty(List<PropertyDeclarationSyntax> propertyList)
    {
		string propertyLogString = string.Empty;
		
		// 関数部分のチェック
		for (int i = 0; i < propertyList.Count; i++) {

			string returnType = propertyList[i].Type.ToString();

			propertyLogString += "プロパティ名：" + propertyList[i].Identifier.ValueText + "\n";

			string propertyName = propertyList[i].Identifier.ValueText;
			
			// private,public,protected/async,virtualの確認
			var modifierTexts = GetPropertyModifiers(propertyList[i]);
			if (modifierTexts.Contains("public"))
			{
				char c1 = propertyName[0];
				if (char.IsUpper(c1) != true)
				{
					propertyLogString += "public属性のプロパティ名は、先頭大文字にしてください\n";
				}
			}
			else if (modifierTexts.Contains("protected"))
			{
				char c1 = propertyName[0];
				if (char.IsLower(c1) != true)
				{
					propertyLogString += "protected属性のプロパティ名は、先頭小文字にしてください\n";
				}
			}
			else if (modifierTexts.Contains("private"))
			{
				propertyLogString += "private属性のプロパティです。使用しているか、確認してください\n";
			}
			else
			{
				// privateと同じ扱い
				propertyLogString += "private属性のプロパティです。使用しているか、確認してください\n";
			}

			// プロパティでasyncはつけないのでは。
			if (modifierTexts.Contains("async"))
			{
				propertyLogString += "【警告】Asyncに対するチェックは未実装";
				//if (!propertyName.EndsWith("Async"))
				//{
				//	propertyLogString += "async属性のメソッド名には、末尾にAsyncをつけてください\n";
				//}
			}
			
			//// プロパティに引数は存在しないのでは
			//var parameterTexts = GetPropertyParameters(propertyList[i]);
			//for (int i2 = 0; i2 < parameterTexts.Length; i2++) {
			//	string name = parameterTexts[i2].Identifier.ValueText;
			//	string fullName = parameterTexts[i2].ToString();
			//	char c1 = name[0];
			//	if (char.IsLower(c1) != true)
			//	{
			//		propertyLogString += "引数" + fullName + "の先頭文字は、小文字にしてください\n";
			//	}
			//}

			// コメント調べたい
			if (propertyList[i].HasLeadingTrivia == true)
			{
				var commentSyntaxTriviaArray = GetPropertyComments(propertyList[i]);
				if (commentSyntaxTriviaArray.Length == 0)
				{
					propertyLogString += "コメントが無いので、記載してください\n";
				}
				else
				{
					if (commentSyntaxTriviaArray[0].ToString().Contains("inheritdoc"))
					{
						// 継承元のコメントを参照するはずなので、他のコメントチェックルールは無視
					}
					else
					{
						// 引数無いはずなので、コメントアウト
						//// 引数コメント確認用書庫
						//var parameterDict = new Dictionary<string, bool>();
						//for (int i2 = 0; i2 < parameterTexts.Length; i2++) {
						//	parameterDict.Add(parameterTexts[i2].Identifier.ValueText, false);
						//}

						bool findSummary = false;
						bool findReturnParam = false;

						if (returnType == "void")
						{
							// 戻り値無しの場合は、戻り値用コメントのチェックは不要
							findReturnParam = true;
						}

						// Doc形式かどうかと、引数が変数名分あるか、戻り値の説明があるか
						for (int i2 = 0; i2 < commentSyntaxTriviaArray.Length; i2++) {
							if (commentSyntaxTriviaArray[i2].ToString().Contains("<summary>"))
							{
								findSummary = true;
							}
							
							if (commentSyntaxTriviaArray[i2].ToString().Contains("<returns>"))
							{
								findReturnParam = true;
							}
							
							// 引数無いはずなので、コメントアウト
							//if (commentSyntaxTriviaArray[i2].ToString().Contains("<param name"))
							//{
							//	var parameterComments = Regex.Matches(commentSyntaxTriviaArray[i2].ToString(), "\"(.+?)\"");

							//	if (parameterComments.Count > 0)
							//	{
							//		string comment = parameterComments[0].ToString().Replace("\"", "");
							//		if (parameterDict.ContainsKey(comment))
							//		{
							//			parameterDict[comment] = true;
							//		}
							//	}
							//}
						}

						if (findSummary == false)
						{
							propertyLogString += "doc形式のコメントが無いので、<summary>を記載してください\n";
						}
						
						if (findReturnParam == false)
						{
							propertyLogString += "戻り値のコメントが無いので、<returns>を記載してください\n";
						}
						
						// 引数無いはずなので、コメントアウト
						//foreach (var data in parameterDict)
						//{
						//	if (data.Value == false)
						//	{
						//		propertyLogString += "引数" + data.Key + "のコメントが無いので、記載してください\n";
						//	}
						//}
					}
				}
			}
			else
			{
				propertyLogString += "コメントが無いので、記載してください\n";
			}
			
		}
		return propertyLogString;
	}
	
	// フィールドチェック
	public static string CheckField(List<FieldDeclarationSyntax> fieldList)
    {
		string fieldLogString = string.Empty;
		
		for (int i = 0; i < fieldList.Count; i++) {

			fieldLogString += "フィールド名：" + fieldList[i] + "\n";

			string returnType = fieldList[i].Declaration.Type.ToString();
			var variables = fieldList[i].Declaration.Variables.ToList();

			// private,public,protected/async,virtualの確認
			var modifierTexts = GetFieldModifiers(fieldList[i]);
			if (modifierTexts.Contains("public"))
			{
				char c1 = variables[0].Identifier.ToString()[0];
				if (char.IsUpper(c1) != true)
				{
					fieldLogString += "public属性のフィールド名は、先頭大文字にしてください\n";
				}
			}
			else if (modifierTexts.Contains("protected"))
			{
				char c1 = variables[0].Identifier.ToString()[0];
				if (char.IsLower(c1) != true)
				{
					fieldLogString += "protected属性のフィールド名は、先頭小文字にしてください\n";
				}
			}
			else if (modifierTexts.Contains("private"))
			{
				char c1 = variables[0].Identifier.ToString()[0];
				if (char.IsLower(c1) != true)
				{
					fieldLogString += "private属性のフィールド名は、先頭小文字にしてください\n";
				}
			}
			else
			{
				// privateと同じ扱い
				char c1 = variables[0].Identifier.ToString()[0];
				if (char.IsLower(c1) != true)
				{
					fieldLogString += "private属性のフィールド名は、先頭小文字にしてください\n";
				}
			}

			// フィールドでasyncはつけないのでは。
			if (modifierTexts.Contains("async"))
			{
				fieldLogString += "【警告】Asyncに対するチェックは未実装";
				//if (!fieldName.EndsWith("Async"))
				//{
				//	fieldLogString += "async属性のメソッド名には、末尾にAsyncをつけてください\n";
				//}
			}
			
			//// フィールドに引数は存在しないのでは
			//var parameterTexts = GetfieldParameters(fieldList[i]);
			//for (int i2 = 0; i2 < parameterTexts.Length; i2++) {
			//	string name = parameterTexts[i2].Identifier.ValueText;
			//	string fullName = parameterTexts[i2].ToString();
			//	char c1 = name[0];
			//	if (char.IsLower(c1) != true)
			//	{
			//		fieldLogString += "引数" + fullName + "の先頭文字は、小文字にしてください\n";
			//	}
			//}

			// コメント調べたい
			if (fieldList[i].HasLeadingTrivia == true)
			{
				var commentSyntaxTriviaArray = GetFieldComments(fieldList[i]);
				if (commentSyntaxTriviaArray.Length == 0)
				{
					fieldLogString += "コメントが無いので、記載してください\n";
				}
				else
				{
					if (commentSyntaxTriviaArray[0].ToString().Contains("inheritdoc"))
					{
						// 継承元のコメントを参照するはずなので、他のコメントチェックルールは無視
					}
					else
					{
						// 引数無いはずなので、コメントアウト
						//// 引数コメント確認用書庫
						//var parameterDict = new Dictionary<string, bool>();
						//for (int i2 = 0; i2 < parameterTexts.Length; i2++) {
						//	parameterDict.Add(parameterTexts[i2].Identifier.ValueText, false);
						//}

						bool findSummary = false;
						bool findReturnParam = false;

						if (returnType == "void")
						{
							// 戻り値無しの場合は、戻り値用コメントのチェックは不要
							findReturnParam = true;
						}

						// Doc形式かどうかと、引数が変数名分あるか、戻り値の説明があるか
						for (int i2 = 0; i2 < commentSyntaxTriviaArray.Length; i2++) {
							if (commentSyntaxTriviaArray[i2].ToString().Contains("<summary>"))
							{
								findSummary = true;
							}
							
							if (commentSyntaxTriviaArray[i2].ToString().Contains("<returns>"))
							{
								findReturnParam = true;
							}
							
							// 引数無いはずなので、コメントアウト
							//if (commentSyntaxTriviaArray[i2].ToString().Contains("<param name"))
							//{
							//	var parameterComments = Regex.Matches(commentSyntaxTriviaArray[i2].ToString(), "\"(.+?)\"");

							//	if (parameterComments.Count > 0)
							//	{
							//		string comment = parameterComments[0].ToString().Replace("\"", "");
							//		if (parameterDict.ContainsKey(comment))
							//		{
							//			parameterDict[comment] = true;
							//		}
							//	}
							//}
						}

						if (findSummary == false)
						{
							fieldLogString += "doc形式のコメントが無いので、<summary>を記載してください\n";
						}
						
						if (findReturnParam == false)
						{
							fieldLogString += "戻り値のコメントが無いので、<returns>を記載してください\n";
						}
						
						// 引数無いはずなので、コメントアウト
						//foreach (var data in parameterDict)
						//{
						//	if (data.Value == false)
						//	{
						//		fieldLogString += "引数" + data.Key + "のコメントが無いので、記載してください\n";
						//	}
						//}
					}
				}
			}
			else
			{
				fieldLogString += "コメントが無いので、記載してください\n";
			}
			
		}
		return fieldLogString;
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
	
	// コメントの取得
	public static SyntaxTrivia[] GetPropertyComments(SyntaxNode propertyNode)
    {
		var triviaList = propertyNode.GetLeadingTrivia();
		var commentSyntaxTriviaArray = triviaList.Where(trivia => (!trivia.IsKind(SyntaxKind.WhitespaceTrivia)) && (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))).ToArray();

		return commentSyntaxTriviaArray;
	}
	
	// private/punlic等の修飾子取得
	public static string[] GetPropertyModifiers(BasePropertyDeclarationSyntax propertyNode)
    {
		var modifierTexts = propertyNode.Modifiers.Select(x => x.Text).ToArray();
		return modifierTexts;
	}
	
	// コメントの取得
	public static SyntaxTrivia[] GetFieldComments(SyntaxNode FieldNode)
    {
		var triviaList = FieldNode.GetLeadingTrivia();
		var commentSyntaxTriviaArray = triviaList.Where(trivia => (!trivia.IsKind(SyntaxKind.WhitespaceTrivia)) && (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))).ToArray();

		return commentSyntaxTriviaArray;
	}
	
	// private/punlic等の修飾子取得
	public static string[] GetFieldModifiers(BaseFieldDeclarationSyntax fieldNode)
    {
		var modifierTexts = fieldNode.Modifiers.Select(x => x.Text).ToArray();
		return modifierTexts;
	}
	
	// コメントの取得
	public static SyntaxTrivia[] GetConstructorComments(SyntaxNode constructorNode)
    {
		var triviaList = constructorNode.GetLeadingTrivia();
		var commentSyntaxTriviaArray = triviaList.Where(trivia => (!trivia.IsKind(SyntaxKind.WhitespaceTrivia)) && (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))).ToArray();

		return commentSyntaxTriviaArray;
	}
}
