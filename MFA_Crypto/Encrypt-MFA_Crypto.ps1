$location        = $MyInvocation.MyCommand.Source.Replace($MyInvocation.MyCommand.Name,"")
$exeName         = "YKBMFA_Crypto"
$serviceUser     = Read-Host "Service username"
$servicePassword = Read-Host "Password" -AsSecureString 


$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $serviceUser, $servicePassword
Set-location $location
Start-Process "$location\$exeName.exe" -ArgumentList "/d", """$location\YKBMFAConfig.xml""" -Wait -NoNewWindow -Credential $cred