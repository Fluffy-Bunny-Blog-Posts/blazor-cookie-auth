$Time = [System.Diagnostics.Stopwatch]::StartNew()

function PrintElapsedTime {
    Log $([string]::Format("Elapsed time: {0}.{1}", $Time.Elapsed.Seconds, $Time.Elapsed.Milliseconds))
}

function Log {
    Param ([string] $s)
    Write-Output "###### $s"
}

function Check {
    Param ([string] $s)
    if ($LASTEXITCODE -ne 0) { 
        Log "Failed: $s"
        throw "Error case -- see failed step"
    }
}

$DockerOS = docker version -f "{{ .Server.Os }}"

$Version = "0.0.3"

PrintElapsedTime

Log "Build application image"

docker build . --file ./Build-Dockerfile     --tag fluffybunny/blazorcookieauthbuild 
docker build . --file ./App-Dockerfile       --tag fluffybunny/blazorcookieauthapp

 