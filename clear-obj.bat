@echo off

rem for %%d in (bin obj) do for /f %%f in ('dir /s /b /d %%d') do rd /s /q %%f

for /f %%f in ('dir /s /b obj') do (del /f /s /q %%f > nul)
for /f %%f in ('dir /s /b bin') do (del /f /s /q %%f > nul)

for /f %%f in ('dir /s /b obj') do (rmdir /s /q %%f > nul)
for /f %%f in ('dir /s /b bin') do (rmdir /s /q %%f > nul)