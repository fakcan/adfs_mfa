$location        = $MyInvocation.MyCommand.Source.Replace($MyInvocation.MyCommand.Name,"")
$exeName         = "YKBMFA_Crypto"

Start-Process "$location\$exeName.exe" -ArgumentList "/c", """$location\YKBMFAConfig.xml""" -Wait -NoNewWindow