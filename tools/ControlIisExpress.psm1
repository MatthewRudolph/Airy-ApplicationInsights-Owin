<##########################################################################################
#Date: 		05 January 2017                                                               #
#Author: 	Matthew Rudolph                                                               #
#Script:	Allows the starting and stopping of an IIS Express site based on its location.#
#Version:	1.0																              #
###########################################################################################
 <#
.SYNOPSIS
	Allows the starting and stopping of an IIS Express site.
.DESCRIPTION
	Allows the starting and stopping of an IIS Express site based on its location.
.EXAMPLE
	Import-Module .\tools\ControlIisExpress.psm1
	Start-IISExpress "C:projects\Dematt.Airy.ApplicationInsights.Owin\samples\Dematt.Airy.ApplicationInsights.Sample" 11156
.NOTES
  References original code from: https://gist.github.com/drmohundro/5a131d7ff6f291a33334
#>

function Start-IISExpress (
	[Parameter(Mandatory=$true)][string]$path,
	[Parameter(Mandatory=$true)][string]$port,
	[Parameter(Mandatory=$false)][string]$jobName = "IISExpressJob",
	[Parameter(Mandatory=$false)][string]$iisExpressExe = "C:\Program Files\IIS Express\iisexpress.exe"
)
{
	if ([bool](get-job -Name $jobName -ea silentlycontinue)){
		Stop-IISExpress -JobName $jobName
	}

	Start-Job -Name $jobName -Arg $iisExpressExe, $port, $path -ScriptBlock {
		param ($iisExpressExe, $port, $path)
		Start-process $iisExpressExe -ArgumentList "/port:$port /path:$path" -WindowStyle Hidden
		Start-Sleep -m 2500
	}

	Write-Host "IIS Express started"
}

function Stop-IISExpress(
	[Parameter(Mandatory=$false)][string]$jobName = "IISExpressJob"
)
{
	if ([bool](get-job -Name $jobName -ea silentlycontinue)){
		Stop-Job -Name $jobName
		Remove-Job -Name $jobName
	}
}
