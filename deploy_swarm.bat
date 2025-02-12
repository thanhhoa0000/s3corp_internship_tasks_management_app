@echo off
set AES_KEY=Eh4Vwd06eZQu2H1foTSWwFaq5NMDELRw
set AES_IV=iik0qS6MIz3pZgLg

REM Set stack name (default: hoathstack)
set STACK_NAME=%1
if "%STACK_NAME%"=="" set STACK_NAME=hoathstack

REM Check if Swarm is initialized (redirecting errors to nul)
docker info | findstr /C:"Swarm: active" >nul
if %errorlevel% neq 0 (
    echo 🟢 Initializing Docker Swarm...
    docker swarm init
) else (
    echo ✅ Docker Swarm already initialized.
)

REM Remove existing secrets
echo 🔄 Removing old secrets (if any)...
docker secret rm aes_key aes_iv 2>nul

REM Create new secrets
echo 🔐 Creating new Docker secrets...
echo %AES_KEY% | docker secret create aes_key -
echo %AES_IV% | docker secret create aes_iv -

REM Deploy stack
echo 🚀 Deploying stack: %STACK_NAME%...
docker stack deploy -c docker-compose.yml %STACK_NAME%

echo ✅ Deployment Complete!
pause
