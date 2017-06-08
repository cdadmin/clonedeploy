param([string]$web,[string]$computerId,[string]$partitionNumber,[string]$direction,[string]$curlOptions,[string]$userTokenEncoded)

. x:\winpe_global_functions.ps1

 Write-Host " ** $direction Image For Partition $partitionNumber ** "
 Write-Host

while(Test-Path x:\wim.progress)
{
    clear
    Write-Host " ** $direction Image For Partition $partitionNumber ** "
    Write-Host
    $post=$(Get-Content x:\wim.progress | Select -last 1)
    Write-Host $post


    $result=$(curl.exe $curlOptions -H Authorization:$userTokenEncoded --data "computerId=$computerId&progress=$post&progressType=wim" ${web}UpdateProgress --connect-timeout 10 --stderr -)
    
    Start-Sleep -s 2
}