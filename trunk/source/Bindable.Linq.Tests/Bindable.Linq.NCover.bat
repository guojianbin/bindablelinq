@echo off
title Running Tests
echo ===============================================================
color 0f
..\..\tools\ncover\NCover.Console.exe ..\..\tools\nunit\bin\nunit-console.exe ..\..\source\Bindable.Linq.Tests\Bindable.Linq.nunit //reg //a "Bindable.Linq" //x ..\..\source\Bindable.Linq.Tests\bin\Bindable.Linq.NCover-Last.xml
color 0a
echo Launching NCover Browser
title Coverage complete
start ..\..\tools\ncoverbrowser\ncoverbrowser.exe ..\..\source\Bindable.Linq.Tests\bin\Bindable.Linq.ncover-last.xml
exit 0