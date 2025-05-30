
# == 10
(def foo (+ ((fn (a b) (+ a b 2)) 3 4) 1))

# == 13
(print ((if true (fn (a b) (+ a b)) -) 3 foo))

(def add (fn (a b) (+ a b)))

# 5
(print (add 2 3))

(def fac (fn (n) (if (== n 0) 1 5)))

(fac 0)