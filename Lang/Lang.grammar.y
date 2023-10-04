%namespace ChineseObjects.Lang
%partial
%parsertype LangParser
%visibility internal
%tokentype Token

%union {
            public int n;
            public string s;
       }

%start program

%token IDENTIFIER, OP_PLUS, OP_MINUS, OP_MULT, OP_DIV, P_OPEN, P_CLOSE, COLON, DOT
%token OP_MOD, OP_EQUAL, OP_LESS, OP_GREATER, OP_LESS_EQUAL, OP_GREATER_EQUAL
%token IS, END, LOOP, THEN, RETURN, ELSE, WHILE, ASSIGN, IF
%token VAR, METHOD
%token CLASS, EXTENDS, THIS, NEW
%token INTEGER_LITERAL, REAL_LITERAL, BOOLEAN_LITERAL

%%

program : classDeclaration               { }
        | classDeclaration program       { }
        ;

classDeclaration : CLASS IDENTIFIER IS memberDeclarations END {}
		 | CLASS IDENTIFIER EXTENDS identifiers IS memberDeclarations END {}
       		 ;

identifiers : IDENTIFIER ',' identifiers        {}
            | IDENTIFIER                            {}
            ;

memberDeclarations : memberDeclaration memberDeclarations {}
                   | memberDeclaration {}
                   ;

memberDeclaration : variableDeclaration    {}
                  | methodDeclaration      {}
                  | constructorDeclaration {}
                  ;

variableDeclaration : VAR IDENTIFIER COLON expr;

methodDeclaration : METHOD IDENTIFIER P_OPEN parameters P_CLOSE COLON IDENTIFIER IS body END;

constructorDeclaration : THIS P_OPEN parameters P_CLOSE IS body END;

parameters :
           | parameter ',' parameters
           | parameter
           ;

parameter : IDENTIFIER COLON IDENTIFIER;

body : variableDeclaration body
     | statement body
     | statement
     ;

statement : assignment
          | whileLoop
          | ifStatement
          | returnStatement
          ;

assignment : IDENTIFIER ASSIGN expr;

whileLoop : WHILE expr LOOP body END;

ifStatement : IF expr THEN body END
            | IF expr THEN body ELSE body END
            ;

returnStatement : RETURN expr;

expr   : expr DOT methodCall            {      }
       | primary                        {      }
       ;

methodCall : IDENTIFIER P_OPEN arguments P_CLOSE;

arguments :
          | argument ',' arguments
          | argument
          ;

argument : expr;

primary : classInstantiation             {}
        | INTEGER_LITERAL                {}
        | REAL_LITERAL                   {}
        | BOOLEAN_LITERAL                {}
        | THIS                           {}
        | IDENTIFIER  /* To be changed */
        ;

classInstantiation : NEW IDENTIFIER P_OPEN parameters P_CLOSE;

%%
