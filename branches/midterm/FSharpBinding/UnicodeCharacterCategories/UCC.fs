open System.Text
open System.Text.RegularExpressions

let categories =
  [ "Cc", "OtherControl";
    "Cf", "OtherFormat";
    "Cn", "OtherNotAssigned";
    "Co", "OtherPrivateUse";
    "Cs", "OtherSurrogate";
    "Ll", "LetterLowercase";
    "Lm", "LetterModifier";
    "Lo", "LetterOther";
    "Lt", "LetterTitlecase";
    "Lu", "LetterUppercase";
    "Mc", "MarkSpacingCombining";
    "Me", "MarkEnclosing";
    "Mn", "MarkNonspacing";
    "Nd", "NumberDecimalDigit";
    "Nl", "NumberLetter";
    "No", "NumberOther";
    "Pc", "PunctuationConnector";
    "Pd", "PunctuationDash";
    "Pe", "PunctuationClose";
    "Pf", "PunctuationFinal";
    "Pi", "PunctuationInitial";
    "Po", "PunctuationOther";
    "Ps", "PunctuationOpen";
    "Sc", "SymbolCurrency";
    "Sk", "SymbolModifier";
    "Sm", "SymbolMath";
    "So", "SymbolOther";
    "Zl", "SeparatorLine";
    "Zp", "SeparatorParagraph";
    "Zs", "SeparatorSpace"  ] |> Map.ofList

open System
open System.IO
open System.Globalization

let dest = new StreamWriter("UCC.g", false, Encoding.UTF8)
let inline printfn i = fprintfn dest i
let mutable inrange = false
let mutable start = Char.MinValue

let printRanges (category: UnicodeCategory) =
    printfn "fragment"
    printfn "%A :" category

    inrange <- false

    for c in Char.MinValue .. Char.MaxValue do
        let curr = Char.GetUnicodeCategory(c)
        match curr = category, inrange with
        | false, false -> ()
        | true, false ->
            inrange <- true
            start <- c
        | true, true -> ()
        | false, true ->
            inrange <- false
            let last = uint16 c - 1us |> char
            if last = start then
                printfn "\t'\\u%04X'\t|" (uint16 last)
            else
                printfn "\t'\\u%04X' .. '\\u%04X'\t|" (uint16 start) (uint16 last)

    inrange <- false
    printfn ""

for cat in 0 .. 29 do
    printRanges (enum cat)

dest.Close()
