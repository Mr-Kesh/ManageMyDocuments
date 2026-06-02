$ErrorActionPreference = "Stop"

$mysqlHome = Join-Path $env:LOCALAPPDATA "Programs\MySQL\mysql-8.4.9-winx64"
$myIni = Join-Path $env:LOCALAPPDATA "Programs\MySQL\my.ini"
$mysqld = Join-Path $mysqlHome "bin\mysqld.exe"
$outLog = Join-Path $env:LOCALAPPDATA "Programs\MySQL\mysql-dev.out.log"
$errLog = Join-Path $env:LOCALAPPDATA "Programs\MySQL\mysql-dev.err.log"

if (Get-Process mysqld -ErrorAction SilentlyContinue) {
    Write-Host "MySQL is already running."
    exit 0
}

Start-Process -FilePath $mysqld `
    -ArgumentList @("--defaults-file=$myIni", "--console") `
    -RedirectStandardOutput $outLog `
    -RedirectStandardError $errLog `
    -WindowStyle Hidden

Write-Host "MySQL started on 127.0.0.1:3306."
