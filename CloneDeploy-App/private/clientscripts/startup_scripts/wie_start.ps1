. x:\wie_menu.ps1

$script:web=$(Get-Content x:\windows\system32\web.txt).Trim()
try
{
    $private:uToken=$(Get-Content x:\windows\system32\uToken.txt -ErrorAction Ignore).Trim()
}
catch
{
    #Suppress error if universal token is not used
}
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
    clear
    Write-Host
    
    Write-Host "Could Not Contact CloneDeploy Server.  Possible Reasons:"
    Write-Host "The CloneDeploy Web Service Is Not Functioning."  
    Write-Host "Try Entering ${script:web}Test In A Web Browser. "
    Write-Host "A Driver Could Not Be Found For This NIC."
    Write-Host "The Computer Did Not Receive An Ip Address."

    $taskTable=[ordered]@{"display"="Display Available NICs";"static"="Assign Static IP";"shutdown"="Shutdown";"exit"="Exit To Shell";}
    
    while($true)
    {
        Write-Host
        $taskType=$(fShowMenu "Select An Action" $taskTable)
        if($taskType -eq "display")
        {
            clear
            Write-Host "Displaying Available Network Interfaces"
            $nicList = Get-WmiObject -Class "Win32_NetworkAdapterConfiguration"
            $nicList | fl description, macaddress, ipaddress, ipenabled, dhcpenabled
        }
        elseif($taskType -eq "static")
        {
            Assign-Static-IP
            break
        }
        elseif($taskType -eq "exit")
        {
            [Environment]::Exit(1)
        }
        elseif($taskType -eq "shutdown")
        {
            wpeutil shutdown
        }
    }
  }
}


function Assign-Static-IP()
{
    $nicSelection=@{}
    $nicList = Get-WmiObject -Class "Win32_NetworkAdapterConfiguration"
    foreach($nic in $nicList)
    {
        $nicSelection.Add("$($nic.InterfaceIndex)","$($nic.Description)")
    }
    Write-Host
    $selectedNic=$(fShowMenu "Select A NIC" $nicSelection)
    Write-Host
    $ip = Read-Host -Prompt "Ipv4 Address "
    $subnet = Read-Host -Prompt " Subnet Mask "
    $gateway = Read-Host -Prompt "     Gateway "
    $dns = Read-Host -Prompt "         DNS "

    netsh int ipv4 set address "$selectedNic" static $ip $subnet $gateway
    netsh int ipv4 set dns "$selectedNic" static $dns
    Start-Sleep -s 5
    Test-Server-Conn
}

Test-Server-Conn
clear

if ($private:uToken)
{
    $private:userToken=$private:uToken
}
else
{
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
}

Encode-User-Token -userToken $private:userToken
clear

Write-Host " ** Downloading Core Scripts ** "
foreach($scriptName in "wie_task_select.ps1","wie_global_functions.ps1","wie_upload.ps1","wie_ond.ps1","wie_deploy.ps1","wie_reporter.ps1","wie_register.ps1")
{
  $private:dlResult=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "scriptName=$scriptName" ${script:web}DownloadCoreScripts -o x:\$scriptName -w "%{http_code}" --connect-timeout 10 --stderr x:\dlerror.log)
  Check-Download -dlResult $private:dlResult -scriptName $scriptName
}
Write-Host " ...... Success"

. x:\wie_task_select.ps1

