. x:\wie_global_functions.ps1
Write-Host

if(Test-Path $clientLog) { rm $clientLog }
log -message "WinPE Version:"
log -message $([System.Environment]::OSVersion.Version)
log -message "System Architecture:"
log -message $(gwmi win32_operatingsystem | select OSArchitecture)


try
{
    $efi=Confirm-SecureBootUEFI
    if($efi -eq $false)
    {
        $script:bootType="efi"
        log -message "EFI Enabled / Secure Boot Disabled"
    }
    elseif($efi -eq $true)
    {
        $script:bootType="efi"
        log -message "EFI Enabled / Secure Boot Enabled"
    }
}
catch
{
     $script:bootType="bios"
     log -message "Using Legacy BIOS"
}



#Assume task is on demand and computer does not exist until each are found
$script:task="ond"
$script:computer_id="false"
$script:serialNumber=$(get-wmiobject Win32_ComputerSystemProduct  | Select-Object -ExpandProperty IdentifyingNumber)
$script:systemUuid=$(get-wmiobject Win32_ComputerSystemProduct  | Select-Object -ExpandProperty UUID)
$script:systemModel=$(get-wmiobject Win32_ComputerSystemProduct  | Select-Object -ExpandProperty Name)
$nicList = Get-WmiObject -Class "Win32_NetworkAdapterConfiguration" #-Filter "IpEnabled = TRUE"

ForEach ($nic in $nicList) 
{
    
    $script:mac=$nic.MacAddress
    $script:clientId="$script:mac.$script:serialNumber.$script:systemUuid"
    Write-Host
    log -message " ** Looking For Active Task For $($script:clientId) **"  -isDisplay "true"
   
    $computerTaskObject=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "idType=clientId&id=$($script:clientId)" ${script:web}DetermineTask  --connect-timeout 10 --stderr -)
    log -message $computerTaskObject
    $computerTaskObject=$computerTaskObject | ConvertFrom-Json
    if(!$?)
    {
        $Error[0].Exception.Message
        $computerTaskObject
        exit 1
    }
    else
    {
        if($computerTaskObject.computerId -eq "false" -or ! $computerTaskObject.computerId)
        {
            log -message " ...... This Computer Was Not Found" -isDisplay "true"
            continue
        }
        else
        {
            if($computerTaskObject.task -eq "upload" -or $computerTaskObject.task -eq "deploy" -or $computerTaskObject.task -eq "permanentdeploy" -or $computerTaskObject.task -eq "multicast")
            {
                $script:computer_id=$computerTaskObject.computerId
                $script:task=$computerTaskObject.task
                $script:taskId=$computerTaskObject.taskId
                log -message " ...... Success" -isDisplay "true"
                $taskFound=$true
                break
            }
            else
            {
                $script:computer_id=$computerTaskObject.computerId
                log -message " ...... An Active Task Was Not Found For This Computer" -isDisplay "true"
		        continue
            }
        }
    }
}

 #Only check by mac if the clientId wasn't found.  
 if(!$taskFound -and $script:computer_id -eq "false")
 {
    ForEach ($nic in $nicList) 
    {
        $script:mac=$nic.MacAddress
        Write-Host
        log -message " ** Looking For Active Task For $($script:mac) **"  -isDisplay "true"
   
        $computerTaskObject=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "idType=mac&id=$($script:mac)" ${script:web}DetermineTask  --connect-timeout 10 --stderr -)
        log -message $computerTaskObject
        $computerTaskObject=$computerTaskObject | ConvertFrom-Json
        if(!$?)
        {
            $Error[0].Exception.Message
            $computerTaskObject
            exit 1
        }
        else
        {
            if($computerTaskObject.computerId -eq "false" -or ! $computerTaskObject.computerId)
            {
                log -message " ...... This Computer Was Not Found" -isDisplay "true"
                continue
            }
            else
            {
                if($computerTaskObject.task -eq "upload" -or $computerTaskObject.task -eq "deploy" -or $computerTaskObject.task -eq "permanantdeploy" -or $computerTaskObject.task -eq "multicast")
                {
                    $script:computer_id=$computerTaskObject.computerId
                    $script:task=$computerTaskObject.task
                    $script:taskId=$computerTaskObject.taskId
                    log -message " ...... Success" -isDisplay "true"
                    $taskFound=$true
                    break
                }
                else
                {
                    $script:computer_id=$computerTaskObject.computerId
                    log -message " ...... An Active Task Was Not Found For This Computer" -isDisplay "true"
		            continue
                }
            }
        }
    }
}


  
if(!$taskFound)
{
    log -message " ** Using On Demand Mode ** "
    $script:isOnDemand=$true
    Write-Host
	$script:mac=$nicList.MacAddress | select -first 1

    log -message " ** Looking For Model Match Task For $script:systemModel ** " -isDisplay "true"
    $modelMatchTask=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "environment=winpe&systemModel=$script:systemModel" ${script:web}ModelMatch --connect-timeout 10 --stderr -)
    $modelMatchTask=$modelMatchTask | ConvertFrom-Json
    if(!$?)
    {
        log -message " ** Could Not Determine Model Match.  Ignoring. ** "
    }  
    elseif($modelMatchTask.imageName)
    {
        $script:task="modelmatchdeploy"
        $script:imageProfileId=$modelMatchTask.imageProfileId
        if($script:computer_id -eq "false")
        {
            log -message "This Computer Is Not Registered.  A Model Match Was Found For This Computer.  Image Deploy Will Auto Start After Registration, If Enabled.  Image: $($modelMatchTask.imageName) Profile: $($modelMatchTask.imageProfileName)" -isDisplay "true"
            . x:\wie_register.ps1
        }
        else
        {
             log -message "A Model Match Was Found For This Computer.  Image: $($modelMatchTask.imageName) Profile: $($modelMatchTask.imageProfileName)" -isDisplay "true"
             Write-Host
             Write-Host "Image Deployment Will Begin Now.   Close This Window To Cancel, Or:"
             pause
        }
    }
    else
    {
        
        if($script:computer_id -eq "false")
        {
            log -message "This Computer Is Not Registered.  No Active Web Tasks Were Found For This Computer.  Starting Registration." -isDisplay "true"
            . x:\wie_register.ps1
        }


        . x:\wie_ond.ps1
    }
    
  
    if($script:task -eq "ondmulticast")
    {
        $checkInStatus=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "mac=$script:mac&objectId=$script:multicastId&task=$script:task&userId=$script:userId&computerId=$script:computer_id" ${script:web}OnDemandCheckIn --connect-timeout 10 --stderr -)
    }
    else
    {
        $checkInStatus=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "mac=$script:mac&objectId=$script:imageProfileId&task=$script:task&userId=$script:userId&computerId=$script:computer_id" ${script:web}OnDemandCheckIn --connect-timeout 10 --stderr -)
    }
    
}
else
{
    log -message " ** Verifying Active Task ** " -isDisplay "true"
    $checkInStatus=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "taskId=$script:taskId" ${script:web}CheckIn  --connect-timeout 10 --stderr -)
}

