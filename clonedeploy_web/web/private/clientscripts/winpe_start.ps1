$web=$(Get-Content x:\windows\system32\web.txt).Trim()
powercfg /s 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c

function Set-Curl-Options()
{
  $env:curlOptions="-sSk"
}

function Encode-User-Token($userToken)
{
    $userTokenBytes = [System.Text.Encoding]::UTF8.GetBytes($userToken)
    $env:userTokenEncoded =[Convert]::ToBase64String($userTokenBytes)
}

function Check-Download($dlResult, $scriptName)
{
  if($dlResult -ne "200")
  {
    echo " ...... Could Not Download Script $scriptName "
    echo " ...... Response Code: $dlResult "
    Get-Content x:\dlerror.log
    exit 1
  }
}

function Test-Server-Conn()
{
  $connResult=$(curl.exe  $env:curlOptions "${web}Test" --connect-timeout 10 --stderr -)
  if("$connResult" -ne "true") 
  {
    $connResult
    Write-Host
    Write-Host "Could Not Contact CloneDeploy Server.  Try Entering ${web}Test In A Web Browser. "
    
    
    exit 1
  }
}

Set-Curl-Options
Test-Server-Conn
clear
Write-Host " ** CloneDeploy Login To Continue Or Close Window To Cancel ** "
Write-Host
Write-Host
$loginCount = 1
while($loginCount -le 2)
{
    $username = Read-Host -Prompt "username"
    $password = Read-Host -Prompt "password" -AsSecureString
    $decodedpassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))
 
    $usernameBytes = [System.Text.Encoding]::UTF8.GetBytes($username)
    $usernameEncoded =[Convert]::ToBase64String($usernameBytes)

    $passwordBytes = [System.Text.Encoding]::UTF8.GetBytes($decodedpassword)
    $passwordEncoded =[Convert]::ToBase64String($passwordBytes)

    $loginResult=$(curl.exe -sSk -F username="$usernameEncoded" -F password="$passwordEncoded" -F clientIP="" -F task="" "${web}ConsoleLogin" --connect-timeout 10 --stderr -)
    $loginResult=$loginResult | ConvertFrom-Json
    if(!$?)
    {
        $Error[0].Exception.Message
        $loginResult
        exit 1
    }
    else
    {
        if($loginResult.valid -eq "true")
        {
            Write-Host
            Write-Host " ...... Login Successful"
            Write-Host
            $env:userToken=$loginResult.user_token
            $env:userId=$loginResult.user_id
            break
        }
        else
        {
            if($loginCount -eq 2)
            {
                Write-Host 
                Write-Host " ...... Incorrect Login...Exiting"
                Write-Host
                exit 1
            }
            else
            {
                Write-Host
                Write-Host " ...... Incorrect Login...Try Again"
                Write-Host
            }
        }
    }
    $loginCount++
}

Encode-User-Token -userToken $env:userToken
 
Write-Host " ** Downloading Core Scripts ** "
foreach($scriptName in "winpe_task_select.ps1","winpe_global_functions.ps1","winpe_pull.ps1","winpe_ond.ps1","winpe_push.ps1","winpe_reporter.ps1")
{
  $dl_result=$(curl.exe $env:curlOptions -H Authorization:$env:userTokenEncoded --data "scriptName=$scriptName" ${web}DownloadCoreScripts -o x:\$scriptName -w "%{http_code}" --connect-timeout 10 --stderr x:\dlerror.log)
  Check-Download -dlResult $dl_result -scriptName $scriptName
}
Write-Host " ...... Success"

. x:\winpe_task_select.ps1