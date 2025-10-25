#Projeto Encurtador de URL Full-Stack

Este é um projeto de um encurtador de URLs construído com uma arquitetura robusta e focada em boas práticas de desenvolvimento, segurança e escalabilidade.

#Tecnologias Utilizadas:

Backend: C# / .NET 8 (ASP.NET Core Web API)

Frontend: Next.js 14+ (React com TypeScript e App Router)

Database: PostgreSQL

Estilização: Tailwind CSS

#Arquitetura e Padrões:
O backend deste projeto foi estruturado seguindo os princípios da Clean Architecture, separando as responsabilidades em camadas distintas para facilitar a manutenção, testes e evolução do código.

#Estrutura do Backend:

A solução .sln é dividida em três projetos principais:

Encurtador.Core (Camada de Domínio):
Contém as entidades de negócio (ex: ShortUrl.cs).
Define os "contratos" (Interfaces, ex: IUrlRepository.cs) que a camada de infraestrutura deve implementar.
Não possui dependências externas (como banco de dados ou frameworks de API).

Encurtador.Infrastructure (Camada de Infraestrutura):
Implementa as interfaces definidas no Core (ex: UrlRepository.cs).
Contém toda a lógica de acesso a dados, incluindo o ApiDbContext do Entity Framework Core.
Gerencia a comunicação com o PostgreSQL e a execução das migrações.

Encurtador.WebApi (Camada de Apresentação/API):
É o projeto de "startup" que expõe os endpoints HTTP.
Contém os Controladores (UrlController.cs), DTOs (ShortenRequestDto.cs) e a lógica de serviços da aplicação (UrlShorteningService.cs).
Orquestra o fluxo de dados, recebendo requisições, validando-as e chamando as camadas inferiores.

#Padrões de Design Aplicados:
Repository Pattern: Abstrai a lógica de acesso aos dados. O UrlController não sabe como os dados são salvos; ele apenas usa a interface IUrlRepository, cuja implementação (UrlRepository) pode ser trocada (ex: de PostgreSQL para SQL Server) sem afetar o resto da aplicação.

Dependency Injection (DI): Usamos a injeção de dependência nativa do .NET para "conectar" todas as camadas. O Program.cs é responsável por registrar os serviços e repositórios, que são automaticamente injetados nos construtores das classes que precisam deles (ex: IUrlRepository é injetado no UrlController).

Features de Segurança e Boas Práticas
Este projeto vai além do básico, implementando medidas de segurança essenciais em aplicações reais:

Gerenciador de Segredos (User Secrets): A string de conexão do PostgreSQL não está no appsettings.json (que vai para o GitHub). Ela é armazenada localmente e de forma segura usando o dotnet user-secrets, evitando o vazamento de credenciais sensíveis.

Rate Limiting (Limitação de Taxa): A API está configurada para aceitar um número limitado de requisições por minuto (ex: 10/minuto). Isso previne ataques de força bruta ou abuso dos endpoints, garantindo a estabilidade do serviço.

Validação Robusta (FluentValidation): Em vez de usar if (string.IsNullOrEmpty(url)) no controlador, usamos a biblioteca FluentValidation para criar regras complexas de validação (ex: Url não pode ser nula E deve ser uma URL válida) de forma limpa e separada da lógica de negócio.

Variáveis de Ambiente no Frontend (.env.local): O frontend Next.js não "chumba" a URL da API no código. Ele a lê de um arquivo .env.local (ignorado pelo Git), permitindo que diferentes desenvolvedores usem portas diferentes e que o ambiente de produção use uma URL diferente.

CORS (Cross-Origin Resource Sharing): A API é configurada explicitamente para aceitar requisições apenas da origem do frontend (http://localhost:3000), bloqueando tentativas de acesso de outros sites.

#Como Rodar o Projeto Localmente:
Siga estes passos para configurar e executar a aplicação completa na sua máquina.

Pré-requisitos:
.NET 8 SDK, Node.js (LTS), PostgreSQL (e o pgAdmin).

1. Configuração do Banco de Dados:
Abra o pgAdmin e crie um novo banco de dados vazio. (Ex: encurtador_prod_db).

Anote seu usuário, senha e a porta do PostgreSQL(geralmente 5432).

2. Configuração do Backend (.NET):
Abra um terminal e navegue até a pasta Encurtador.WebApi:

cd EncurtadorProjeto/Encurtador.WebApi

Configure o User Secrets (Substitua pelos seus dados do PostgreSQL):
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=encurtador_prod_db;Username=SEU_USUARIO;Password=SUA_SENHA;"

Execute as Migrações para criar as tabelas no banco de dados:
dotnet ef database update

Inicie o Backend:
dotnet run

O terminal mostrará a URL em que a API está rodando (ex: http://localhost:5123). Anote essa URL.

3. Configuração do Frontend (Next.js)
Abra um novo terminal e navegue até a pasta EncurtadorWeb:
cd EncurtadorProjeto/EncurtadorWeb


Instale as dependências:
npm install

#Crie o arquivo de ambiente:
Crie um arquivo chamado .env.local na raiz da pasta EncurtadorWeb.

Adicione a URL da sua API (que você anotou no passo anterior):
Em EncurtadorWeb/.env.local:

NEXT_PUBLIC_API_URL=http://localhost:5123

#Inicie o Frontend:

npm run dev

4. Acesse a Aplicação

Seu backend está rodando em http://localhost:5123 e seu frontend em http://localhost:3000.

Abra seu navegador e acesse: http://localhost:3000