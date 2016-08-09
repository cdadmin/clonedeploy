$script:web=$(Get-Content x:\windows\system32\web.txt).Trim()
$script:curlOptions="-sSk"
powercfg /s 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c

function Encode-User-Token($userToken)
{
    $private:userTokenBytes = [System.Text.Encoding]::UTF8.GetBytes($userToken)
    $script:userTokenEncoded =[Convert]::ToBase64String($private:userTokenBytes)
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
  $connResult=$(curl.exe  $script:curlOptions "${script:web}Test" --connect-timeout 10 --stderr -)
  if("$connResult" -ne "true") 
  {
    $connResult
    Write-Host
    Write-Host "Could Not Contact CloneDeploy Server.  Try Entering ${script:web}Test In A Web Browser. "
    
    exit 1
  }
}


Test-Server-Conn
clear
Write-Host " ** CloneDeploy Login To Continue Or Close Window To Cancel ** "
Write-Host
Write-Host
$private:loginCount = 1
while($private:loginCount -le 2)
{
    $private:username = Read-Host -Prompt "username"
    $private:password = Read-Host -Prompt "password" -AsSecureString
    $private:decodedpassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($private:password))
 
    $private:usernameBytes = [System.Text.Encoding]::UTF8.GetBytes($private:username)
    $private:usernameEncoded =[Convert]::ToBase64String($private:usernameBytes)

    $private:passwordBytes = [System.Text.Encoding]::UTF8.GetBytes($private:decodedpassword)
    $private:passwordEncoded =[Convert]::ToBase64String($private:passwordBytes)

    $private:loginResult=$(curl.exe -sSk -F username="$private:usernameEncoded" -F password="$private:passwordEncoded" -F clientIP="" -F task="" "${script:web}ConsoleLogin" --connect-timeout 10 --stderr -)
    $private:loginResult=$private:loginResult | ConvertFrom-Json
    if(!$?)
    {
        $Error[0].Exception.Message
        $private:loginResult
        exit 1
    }
    else
    {
        if($private:loginResult.valid -eq "true")
        {
            Write-Host
            Write-Host " ...... Login Successful"
            Write-Host
            $private:userToken=$private:loginResult.user_token
            $script:userId=$private:loginResult.user_id
            break
        }
        else
        {
            if($private:loginCount -eq 2)
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
    $private:loginCount++
}

Encode-User-Token -userToken $private:userToken
 
Write-Host " ** Downloading Core Scripts ** "
foreach($scriptName in "winpe_task_select.ps1","winpe_global_functions.ps1","winpe_pull.ps1","winpe_ond.ps1","winpe_push.ps1","winpe_reporter.ps1","winpe_menu.ps1")
{
  $private:dlResult=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "scriptName=$scriptName" ${script:web}DownloadCoreScripts -o x:\$scriptName -w "%{http_code}" --connect-timeout 10 --stderr x:\dlerror.log)
  Check-Download -dlResult $private:dlResult -scriptName $scriptName
}
Write-Host " ...... Success"

. x:\winpe_task_select.ps1

