. x:\wie_global_functions.ps1
clear

$script:clientId="$script:mac.$script:serialNumber.$script:systemUuid"
Write-Host " ** This Computer Is Not Registered ** "
Write-Host

Write-Host " ** MAC Address: $script:mac ** "
Write-Host " ** Client Id  : $script:clientId"

while($true)
{
	
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