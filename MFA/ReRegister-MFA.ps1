$location       = $MyInvocation.MyCommand.Source.Replace($MyInvocation.MyCommand.Name,"")
$dllName        = "MFA"
$dllVersion     = "1.0.0.1"
$publicKeyToken = "6ea2b24ccf4019fc"

Set-location $location
Unregister-AdfsAuthenticationProvider -Name $dllName -Confirm:$false

net stop adfssrv
net start adfssrv

[System.Reflection.Assembly]::Load("System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
$publish = New-Object System.EnterpriseServices.Internal.Publish
$publish.GacRemove("$location\$dllName.dll")
$publish.GacInstall("$location\$dllName.dll")

$typeName = "$dllName.MFAuthenticationAdapter, $dllName, Version=$dllVersion, Culture=neutral, PublicKeyToken=$publicKeyToken, processorArchitecture=MSIL"
Register-AdfsAuthenticationProvider -TypeName $typeName -Name $dllName -ConfigurationFilePath "$location\MFAConfigPointer.xml" -Verbose

net stop adfssrv
net start adfssrv
