
# (print (+ 2 3))
# (def z (+ 1 1))
# (print (+ 3 z))


# == 10
# (def foob (+ ((fn (a b) (+ a b 2)) 3 4) 1))
# == 13
# (print ((if true (fn (a b) (+ a b)) -) 3 foob))

(def add (fn (a b) (+ a b)))
# 5
(print (add 2 3))
(print (add 1 13))

# (def same (fn (a b) (if (== a b) a (+ 1 (same (+ 1 4) 5)))))
# (print (same 2 2))
# (print (same 4 3))
# (print (same 10 10))

# (def same2 (fn (a b) (if (== a b) a (same2 (+ 1 a) b))))
# (print (same2 2 3))

# (def err (fn () (+ none)))
# (err)

# (def foo (fn (bool) (if bool (print "foo") (print "bar"))))
# (foo true)
# (def bar (fn (bool) (if bool (print "foo") (err))))
# (bar true)

(def fac (fn (n) (if (== n 0) 1 (* n (fac (- n 1))))))

# (print (fac 0))
# (fac 1)