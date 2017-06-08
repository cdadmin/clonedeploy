. x:\winpe_global_functions.ps1
. x:\winpe_menu.ps1
$script:isOnDemand=$true

clear
$taskTable=[ordered]@{"deploy"="Deploy";"upload"="Upload";"multicast"="Multicast";}
$taskType=$(fShowMenu "Select A Task" $taskTable)


if($taskType -eq "deploy")
{
    
    $imageList=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "environment=winpe&userId=$script:userId" ${script:web}ListImages --connect-timeout 10 --stderr -)
    $imageList = $imageList | ConvertFrom-Json
    $imageTable=[ordered]@{}
    foreach($image in $imageList)
    {
        $imageTable.Add($image.ImageId,$image.ImageName)
    }
    clear
    $imageId=$(fShowMenu "Select An Image" $imageTable)


    if(!$imageId)
    {
        error "No Image Was Selected Or No Images Have Been Added Yet"
    }
    $imageProfileList=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "imageId=$imageId" ${script:web}ListImageProfiles --connect-timeout 10 --stderr -)
	$imageProfileList = $imageProfileList | ConvertFrom-Json
    if($imageProfileList.Count -eq "1")
    {
		$imageProfileId=$imageProfileList.FirstProfileId
    }
	else
    {
        $profileTable=[ordered]@{}
        foreach($imageProfile in $imageProfileList.ImageProfiles)
        {
            $profileTable.Add($imageProfile.ProfileId,$imageProfile.ProfileName)
        }
        clear
        $imageProfileId=$(fShowMenu "Select An Image Profile" $profileTable)	
    }
    $task="push"
    $ondObjectId=$imageProfileId
    #$script:ondArgs=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "mac=$script:mac&objectId=$imageProfileId&task=push" ${script:web}GetOnDemandArguments --connect-timeout 10 --stderr -)
	
}
elseif($taskType -eq "upload")
{
    clear
    $newExistingTable=[ordered]@{"new"="New";"existing"="Existing";}
    $newOrExisting=$(fShowMenu "New Or Existing Image?" $newExistingTable)
    
    if($newOrExisting -eq "new")
    {
    while($isError -ne "false")
    {
	  if($isError -eq "true")
        {
            $newImageName = Read-Host -Prompt "Invalid Name.  Enter An Image Name"
	   
        }
        	  
      else
      {
	     $newImageName = Read-Host -Prompt "Enter An Image Name"
	  }	
	  
	  if(!$newImageName)
        {
	    $isError="true"
        continue
	  }
        $addImageResult=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "name=$newImageName" ${script:web}AddImageWinPEEnv --connect-timeout 10 --stderr -)
	    $addImageResult = $addImageResult | ConvertFrom-Json
		if(!$?)
        {
            $Error[0].Exception.Message
            log $addImageResult "true"
            error "Could Not Parse Add New Image Result"
        }
		
			if($addImageResult.IsValid -eq "true")
            {
			  $imageId=$addImageResult.Message
			  $isError="false"
            }
			else
            {
			  $isError="true"
                continue
			}
		
     }
   }
    
  elseif($newOrExisting -eq "existing")
  {
   $imageList=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "environment=winpe&userId=$script:userId" ${script:web}ListImages --connect-timeout 10 --stderr -)
    $imageList = $imageList | ConvertFrom-Json
    $imageTable=[ordered]@{}
    foreach($image in $imageList)
    {
        $imageTable.Add($image.ImageId,$image.ImageName)
    }
    clear
    $imageId=$(fShowMenu "Select An Image" $imageTable)
  }
  else
  {
    error "Could Not Determine If This Is A New Or Existing Image"
  }
  
    if(!$imageId)
    {
	  error "No Image Was Selected Or No Images Have Been Added Yet"
    }

    $imageProfileList=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "imageId=$imageId" ${script:web}ListImageProfiles --connect-timeout 10 --stderr -)
	$imageProfileList = $imageProfileList | ConvertFrom-Json
    if($imageProfileList.Count -eq "1")
    {
		$imageProfileId=$imageProfileList.FirstProfileId
    }
	else
    {
        $profileTable=[ordered]@{}
        foreach($imageProfile in $imageProfileList.ImageProfiles)
        {
            $profileTable.Add($imageProfile.ProfileId,$imageProfile.ProfileName)
        }
        clear
        $imageProfileId=$(fShowMenu "Select An Image Profile" $profileTable)	
    }
    $task="pull"
    $ondObjectId=$imageProfileId
    #$script:ondArgs=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "mac=$script:mac&objectId=$imageProfileId&task=pull" ${script:web}GetOnDemandArguments --connect-timeout 10 --stderr -)
}

elseif($taskType -eq "multicast")
{
    $multicastList=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "environment=winpe" ${script:web}ListMulticasts --connect-timeout 10 --stderr -)
    $multicastList = $multicastList | ConvertFrom-Json
    $multicastTable=[ordered]@{}
    foreach($multicast in $multicastList)
    {
        $multicastTable.Add($multicast.Port,$multicast.Name)
    }
    clear
    $multicastId=$(fShowMenu "Select A Multicast Session" $multicastTable)

    if(!$multicastId)
    {
	  error "No Multicast Session Was Selected Or There Are No Active Sessions"
	}

    $task="multicast"
    $ondObjectId=$multicastId
    #$script:ondArgs=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "mac=$script:mac&objectId=$multicastId&task=multicast" ${script:web}GetOnDemandArguments --connect-timeout 10 --stderr -)
}

else
{
    error "Could Not Determine Task Type"
}

log " ** Using On Demand Mode ** "
  $checkInStatus=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "mac=$script:mac&objectId=$ondObjectId&task=$task&userId=$script:userId" ${script:web}OnDemandCheckIn --connect-timeout 10 --stderr -)
    $checkInStatus=$checkInStatus | ConvertFrom-Json
    if(!$?)
    {
        $Error[0].Exception.Message
        $checkInStatus
        exit 1
    }
    else
    {
           log -message $checkInStatus.TaskArguments
            $arr = $checkInStatus.TaskArguments -split '\r\n'
            $pos = 0
            while($pos -lt $arr.Count - 1)
            {
                $arg=$arr[$pos] -split '='
                New-Variable -Name $arg[0] -Value $arg[1] -Scope Script -Force
                $pos++
            }
    }


clear
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

