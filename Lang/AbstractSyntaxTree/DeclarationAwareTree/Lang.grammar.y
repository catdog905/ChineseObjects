%namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree
%partial
%parsertype LangParser
%visibility internal
%tokentype Token

%union {
    public Statement.Expression.NumLiteral num_literal;
    public Statement.Expression.BoolLiteral bool_literal;
    public Statement.Expression.Identifier identifier;
    public Statement.Expression.Identifiers identifiers;
    public Declaration.This thisRef;

    /* More specific methods shall use more specific fields,
     * such as `.num_literal`, `.identifier`, etc. On a higher
     * level, the resulting expression is then assigned to
     * `.obj` for higher `AST` nodes which don't care about
     * the concrete type of expression.
     */
    public Statement.Expression.IExpression expr;

    public Statement.Return ret;
    public Statement.Assignment assign;
    public Statement.IfElse ifelse;
    public Statement.While while_;
    public Statement.StatementsBlock body;
    public Statement.Expression.MethodCall methodCall;
    public Statement.Expression.ClassInstantiation classInstantiation;

    public Statement.IStatement stmt;

    public Declaration.Parameter.Parameter param;
    public Declaration.Parameter.Parameters parames;

    public Statement.Expression.Argument argument;
    public Statement.Expression.Arguments arguments;

    public Declaration.Program program;
    public Declaration.ClassDeclaration classDeclaration;
    public Declaration.IMemberDeclaration memberDeclaration;
    public Declaration.MemberDeclarations memberDeclarations;
    public Declaration.VariableDeclaration variableDeclaration;
    public Declaration.MethodDeclaration methodDeclaration;
    public Declaration.ConstructorDeclaration constructorDeclaration;
}

%start program

%token IDENTIFIER, OP_PLUS, OP_MINUS, OP_MULT, OP_DIV, P_OPEN, P_CLOSE, COLON, DOT, COMMA
%token OP_MOD, OP_EQUAL, OP_LESS, OP_GREATER, OP_LESS_EQUAL, OP_GREATER_EQUAL
%token IS, END, LOOP, THEN, RETURN, ELSE, WHILE, ASSIGN, IF
%token VAR, METHOD
%token CLASS, EXTENDS, THIS, NEW
%token INTEGER_LITERAL, REAL_LITERAL, BOOLEAN_LITERAL

%%

program : classDeclaration               { $$.program = new Declaration.Program( $1.classDeclaration ); }
        | program classDeclaration       { $$.program = new Declaration.Program( $1.program, $2.classDeclaration ); }
        ;

classDeclaration : CLASS IDENTIFIER IS memberDeclarations END                     { $$.classDeclaration = new Declaration.ClassDeclaration($2.identifier, $4.memberDeclarations); }
                 | CLASS IDENTIFIER EXTENDS identifiers IS memberDeclarations END { $$.classDeclaration = new Declaration.ClassDeclaration($2.identifier, $4.identifiers, $6.memberDeclarations); }
           	 ;

identifiers : identifiers COMMA IDENTIFIER        { $$.identifiers = new Statement.Expression.Identifiers( $1.identifiers, $3.identifier ); }
            | IDENTIFIER                        { $$.identifiers = new Statement.Expression.Identifiers( $1.identifier ); }
            ;

memberDeclarations :                                      { $$.memberDeclarations = new Declaration.MemberDeclarations(); }
                   | memberDeclarations memberDeclaration { $$.memberDeclarations = new Declaration.MemberDeclarations( $1.memberDeclarations, $2.memberDeclaration ); }
                   ;

memberDeclaration : variableDeclaration    { $$.memberDeclaration = $1.variableDeclaration; }
                  | methodDeclaration      { $$.memberDeclaration = $1.methodDeclaration; }
                  | constructorDeclaration { $$.memberDeclaration = $1.constructorDeclaration; }
                  ;

