module fsharp_cpu.linker
open Microsoft.VisualBasic
open System.IO
open System.Text.RegularExpressions


let (|Match|_|) pattern input =
    let m = Regex.Match(input, pattern) in
    if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ]) else None


let parseLine (line:string) =
    line
    |> fun (s:string) -> Regex.Replace(s, "#.*$", "")
    |> Strings.Trim
    |> fun s ->
        match s with
        | Match "^(\w+)\s+(\w+),\s*(\w+),\s*(\w+),\s*(\w+)" result -> Some result
        | Match "^(\w+)\s+(\w+),\s*(\w+),\s*(\w+)" result -> Some result
        | Match "^(\w+)\s+(\w+),\s*(\w+)" result -> Some result
        | Match "^(\w+)\s+(\w+)" result -> Some result
        | Match "^(\w+)" result -> Some result
        | _ -> None

let compileAndLink (program:Stream) =
    
    use sr = new StreamReader(program)
    let lines = seq { while not sr.EndOfStream do yield sr.ReadLine() }
    
    lines
    |> Seq.map parseLine
    |> Seq.choose id
    |> Seq.toList
