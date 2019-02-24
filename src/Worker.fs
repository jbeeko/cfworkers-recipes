module Worker
open Fable.Core
open Fable.Import.Browser
open WorkersInterop

// Request router written as a match statement on the 
// HTTP Verb and the route. It handles multi-part routes,
// route variables, and subroutes. 
let rec private routeRequest verb path req =
  match (verb, path) with
  | _, ["aggregating"] -> AggregatingMultRequests.fetchAndApply req
  | _, ["headers"] -> AlterHeaders.handleRequest req
  | _, ["counrty-blocking"] -> CountryBlocking.blockCountries req
  | _, ["tls-blocking"] -> TLSBlocking.sslBlock req
  | _,_ -> noHandler req
and private noHandler req =
  newResponse (sprintf "No handler for: %s on: %s" req.method req.url ) "404" |> wrap

// Dispatch to the web App wich will generate a
// Response and return it as a Promise<Response>
let private handleRequest (req:CFWRequest) =
    routeRequest (verb req) (path req) req

// Register a listner for the ServiceWorker 'fetch' event
addEventListener (fun (e:FetchEvent) ->
  e.respondWith (U2.Case1 (handleRequest (e.request :?> CFWRequest))))

