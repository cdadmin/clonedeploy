. x:\winpe_global_functions.ps1
clear

Write-Host " ** Computer Registration ** "
Write-Host

Write-Host " ** System MAC Address: $script:mac ** "
Write-Host

while($true)
{
	
	Write-Host " ** Enter A Computer Name To Register.  Leave Blank To Skip Registration. ** "
	Write-Host
	$name = Read-Host -Prompt "Computer Name: "
	if(!$name)
    { 
		break
	}
	           
    $registerResult=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "name=$name&mac=$mac" ${script:web}AddComputer --connect-timeout 10 --stderr -)
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
	    if($registerResult.IsValid -eq "true")
        {
		    Write-Host " ...... Success"
			break
	    }	
    	else
		{
    		Write-Host " ...... $($registerResult.Message)"
			Write-Host
		}
    }
}