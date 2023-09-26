%namespace ChineseObjects.Calculator
%scannertype CalculatorScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Eol             (\r\n?|\n)
NotWh           [^ \t\r\n]
Space           [ \t]
Number          [0-9]+
OpPlus			\+
OpMinus			\-
OpMult			\*
OpDiv			\/
POpen			\(
PClose			\)
Is              is
End             end
Class           class
Extends         extends
Identifier      [a-zA-z0-9]+

%{

%}

%%

{Number}		{ Console.WriteLine("token: {0}", yytext);		GetNumber(); return (int)Token.NUMBER; }

{Space}+		/* skip */

{OpPlus}		{ Console.WriteLine("token: {0}", yytext);		return (int)Token.OP_PLUS; }
{OpMinus}		{ Console.WriteLine("token: {0}", yytext);		return (int)Token.OP_MINUS; }
{OpMult}		{ Console.WriteLine("token: {0}", yytext);		return (int)Token.OP_MULT; }
{OpDiv}			{ Console.WriteLine("token: {0}", yytext);		return (int)Token.OP_DIV; }
{POpen}			{ Console.WriteLine("token: {0}", yytext);		return (int)Token.P_OPEN; }
{PClose}		{ Console.WriteLine("token: {0}", yytext);		return (int)Token.P_CLOSE; }
{Is}		    { Console.WriteLine("token: {0}", yytext);		return (int)Token.IS; }
{End}		    { Console.WriteLine("token: {0}", yytext);		return (int)Token.END; }
{Class}		    { Console.WriteLine("token: {0}", yytext);		return (int)Token.CLASS; }
{Extends}		{ Console.WriteLine("token: {0}", yytext);		return (int)Token.EXTENDS; }
{Identifier}    { Console.WriteLine("token: {0}", yytext);		return (int)Token.IDENTIFIER; }

%%