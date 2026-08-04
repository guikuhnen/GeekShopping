# GeekShopping - Documentação Técnica

Sistema de e-commerce construído com arquitetura de microsserviços utilizando ASP.NET Core, .NET 7 e C#.

## 📋 Índice da Documentação

| Documento | Descrição |
|-----------|-----------|
| [01-visao-geral.md](./01-visao-geral.md) | Arquitetura geral do sistema, fluxo de dados e componentes principais |
| [02-modulos.md](./02-modulos.md) | Detalhamento de cada módulo/microsserviço e suas responsabilidades |
| [03-tecnologias.md](./03-tecnologias.md) | Stack tecnológico completo por módulo |
| [04-regras-de-negocio.md](./04-regras-de-negocio.md) | Regras de negócio, fluxos condicionais e validações |
| [05-guia-desenvolvimento-local.md](./05-guia-desenvolvimento-local.md) | Configuração do ambiente de desenvolvimento local |

## 🚀 Início Rápido

### Pré-requisitos
```bash
# .NET 7 SDK
dotnet --version

# MySQL 8.0.35
mysql --version

# Docker (para RabbitMQ)
docker --version
```

### Subir o RabbitMQ
```bash
docker run -d --hostname my-rabbit --name some-rabbit \
  -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### Executar a Aplicação
```bash
# 1. Criar as bases de dados de cada serviço (manual)
# 2. Inicializar todos os serviços (9 serviços)
# 3. Acessar via Visual Studio com Multiple Startup Projects
```

### Endpoints Principais
- **Web UI**: https://localhost:5148
- **Identity Server**: https://localhost:{porta_identity}
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)

### Validação
```bash
# Verificar RabbitMQ
curl http://localhost:15672

# Verificar serviços rodando
dotnet list package
```

## 📦 Estrutura do Repositório

- **GeekShopping.Web** - Frontend MVC
- **GeekShopping.IdentityServer** - Servidor de autenticação/autorização
- **GeekShopping.ProductAPI** - API de produtos
- **GeekShopping.CartAPI** - API de carrinho de compras
- **GeekShopping.CouponAPI** - API de cupons de desconto
- **GeekShopping.OrderAPI** - API de pedidos
- **GeekShopping.PaymentAPI** - API de pagamento
- **GeekShopping.Email** - Serviço de e-mail
- **GeekShopping.MessageBus** - Library de mensageria

## 🔑 Credenciais Padrão

**Identity Server:**
- Client ID: `geek_shopping`
- Client Secret: `my_super_secret`
- Scopes: `geek_shopping`

---