module CountryBlocking
// FSharp implementation of the Country Blocking recipe.
// https://developers.cloudflare.com/workers/recipes/country-blocking/

open Fetch
open WorkersInterop

let blockCountries (req:CFWRequest) =
    promise {
        let rsp = 
            match req.cf.country with
            | "US" | "SG" | "BC" -> 
                newResponse "This page not available in your country" "403"
                //{ status: 403, statusText: "Forbidden" })
            | _ -> fetch req.url [] :?> Fable.Import.Browser.Response
        return rsp}        


// Block countries based on request origin
//
// //Add countries to this Set to block them
// const countries = new Set([  
//   "US", // United States
//   "SG", // Singapore 
//   "BR"  // Brazil
// ])

// async function blockCountries(request) {
//   // Get country value from request headers
//   let country = request.headers.get('cf-ipcountry')

//   // Find out if country is on the block list
//   let countryBlocked = countries.has(country)

//   // If it's on the blocked list, give back a 403
//   if (countryBlocked){
//     return new Response("This page not available in your country",
//         { status: 403, statusText: "Forbidden" })
//   }

//   // Catch-all return of the original response
//   return await fetch(request)
// }