# Visão Geral do Sistema

## Objetivo

GeekShopping é uma plataforma de e-commerce desenvolvida com arquitetura de microsserviços para demonstrar conceitos avançados de desenvolvimento distribuído, incluindo autenticação OAuth2/OpenID, comunicação assíncrona via mensageria e orquestração de serviços.

## Arquitetura de Alto Nível

```
┌─────────────────────────────────────────────────────────────────┐
│                         CLIENTE (Browser)                       │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
                  ┌──────────────────────┐
                  │  GeekShopping.Web    │
                  │  (Frontend MVC)      │
                  └──────────┬───────────┘
                             │
        ┌────────────────────┼────────────────────┐
        │                    │                    │
        ▼                    ▼                    ▼
┌──────────────┐   ┌──────────────────┐   ┌────────────┐
│ Identity     │   │   API Gateway    │   │  Demais    │
│ Server       │   │   (Ocelot)       │   │  APIs      │
│ (Duende)     │   └─────────┬────────┘   └────────────┘
└──────────────┘             │
                             ▼
        ┌────────────────────┼────────────────────────────┐
        │                    │                            │
        ▼                    ▼                            ▼
┌──────────────┐   ┌──────────────────┐       ┌─────────────────┐
│ Product API  │   │   Cart API       │       │  Coupon API     │
└──────┬───────┘   └────────┬─────────┘       └─────────────────┘
       │                    │
       ▼                    ▼
┌──────────────┐   ┌──────────────────┐
│  MySQL DB    │   │   MySQL DB       │
│  (Product)   │   │   (Cart)         │
└──────────────┘   └──────────────────┘

                        ┌──────────────────┐
                        │   RabbitMQ       │
                        │   (Message Bus)  │
                        └────────┬─────────┘
                                 │
                ┌────────────────┼─────────────────┐
                │                │                 │
                ▼                ▼                 ▼
        ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
        │  Order API   │  │ Payment API  │  │  Email API   │
        └──────────────┘  └──────────────┘  └──────────────┘
```

## Fluxo de Dados Ponta a Ponta

### 1. Autenticação do Usuário
```
Browser → Web → Identity Server → Token JWT → Web (Cookie)
```

### 2. Listagem de Produtos
```
Browser → Web → Product API → MySQL → Response → Web → Browser
```

### 3. Adicionar ao Carrinho
```
Browser → Web (Token) → Cart API → MySQL → Response → Web → Browser
```

### 4. Aplicar Cupom
```
Browser → Web → Cart API → Coupon API → Validação → Cart API → MySQL
```

### 5. Checkout (Fluxo Assíncrono)
```
Browser → Web → Cart API → RabbitMQ (Checkout Queue)
                              │
                              ├→ Order API → MySQL
                              │
                              └→ Payment API → RabbitMQ (Payment Result Queue)
                                                  │
                                                  └→ Email API → Envio
```

## Papel de Cada Macrocomponente

### GeekShopping.Web (Frontend)
- **Responsabilidade**: Interface do usuário, renderização de views, coordenação de chamadas aos serviços
- **Tecnologia**: ASP.NET Core MVC
- **Autenticação**: Cliente OpenID Connect

### GeekShopping.IdentityServer
- **Responsabilidade**: Autenticação e autorização centralizada
- **Tecnologia**: Duende IdentityServer
- **Protocolos**: OAuth2, OpenID Connect

### APIs de Domínio
- **Product API**: CRUD de produtos
- **Cart API**: Gerenciamento de carrinho de compras
- **Coupon API**: Validação e aplicação de cupons
- **Order API**: Criação e gerenciamento de pedidos
- **Payment API**: Processamento de pagamentos

### GeekShopping.MessageBus (Library)
- **Responsabilidade**: Abstração de comunicação assíncrona via RabbitMQ
- **Padrão**: Publisher/Subscriber

### Infraestrutura
- **MySQL**: Persistência de dados (banco por microsserviço)
- **RabbitMQ**: Mensageria assíncrona entre serviços
- **Ocelot**: API Gateway para roteamento e agregação

## Padrões Arquiteturais Aplicados

| Padrão | Aplicação |
|--------|-----------|
| **Database per Service** | Cada API possui seu próprio banco MySQL |
| **API Gateway** | Ocelot centraliza roteamento |
| **Event-Driven Architecture** | RabbitMQ para comunicação assíncrona (checkout, pagamento) |
| **Token-based Authentication** | JWT via OAuth2/OpenID Connect |
| **Service Discovery** | Configuração estática via appsettings |

---