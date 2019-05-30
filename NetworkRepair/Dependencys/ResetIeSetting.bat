@echo off
title ÖØÖÃIEä¯ÀÀÆ÷
echo Set objAP = CreateObject("wscript.shell")>>ResetIE.vbs
echo objAP.Run "rundll32.exe inetcpl.cpl ResetIEtoDefaults">>ResetIE.vbs
echo wscript.sleep 800>>ResetIE.vbs
echo objAP.AppActivate "Reset Internet Explorer Settings">>ResetIE.vbs
echo objAP.SendKeys "{TAB}">>ResetIE.vbs
echo objAP.SendKeys "{ }">>ResetIE.vbs
echo wscript.sleep 800>>ResetIE.vbs
echo objAP.SendKeys "{TAB}">>ResetIE.vbs
echo objAP.SendKeys "{TAB}">>ResetIE.vbs
echo objAP.SendKeys "{ }">>ResetIE.vbs
echo wscript.sleep 3000>>ResetIE.vbs
echo objAP.SendKeys "{ENTER}">>ResetIE.vbs
cscript ResetIE.vbs
echo/

echo ÒÑÖØÖÃIEä¯ÀÀÆ÷!
del /q ResetIE.vbs

pause