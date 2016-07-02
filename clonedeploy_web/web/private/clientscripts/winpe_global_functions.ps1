$clientLog="x:\clientlog.log"

function log($message, $isDisplay)
{
	Add-Content $clientLog ""
	if($isDisplay -eq "true")
    {
        Write-Host $message | tee -Append -FilePath $clientLog
    }
	else
    {
		Add-Content $clientLog $message
	}
}