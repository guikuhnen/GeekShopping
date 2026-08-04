# Detalhamento dos Módulos

## 1. GeekShopping.Web

**Linguagem**: C# / .NET 7  
**Tipo**: Aplicação MVC (Frontend)

### Responsabilidade
Interface web do e-commerce, responsável por:
- Autenticação de usuários via OpenID Connect
- Exibição de catálogo de produtos
- Gerenciamento de carrinho de compras
- Aplicação de cupons
- Finalização de compras (checkout)

### Estrutura Interna

```
GeekShopping.Web/
├── Controllers/
│   ├── HomeController.cs         # Listagem produtos, login/logout
│   ├── ProductController.cs      # CRUD produtos (Admin)
│   └── CartController.cs         # Carrinho, checkout, cupons
├── Models/
│   ├── ProductViewModel.cs
│   ├── CartViewModel.cs
│   ├── CartHeaderViewModel.cs
│   ├── CartDetailViewModel.cs
│   └── CouponViewModel.cs
├── Services/
│   ├── IProductService.cs
│   ├── ProductService.cs
│   ├── ICartService.cs
│   ├── CartService.cs
│   ├── ICouponService.cs
│   └── CouponService.cs
├── Utils/
│   ├── HttpClientExtensions.cs   # Serialização JSON, POST/PUT helpers
│   └── Role.cs                   # Constantes de roles
├── Views/                         # Razor Pages
└── appsettings.json
```

### Entradas
- **Requisições HTTP**: Navegação do usuário (GET/POST)
- **Tokens**: Access Token do Identity Server (via cookie)

### Saídas
- **HTML**: Views renderizadas
- **HTTP Requests**: Chamadas para Product API, Cart API, Coupon API

### Integrações
| Serviço | Configuração | Finalidade |
|---------|--------------|------------|
| Identity Server | `ServiceUrls:IdentityServer` | Autenticação OAuth2/OIDC |
| Product API | `ServiceUrls:ProductAPI` | CRUD de produtos |
| Cart API | `ServiceUrls:CartAPI` | Operações de carrinho |
| Coupon API | `ServiceUrls:CouponAPI` | Validação de cupons |

### Validação
- **Endpoint**: https://localhost:5148
- **Tela de Login**: Redireciona para Identity Server se não autenticado
- **Swagger**: Não disponível (aplicação MVC)

---

## 2. GeekShopping.IdentityServer

**Linguagem**: C# / .NET 7  
**Tipo**: Servidor de Autenticação (Duende IdentityServer)

### Responsabilidade
- Emissão de tokens JWT (Access Token, ID Token)
- Validação de credenciais
- Gerenciamento de escopos e claims
- Suporte a OAuth2 e OpenID Connect

### Estrutura Interna
```
GeekShopping.IdentityServer/
├── Configuration/
│   ├── IdentityConfiguration.cs   # Usuários in-memory
│   ├── IdentityResourcesConfig.cs # OpenID scopes
│   └── ClientsConfig.cs            # Clients configurados
├── wwwroot/                        # UI do Identity Server
└── appsettings.json
```

### Entradas
- **Login Request**: Credenciais do usuário
- **Token Request**: Authorization code flow

### Saídas
- **Access Token**: JWT com claims do usuário
- **ID Token**: Informações de identidade

### Integrações
- Nenhuma integração com outros serviços (isolado)

### Validação
```bash
# Verificar discovery document
curl https://localhost:{porta}/.well-known/openid-configuration
```

---

## 3. GeekShopping.ProductAPI

**Linguagem**: C# / .NET 7  
**Tipo**: Web API (RESTful)

### Responsabilidade
Gerenciamento do catálogo de produtos:
- CRUD de produtos
- Consulta pública (listagem)
- Operações administrativas (criar, editar, deletar)

### Estrutura Interna
```
GeekShopping.ProductAPI/
├── Controllers/
│   └── ProductController.cs       # api/v1/product
├── Models/
│   └── Product.cs                 # Entidade
├── Data/
│   └── ProductContext.cs          # EF Core DbContext
├── Repository/
│   ├── IProductRepository.cs
│   └── ProductRepository.cs
└── appsettings.json
```

### Entradas
- **GET /api/v1/product**: Lista todos os produtos
- **GET /api/v1/product/{id}**: Busca produto por ID
- **POST /api/v1/product**: Cria produto (requer autenticação Admin)
- **PUT /api/v1/product**: Atualiza produto (requer autenticação Admin)
- **DELETE /api/v1/product/{id}**: Remove produto (requer autenticação Admin)

