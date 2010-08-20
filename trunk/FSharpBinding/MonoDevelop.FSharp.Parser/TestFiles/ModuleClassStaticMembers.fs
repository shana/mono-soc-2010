#light "off"

module Test

type MyType = class
	static member X = 10 + 3
	static member Y = MyType.X -10
end

let rec fact = function 0 | 1 -> 1 | x -> x * fact(x - 1)
