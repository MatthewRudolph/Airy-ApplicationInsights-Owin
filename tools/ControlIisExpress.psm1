function Start-IISExpress (
	[Parameter(Mandatory=$true)][string]$path,
	[Parameter(Mandatory=$true)][string]$port,
	[Parameter(Mandatory=$false)][string]$jobName = "IISExpressJob",
	[Parameter(Mandatory=$false)][string]$iisExpressExe = "C:\Program Files\IIS Express\iisexpress.exe"
){
	if ( [bool](get-job -Name $jobName -ea silentlycontinue) ){
		Stop-IISExpress -JobName $jobName
	}

	Start-Job -Name $jobName -Arg $iisExpressExe, $port, $path -ScriptBlock {
		param ($iisExpressExe, $port, $path)
		Start-process $iisExpressExe -ArgumentList "/port:$port /path:$path" -WindowStyle Hidden
		Start-Sleep -m 1000
	}

	Write-Host "IIS Express started"
}

function Stop-IISExpress(
	[Parameter(Mandatory=$false)][string]$jobName = "IISExpressJob"
){
	if ( [bool](get-job -Name $jobName -ea silentlycontinue) ){
		Stop-Job -Name $jobName
		Remove-Job -Name $jobName
	}
}
