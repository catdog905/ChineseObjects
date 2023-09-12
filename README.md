# Chinese Objects

## Code snippets
### *HelloWorld* example
```
class HelloWorldProgram extends Class is
    this is ConsolePrintableText(
                Text(
                    Array[Symbol](
                        "H", "e", "l", "l", "o", " ", "W", "o", "r", "l", "d")
                    )
                ),
                Console()
            ).print()
    end
end
```

### `Text` class implementation
```
class Text extends AnyValue is 
    this symbols: Array[Symbol]
    method symbols() is
        return symbols
    end
end
```

### `Printable` class implementation
```
class Printable extends Class is
    method print() : Void
end
```

### `Console` class abstraction 
```
class Console extends Class is
    method write(text: Text) : Void
    method read() : TextStream
end 
```

### `ConsolePrintableText` class implementation
```
class ConsolePrintableText extends Printable, Text is
    super symbols: Array[Symbol] this console: Console
    method print() is
        console.write(this)
    end
end
```

### Eratosthenes sieve algorithm
```
class PrimeNumbers is
    this n
    method value() : List[Integer] is
        var prime : FilledArray[Boolean](n+1, true)
        prime.set(0, false)
        prime.set(1, false)
        var i : Integer(2)
        while i.LessEqual(n) loop
            if prime.get(i) then
                if i.Mult(i).Equal(n) then
                    var j : Integer(i.Mult(i))
                    while j.LessEqual(n) then
                        prime.set(j, false)
                        j.Plus(1)
                    end
                end
            end
            i.Plus(1)
        end
        i := Integer(0)
        primeNumbers : List[Integer]()
        while i.Less(prime.Length) loop
            if prime.get(i).Equals(true)
                primeNumbers.append(i)        
        end
        return primeNumbers
    end
end
```

```
class FilledArray[T] extends Array[T] is
    super l: Integer this fill: T is
        var i : Integer(0)
        while i.Less(l) loop
            set(i, fill)
            i.Plus(1)
        end
    end
end
```


## Questions

- Is every class inherited from `Class`?
- Does Chinese Objects language have multiple inheritance?
- Does Chinese Objects language have virtual methods? Will there any way to make abstractions?
- Can we instantiate the object of class with abstract methods (methods without body), such as provided `Integer` class 
in the documentation?
- What is the way to implement encapsulation in Chinese Objects? Does this language have any analogies to final keyword
in Java
- How to call constructor of the base class in the constructor of derived class?
Suggestion:
```
class Derived extends Base is
    super { BaseConstructorParameters }
    this  { ThisConstructorParameters } [ is Body end ]
end
```