. x:\winpe_global_functions.ps1
Write-Host

if(Test-Path $clientLog) { rm $clientLog }
log -message "WinPE Version:"
log -message $([System.Environment]::OSVersion.Version)
log -message "System Architecture:"
log -message $(gwmi win32_operatingsystem | select OSArchitecture)
try
{
    $efi=Confirm-SecureBootUEFI 2>&1 >> $clientLog
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

$nicList = Get-WmiObject -Class "Win32_NetworkAdapterConfiguration" -Filter "IpEnabled = TRUE"

ForEach ($nic in $nicList) 
{
    log -message " ** Looking For Active Task For $($nic.MacAddress) **"  -isDisplay "true"
    $checkInStatus=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "computerMac=$($nic.MacAddress)" ${script:web}CheckIn  --connect-timeout 10 --stderr -)
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
            log -message $checkInStatus.Message -isDisplay "true"
	        if($checkInStatus.Message -eq "An Active Task Was Not Found For This Computer")
            {
	            $computerIsRegistered="true"
            }
            Start-Sleep -s 1
            Write-Host
            continue
        }
        else
        {
            Start-Sleep -s 1

            #This is checked outside of the task arguments because if the environment doesn't match the arguments won't parse
            if($checkInStatus.ImageEnvironment -ne "winpe")
            {
                error "The Imaging Environment For The Image Does Not Match The Currently Loaded Environment"
            }

            if($checkInStatus.TaskType -eq "permanent_push")
            {
                $script:isPermanentTask=$true
            }

            log -message $checkInStatus.TaskArguments
            $arr = $checkInStatus.TaskArguments -split '\r\n'
            $pos = 0
            while($pos -lt $arr.Count - 1)
            {
                $arg=$arr[$pos] -split '='
                New-Variable -Name $arg[0] -Value $arg[1] -Scope Script -Force
                $pos++
            }
                      
            $script:mac=$nic.MacAddress
            log -message " ...... Success" -isDisplay "true"
            break;
        }
    }
}

if(!$script:mac)
{
  $script:mac=$nicList.MacAddress | select -first 1
  if($computerIsRegistered -ne "true")
  {
    . x:\winpe_register.ps1
  }

  log -message " ...... Could Not Find An Active Web Task For This Computer." -isDisplay "true"
  log -message " ...... Falling Back To On Demand Mode" -isDisplay "true"
  
  Start-Sleep -s 7
  . x:\winpe_ond.ps1
}
else
{
    $script:isOnDemand=$false
}

Write-Host

if($image_direction -eq "pull")
{
  . x:\winpe_pull.ps1
}
elseif($image_direction -eq "push")
{
  . x:\winpe_push.ps1
}
else
{
  error -message "Could Not Determine Task Direction"
}