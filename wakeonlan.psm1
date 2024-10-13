function Get-AccessToken
(
    [Parameter(Mandatory=$true)]
    [string] $TenantID,

    [Parameter(Mandatory=$true)]
    [string] $ClientID,

    [Parameter(Mandatory=$true)]
    [string] $ClientSecret,
    
    [Parameter(Mandatory=$true)]
    [string] $ServerApplicationID
)
{
    $uri = "https://login.microsoftonline.com/$TenantID/oauth2/v2.0/token"
    $form = @{                                                                
        client_id = $ClientID
        scope = "api://$ServerApplicationID/.default"
        client_secret = $ClientSecret
        grant_type = "client_credentials"
    }
    $result = Invoke-RestMethod -Uri $uri -Method Post -Form $form
    return $result.access_token
}