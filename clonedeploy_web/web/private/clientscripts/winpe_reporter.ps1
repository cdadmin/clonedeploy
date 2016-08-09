param([string]$web,[string]$computerId,[string]$partitionNumber)

. x:\winpe_global_functions.ps1

while(Test-Path x:\wim.progress)
{
    clear
    Write-Host " ** Uploading Image For Partition $partitionNumber ** "
    Write-Host

    $post=$(cat x:\wim.progress -tail 1)
    Write-Host $post 
    $result=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "computerId=$computerId&progress=$post&progressType=wim" ${script:web}UpdateProgress --connect-timeout 10 --stderr -)
    
    Start-Sleep 2
}