set WORKSPACE=..

set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-bin ^
    -d bin  ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=%WORKSPACE%/Assets/Scripts/Gen/DataTables ^
    -x outputDataDir=%WORKSPACE%/Assets/Packs/DataTables
    rem -x pathValidator.rootDir=%WORKSPACE%\Projects\Csharp_Unity_bin ^
    rem -x l10n.textProviderFile=*@%WORKSPACE%\DataTables\Datas\l10n\texts.json
