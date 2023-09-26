%namespace ChineseObjects.Calculator
%partial
%parsertype CalculatorParser
%visibility internal
%tokentype Token

%union { 
			public int n; 
			public string s; 
	   }

%start program

%token NUMBER, IDENTIFIER, OP_PLUS, OP_MINUS, OP_MULT, OP_DIV, P_OPEN, P_CLOSE, COLON
%token IS, END, LOOP, THEN, RETURN, ELSE, WHILE, ASSIGN, IF
%token VAR, METHOD
%token CLASS, EXTENDS, THIS

%%

program: classDeclaration               { }
       | classDeclaration program       { }
       ;

classDeclaration: CLASS IDENTIFIER IS memberDeclarations END {}
       | CLASS IDENTIFIER EXTENDS identifiers IS memberDeclarations END {}
       ;
       
identifiers: IDENTIFIER ',' identifiers          {}
       | IDENTIFIER                            {}
       ;

memberDeclarations: memberDeclaration memberDeclarations {}
       | memberDeclaration {}
       ;

memberDeclaration: variableDeclaration    {}
 		 | methodDeclaration      {}
 		 | constructorDeclaration {}
 		 ;
 		 
variableDeclaration: VAR IDENTIFIER COLON exp;

methodDeclaration: METHOD IDENTIFIER P_OPEN parameters P_CLOSE COLON IDENTIFIER IS body END;

constructorDeclaration: THIS P_OPEN parameters P_CLOSE IS body END;

parameters: 
          | parameter ',' parameters
	  | parameter
	  ;
	 
parameter: IDENTIFIER COLON IDENTIFIER;

body: statement body
    | statement
    ;
    
statement: assignment
         | whileLoop
         | ifStatement
         | returnStatement
         ;
         
assignment: IDENTIFIER ASSIGN exp;

whileLoop: WHILE exp LOOP body END;

ifStatement: IF exp THEN body END
           | IF exp THEN body ELSE body END
           ;
           
returnStatement: RETURN exp;

line   : exp				{ }
       ;

exp    : term                           { $$.n = $1.n;		 }
       | exp OP_PLUS term               { $$.n = $1.n + $3.n;	 }
       | exp OP_MINUS term              { $$.n = $1.n - $3.n;	 }
       ;

term   : factor				{$$.n = $1.n;		 }
       | term OP_MULT factor            {$$.n = $1.n * $3.n;	 }
       | term OP_DIV factor             {$$.n = $1.n / $3.n;	 }
       ; 

factor : number                         {$$.n = $1.n;		 }
       | P_OPEN exp P_CLOSE             {$$.n = $2.n;		 }
       ;

number : 
       | NUMBER				{  }
       ;

%%