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
    public Assignment assign;
    public IfElse ifelse;
    public While while_;
    public StatementsBlock body;

    public Statement stmt;

    public VariableDeclaration vardecl;
    // public MethodDeclaration methdecl;
    public Parameter param;
    public Parameters parames;
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
            | IDENTIFIER                        {}
            ;

memberDeclarations : memberDeclaration memberDeclarations {}
                   | memberDeclaration {}
                   ;

memberDeclaration : variableDeclaration    {}
                  | methodDeclaration      {}
                  | constructorDeclaration {}
                  ;

variableDeclaration : VAR IDENTIFIER COLON expr     { $$.vardecl = new VariableDeclaration($2.identifier.name, $4.expr); }
                    ;

methodDeclaration   : METHOD IDENTIFIER P_OPEN parameters P_CLOSE COLON expr IS body END
                                                    {  }
                    ;

constructorDeclaration : THIS P_OPEN parameters P_CLOSE IS body END;

parameter   : IDENTIFIER COLON expr        { $$.param = new Parameter($1.identifier.name, $3.expr); }
            ;

parameters  :
            | parameter ',' parameters       { $$.parames = $3.parames.Append($1.param); }
            |                                { $$.parames = new Parameters(); }
            ;

body : body statement    { $$.body = $1.body.Append($2.stmt); }
     |                   { $$.body = new StatementsBlock(); }
     ;

// TODO: do we want to convert these to expressions?
statement : assignment          { $$.stmt = $1.assign; }
          | whileLoop           { $$.stmt = $1.while_; }
          | ifStatement         { $$.stmt = $1.ifelse; }
          | returnStatement     { $$.stmt = $1.ret; }
          ;


// TODO: The below grammar does not cover the case of array element assignment or object field assignment
assignment  : IDENTIFIER ASSIGN expr    { $$.assign = new Assignment($1.identifier.name, $3.expr); }
            ;

whileLoop   : WHILE expr LOOP body END      { $$.while_ = new While($2.expr, $4.stmt); }
            ;

// TODO: this may be a trap of if-else described in one of the lectures, isn't it?..
ifStatement : IF expr THEN body END             { $$.ifelse = new IfElse($2.expr, $4.body, null); }
            | IF expr THEN body ELSE body END   { $$.ifelse = new IfElse($2.expr, $4.body, $6.body); }
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
