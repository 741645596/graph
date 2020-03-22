@echo off


@set Define="UNITY_5_3_OR_NEWER;UNITY_2017_1_OR_NEWER;Release;UNITY_2019_2_OR_NEWER"

cd %~dp0

@set DllPath=%cd%\UnityDll

@set GameSrcPath=%cd%\Assets\Scripts\Graphics

@set Output=%cd%\OutDll\Graphics.Dll

@set XmlPath=%cd%\OutDll\Graphics.xml

 rem set path=%GameSrcPath%;

rem echo %path%

rem pause
rem pause

call %cd%\Compile.bat

pause






