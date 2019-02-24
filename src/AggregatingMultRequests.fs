module AggregatingMultRequests
// FSharp implementation of the Aggegating Multiple Requests recipe.
// https://developers.cloudflare.com/workers/recipes/aggregating-multiple-requests/


open Fetch
open WorkersInterop

type private CoinBaseRsp = {
    data : Data}
and Data = {
    ``base``: string
    currency: string
    amount: string
}

type Rsp = {
    btc : string
    ltc : string
    eth : string
}

let fetchAndApply req = 
    promise {
        let! httpRsps = 
            [| "https://api.coinbase.com/v2/prices/BTC-USD/spot"
               "https://api.coinbase.com/v2/prices/ETH-USD/spot"
               "https://api.coinbase.com/v2/prices/LTC-USD/spot"|]
            |> Array.map (fun url -> fetch url [])
            |> Promise.Parallel
        let! cbRsps = 
            httpRsps 
            |> Array.map (fun r -> r.json())
            |> Promise.Parallel
        let rates = 
            cbRsps 
            |> Array.map (fun r -> (r :?> CoinBaseRsp).data.amount)  
        let result = {btc = rates.[0]; ltc = rates.[1]; eth = rates.[2]}
        return (newResponse (result.ToString()) "200")
    }
    


// /**
//  * Make multiple requests, 
//  * aggregate the responses and 
//  * send it back as a single response
//  */
// async function fetchAndApply(request) {
//     const init = {
//       method: 'GET',
//       headers: {'Authorization': 'XXXXXX'}
//     }
//     const [btcResp, ethResp, ltcResp] = await Promise.all([
//       fetch('https://api.coinbase.com/v2/prices/BTC-USD/spot', init),
//       fetch('https://api.coinbase.com/v2/prices/ETH-USD/spot', init),
//       fetch('https://api.coinbase.com/v2/prices/LTC-USD/spot', init)
//     ])
  
//     const btc = await btcResp.json()
//     const eth = await ethResp.json()
//     const ltc = await ltcResp.json()
  
//     let combined = {}
//     combined['btc'] = btc['data'].amount
//     combined['ltc'] = ltc['data'].amount
//     combined['eth'] = eth['data'].amount
  
//     const responseInit = {
//       headers: {'Content-Type': 'application/json'}
//     }
//     return new Response(JSON.stringify(combined), responseInit)
// }