. x:\winpe_global_functions.ps1
log -message $([System.Environment]::OSVersion.Version)

$nicList = Get-WmiObject -Class "Win32_NetworkAdapterConfiguration" -Filter "IpEnabled = TRUE"

function test()
{
 Write-Host name from function $computer_name
}

ForEach ($nic in $nicList) 
{
    log " ** Looking For Active Task For $($nic.MacAddress) **" "true"
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
	            $computer_is_registered="true"
            }
            Start-Sleep -s 1
            Write-Host
            continue
        }
        else
        {
            $checkInStatus.TaskArguments
            Start-Sleep -s 1
            $arr = $checkInStatus.TaskArguments -split '\r\n'
            $pos = 0
            while($pos -lt $arr.Count - 1)
            {
                $arg=$arr[$pos] -split '='
                New-Variable -Name $arg[0] -Value $arg[1] -Scope Global -Force
               test
                $pos++
            }
            break;
        }
    }
}

. x:\winpe_pull.ps1