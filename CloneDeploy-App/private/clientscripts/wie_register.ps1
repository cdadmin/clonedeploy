. x:\wie_global_functions.ps1
clear

$registration=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded ${script:web}RegistrationSettings --connect-timeout 10 --stderr -)
$registration=$registration | ConvertFrom-Json
if($registration.registrationEnabled -eq "No" -and $registration.keepNamePrompt -eq "No")
{
    return 0
}

$script:clientId="$script:mac.$script:serialNumber.$script:systemUuid"
Write-Host " ** This Computer Is Not Registered ** "
Write-Host

Write-Host " ** MAC Address: $script:mac ** "
Write-Host " ** Client Id  : $script:clientId"

while($true)
{

    if($registration.registrationEnabled -eq "No" -and $registration.keepNamePrompt -eq "Yes")
    {
        Write-Host " ** Registration Is Disabled.  Enter A Computer Name To Rename During Imaging.  Leave Blank To Skip. ** "
        Write-Host
	    $computer_name = Read-Host -Prompt "Computer Name "
	    break
    }    	


	Write-Host " ** Enter A Computer Name To Register.  Leave Blank To Skip Registration. ** "
    Write-Host
	$name = Read-Host -Prompt "Computer Name "
	if(!$name)
    { 
		break
	}
	           
    $registerResult=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "name=$name&mac=$script:mac&clientIdentifier=$clientId" ${script:web}AddComputer --connect-timeout 10 --stderr -)
	if(!$?)
    {
	    Write-Host " ...... Could Not Register Computer: $registerResult"
    }
    else
    {
        $registerResult=$registerResult | ConvertFrom-Json
        if(!$?)
        {
            $Error[0].Exception.Message
            $registerResult
            log "Could Not Parse Computer Registration Result"
            continue
        }
	    if($registerResult.Success -eq "true")
        {
		    Write-Host " ...... Success"
            $script:computer_id=$registerResult.Id
			break
	    }	
    	else
		{
    		Write-Host " ...... $($registerResult.ErrorMessage)"
			Write-Host
		}
    }
}