$checkInStatus=$checkInStatus | ConvertFrom-Json
if(!$?)
{
    $Error[0].Exception.Message
    $checkInStatus
    exit 1
}
else
{
    if($checkInStatus.Result -ne "true")
    {
        error $checkInStatus.Message
    }
    else
    {
        Start-Sleep -s 1

        log -message $checkInStatus.TaskArguments
        $arr = $checkInStatus.TaskArguments -split '\r\n'
        $pos = 0
        while($pos -lt $arr.Count - 1)
        {
            $arg=$arr[$pos] -split '='
            New-Variable -Name $arg[0] -Value $arg[1] -Scope Script -Force
            $pos++
        }

        if($script:isOnDemand)
        {
            #On demand task ids are created later, get it now
            $script:taskId=$checkInStatus.TaskId
        }

        #This is checked outside of the task arguments because if the environment doesn't match the arguments won't parse
        if($checkInStatus.ImageEnvironment -ne "winpe")
        {
            error "The Imaging Environment For The Image Does Not Match The Currently Loaded Environment"
        }
        
        
                      
        log -message " ...... Success" -isDisplay "true"
            
    }
}



if($script:task -eq "upload" -or $script:task -eq "unregupload" -or $script:task -eq "ondupload")
{
  . x:\wie_upload.ps1
}
elseif($script:task -eq "deploy" -or $script:task -eq "permanentdeploy" -or $script:task -eq "multicast" -or $script:task -eq "ondmulticast" -or $script:task -eq "unregdeploy" -or $script:task -eq "onddeploy" -or $script:task -eq "clobber" -or $script:task -eq "modelmatchdeploy")
{
  . x:\wie_deploy.ps1
}
else
{
  error -message "Could Not Determine Task Type"
}

  #Shouldn't get this far but catch it anyway
if($script:task_completed_action.trim("`"") -eq "Power Off")
{
	wpeutil shutdown
}

elseif($script:task_completed_action.trim("`"") -eq "Exit To Shell")
{
	[Environment]::Exit(0)
}
else
{
	wpeutil reboot
} 