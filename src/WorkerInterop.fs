// Common JS Interop code and utility functions

module WorkersInterop
open Fable.Core
open Fable.Import.Browser
open Fable.Import.JS

open System
open System.Text.RegularExpressions


[<Emit("addEventListener('fetch', $0)")>]
let addEventListener (e:FetchEvent->Promise<Response>) : unit = jsNative

[<Emit("new Response($0, {status: $1})")>]
let newResponse (a:string) (b:string) : Response = jsNative

type CFWRequest =
    inherit Request
    abstract member cf : CFDetails
and CFDetails = {
    tlsVersion: string
    tlsCipher: string
    country: string
    colo: string
}

let wrap x = promise {return x}

let path (r:CFWRequest)=
    match Regex.Split((Uri r.url).AbsolutePath.ToLower(), "\/") with
    | [|"";""|] -> [||]   // this is for paths http://somthing.com/ and http://something.com
    | p -> p.[1..]        // becuase the first path element is always ""
    |> Array.toList

let textResponse txt =
  newResponse txt "200" |> wrap


type Verb =
    | GET
    | POST
    | PUT
    | PATCH
    | DELETE
    | OPTION
    | HEAD
    | TRACE
    | CONNECT
    | UNDEFINED

let verb (r:CFWRequest) =
    match r.method.ToUpper() with
    | "GET" -> GET
    | "POST" -> POST
    | "PUT" -> PUT
    | "PATCH" -> PATCH
    | "DELETE" -> DELETE
    | "OPTION" -> OPTION
    | "HEAD" -> HEAD
    | "TRACE" -> TRACE
    | "CONNECT" -> CONNECT
    | _ -> UNDEFINED
