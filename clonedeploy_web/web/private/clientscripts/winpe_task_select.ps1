. x:\winpe_global_functions.ps1
log -message $([System.Environment]::OSVersion.Version)

$nicList = Get-WmiObject -Class "Win32_NetworkAdapterConfiguration" -Filter "IpEnabled = TRUE"

ForEach ($nic in $nicList) 
{
    log -message " ** Looking For Active Task For $($nic.MacAddress) **"  -isDisplay "true"
    $checkInStatus=$(curl.exe $env:curlOptions -H Authorization:$env:userTokenEncoded --data "computerMac=$($nic.MacAddress)" ${web}CheckIn  --connect-timeout 10 --stderr -)
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
            log -message $checkInStatus.TaskArguments
            $arr = $checkInStatus.TaskArguments -split '\r\n'
            $pos = 0
            while($pos -lt $arr.Count - 1)
            {
                $arg=$arr[$pos] -split '='
                New-Variable -Name $arg[0] -Value $arg[1] -Scope Global -Force
                $pos++
            }
            curl.exe $env:curlOptions -H Authorization:$env:userTokenEncoded --data "computerId=$computer_id" ${web}UpdateStatusInProgress  --connect-timeout 10 --stderr -
           
            $mac=$nic.MacAddress
            log -message " ...... Success" -isDisplay "true"
            break;
        }
    }
}

if(!$mac)
{
  if($computerIsRegistered -ne "true")
  {
    . x:\winpe_register.ps1
  }
  Write-Host
  log -message "Could Not Find An Active Web Task For This Computer." -isDisplay "true"
  Write-Host
  log -message "Falling Back To On Demand Mode" -isDisplay "true"
  $mac=$nicList[0].MacAddress
  Start-Sleep -s 7
  . x:\winpe_ond.ps1
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