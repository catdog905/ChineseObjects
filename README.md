# Introduction to Chinese Objects

## Code snippets
### *HelloWorld* example
```
class HelloWorldProgram extends Class is
    this () is ConsolePrintableText(
                "Hello world!",
                Console()
            ).print()
    end
end
```

### `Text` class implementation
```
class Text extends AnyValue is 
    this (symbols: Array[Symbol])
    this (text: Text) is
        this(text.symbols())
    end
    this (number: Number) is
        this(new TextFromNumber())
    end
    method symbols() : Array[Symbol] is
        return symbols
    end
end
```
All expressions enclosed with double quotes (") converts to Text implicitly during compilation

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
    var text: Text
    var console: Console
    this (symbols: Array[Symbol], console: Console) is
        this.text = Text(text)
    end
    method print() : Void is
        console.write(this)
    end
    method symbols() : Array[Symbols]
        return text.symbols()
    end
end
```

### `Shape` class inheritance implementation
```
class Shape extends Class is 
    method area() : Real
end

class Rectangle extends Shape is
    this(height: Integer) 
    this(width: Integer)
    method area() : Real is
        return height.Mult(width) // Returns Real
    end
end
```
 
### Eratosthenes sieve algorithm
```
class PrimeNumbers is
    var n: Integer
    this (n: Integer)
    method value() : List[Integer] is
        var prime : FilledArray[Boolean](n.Plus(1), true)
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
    var array: Array[T]
    this (l: Integer, fill: T) is
        var array: Array[T]
        var i: Integer(0)
        while i.Less(l) loop
            array.set(i, fill)
            i.Plus(1)
        end
        this.array = array
    end
    method ToList() : List is
        return array.ToList()
    end
    method Length() : Integer is
        return array.Integer()
    end
    method get(i: Integer) : T is
        array.Get(i)
    end
    method put(i: Integer, v: T) is
        array.Set(i, v)
    end
end
```

## Basic Object-oriented features
### Object instantiation
```
class Employee is
    var name: Text
    var salary: Integer
    this(name: Text, salary: Integer) throwing TooBigSalary Exception is
        if salary.Greater(200000)
            throw new class TooBigSalaryException extends Exception
        end
        this.name = name
        this.salary = salary
    end 
    
    method work() : Text is
        return new FormattedText(
            "%s works, earning %d",
            name,
            salary
        )
    end
end
```

Class above can be instantiated this way 
```
new Employee("Alice", 32).work() // -> returns "Alice works, earning 32"
new Employee("Alice", 320000).work() // -> throws TooBigSalaryException
```

### Inheritance and Polymorphism
```
class Programmer extends Employee
    var employee: Employee
    this (employee)
    
    method work() : Text is
        return new FormattedText(
            "Programmer %s",
            employee.work()
        )
    end
end
```

Example of usage:
```
new Programmer(new Employee("Harry", 10000)).work()
```