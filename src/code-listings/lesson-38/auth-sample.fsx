#I @"..\..\..\packages\"

#r @"Http.fs\lib\net40\HttpClient.dll"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
open HttpClient
open System

module Auth =
    let [<Literal>] private ResponseSample = """{"token_type":"bearer","expires_in":3600,"scope":"wl.emails wl.basic wl.offline_access wl.signin","access_token":"access-token","refresh_token":"refresh-token","user_id":"cc2c4130f73448339235ed7862f1d4b5"}"""

    type private OAuthResponse = FSharp.Data.JsonProvider<ResponseSample>
    let private buildAccessTokenUrl clientId clientSecret authCode = sprintf "https://login.live.com/oauth20_token.srf?client_id=%s&client_secret=%s&redirect_uri=https://login.live.com/oauth20_desktop.srf&grant_type=authorization_code&code=%s" clientId clientSecret authCode
    let private buildRefreshTokenUrl clientId clientSecret refreshToken = sprintf "https://login.live.com/oauth20_token.srf?client_id=%s&client_secret=%s&redirect_uri=https://login.live.com/oauth20_desktop.srf&grant_type=refresh_token&refresh_token=%s" clientId clientSecret refreshToken
    /// Opens a browser window to let you grant permissions to your app.
    let grantPermissions clientId =
        let scope = "wl.emails%20wl.basic%20wl.offline_access%20wl.signin"
        let url = sprintf "https://login.live.com/oauth20_authorize.srf?client_id=%s&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=code&scope=%s" clientId scope
        System.Diagnostics.Process.Start url
    
    /// Try to get initial set of OAuthTokens
    let tryGetTokens clientId clientSecret authCode =
        let parseInitialAuth response =
            let response = OAuthResponse.Parse response
            response.AccessToken, response.RefreshToken

        let tryGetBody = function
            | { StatusCode = 200; EntityBody = Some body } -> Some body
            | _ -> None

        buildAccessTokenUrl clientId clientSecret authCode
        |> createRequest HttpMethod.Get
        |> getResponse
        |> tryGetBody
        |> Option.map parseInitialAuth

/// Get client ID and Secret from  MS Live OAuth app registration site
let clientId, clientSecret = "", ""

/// Opens a browser window to grant permissions. Copy the "code" query string value to authCode below!
Auth.grantPermissions clientId
let authCode = ""

// Now we can finally create the access token!
let tokens = Auth.tryGetTokens clientId clientSecret authCode