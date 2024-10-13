function Get-AccessToken
(
    [Parameter(Mandatory=$true)]
    [string] $TenantID,

    [Parameter(Mandatory=$true)]
    [string] $ClientID,

    [Parameter(Mandatory=$true)]
    [securestring] $ClientSecret,
    
    [Parameter(Mandatory=$true)]
    [string] $ServerApplicationID
)
{
    $uri = "https://login.microsoftonline.com/$TenantID/oauth2/v2.0/token"
    $form = @{                                                                
        client_id = $ClientID
        scope = "api://$ServerApplicationID/.default"
        client_secret = ConvertFrom-SecureString $ClientSecret -AsPlainText
        grant_type = "client_credentials"
    }
    $result = Invoke-RestMethod -Uri $uri -Method Post -Form $form
    return ConvertTo-SecureString $result.access_token -AsPlainText
}

function Get-Computer
(
    [Parameter(Mandatory=$true)]
    [string] $Url,

    [Parameter(Mandatory=$true)]
    [securestring] $AccessToken,

    [string] $Name,

    [switch] $SkipCertificateCheck
)
{
    $uri = "$Url/api/wakeonlan/$Name";
    return Invoke-RestMethod `
        -Uri $uri `
        -SkipCertificateCheck:$SkipCertificateCheck `
        -Token $AccessToken `
        -Authentication Bearer
}

function Set-Computer
(
    [Parameter(Mandatory=$true)]
    [string] $Url,

    [Parameter(Mandatory=$true)]
    [securestring] $AccessToken,

    [Parameter(Mandatory=$true)]
    [string] $Name,

    [Parameter (Mandatory=$true, ValueFromPipeline)]
    [pscustomobject] $Computer,

    [switch] $SkipCertificateCheck
)
{
    $uri = "$Url/api/wakeonlan/$Name";
    return $Computer | ConvertTo-Json | Invoke-RestMethod `
        -Method PUT `
        -Uri $uri `
        -ContentType "application/json" `
        -SkipCertificateCheck:$SkipCertificateCheck `
        -Token $AccessToken `
        -Authentication Bearer
}