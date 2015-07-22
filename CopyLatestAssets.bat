@ECHO OFF

REM File: CopyLatestAssets.bat
REM Created: 22/07/2015
REM Author: Patrick Ferguson
REM Copyright: 2015 Storms Project All Rights Reserved.
REM Purpose: Update the Assets folder in this directory with the latest one from the Project Storms Google Drive.

REM Store copy paths
SET orig_assets_path="C:\Users\%USERNAME%\Google Drive\Storms\Assets\"
SET dest_path="%~dp0"

REM For description of %~dp0 see https://stackoverflow.com/questions/17063947/get-current-batchfile-directory
ECHO --------------------------------
ECHO Copying "\Assets\" from: %orig_assets_path%
ECHO Copying to (replace with newer): %dest_path%
ECHO --------------------------------

REM XCOPY options are: /S Copy dir. including empty, /C Continue on error, /Q Quiet, /K Copy file attributes (e.g. read-only), /Y Override all.
REM ROBOCOPY options are: /E Copy dir. including empty, /DCOPY:DAT Copy directory timestamps too, /XO Exclude older files.

REM XCOPY %orig_assets_path% %dest_path% /S /C /Q /K /Y
	
REM Check if the Google Drive directory exists
IF NOT EXIST %orig_assets_path% (
    ECHO Google Drive folder couldn't be found at: %orig_assets_path%! && ECHO Download the app from https://www.google.com.au/drive/download/ before running this script again.
) ELSE (
    ROBOCOPY %orig_assets_path% %dest_path% /E /DCOPY:DAT /XO
)

ECHO Done!

REM "Press any key to continue."
PAUSE