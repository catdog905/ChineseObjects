%namespace ChineseObjects.Lang
%scannertype LangScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers

Space           [ \t]
IntegerLiteral  [0-9]+
RealLiteral     [0-9]+\.[0-9]
BooleanLiteral  (true|false)
OpPlus          \+
OpMinus         \-
OpMult          \*
OpDiv           \/
POpen           \(
PClose          \)
Colon           \:
Assign          :=
Is              is
End             end
Class           class
Extends         extends
Loop            loop
Then            then
Return          return
Else            else
Var             var
Method          method
While           while
If              if
This            this
New             new
Dot             \.
Identifier      [a-zA-z0-9]+
OpMod           %
OpEqual         ==
OpLess          <
OpGreater       >
OpLessEqual     <=
OpGreaterEqual  >=

%{

%}

%%

{Space}+        /* skip */

{OpPlus} 		{ Console.WriteLine("PlusToken: '{0}'", yytext);    return (int)Token.OP_PLUS; }
{OpMinus}       	{ Console.WriteLine("MinusToken: '{0}'", yytext);    return (int)Token.OP_MINUS; }
{OpMult}        	{ Console.WriteLine("MultiplicationToken: '{0}'", yytext);    return (int)Token.OP_MULT; }
{OpDiv}         	{ Console.WriteLine("DivisionToken: '{0}'", yytext);    return (int)Token.OP_DIV; }
{POpen}         	{ Console.WriteLine("OpenParenthesisToken: '{0}'", yytext);    return (int)Token.P_OPEN; }
{PClose}        	{ Console.WriteLine("ClosedParenthesisToken: '{0}'", yytext);    return (int)Token.P_CLOSE; }
{Colon}         	{ Console.WriteLine("ColonToken: '{0}'", yytext);    return (int)Token.COLON; }
{Is}            	{ Console.WriteLine("isKeywordToken: '{0}'", yytext);    return (int)Token.IS; }
{End}           	{ Console.WriteLine("endKeywordToken: '{0}'", yytext);    return (int)Token.END; }
{Class}         	{ Console.WriteLine("classKeywordToken: '{0}'", yytext);    return (int)Token.CLASS; }
{Extends}       	{ Console.WriteLine("extendsKeywordToken: '{0}'", yytext);    return (int)Token.EXTENDS; }
{Loop}          	{ Console.WriteLine("loopKeywordToken: '{0}'", yytext);    return (int)Token.LOOP; }
{Then}          	{ Console.WriteLine("thenKeywordToken: '{0}'", yytext);    return (int)Token.THEN; }
{Return}        	{ Console.WriteLine("ReturnKeywordToken: '{0}'", yytext);    return (int)Token.RETURN; }
{Else}          	{ Console.WriteLine("elseKeywordToken: '{0}'", yytext);    return (int)Token.ELSE; }
{Var}           	{ Console.WriteLine("varKeywordToken: '{0}'", yytext);    return (int)Token.VAR; }
{Method}        	{ Console.WriteLine("methodKeywordToken: '{0}'", yytext);    return (int)Token.METHOD; }
{While}         	{ Console.WriteLine("whileKeywordToken: '{0}'", yytext);    return (int)Token.WHILE; }
{Assign}        	{ Console.WriteLine("AssignToken: '{0}'", yytext);    return (int)Token.ASSIGN; }
{If}            	{ Console.WriteLine("ifKeywordToken: '{0}'", yytext);    return (int)Token.IF; }
{This}          	{ Console.WriteLine("thisKeywordToken: '{0}'", yytext);    return (int)Token.THIS; }
{IntegerLiteral} 	{ Console.WriteLine("IntegerToken: '{0}'", yytext);    return (int)Token.INTEGER_LITERAL; }
{RealLiteral}   	{ Console.WriteLine("RealToken: '{0}'", yytext);    return (int)Token.REAL_LITERAL; }
{BooleanLiteral} 	{ Console.WriteLine("BoolToken: '{0}'", yytext);    return (int)Token.BOOLEAN_LITERAL; }
{New}           	{ Console.WriteLine("newKeywordToken: '{0}'", yytext);    return (int)Token.NEW; }
{Dot}           	{ Console.WriteLine("DotToken: '{0}'", yytext);    return (int)Token.DOT; }
{Identifier}    	{ Console.WriteLine("IdentifierToken: '{0}'", yytext);    return (int)Token.IDENTIFIER; }
{OpMod} 		{ Console.WriteLine("ModuloToken: '{0}'", yytext);    return (int)Token.OP_MOD; }
{OpEqual} 		{ Console.WriteLine("EqualToken: '{0}'", yytext);    return (int)Token.OP_EQUAL; }
{OpLess} 		{ Console.WriteLine("LessToken: '{0}'", yytext);    return (int)Token.OP_LESS; }
{OpGreater} 		{ Console.WriteLine("GreaterToken: '{0}'", yytext);    return (int)Token.OP_GREATER; }
{OpLessEqual} 		{ Console.WriteLine("LessEqualToken: '{0}'", yytext);    return (int)Token.OP_LESS_EQUAL; }
{OpGreaterEqual} 	{ Console.WriteLine("GreaterEqualToken: '{0}'", yytext);    return (int)Token.OP_GREATER_EQUAL; }

%%
