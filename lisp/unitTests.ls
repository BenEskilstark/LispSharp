(def expect (fn (val is msg) 
    (if (== val is) 
        (print PASS: msg)
        (print FAIL: msg -- expected is got val)
    )
))

(expect (+ 2 2) 4 "2 + 2 = 4")
(expect (+ 2 (- 1 3)) 0 "nested exp")

(expect (if true (+ 3 3) (+ 2 2)) 6 "if statement 1")
(expect (if false (+ 3 3) (+ 2 2)) 4 "if statement 2")
(expect (if (== 0 (- 1 1)) (+ 3 3) (+ 2 2)) 6 "if statement 3")
(expect (if (> 0 (- 1 1)) (+ 3 3) (+ 2 2)) 4 "if statement 4")

(def foo 10)
(expect foo 10 "assignment 1")
(expect (* foo 3) 30 "assignment 2")

(def add (fn (a b) (+ a b)))
(expect (add 2 3) 5 "function 1")
(expect (add 2 foo) 12 "function 2")

(def fac (fn (n) 
    (if (== n 0) 
        1 
        (* n (fac (- n 1)))
    )
))
(expect (fac 5) 120 "recursion")