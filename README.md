🔐Fiap Quod API
API RESTful desenvolvida como parte do desafio da Quod. Este serviço é responsável por expor endpoints relacionados a verificação de biometrias e notificações de fraudes.

O projeto está containerizado com Docker.

🚀 Tecnologias Utilizadas
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download)
- [Swagger/OpenAPI](https://swagger.io/specification/)
- AutoMapper
- MongoDB
- OpenCvSharp4
- SixLabors.ImageSharp

🧪 Executando o Projeto com Docker

Para executar a API localmente, siga os seguintes passos:

1.  **Pré-requisitos:**
    * Certifique-se de ter o **.NET SDK** (Software Development Kit) instalado na sua máquina. Você pode verificar a instalação executando o comando `dotnet --version` no seu terminal ou prompt de comando. Caso não tenha, você pode baixá-lo e instalá-lo no site oficial da Microsoft: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).

2.  **Obter o Código:**
    * Clone o repositório do projeto para sua máquina local, caso ainda não o tenha feito. Utilize o seguinte comando no seu terminal:
        ```bash
        git clone https://github.com/juujb/ChallengeQuod.git
        cd ChallengeQuod
        ```

3.  **Navegar até o Diretório do Projeto:**
    * No seu terminal, navegue até o diretório raiz do projeto da API (onde o arquivo `.csproj` está localizado).

4.  **Configuração do MongoDB:**
    * Este projeto utiliza o MongoDB para armazenar os dados. Você precisará configurar a string de conexão no arquivo de configuração da aplicação.
    * Localize a seção `"MongoDb"` e preencha o valor de `"ConnectionString"` com a sua string de conexão do MongoDB:
        ```json
        "MongoDb": {
          "ConnectionString": "SUA_STRING_DE_CONEXÃO_MONGODB",
          "DatabaseName": "QuodApi"
        }
        ```
    * **Recomendação:** Para facilitar o desenvolvimento local e até mesmo para ambientes de produção com requisitos menores, você pode utilizar o **MongoDB Atlas**, que oferece um plano gratuito com um cluster compartilhado. Para obter uma string de conexão, siga os passos no site do MongoDB Atlas: [https://www.mongodb.com/atlas/database](https://www.mongodb.com/atlas/database).

5.  **Executar a API:**
    * Execute o seguinte comando no seu terminal:
        ```bash
        dotnet run
        ```
    * Este comando irá compilar e executar a sua API .NET. Você deverá ver algumas mensagens no terminal indicando que o servidor web foi iniciado e está ouvindo em alguma porta (por exemplo, `http://localhost:5000` ou `https://localhost:5001`).

6.  **Acessar a API:**
    * Com a API em execução, você pode interagir com os endpoints utilizando um cliente HTTP (como Postman, Insomnia, ou até mesmo o seu navegador para requisições GET simples).
    A documentação Swagger estará disponível automaticamente após iniciar a api: [http://localhost:5000/swagger](http://localhost:5000/swagger)
