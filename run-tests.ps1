# Build e execução dos testes
docker build -t calculo-imposto-tests -f Dockerfile.test .

# Limpeza
docker rmi calculo-imposto-tests