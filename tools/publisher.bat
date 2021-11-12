
nuget.exe sources Add -Name "Dime.Scheduler" -Source "https://dimesoftware.pkgs.visualstudio.com/_packaging/Dime.Scheduler/nuget/v3/index.json"
nuget.exe push -Source "Dime.Scheduler" -ApiKey VSTS ..\src\core\bin\Release\Dime.Utilities.Expressions.2.1.0.nupkg
nuget.exe push -Source "Dime.Scheduler" -ApiKey VSTS ..\src\core\bin\Release\Dime.Utilities.Expressions.2.1.0.symbols.nupkg