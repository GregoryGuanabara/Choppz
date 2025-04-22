# 🍺 Choppz - Serviço de Cálculo de Imposto  

[![.NET](https://img.shields.io/badge/.NET-9-purple)](https://dotnet.microsoft.com)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)

Documentação do Serviço de Cálculo de Imposto (Choppz)

## 📌 Visão Geral

O Serviço de Cálculo de Imposto é um microsserviço desenvolvido em .NET 9, seguindo os princípios da Clean Architecture e utilizando tecnologias modernas para garantir escalabilidade, manutenibilidade e performance.

## 📝 Sobre
Este projeto foi criado com o propósito de **estudo e aprimoramento de habilidades**, proporcionando um ambiente prático para explorar conceitos avançados de arquitetura de software, padrões de desenvolvimento e tecnologias modernas.  

Por meio da implementação da **Clean Architecture**, combinada com **CQRS, Event-Driven Architecture, UnitOfWork e Outbox Pattern**, buscamos reforçar boas práticas e aprofundar o conhecimento em tópicos essenciais para projetos escaláveis e bem estruturados.  

Além disso, a utilização de ferramentas como **.NET 9, MediatR, FluentValidation, Hangfire, Serilog, Pub/Sub e Feature Flags** permite a experimentação de abordagens que são amplamente adotadas em sistemas robustos e de alto desempenho.  

O principal objetivo é **aprender na prática**, testando integrações, otimizando processos e explorando técnicas que podem ser aplicadas em projetos reais. Esse espaço serve não apenas para estudo individual, mas também para colaboração e troca de experiências entre desenvolvedores.  

Se você deseja contribuir, revisar conceitos ou apenas acompanhar o desenvolvimento, este projeto é um excelente ponto de partida para aprofundar seus conhecimentos! 🚀  

## 🔧 Tecnologias e Padrões Utilizados

| Categoria           | Tecnologia/Padrão         | Descrição                                                                 |
|---------------------|---------------------------|---------------------------------------------------------------------------|
| **Arquitetura**     | Clean Architecture        | Separação clara entre Domain, Application, Infrastructure e Presentation  |
| **Padrões**         | CQRS, MediatR             | Separação entre comandos e consultas                                      |
| **Banco de Dados**  | Entity Framework Core     | ORM para acesso a dados                                                   |
| **Cache**           | Cache em Memória          | Melhoria de performance com `IMemoryCache`                                |
| **Validação**       | FluentValidation          | Validação de requisições de forma fluente                                 |
| **API Docs**        | Scalar                    | Documentação interativa da API                                            |
| **Background Jobs** | Hangfire                  | Processamento de jobs em segundo plano                                    |
| **Resiliência**     | Padrão Outbox             | Garantia de entrega de mensagens em sistemas distribuídos                 |
| **Mensageria**      | Pub/Sub                   | Comunicação assíncrona entre serviços                                     |
| **Transações**      | UnitOfWork                | Gerenciamento de transações no EF Core                                    |
| **Logs**            | Serilog                   | Logs estruturados (Elasticsearch/Seq/Console)                             |
| **Feature Toggle**  | FeatureFlag               | Liberação progressiva de funcionalidades                                  |
| **Repositório**     | Padrão Repository         | Abstraction do acesso a dados                                             |

## 🚀 Como Executar o Projeto

1️⃣ Opção 1: Rodando com Docker (Recomendado)
Na pasta raiz do projeto, execute:

```bash
docker-compose up --build
```
🔹 O que acontece?

  A imagem do projeto é construída
  
  O container é iniciado nas portas 8080 (HTTP) e 8081 (HTTPS)
  
  A API estará disponível em:
  
  Scalar API Docs: http://localhost:8080/ OU https://localhost:8081/

2️⃣ Opção 2: Rodando Localmente (Sem Docker):

  Pré-requisitos
  ✔ .NET 9 SDK instalado
  ✔ Banco de dados em memória (não requer instalação adicional)

  Passo a Passo

Restaurar pacotes NuGet:
```bash
dotnet restore

```
Executar a aplicação:
```bash
dotnet run --project src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Api/
```
Acessar a API através do endereço fornecido e sera exibida a documentação do Scalar

## 🔍 Estrutura do Projeto:

    📦 Choppz.Servicos.CalculoImposto
    ├── 📂 src
    │   ├── 📂 Servicos.CalculoImposto.Api          # Camada de Apresentação (API)
    │   ├── 📂 Servicos.CalculoImposto.Application  # Casos de Uso, CQRS, MediatR
    │   ├── 📂 Servicos.CalculoImposto.Core         # Domínio (Entities, Value Objects)
    │   └── 📂 Servicos.CalculoImposto.Infra        # EF Core, Repositórios, Hangfire
    ├── 📂 tests                                    # Testes Unitários/Integração
    ├── 📜 docker-compose.yml                       # Configuração Docker
    └── 📜 Dockerfile                               # Definição da Imagem

## 🧪 Testes Automatizados - Unidade e Integração

Este projeto utiliza XUnit para estruturação de testes automatizados, garantindo que a lógica de negócios e integrações sejam validadas corretamente. Asserções são realizadas com FluentAssertions, enquanto Bogus auxilia na geração de dados fictícios e NSubstitute permite mockar dependências de forma eficiente

O projeto segue uma abordagem de testes separados em unidade e integração, cada um com um objetivo específico:
✔ Testes de Unidade: Validam componentes isolados da aplicação, sem dependências externas.
✔ Testes de Integração: Verificam a comunicação entre componentes, garantindo que o sistema funcione corretamente em conjunto.

 Tecnologias Utilizadas
| Tecnologia       | Propósito                                 | 
| XUnit            | Framework de testes unitários             | 
| FluentAssertions | Asserções fluentes e legíveis             | 
| Bogus            | Geração de dados fictícios para testes    | 
| NSubstitute      | Mock de dependências para testes isolados | 

🚀 Como Executar os Testes

Antes de rodar os testes, certifique-se de que todos os pacotes necessários estão instalados:
```bash
dotnet restore
```
Depois navegue até a pasta do projeto que deseja executar
e execute os testes unitários e de integração com o seguinte comando:
```bash
dotnet test --logger "trx;LogFileName=test-results.trx"
```
Isso gerará um relatório de testes no formato .trx, que pode ser analisado posteriormente.

Se preferir rodar os testes em containers:

Na raiz do projeto utilize o comando:

```bash
docker-compose -f docker-compose.test.yml up --abort-on-container-exit
```
