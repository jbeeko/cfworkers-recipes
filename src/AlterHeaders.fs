module AlterHeaders
// FSharp implementation of the Altering Headers recipe.
// https://developers.cloudflare.com/workers/recipes/altering-headers/


open Fetch
open WorkersInterop

let handleRequest (req:CFWRequest) = 
    promise {

        let newReq = req.clone()
        newReq.headers.set("x-my-header1", "foo")

        // This is awkward. There is no nice way to modify an existijng
        // request in a minor way and then make the fetch request
        let! rsp = fetch newReq.url []
        rsp.Headers.set ("x-my-header2", "bar")
        // Browser.Response and Fetch.Response are not type compatible
        // Hence the need for the ugly case.
        return rsp :?> Fable.Import.Browser.Response
    }
    

// Adding a header to the request. 
// 1. clones the request
// 2. sets header
// 3. call fetch and return
//
// async function handleRequest(request) {
//   // Make the headers mutable by re-constructing the Request.
//   request = new Request(request)
//   request.headers.set('x-my-header', 'custom value')

//   return await fetch(request)
// }



// Adding a header to response.
// 1. make the fetch call with the incoming request
// 2. clone the response
// 3. set new header and return

// async function handleRequest(request) {
//   let response = await fetch(request)

//   // Make the headers mutable by re-constructing the Response.
//   response = new Response(response.body, response)
//   response.headers.set('x-my-header', 'custom value')

//   return response
// }