variableDeclaration : VAR IDENTIFIER COLON IDENTIFIER     { $$.variableDeclaration = new Declaration.VariableDeclaration( $2.identifier, $4.identifier ); }
                    ;

methodDeclaration   : METHOD IDENTIFIER P_OPEN parameters P_CLOSE COLON IDENTIFIER IS body END
                                                    { $$.methodDeclaration = new Declaration.MethodDeclaration( $2.identifier, $4.parames, $7.identifier, $9.body ); }
                    ;

constructorDeclaration : THIS P_OPEN parameters P_CLOSE IS body END  { $$.constructorDeclaration = new Declaration.ConstructorDeclaration( $3.parames, $6.body ); }
                       ;

parameter   : IDENTIFIER COLON IDENTIFIER        { $$.param = new Declaration.Parameter.Parameter( $1.identifier, $3.identifier ); }
            ;

parameters  :                                { $$.parames = new Declaration.Parameter.Parameters(); }
            | parameters COMMA parameter       { $$.parames = new Declaration.Parameter.Parameters( $1.parames, $3.param ); }
            | parameter                      { $$.parames = new Declaration.Parameter.Parameters( $1.param ); }
            ;

body : body statement             { $$.body = new Statement.StatementsBlock( $1.body, $2.stmt); }
     | statement                  { $$.body = new Statement.StatementsBlock( $1.stmt ); }
     ;

statement : assignment          { $$.stmt = $1.assign; }
          | whileLoop           { $$.stmt = $1.while_; }
          | ifStatement         { $$.stmt = $1.ifelse; }
          | returnStatement     { $$.stmt = $1.ret; } 
          | methodCall          { $$.stmt = $1.methodCall; }
          | expr                { $$.stmt = $1.expr; }
          ;


assignment  : IDENTIFIER COLON IDENTIFIER ASSIGN expr    { $$.assign = new Statement.Assignment($1.identifier, $5.expr, $3.identifier); }
            ;

whileLoop   : WHILE expr LOOP body END      { $$.while_ = new Statement.While($2.expr, $4.body); }
            ;

// TODO: thisRef may be a trap of if-else described in one of the lectures, isn't it?..
ifStatement : IF expr THEN body END             { $$.ifelse = new Statement.IfElse($2.expr, $4.body); }
            | IF expr THEN body ELSE body END   { $$.ifelse = new Statement.IfElse($2.expr, $4.body, $6.body); }
            ;

returnStatement : RETURN expr			{ $$.ret = new Statement.Return($2.expr); }
                ;
    
expr : methodCall            { $$.expr = $1.methodCall; }
     | classInstantiation    { $$.expr = $1.classInstantiation; }
     | INTEGER_LITERAL       { $$.expr = $1.num_literal; }
     | REAL_LITERAL          { $$.expr = $1.num_literal; }
     | BOOLEAN_LITERAL       { $$.expr = $1.bool_literal; }
     | THIS                  { $$.expr = $1.thisRef; }
     | IDENTIFIER            { $$.expr = new Statement.Expression.Reference($1.identifier); }
     ;

methodCall : expr DOT IDENTIFIER P_OPEN arguments P_CLOSE  { $$.methodCall = new Statement.Expression.MethodCall( $1.expr, $3.identifier, $5.arguments ); }
           ;

arguments :                           { $$.arguments = new Statement.Expression.Arguments(); }
          | arguments COMMA argument    { $$.arguments = new Statement.Expression.Arguments( $1.arguments, $3.argument ); }
          | argument                  { $$.arguments = new Statement.Expression.Arguments( $1.argument ); }
          ;

argument : expr  { $$.argument = new Statement.Expression.Argument( $1.expr ); }
         ;

classInstantiation : NEW IDENTIFIER P_OPEN arguments P_CLOSE    { $$.classInstantiation = new Statement.Expression.ClassInstantiation( $2.identifier, $4.arguments ); }
                   ;

%%
