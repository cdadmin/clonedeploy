###############################################################################
# Simple Textbased Powershell Menu
# Author : Michael Albert
# E-Mail : info@michlstechblog.info
# License: none, feel free to modify
# usage:
# Source the menu.ps1 file in your script:
# . .\menu.ps1
# fShowMenu requieres 2 Parameters:
# Parameter 1: [string]MenuTitle
# Parameter 2: [hashtable]@{[string]"ReturnString1"=[string]"Menu Entry 1";[string]"ReturnString2"=[string]"Menu Entry 2";[string]"ReturnString3"=[string]"Menu Entry 3"
# Return     : Select String
# For example:
# fShowMenu "Choose your favorite Band" @{"sl"="Slayer";"me"="Metallica";"ex"="Exodus";"an"="Anthrax"}
# #############################################################################

function fShowMenu([System.String]$sMenuTitle,$hMenuEntries)
{
	# Orginal Konsolenfarben zwischenspeichern
	[System.Int16]$iSavedBackgroundColor=[System.Console]::BackgroundColor
	[System.Int16]$iSavedForegroundColor=[System.Console]::ForegroundColor
	# Menu Colors
	# inverse fore- and backgroundcolor 
	[System.Int16]$iMenuForeGroundColor=$iSavedForegroundColor
	[System.Int16]$iMenuBackGroundColor=$iSavedBackgroundColor
	[System.Int16]$iMenuBackGroundColorSelectedLine=$iMenuForeGroundColor
	[System.Int16]$iMenuForeGroundColorSelectedLine=$iMenuBackGroundColor
	# Alternative, colors
	#[System.Int16]$iMenuBackGroundColor=0
	#[System.Int16]$iMenuForeGroundColor=7
	#[System.Int16]$iMenuBackGroundColorSelectedLine=10
	# Init
	[System.Int16]$iMenuStartLineAbsolute=0
	[System.Int16]$iMenuLoopCount=0
	[System.Int16]$iMenuSelectLine=1
	[System.Int16]$iMenuEntries=$hMenuEntries.Count
	[Hashtable]$hMenu=[ordered]@{};
	[Hashtable]$hMenuHotKeyList=[ordered]@{};
	[Hashtable]$hMenuHotKeyListReverse=[ordered]@{};
	[System.Int16]$iMenuHotKeyChar=0
	[System.String]$sValidChars=""
	[System.Console]::WriteLine(" "+$sMenuTitle)
	# Für die eindeutige Zuordnung Nummer -> Key
	$iMenuLoopCount=1
	# Start Hotkeys mit "1"!
	$iMenuHotKeyChar=49
	foreach ($sKey in $hMenuEntries.Keys){       
		$hMenu.Add([System.Int16]$iMenuLoopCount,[System.String]$sKey)
		# Hotkey zuordnung zum Menueintrag
		$hMenuHotKeyList.Add([System.Int16]$iMenuLoopCount,[System.Convert]::ToChar($iMenuHotKeyChar))
		$hMenuHotKeyListReverse.Add([System.Convert]::ToChar($iMenuHotKeyChar),[System.Int16]$iMenuLoopCount)
		$sValidChars+=[System.Convert]::ToChar($iMenuHotKeyChar)
		$iMenuLoopCount++
		$iMenuHotKeyChar++
		# Weiter mit Kleinbuchstaben
		if($iMenuHotKeyChar -eq 58){$iMenuHotKeyChar=97}
		# Weiter mit Großbuchstaben
		elseif($iMenuHotKeyChar -eq 123){$iMenuHotKeyChar=65}
		# Jetzt aber ende
		elseif($iMenuHotKeyChar -eq 91){
			Write-Error " Menu too big!"
			exit(99)
		}
	}
	# Remember Menu start
	[System.Int16]$iBufferFullOffset=0
	$iMenuStartLineAbsolute=[System.Console]::CursorTop
	do{
		####### Draw Menu  #######
		[System.Console]::CursorTop=($iMenuStartLineAbsolute-$iBufferFullOffset)
		for ($iMenuLoopCount=1;$iMenuLoopCount -le $iMenuEntries;$iMenuLoopCount++){
			[System.Console]::Write("`r")
			[System.String]$sPreMenuline=""
			$sPreMenuline="  "+$hMenuHotKeyList[[System.Int16]$iMenuLoopCount]
			$sPreMenuline+=": "
			if ($iMenuLoopCount -eq $iMenuSelectLine){
				[System.Console]::BackgroundColor=$iMenuBackGroundColorSelectedLine
				[System.Console]::ForegroundColor=$iMenuForeGroundColorSelectedLine
			}
			if ($hMenuEntries.Item([System.String]$hMenu.Item($iMenuLoopCount)).Length -gt 0){
				[System.Console]::Write($sPreMenuline+$hMenuEntries.Item([System.String]$hMenu.Item($iMenuLoopCount)))
			}
			else{
				[System.Console]::Write($sPreMenuline+$hMenu.Item($iMenuLoopCount))
			}
			[System.Console]::BackgroundColor=$iMenuBackGroundColor
			[System.Console]::ForegroundColor=$iMenuForeGroundColor
			[System.Console]::WriteLine("")
		}
		[System.Console]::BackgroundColor=$iMenuBackGroundColor
		[System.Console]::ForegroundColor=$iMenuForeGroundColor
		[System.Console]::Write("  Your choice: " )
		if (($iMenuStartLineAbsolute+$iMenuLoopCount) -gt [System.Console]::BufferHeight){
			$iBufferFullOffset=($iMenuStartLineAbsolute+$iMenuLoopCount)-[System.Console]::BufferHeight
		}
		####### End Menu #######
		####### Read Kex from Console 
		$oInputChar=[System.Console]::ReadKey($true)
		# Down Arrow?
		if ([System.Int16]$oInputChar.Key -eq [System.ConsoleKey]::DownArrow){
			if ($iMenuSelectLine -lt $iMenuEntries){
				$iMenuSelectLine++
			}
		}
		# Up Arrow
		elseif([System.Int16]$oInputChar.Key -eq [System.ConsoleKey]::UpArrow){
			if ($iMenuSelectLine -gt 1){
				$iMenuSelectLine--
			}
		}
		elseif([System.Char]::IsLetterOrDigit($oInputChar.KeyChar)){
			[System.Console]::Write($oInputChar.KeyChar.ToString())	
		}
		[System.Console]::BackgroundColor=$iMenuBackGroundColor
		[System.Console]::ForegroundColor=$iMenuForeGroundColor
	} while(([System.Int16]$oInputChar.Key -ne [System.ConsoleKey]::Enter) -and ($sValidChars.IndexOf($oInputChar.KeyChar) -eq -1))
	
	# reset colors
	[System.Console]::ForegroundColor=$iSavedForegroundColor
	[System.Console]::BackgroundColor=$iSavedBackgroundColor
	if($oInputChar.Key -eq [System.ConsoleKey]::Enter){
		[System.Console]::Writeline($hMenuHotKeyList[$iMenuSelectLine])
		return([System.String]$hMenu.Item($iMenuSelectLine))
	}
	else{
		[System.Console]::Writeline("")
		return($hMenu[$hMenuHotKeyListReverse[$oInputChar.KeyChar]])
	}
}