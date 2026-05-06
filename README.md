# LEVE Gestão

Sistema web de gestão de usuários e tarefas desenvolvido com ASP.NET Core 8 MVC.

## Tecnologias

- **Backend:** C# / ASP.NET Core 8 MVC
- **Frontend:** Razor + UIkit 3 + jQuery + TypeScript
- **Banco de dados:** SQL Server
- **ORM:** Entity Framework Core 8 (com Migrations)
- **Autenticação:** Cookie Authentication
- **E-mail:** MailKit (SMTP)
- **Hash de senha:** BCrypt.Net

---

## Pré-requisitos

| Ferramenta | Versão mínima | Download |
|---|---|---|
| .NET SDK | 8.0 | https://dot.net/download |
| SQL Server | 2019 ou LocalDB | https://www.microsoft.com/sql-server |
| Node.js (para `@types/jquery`) | 18.x | https://nodejs.org |

Verifique a versão instalada:

```bash
dotnet --version   # deve retornar 8.x.x
node --version     # deve retornar v18.x ou superior
```

---

## Configuração

### 1. Clone o repositório

```bash
git clone <url-do-repositorio>
cd projeto-case
```

### 2. Restaure os pacotes

```bash
# Pacotes .NET
dotnet restore LeveGestao.sln

# Tipagens jQuery (TypeScript)
cd src/LeveGestao.Web
npm install
cd ../..
```

### 3. Ajuste a connection string

Abra `src/LeveGestao.Web/appsettings.json` e altere conforme seu ambiente:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LeveGestao;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Outras opções:

```
-- SQL Server Express
Server=localhost\SQLEXPRESS;Database=LeveGestao;Trusted_Connection=True;TrustServerCertificate=True;

-- SQL Server com usuário e senha
Server=localhost;Database=LeveGestao;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;
```

### 4. (Opcional) Configure o envio de e-mails

Em `appsettings.json`, preencha a seção `Email`:

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "Username": "seu-email@gmail.com",
  "Password": "sua-senha-de-app",
  "FromName": "LEVE Investimentos",
  "FromEmail": "seu-email@gmail.com"
}
```

> Para Gmail, gere uma **Senha de app** em https://myaccount.google.com/apppasswords (requer verificação em duas etapas ativa).

---

## Banco de dados

O projeto usa **EF Core Migrations**. Na primeira execução, a aplicação aplica as migrations automaticamente (cria o banco e o schema) e insere o usuário gestor padrão.

Se preferir aplicar as migrations manualmente antes de rodar o app:

```bash
dotnet tool install --global dotnet-ef        # apenas na primeira vez
dotnet ef database update --project src/LeveGestao.Infrastructure --startup-project src/LeveGestao.Web
```

---

## Executando a aplicação

### Via linha de comando

```bash
dotnet run --project src/LeveGestao.Web/LeveGestao.Web.csproj
```

Acesse: `http://localhost:5000`

### Via Visual Studio 2022

1. Abra `LeveGestao.sln` na raiz
2. Clique com botão direito em `LeveGestao.Web` → **Definir como projeto de inicialização**
3. Pressione **F5**

### Via VS Code

```bash
cd src/LeveGestao.Web
dotnet run
```

---

## Primeiro acesso

Na primeira execução o banco é criado automaticamente com o usuário padrão:

| Campo | Valor |
|---|---|
| E-mail | `ti@leveinvestimentos.com.br` |
| Senha | `teste123` |
| Perfil | Gestor |

---

## Estrutura do projeto

```
projeto-case/
└── src/
    ├── LeveGestao.Domain/          # Entidades, interfaces e enums
    ├── LeveGestao.Application/     # DTOs, interfaces de serviços e regras de negócio
    ├── LeveGestao.Infrastructure/  # EF Core, Migrations, repositórios, e-mail
    └── LeveGestao.Web/             # Controllers, Views (Razor + UIkit), TypeScript, Program.cs
```

### Arquitetura em camadas

```
Web → Application → Domain
 ↓                    ↑
Infrastructure ────────┘
```

- **Domain:** núcleo da aplicação, sem dependências externas
- **Application:** casos de uso e regras de negócio
- **Infrastructure:** implementações concretas (banco, e-mail)
- **Web:** camada de apresentação (MVC + Razor + TypeScript)

---

## Funcionalidades

### Autenticação
- Login por e-mail e senha
- Sessão via cookie HttpOnly (8 horas)
- Logout

### Gestão de usuários *(somente gestores)*
- Listar, cadastrar, editar e excluir usuários
- Upload de foto de perfil (JPG, PNG, GIF, WEBP, até 5MB)
- Endereço com preenchimento automático via **ViaCEP**
- Campos: nome, data de nascimento, telefones, e-mail, endereço e foto

### Agendamento de tarefas
- Gestores criam tarefas para colaboradores com mensagem e data limite
- Colaboradores acompanham apenas suas tarefas
- Fluxo de status: **Pendente → Em andamento → Concluída**
- Notificação por e-mail ao colaborador quando uma tarefa é atribuída
- Notificação por e-mail ao gestor quando a tarefa é concluída
- Indicação visual de tarefas atrasadas

---

## TypeScript

O JavaScript do projeto é escrito em TypeScript (`wwwroot/js/site.ts`) e **compila automaticamente** em build via `Microsoft.TypeScript.MSBuild`. Nenhum comando manual é necessário, basta rodar `dotnet build` ou `dotnet run`.

Configuração em `tsconfig.json`:
- `target: ES2015`
- `strict: true`
- `types: ["jquery"]`

---

## API REST

Algumas operações usam endpoints REST consumidos via jQuery AJAX:

| Verbo | Rota | Ação |
|---|---|---|
| `PUT` | `/api/tarefas/{id}/iniciar` | Inicia o andamento de uma tarefa |
| `PUT` | `/api/tarefas/{id}/concluir` | Marca uma tarefa como concluída |
