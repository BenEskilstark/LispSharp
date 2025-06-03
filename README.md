Trying to implement a lisp interpreter off the top of my head

Invoke like:
> dotnet run path/to/lispfile

or for a repl

> dotnet run 

> dotnet run --repl file/to/start/repl/session/with

Syntax:
```
(def foo 4) # assign 4 to foo. hashtag denotes comments
(print (+ foo 4)) # prints 8
(def square (fn (n) (* n n))) # defines function called square
(print (square (+ foo 1))) # prints 25
(square foo) # last statement of the file is always printed
(def fac (fn (n) (
   (if (== n 0) 1 (* n (fac (- n 1))))
))) # multi-line functions and recursion
```

To Do:
   - Lists
