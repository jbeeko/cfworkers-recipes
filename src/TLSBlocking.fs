module TLSBlocking
// FSharp implementation of the TLS Blocking recipe.
// https://developers.cloudflare.com/workers/recipes/tls-version-blocking/
open Fetch
open WorkersInterop

let sslBlock req =
    promise {

        return newResponse "response" "200"
    }


// async function sslBlock(request) {
//   // Find TLS version - This will appear as "undefined" in the Preview
//   // For specific cipher blocking, you can inspect request.cf.tlsCipher
//   let tlsVersion = (request.cf || {}).tlsVersion

//   // Allow only TLS versions 1.2 and 1.3
//   if ((tlsVersion != 'TLSv1.2') && (tlsVersion != 'TLSv1.3')){
//     return new Response("Please use TLS version 1.2 or higher.",
//     { status: 403, statusText: "Forbidden" })
//   }

//   return fetch(request)
// }