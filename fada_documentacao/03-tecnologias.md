# Stack Tecnológico

## Tabela Consolidada

| Tecnologia | Módulo(s) | Finalidade | Versão |
|------------|-----------|------------|--------|
| **.NET** | Todos | Runtime e framework de aplicação | 7.0 |
| **ASP.NET Core MVC** | GeekShopping.Web | Framework web para frontend | 7.0 |
| **ASP.NET Core Web API** | Product, Cart, Coupon, Order, Payment, Email | APIs RESTful | 7.0 |
| **Entity Framework Core** | Todas as APIs | ORM para acesso a dados | 7.x |
| **MySQL** | Todas as APIs | Banco de dados relacional | 8.0.35 |
| **Pomelo.EntityFrameworkCore.MySql** | Todas as APIs | Provider MySQL para EF Core | - |
| **Duende IdentityServer** | IdentityServer | Servidor de autenticação OAuth2/OIDC | 6.x |
| **RabbitMQ** | Cart, Order, Payment, Email | Message broker para comunicação assíncrona | 3.x (Docker) |
| **Ocelot** | API Gateway | Gateway de roteamento de APIs | - |
| **Bootstrap** | Web, IdentityServer | Framework CSS para UI | 4.5.3 |
| **jQuery** | Web, IdentityServer | Biblioteca JavaScript | 3.5.1 |
| **Serilog** | CHATGPT-Integration | Logging estruturado | - |
| **OpenAI API** | CHATGPT-Integration | Integração com ChatGPT | - |
| **Swagger/OpenAPI** | Todas as APIs | Documentação de APIs | - |

## Detalhamento por Módulo

### GeekShopping.Web
```json
{
  "Framework": "ASP.NET Core MVC 7.0",
  "View Engine": "Razor",
  "Autenticação": "OpenID Connect Client",
  "HTTP Client": "HttpClient (injetado via DI)",
  "Frontend": ["Bootstrap 4.5.3", "jQuery 3.5.1"]
}
```

### GeekShopping.IdentityServer
```json
{
  "Framework": "ASP.NET Core 7.0",
  "Identity Provider": "Duende IdentityServer 6.x",
  "Storage": "In-Memory (configuração de exemplo)",
  "UI": ["Bootstrap", "jQuery"]
}
```

### GeekShopping.ProductAPI (e demais APIs)
```json
{
  "Framework": "ASP.NET Core Web API 7.0",
  "ORM": "Entity Framework Core 7.x",
  "Database": "MySQL 8.0.35",
  "Provider": "Pomelo.EntityFrameworkCore.MySql",
  "Autenticação": "JWT Bearer Token",
  "Documentação": "Swagger/OpenAPI"
}
```

### GeekShopping.MessageBus
```json
{
  "Tipo": ".NET Standard Class Library",
  "Client": "RabbitMQ.Client",
  "Protocolo": "AMQP 0-9-1"
}
```

### Infraestrutura
```json
{
  "Containerização": "Docker (RabbitMQ)",
  "Message Broker": "RabbitMQ 3.x",
  "Portas RabbitMQ": {
    "AMQP": 5672,
    "Management UI": 15672
  }
}
```

## Dependências NuGet Principais

### Comuns em Todas as APIs
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.x" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="7.x" />
<PackageReference Include="Swashbuckle.AspNetCore" />
```

### GeekShopping.Web
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.OpenIdConnect" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" />
```

### GeekShopping.MessageBus
```xml
<PackageReference Include="RabbitMQ.Client" />
```

## Padrões de Configuração

### Connection Strings (appsettings.json)
```json
{
  "ConnectionStrings": {
    "MySQLConnectionString": "Server=localhost;Database={DbName};Uid=root;Pwd={password}"
  }
}
```

### Service URLs (GeekShopping.Web)
```json
{
  "ServiceUrls": {
    "ProductAPI": "https://localhost:{porta}",
    "CartAPI": "https://localhost:{porta}",
    "CouponAPI": "https://localhost:{porta}",
    "IdentityServer": "https://localhost:{porta}"
  }
}
```

### RabbitMQ (MessageBus)
```json
{
  "RabbitMQHost": "localhost",
  "RabbitMQPort": 5672,
  "RabbitMQUsername": "guest",
  "RabbitMQPassword": "guest"
}
```

---