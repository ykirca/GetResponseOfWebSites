$URLs = Import-csv C:\users\administrator\scripts\fqdn.csv #FQDN list

foreach ($url in $URLs)
{
try {
    if ($url.port -eq 443)
        {
            $requestedURL = "https://" + $url.FQDN
            Write-Host($requestedURL)
        }

    elseif ($url.port -eq 80)
        {
            $requestedURL = "http://" + $url.FQDN
            Write-Host($requestedURL)
        }

    else
        {
            $requestedURL = "http://" + $url.FQDN +":" + $url.port
            Write-Host($requestedURL)
        }

        $request = [System.Net.WebRequest]::Create($requestedURL)
        $request.Method="Get"
        $response = $request.GetResponse()

        $requestStream = $response.GetResponseStream()
        $readStream = New-Object System.IO.StreamReader $requestStream
        $data=$readStream.ReadToEnd()

        $filePath = "D:\hb\migration\results\" + $url.FQDN.Split(".")[0] + "." + $url.FQDN.Split(".")[1] + $url.Port + ".txt"
        $data | Out-File -FilePath $filePath 
        } 

        catch [System.Net.WebException] 
        {
            $requestStream = $Error[0].Exception.InnerException.Response.GetResponseStream()
            $readStream = New-Object System.IO.StreamReader $requestStream
            $data =  $readStream.ReadToEnd()

        $filePath = "D:\hb\migration\results\" + $url.FQDN.Split(".")[0] + "." + $url.FQDN.Split(".")[1] + $url.Port + ".txt"
        $data | Out-File -FilePath $filePath 
        }
}

