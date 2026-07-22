# Documentação de Visão Geral - GeekShopping

## 1. O Que o Sistema Faz

O **GeekShopping** é um sistema de e-commerce desenvolvido como demonstração de arquitetura de microsserviços utilizando ASP.NET Core. O sistema resolve o problema de gestão de produtos, carrinho de compras, cupons de desconto e autenticação/autorização de usuários em um ambiente distribuído.

### Objetivo Principal
Demonstrar a implementação prática de:
- Arquitetura de microsserviços com comunicação HTTP
- Autenticação e autorização com OAuth2/OpenID Connect
- Sistema de mensageria assíncrona com RabbitMQ
- API Gateway com Ocelot
- Separação de responsabilidades em serviços independentes

### Problemas que Resolve
- **Catálogo de Produtos**: Gerenciamento completo de produtos (CRUD)
- **Carrinho de Compras**: Adição, remoção e cálculo de valores
- **Sistema de Cupons**: Aplicação de descontos
- **Autenticação Centralizada**: Login único (SSO) com Duende Identity Server
- **Processamento de Pedidos**: Checkout assíncrono via mensageria

---

## 2. Arquitetura

O sistema segue uma arquitetura de **microsserviços** com os seguintes componentes principais:

### Módulos Principais e Responsabilidades

| Módulo | Responsabilidade |
|--------|------------------|
| **GeekShopping.Web** | Interface web (MVC) - Frontend que consome os microsserviços |
| **GeekShopping.ProductAPI** | Gerenciamento de produtos (catálogo) |
| **GeekShopping.CartAPI** | Gerenciamento do carrinho de compras |
| **GeekShopping.CouponAPI** | Gestão de cupons de desconto |
| **GeekShopping.IdentityServer** | Servidor de autenticação OAuth2/OpenID Connect |
| **GeekShopping.OrderAPI** | Processamento de pedidos |
| **GeekShopping.PaymentAPI** | Processamento de pagamentos |
| **GeekShopping.Email** | Serviço de envio de e-mails |
| **API Gateway (Ocelot)** | Roteamento e agregação de requisições |
| **RabbitMQ** | Message broker para comunicação assíncrona |

### Padrões Arquiteturais Utilizados
- **API Gateway Pattern**: Ponto único de entrada via Ocelot
- **Database per Service**: Cada microsserviço possui seu próprio banco de dados MySQL
- **Event-Driven Architecture**: Comunicação assíncrona via RabbitMQ
- **BFF (Backend for Frontend)**: GeekShopping.Web atua como BFF

---

## 3. Fluxo de Dados

### Fluxo de Navegação e Compra

```
1. Usuário acessa GeekShopping.Web
   ↓
2. Lista de produtos é carregada (ProductAPI)
   ↓
3. Usuário clica em "Detalhes" → Autenticação requerida
   ↓
4. Redirecionamento para IdentityServer → Login
   ↓
5. Token JWT é retornado ao navegador
   ↓
6. Usuário adiciona produto ao carrinho (CartAPI)
   ↓
7. Aplica cupom de desconto (CouponAPI valida)
   ↓
8. Realiza checkout → Mensagem enviada ao RabbitMQ
   ↓
9. OrderAPI consome mensagem e processa pedido
   ↓
10. PaymentAPI processa pagamento
    ↓
11. Email API envia confirmação
```

### Fluxo de Autenticação

```
Web → IdentityServer (OAuth2)
      ↓
   Valida credenciais
      ↓
   Retorna Access Token
      ↓
Web inclui token em todas as requisições (Bearer Token)
      ↓
APIs validam token antes de processar
```

---

## 4. Tecnologias Utilizadas

### Backend
- **.NET 7**: Framework principal para construção dos microsserviços
- **ASP.NET Core MVC**: Frontend web
- **Entity Framework Core**: ORM para acesso ao banco de dados
- **MySQL 8.0.35**: Banco de dados relacional

### Autenticação e Segurança
- **Duende Identity Server**: Implementação de OAuth2 e OpenID Connect
- **JWT Bearer Authentication**: Autenticação baseada em tokens

### Comunicação
- **RabbitMQ**: Message broker para comunicação assíncrona entre serviços
- **Ocelot**: API Gateway para roteamento e agregação
- **HTTP/REST**: Comunicação síncrona entre serviços

### Frontend
- **Bootstrap 4.5**: Framework CSS para interface responsiva
- **jQuery 3.5.1**: Biblioteca JavaScript
- **Razor**: Template engine do ASP.NET

### Ferramentas de Desenvolvimento
- **Docker**: Containerização do RabbitMQ
- **Serilog**: Logging estruturado (integrado no projeto ChatGPT)
- **Swagger/OpenAPI**: Documentação de APIs

---

## 5. Estrutura de Pastas

