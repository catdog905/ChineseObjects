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

%token NUMBER, IDENTIFIER, OP_PLUS, OP_MINUS, OP_MULT, OP_DIV, P_OPEN, P_CLOSE
%token IS, END
%token CLASS, EXTENDS

%%

program: classDeclaration               { }
       | classDeclaration program       { }
       ;

classDeclaration: CLASS IDENTIFIER IS memberDeclarations END {}
       | CLASS IDENTIFIER '[' EXTENDS identifiers ']' IS memberDeclarations END {}
       ;
       
identifiers: IDENTIFIER ',' identifiers          {}
       | IDENTIFIER                            {}
       ;

memberDeclarations: memberDeclaration memberDeclarations {}
       | memberDeclaration {}
       ;

memberDeclaration: IDENTIFIER {};

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