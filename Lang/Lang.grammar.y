%namespace ChineseObjects.Lang
%partial
%parsertype LangParser
%visibility internal
%tokentype Token

%union {
    public NumLiteral num_literal;
    public BoolLiteral bool_literal;
    public Identifier identifier;
    public Identifiers identifiers;
    public This thisRef;

    /* More specific methods shall use more specific fields,
     * such as `.num_literal`, `.identifier`, etc. On a higher
     * level, the resulting expression is then assigned to
     * `.obj` for higher `AST` nodes which don't care about
     * the concrete type of expression.
     */
    public IExpression expr;

    public Return ret;
    public Assignment assign;
    public IfElse ifelse;
    public While while_;
    public StatementsBlock body;
    public MethodCall methodCall;
    public ClassInstantiation classInstantiation;

    public IStatement stmt;

    public Parameter param;
    public Parameters parames;

    public Argument argument;
    public Arguments arguments;

    public Program program;
    public ClassDeclaration classDeclaration;
    public IMemberDeclaration memberDeclaration;
    public MemberDeclarations memberDeclarations;
    public VariableDeclaration variableDeclaration;
    public MethodDeclaration methodDeclaration;
    public ConstructorDeclaration constructorDeclaration;
}

%start program

%token IDENTIFIER, OP_PLUS, OP_MINUS, OP_MULT, OP_DIV, P_OPEN, P_CLOSE, COLON, DOT, COMMA
%token OP_MOD, OP_EQUAL, OP_LESS, OP_GREATER, OP_LESS_EQUAL, OP_GREATER_EQUAL
%token IS, END, LOOP, THEN, RETURN, ELSE, WHILE, ASSIGN, IF
%token VAR, METHOD
%token CLASS, EXTENDS, THIS, NEW
%token INTEGER_LITERAL, REAL_LITERAL, BOOLEAN_LITERAL

%%

program : classDeclaration               { $$.program = new Program( $1.classDeclaration ); }
        | program classDeclaration       { $$.program = new Program( $1.program, $2.classDeclaration ); }
        ;

classDeclaration : CLASS IDENTIFIER IS memberDeclarations END                     { $$.classDeclaration = new ClassDeclaration($2.identifier, $4.memberDeclarations); }
                 | CLASS IDENTIFIER EXTENDS identifiers IS memberDeclarations END { $$.classDeclaration = new ClassDeclaration($2.identifier, $4.identifiers, $6.memberDeclarations); }
           	 ;

identifiers : identifiers COMMA IDENTIFIER        { $$.identifiers = new Identifiers( $1.identifiers, $3.identifier ); }
            | IDENTIFIER                        { $$.identifiers = new Identifiers( $1.identifier ); }
            ;

memberDeclarations :                                      { $$.memberDeclarations = new MemberDeclarations(); }
                   | memberDeclarations memberDeclaration { $$.memberDeclarations = new MemberDeclarations( $1.memberDeclarations, $2.memberDeclaration ); }
                   ;

memberDeclaration : variableDeclaration    { $$.memberDeclaration = $1.variableDeclaration; }
                  | methodDeclaration      { $$.memberDeclaration = $1.methodDeclaration; }
                  | constructorDeclaration { $$.memberDeclaration = $1.constructorDeclaration; }
                  ;

variableDeclaration : VAR IDENTIFIER COLON IDENTIFIER     { $$.variableDeclaration = new VariableDeclaration( $2.identifier, $4.identifier ); }
                    ;

methodDeclaration   : METHOD IDENTIFIER P_OPEN parameters P_CLOSE COLON IDENTIFIER IS body END
                                                    { $$.methodDeclaration = new MethodDeclaration( $2.identifier, $4.parames, $7.identifier, $9.body ); }
                    ;

constructorDeclaration : THIS P_OPEN parameters P_CLOSE IS body END  { $$.constructorDeclaration = new ConstructorDeclaration( $3.parames, $6.body ); }
                       ;

parameter   : IDENTIFIER COLON IDENTIFIER        { $$.param = new Parameter( $1.identifier, $3.identifier ); }
            ;

parameters  :                                { $$.parames = new Parameters(); }
            | parameters COMMA parameter       { $$.parames = new Parameters( $1.parames, $3.param ); }
            | parameter                      { $$.parames = new Parameters( $1.param ); }
            ;

body : body statement             { $$.body = new StatementsBlock( $1.body, $2.stmt); }
     | statement                  { $$.body = new StatementsBlock( $1.stmt ); }
     ;

statement : assignment          { $$.stmt = $1.assign; }
          | whileLoop           { $$.stmt = $1.while_; }
          | ifStatement         { $$.stmt = $1.ifelse; }
          | returnStatement     { $$.stmt = $1.ret; } 
          | methodCall          { $$.stmt = $1.methodCall; }
          | expr                { $$.stmt = $1.expr; }
          ;


assignment  : IDENTIFIER COLON IDENTIFIER ASSIGN expr    { $$.assign = new Assignment($1.identifier, $5.expr, $3.identifier); }
            ;

whileLoop   : WHILE expr LOOP body END      { $$.while_ = new While($2.expr, $4.body); }
            ;

// TODO: thisRef may be a trap of if-else described in one of the lectures, isn't it?..
ifStatement : IF expr THEN body END             { $$.ifelse = new IfElse($2.expr, $4.body); }
            | IF expr THEN body ELSE body END   { $$.ifelse = new IfElse($2.expr, $4.body, $6.body); }
            ;

returnStatement : RETURN expr			{ $$.ret = new Return($2.expr); }
                ;
    
expr : methodCall            { $$.expr = $1.methodCall; }
     | classInstantiation    { $$.expr = $1.classInstantiation; }
     | INTEGER_LITERAL       { $$.expr = $1.num_literal; }
     | REAL_LITERAL          { $$.expr = $1.num_literal; }
     | BOOLEAN_LITERAL       { $$.expr = $1.bool_literal; }
     | THIS                  { $$.expr = $1.thisRef; }
     | IDENTIFIER            { $$.expr = new Reference($1.identifier); }
     ;

methodCall : expr DOT IDENTIFIER P_OPEN arguments P_CLOSE  { $$.methodCall = new MethodCall( $1.expr, $3.identifier, $5.arguments ); }
           ;

arguments :                           { $$.arguments = new Arguments(); }
          | arguments COMMA argument    { $$.arguments = new Arguments( $1.arguments, $3.argument ); }
          | argument                  { $$.arguments = new Arguments( $1.argument ); }
          ;

argument : expr  { $$.argument = new Argument( $1.expr ); }
         ;

classInstantiation : NEW IDENTIFIER P_OPEN arguments P_CLOSE    { $$.classInstantiation = new ClassInstantiation( $2.identifier, $4.arguments ); }
                   ;

%%