```
GeekShopping/
├── GeekShopping.Web/                    # Aplicação Web MVC
│   ├── Controllers/                     # Controladores MVC
│   │   ├── HomeController.cs            # Página inicial e listagem
│   │   ├── ProductController.cs         # CRUD de produtos
│   │   └── CartController.cs            # Carrinho e checkout
│   ├── Models/                          # ViewModels
│   │   ├── ProductViewModel.cs
│   │   ├── CartViewModel.cs
│   │   └── CouponViewModel.cs
│   ├── Services/                        # Clientes HTTP para APIs
│   │   ├── IProductService.cs
│   │   ├── ICartService.cs
│   │   └── ICouponService.cs
│   ├── Utils/                           # Extensões e utilitários
│   │   └── HttpClientExtensions.cs      # Serialização JSON
│   └── Views/                           # Views Razor
│
├── GeekShopping.ProductAPI/             # Microsserviço de Produtos
├── GeekShopping.CartAPI/                # Microsserviço de Carrinho
├── GeekShopping.CouponAPI/              # Microsserviço de Cupons
├── GeekShopping.OrderAPI/               # Microsserviço de Pedidos
├── GeekShopping.PaymentAPI/             # Microsserviço de Pagamentos
├── GeekShopping.Email/                  # Microsserviço de E-mail
│
├── GeekShopping.IdentityServer/         # Servidor de Identidade
│   ├── wwwroot/lib/                     # Bibliotecas front-end
│   │   ├── bootstrap/
│   │   └── jquery/
│   └── (Configurações Duende)
│
├── CHATGPT-Integration/                 # Exemplo de integração ChatGPT
│   └── Extensions/                      # Extensões para configuração
│       ├── ChatGptExtensions.cs         # Configuração OpenAI API
│       ├── SerilogExtensions.cs         # Configuração de logs
│       └── SwaggerExtensions.cs         # Configuração Swagger
│
└── template-duende/                     # Template do Identity Server
```

---

## 6. Como Executar Localmente

### Pré-requisitos

- **.NET 7 SDK** ou superior
- **MySQL Community Server 8.0.35** ou superior
- **Docker Desktop** (para RabbitMQ)
- **Visual Studio 2022** ou VS Code

### Instalação

#### 1. Clonar o Repositório
```bash
git clone https://github.com/guikuhnen/GeekShopping.git
cd GeekShopping
```

#### 2. Configurar Banco de Dados
```sql
-- Criar as bases de dados para cada serviço
CREATE DATABASE geek_shopping_product;
CREATE DATABASE geek_shopping_cart;
CREATE DATABASE geek_shopping_coupon;
CREATE DATABASE geek_shopping_order;
CREATE DATABASE geek_shopping_identity;
```

#### 3. Iniciar RabbitMQ via Docker
```bash
docker run -d --hostname my-rabbit --name some-rabbit \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management
```

Acesse o painel em: `http://localhost:15672` (usuário/senha: guest/guest)

#### 4. Configurar Strings de Conexão
Atualizar `appsettings.json` em cada projeto de API com a connection string do MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=geek_shopping_product;Uid=root;Pwd=sua_senha"
  }
}
```

#### 5. Executar Migrations
```bash
# Em cada projeto de API
dotnet ef database update
```

#### 6. Iniciar os Serviços

**Via Visual Studio:**
- Configurar **Multiple Startup Projects**
- Selecionar todos os projetos **exceto** as class libraries
- Iniciar (F5)

**Via CLI:**
```bash
# Terminal 1 - IdentityServer
cd GeekShopping.IdentityServer
dotnet run

# Terminal 2 - ProductAPI
cd GeekShopping.ProductAPI
dotnet run

# Terminal 3 - CartAPI
cd GeekShopping.CartAPI
dotnet run

# (Repetir para outros serviços)

# Terminal N - Web
cd GeekShopping.Web
dotnet run
```

### Portas Padrão
- **Web**: https://localhost:5148
- **IdentityServer**: https://localhost:5001
- **ProductAPI**: https://localhost:5101
- **CartAPI**: https://localhost:5102
- **CouponAPI**: https://localhost:5103
- **RabbitMQ Management**: http://localhost:15672

---

## 7. Integrações Externas

### Banco de Dados
- **MySQL Community Server 8.0.35**
- **Entity Framework Core** como ORM
- **Padrão**: Um banco de dados por microsserviço (Database per Service)
- **Migrations**: Code First com EF Core Migrations

### Message Broker
- **RabbitMQ 3.x**
- **Porta AMQP**: 5672
- **Porta Management UI**: 15672
- **Uso**: Comunicação assíncrona entre OrderAPI, PaymentAPI e EmailAPI

### Servidor de Identidade
- **Duende Identity Server**
- **Protocolo**: OAuth2 + OpenID Connect
- **Fluxo**: Authorization Code Flow
- **Escopos**: `geek_shopping`, `openid`, `profile`
- **Roles**: `Admin`, `Client`

### APIs de Terceiros (Opcional)
O projeto inclui exemplo de integração com **OpenAI API** (ChatGPT):
- **Biblioteca**: OpenAI_API (NuGet)
- **Configuração**: Via `appsettings.json`
- **Uso**: Demonstração em `CHATGPT-Integration/`

### API Gateway (Planejado)
- **Ocelot**: Mencionado no README mas não presente no código fornecido
- **Função**: Roteamento unificado para todos os microsserviços

---

## Observações Finais

### Próximos Passos (TODOs)
- ✅ Implementação dos microsserviços principais
- ⚠️ Criação automática de bancos de dados
- ⚠️ Configuração completa do Ocelot Gateway
- ⚠️ Testes automatizados
- ⚠️ Docker Compose para todo o ambiente

### Boas Práticas Implementadas
- Separação de responsabilidades
- Injeção de dependência
- Configuração via `appsettings.json`
- Uso de interfaces para serviços
- ViewModels separados de entidades

### Considerações de Segurança
- Tokens JWT com expiração de 10 minutos
- HTTPS obrigatório em produção
- Validação de roles para operações sensíveis (ex: deleção de produtos)

---

**Última atualização:** 22/07/2026  
**Versão da documentação:** 1.2

### Changelog da Documentação

#### v1.2 (22/07/2026)
- Limpeza do repositório: removidos arquivos temporários utilizados para testes de documentação
- Repositório mantido limpo e focado no código de produção

#### v1.1 (06/07/2026)
- Versão inicial da documentação técnica consolidada