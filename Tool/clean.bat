@echo off
title Xoa thu muc bin va obj
echo ===== CHUONG TRINH XOA THU MUC BIN VA OBJ =====
echo.

:: Kiem tra quyen admin
NET SESSION >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Dang yeu cau quyen administrator...
    powershell -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

echo Dang chay voi quyen administrator...
echo.

set "PROJECT_PATH1=C:\Users\TRY\Documents\GitHub\khaothi_2024_net\khaothi_2024_net_server\KhaoThi_2024_net_client"
set "PROJECT_PATH2=C:\Users\TRY\Documents\GitHub\khaothi_2024_net\khaothi_2024_net_server\khaothi_2024_net_server"

echo Dang xoa thu muc bin va obj trong %PROJECT_PATH1%
if exist "%PROJECT_PATH1%" (
    for /d /r "%PROJECT_PATH1%" %%d in (bin obj) do (
        if exist "%%d" (
            echo Dang xoa: %%d
            rd /s /q "%%d"
        )
    )
) else (
    echo Khong tim thay duong dan: %PROJECT_PATH1%
)

echo.
echo Dang xoa thu muc bin va obj trong %PROJECT_PATH2%
if exist "%PROJECT_PATH2%" (
    for /d /r "%PROJECT_PATH2%" %%d in (bin obj) do (
        if exist "%%d" (
            echo Dang xoa: %%d
            rd /s /q "%%d"
        )
    )
) else (
    echo Khong tim thay duong dan: %PROJECT_PATH2%
)

echo.
echo Hoan thanh xoa bin va obj!
echo.
pause