### Saídas
- **JSON**: ProductViewModel

### Integrações
| Componente | Finalidade |
|------------|------------|
| MySQL | Persistência de produtos |
| Identity Server | Validação de tokens JWT (Bearer) |

### Validação
```bash
# Listar produtos (público)
curl https://localhost:{porta}/api/v1/product

# Criar produto (requer token)
curl -X POST https://localhost:{porta}/api/v1/product \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"name":"Produto Teste","price":99.90}'
```

---

## 4. GeekShopping.CartAPI

**Linguagem**: C# / .NET 7  
**Tipo**: Web API (RESTful)

### Responsabilidade
- Adicionar/remover itens do carrinho
- Aplicar/remover cupons de desconto
- Calcular totais e descontos
- Iniciar processo de checkout

### Estrutura Interna
```
GeekShopping.CartAPI/
├── Controllers/
│   └── CartController.cs
├── Models/
│   ├── Cart.cs
│   ├── CartHeader.cs
│   └── CartDetail.cs
├── Data/
│   └── CartContext.cs
├── Repository/
│   ├── ICartRepository.cs
│   └── CartRepository.cs
├── Messages/
│   └── CheckoutHeaderVO.cs        # DTO para RabbitMQ
└── appsettings.json
```

### Entradas
- **POST /api/v1/cart/add-cart**: Adicionar item
- **PUT /api/v1/cart/update-cart**: Atualizar quantidade
- **DELETE /api/v1/cart/remove-cart/{id}**: Remover item
- **POST /api/v1/cart/apply-coupon**: Aplicar cupom
- **DELETE /api/v1/cart/remove-coupon/{userId}**: Remover cupom
- **POST /api/v1/cart/checkout**: Finalizar compra
- **GET /api/v1/cart/find-cart/{userId}**: Buscar carrinho do usuário

### Saídas
- **JSON**: CartViewModel
- **RabbitMQ**: Mensagem de checkout para fila `checkoutqueue`

### Integrações
| Serviço | Finalidade |
|---------|------------|
| MySQL | Persistência de carrinhos |
| Coupon API | Validação de cupons (HTTP) |
| RabbitMQ | Publicação de evento de checkout |

### Validação
```bash
# Buscar carrinho
curl https://localhost:{porta}/api/v1/cart/find-cart/{userId} \
  -H "Authorization: Bearer {token}"
```

---

## 5. GeekShopping.CouponAPI

**Linguagem**: C# / .NET 7  
**Tipo**: Web API (RESTful)

### Responsabilidade
- Validar códigos de cupom
- Retornar valor de desconto

### Estrutura Interna
```
GeekShopping.CouponAPI/
├── Controllers/
│   └── CouponController.cs
├── Models/
│   └── Coupon.cs
├── Data/
│   └── CouponContext.cs
├── Repository/
│   ├── ICouponRepository.cs
│   └── CouponRepository.cs
└── appsettings.json
```

### Entradas
- **GET /api/v1/coupon/{couponCode}**: Buscar cupom por código

### Saídas
- **JSON**: CouponViewModel (com `DiscountAmount`)

### Integrações
| Componente | Finalidade |
|------------|------------|
| MySQL | Persistência de cupons |

---

## 6. GeekShopping.MessageBus (Library)

**Linguagem**: C# / .NET 7  
**Tipo**: Class Library

### Responsabilidade
- Abstração de comunicação com RabbitMQ
- Publicação de mensagens em filas
- Interface genérica para mensageria

### Estrutura Interna
```
GeekShopping.MessageBus/
├── IMessageBus.cs
└── MessageBus.cs                  # Implementação RabbitMQ
```

### Integrações
- **RabbitMQ**: Porta 5672 (AMQP)

### Uso
```csharp
await _messageBus.PublishMessage(messageObject, "queueName");
```

---

## 7. Demais APIs (Order, Payment, Email)

**Padrão**: Estrutura similar às APIs acima  
**Comunicação**: Via RabbitMQ (consumidores de filas)

### Order API
- Consome fila de checkout
- Cria pedidos no banco
- Publica evento de pagamento

### Payment API
- Consome fila de pagamento
- Processa pagamento (mock)
- Publica resultado

### Email API
- Consome fila de confirmação
- Envia e-mails de confirmação

---