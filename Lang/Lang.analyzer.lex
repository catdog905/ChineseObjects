%namespace ChineseObjects.Lang
%scannertype LangScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers

Space           [ \t]
IntegerLiteral  [0-9]+
RealLiteral     ([0-9]+\.[0-9]*|\.[0-9]+)
BooleanLiteral  (true|false)
POpen           \(
PClose          \)
Colon           \:
Assign          :=
Is              is
End             end
Class           class
Extends         extends
If              if
Then            then
Else            else
Return          return
Var             var
Method          method
While           while
Loop            loop
This            this
New             new
Dot             \.
Identifier      [a-zA-z0-9]+

%{

%}

%%

/* Special characters */
{POpen}              { return (int)Token.P_OPEN; }
{PClose}             { return (int)Token.P_CLOSE; }
{Colon}              { return (int)Token.COLON; }
{Dot}                { return (int)Token.DOT; }
{Assign}             { return (int)Token.ASSIGN; }

/* Keywords */
{Class}              { return (int)Token.CLASS; }
{Extends}            { return (int)Token.EXTENDS; }
{Is}                 { return (int)Token.IS; }
{End}                { return (int)Token.END; }
{While}              { return (int)Token.WHILE; }
{Loop}               { return (int)Token.LOOP; }
{If}                 { return (int)Token.IF; }
{Then}               { return (int)Token.THEN; }
{Else}               { return (int)Token.ELSE; }
{Return}             { return (int)Token.RETURN; }
{Var}                { return (int)Token.VAR; }
{Method}             { return (int)Token.METHOD; }
{This}               { return (int)Token.THIS; }
{New}                { return (int)Token.NEW; }

/* Other */
{IntegerLiteral}     { yylval.num_literal = new NumLiteral(double.Parse(yytext)); return (int)Token.INTEGER_LITERAL; }
{RealLiteral}        { yylval.num_literal = new NumLiteral(double.Parse(yytext)); return (int)Token.REAL_LITERAL; }
{BooleanLiteral}     { yylval.bool_literal = new BoolLiteral(yytext[0] == 't');    return (int)Token.BOOLEAN_LITERAL; }
{Identifier}         { yylval.identifier = new Identifier(yytext); return (int)Token.IDENTIFIER; }

%%
