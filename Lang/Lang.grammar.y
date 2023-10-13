%namespace ChineseObjects.Lang
%partial
%parsertype LangParser
%visibility internal
%tokentype Token

%union {
    public NumLiteral num_literal;
    public BoolLiteral bool_literal;
    public Identifier identifier;

    /* More specific methods shall use more specific fields,
     * such as `.num_literal`, `.identifier`, etc. On a higher
     * level, the resulting expression is then assigned to
     * `.expr` for higher `AST` nodes which don't care about
     * the concrete type of expression.
     */
    public Expression expr;

    public Return ret;
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

// TODO: do we want to convert these to expressions?
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

returnStatement : RETURN expr			{ $$.ret = new Return($1.expr); }
                ;
    
expr   : expr DOT methodCall            { /* TODO */ }
       | primary                        { $$.expr = $1.expr; }
       ;

methodCall : IDENTIFIER P_OPEN arguments P_CLOSE;

arguments :
          | argument ',' arguments
          | argument
          ;

argument : expr;

primary : classInstantiation            { /* TODO. Is it a separate kind of expression? I think yes. */ }
        | INTEGER_LITERAL               { $$.expr = $1.num_literal; }
        | REAL_LITERAL                  { $$.expr = $1.num_literal; }
        | BOOLEAN_LITERAL               { $$.expr = $1.bool_literal; }
        | THIS                          { /* TODO. Should it be a special kind of identifier? */ }
        | IDENTIFIER                    { $$.expr = $1.identifier; }
        ;

classInstantiation : NEW IDENTIFIER P_OPEN parameters P_CLOSE;

%%
