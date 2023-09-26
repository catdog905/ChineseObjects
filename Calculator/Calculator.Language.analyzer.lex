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
{Colon}	     	{ Console.WriteLine("token: {0}", yytext);		return (int)Token.COLON; }
{Is}		    { Console.WriteLine("token: {0}", yytext);		return (int)Token.IS; }
{End}		    { Console.WriteLine("token: {0}", yytext);		return (int)Token.END; }
{Class}		    { Console.WriteLine("token: {0}", yytext);		return (int)Token.CLASS; }
{Extends}		{ Console.WriteLine("token: {0}", yytext);		return (int)Token.EXTENDS; }
{Loop}	    	{ Console.WriteLine("token: {0}", yytext);		return (int)Token.LOOP; }
{Then}		    { Console.WriteLine("token: {0}", yytext);		return (int)Token.THEN; }
{Return}	    { Console.WriteLine("token: {0}", yytext);		return (int)Token.RETURN; }
{Else}	        { Console.WriteLine("token: {0}", yytext);		return (int)Token.ELSE; }
{Var}	        { Console.WriteLine("token: {0}", yytext);		return (int)Token.VAR; }
{Method}        { Console.WriteLine("token: {0}", yytext);		return (int)Token.METHOD; }
{While}         { Console.WriteLine("token: {0}", yytext);		return (int)Token.WHILE; }
{Assign}        { Console.WriteLine("token: {0}", yytext);		return (int)Token.ASSIGN; }
{If}            { Console.WriteLine("token: {0}", yytext);		return (int)Token.IF; }
{This}          { Console.WriteLine("token: {0}", yytext);		return (int)Token.THIS; }
{Identifier}    { Console.WriteLine("token: {0}", yytext);		return (int)Token.IDENTIFIER; }

%%