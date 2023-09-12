# Chinese Objects

# Code snippets
*HelloWorld* example
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

`Text` class implementation
```
class Text extends AnyValue is 
    this symbols: Array[Symbol]
    method symbols() is
        return symbols
    end
end
```

`Printable` class implementation
```
class Printable extends Class is
    method print() : Void
end
```

`Console` class abstraction 
```
class Console extends Class is
    method write(text: Text) : Void
    method read() : TextStream
end 
```

`ConsolePrintableText` class implementation
```
class ConsolePrintableText extends Printable, Text is
    super symbols: Array[Symbol] this console: Console
    method print() is
        console.write(this)
    end
end
```


## Questions

- Is every class inherited from `Class`?
- Does Chinese Objects language have multiple inheritance?
- Does Chinese Objects language have interfaces? Will there any way to make abstractions?
- Can we instantiate the object of class with abstract methods (methods without body), such as provided `Integer` class 
in the documentation?
- What is the way to implement encapsulation in Chinese Objects?
- How to call constructor of the base class in the constructor of derived class?
Suggestion:
```
class Derived extends Base is
    super { BaseConstructorParameters }
    this  { ThisConstructorParameters } [ is Body end ]
end
```