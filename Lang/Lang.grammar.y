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
    public Object obj;

    public Return ret;
    public Assignment assign;
    public IfElse ifelse;
    public While while_;
    public StatementsBlock body;
    public MethodCall methodCall;
    public ClassInstantiation classInstantiation;

    public Statement stmt;

    public Parameter param;
    public Parameters parames;

    public Argument argument;
    public Arguments arguments;

    public Program program;
    public ClassDeclaration classDeclaration;
    public MemberDeclaration memberDeclaration;
    public MemberDeclarations memberDeclarations;
    public VariableDeclaration variableDeclaration;
    public MethodDeclaration methodDeclaration;
    public ConstructorDeclaration constructorDeclaration;
}

%start program

%token IDENTIFIER, OP_PLUS, OP_MINUS, OP_MULT, OP_DIV, P_OPEN, P_CLOSE, COLON, DOT
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

identifiers : identifiers ',' IDENTIFIER        { $$.identifiers = new Identifiers( $1.identifiers, $3.identifier ); }
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

parameter   : IDENTIFIER COLON IDENTIFIER        { $$.param = new Parameter( $1.identifier.Name, $3.identifier ); }
            ;

parameters  :                                { $$.parames = new Parameters(); }
            | parameters ',' parameter       { $$.parames = new Parameters( $1.parames, $3.param ); }
            | parameter                      { $$.parames = new Parameters( $1.param ); }
            ;

body : body statement             { $$.body = new StatementsBlock( $1.body, $2.stmt); }
     | statement                  { $$.body = new StatementsBlock( $1.stmt ); }
     ;

// TODO: do we want to convert these to expressions?
statement : assignment          { $$.stmt = $1.assign; }
          | whileLoop           { $$.stmt = $1.while_; }
          | ifStatement         { $$.stmt = $1.ifelse; }
          | returnStatement     { $$.stmt = $1.ret; } 
          | methodCall          { $$.stmt = $1.methodCall; }
          ;


assignment  : IDENTIFIER ASSIGN obj    { $$.assign = new Assignment($1.identifier.Name, $3.obj); }
            ;

whileLoop   : WHILE obj LOOP body END      { $$.while_ = new While($2.obj, $4.stmt); }
            ;

// TODO: thisRef may be a trap of if-else described in one of the lectures, isn't it?..
ifStatement : IF obj THEN body END             { $$.ifelse = new IfElse($2.obj, $4.body); }
            | IF obj THEN body ELSE body END   { $$.ifelse = new IfElse($2.obj, $4.body, $6.body); }
            ;

returnStatement : RETURN obj			{ $$.ret = new Return($1.obj); }
                ;
    
obj : methodCall            { $$.obj = $1.methodCall; }
    | classInstantiation    { $$.obj = $1.classInstantiation; }
    | INTEGER_LITERAL       { $$.obj = $1.num_literal; }
    | REAL_LITERAL          { $$.obj = $1.num_literal; }
    | BOOLEAN_LITERAL       { $$.obj = $1.bool_literal; }
    | THIS                  { $$.obj = $1.thisRef; }
    | IDENTIFIER            { $$.obj = $1.identifier; }
    ;

methodCall : obj DOT IDENTIFIER P_OPEN arguments P_CLOSE  { $$.methodCall = new MethodCall( $1.obj, $3.identifier, $5.arguments ); }
           ;

arguments :                           { $$.arguments = new Arguments(); }
          | arguments ',' argument    { $$.arguments = new Arguments( $1.arguments, $3.argument ); }
          ;

argument : obj  { $$.argument = new Argument( $1.obj ); }
         ;

classInstantiation : NEW IDENTIFIER P_OPEN arguments P_CLOSE    { $$.classInstantiation = new ClassInstantiation( $2.identifier, $4.arguments ); }
                   ;

%%
