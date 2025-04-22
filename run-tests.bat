@echo off
echo Build e execução dos testes...
docker build -t calculo-imposto-tests -f Dockerfile.test .

if %errorlevel% neq 0 (
    echo Ocorreu um erro na construção
    exit /b %errorlevel%
)

echo Limpando...
docker rmi calculo-imposto-tests

echo Concluído!
pause