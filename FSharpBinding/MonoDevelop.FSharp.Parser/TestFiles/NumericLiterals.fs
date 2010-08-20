#light "off"

let (|Even|Odd|) x = if x % 2 = 0 then Even else Odd

type StackItem = 
    | I32
    | I64
    | Native
    | Float
    | Reference of int
    | Object of int list

type MyClass private () = class
        let z = 10
        member this.X = z
        abstract member Y: int -> int
        static member TT: int -> list<_> = fun x -> []
    end

let SBYTE = 0y;
let BYTE = 0uy in
for i :: tail in seq{for t in 1 .. 10 -> t * t} do printfn "%A" [] done
