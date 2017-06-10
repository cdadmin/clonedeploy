. x:\wie_global_functions.ps1
. x:\wie_menu.ps1
$script:isOnDemand=$true

clear
log -message "No Active Web Tasks Were Found For This Computer.  Starting On Demand Imaging." -isDisplay "true"
Write-Host

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
		$script:imageProfileId=$imageProfileList.FirstProfileId
    }
	else
    {
        $profileTable=[ordered]@{}
        foreach($imageProfile in $imageProfileList.ImageProfiles)
        {
            $profileTable.Add($imageProfile.ProfileId,$imageProfile.ProfileName)
        }
        clear
        $script:imageProfileId=$(fShowMenu "Select An Image Profile" $profileTable)	
    }

    if($script:computer_id -eq "false")
    {
        $script:task="unregdeploy"	
    }
    else
    {
        $script:task="onddeploy"
    }
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
		
			if($addImageResult.Success -eq "true")
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
		$script:imageProfileId=$imageProfileList.FirstProfileId
    }
	else
    {
        $profileTable=[ordered]@{}
        foreach($imageProfile in $imageProfileList.ImageProfiles)
        {
            $profileTable.Add($imageProfile.ProfileId,$imageProfile.ProfileName)
        }
        clear
        $script:imageProfileId=$(fShowMenu "Select An Image Profile" $profileTable)	
    }

    if($script:computer_id -eq "false")
    {
        $script:task="unregupload"	
    }
    else
    {
        $script:task="ondupload"
    }

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
    $script:multicastId=$(fShowMenu "Select A Multicast Session" $multicastTable)

    if(!$script:multicastId)
    {
	  error "No Multicast Session Was Selected Or There Are No Active Sessions"
	}

    $script:task="ondmulticast"
}

else
{
    error "Could Not Determine Task Type"
}

