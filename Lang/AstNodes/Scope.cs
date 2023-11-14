namespace ChineseObjects.Lang;

using System.Collections.Immutable;


public class Scope {
  	public readonly ImmutableDictionary<string, ClassDeclaration> classes;
  	public readonly ImmutableDictionary<string, VariableDeclaration> variables;

    public Scope(IEnumerable<KeyValuePair<string, ClassDeclaration>> clss,
                 IEnumerable<KeyValuePair<string, VariableDeclaration>> vars) {
        this.classes = ImmutableDictionary.CreateRange(clss);
        this.variables = ImmutableDictionary.CreateRange(vars);
    }

    public Scope() : this(ImmutableDictionary<string, ClassDeclaration>.Empty,
                          ImmutableDictionary<string, VariableDeclaration>.Empty)            
    {}

    public Scope ExtendWith(IEnumerable<KeyValuePair<string, ClassDeclaration>> classes,
                            IEnumerable<KeyValuePair<string, VariableDeclaration>> variables) {
        return new Scope(this.classes.AddRange(classes), this.variables.AddRange(variables));
    }

  	public ClassDeclaration? GetClass(string name) {
  	  	ClassDeclaration? value = null;
  	  	classes.TryGetValue(name, out value);
  	  	return value;
  	}

    public VariableDeclaration? GetVariable(string name) {
        VariableDeclaration? value = null;
        variables.TryGetValue(name, out value);
        return value;
    }
}
