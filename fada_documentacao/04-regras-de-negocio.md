# Regras de Negócio

## 1. Autenticação e Autorização

### RN-001: Controle de Acesso a Produtos
**Regra**: Operações de CRUD de produtos requerem autenticação e papel de administrador.

**Implementação**:
```csharp
// ProductController.cs
[Authorize(Roles = Role.Admin)]
public async Task<IActionResult> Delete(ProductViewModel model)
```

**Validação**:
- Tentar deletar produto sem token → HTTP 401
- Tentar deletar com usuário comum → HTTP 403
- Listar produtos → Público (sem autenticação)

### RN-002: Sessão de Autenticação
**Regra**: Cookies de sessão expiram em 10 minutos.

**Configuração**:
```csharp
// Program.cs - GeekShopping.Web
.AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
```

**Comportamento**: Após 10 minutos de inatividade, o usuário é redirecionado para login.

---

## 2. Carrinho de Compras

### RN-003: Identificação do Carrinho
**Regra**: Cada usuário possui um carrinho identificado pelo `UserId` (claim "sub" do token JWT).

**Implementação**:
```csharp
// CartController.cs
var userId = User.Claims.Where(c => c.Type == "sub")?.FirstOrDefault()?.Value;
```

**Validação**: Verificar que o `UserId` no `CartHeader` corresponde ao usuário autenticado.

### RN-004: Cálculo do Total do Carrinho
**Regra**: O total da compra é calculado como:
```
PurchaseAmount = Σ(Product.Price * Count) - DiscountAmount
```

**Implementação**:
```csharp
// CartController.cs - FindUserCart()
foreach (var detail in response.CartDetails)
    response.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);

response.CartHeader.PurchaseAmount -= response.CartHeader.DiscountAmount;
```

**Validação**: Conferir cálculos na view do carrinho (`Index`).

### RN-005: Validação de Quantidade
**Regra**: A quantidade de um produto no carrinho deve estar entre 1 e 100.

**Implementação**:
```csharp
// ProductViewModel.cs
[Range(1, 100)]
public int Count { get; set; } = 1;
```

**Validação**: Tentar adicionar quantidade fora do range → Erro de validação.

---

## 3. Cupons de Desconto

### RN-006: Aplicação de Cupom
**Regra**: Um cupom válido aplica um desconto fixo (`DiscountAmount`) ao carrinho.

**Fluxo**:
1. Usuário informa código do cupom
2. Cart API valida via Coupon API
3. Se válido, armazena `CouponCode` e `DiscountAmount` no `CartHeader`
4. Recalcula `PurchaseAmount`

**Validação**:
```csharp
// CartController.cs - FindUserCart()
if (!string.IsNullOrEmpty(response.CartHeader.CouponCode))
{
    var coupon = await _couponService.GetCoupon(response.CartHeader.CouponCode, token);
    if (coupon?.CouponCode != null)
        response.CartHeader.DiscountAmount = coupon.DiscountAmount;
}
```

### RN-007: Cupom Inexistente
**Regra**: Se o cupom não existir, retornar objeto vazio com `CouponCode = string.Empty`.

**Implementação**:
```csharp
// CouponService.cs
if (response.StatusCode != HttpStatusCode.OK)
    return new CouponViewModel() { CouponCode = string.Empty };
```

**Validação**: Tentar aplicar cupom inválido → Desconto não aplicado.

### RN-008: Remoção de Cupom
**Regra**: Remover cupom limpa `CouponCode` e zera `DiscountAmount`.

**Endpoint**: `DELETE /api/v1/cart/remove-coupon/{userId}`

---

## 4. Checkout e Pedidos

### RN-009: Validação de Preço do Cupom
**Regra**: Se o valor do cupom mudou entre a aplicação e o checkout, retornar HTTP 412 (Precondition Failed).

**Implementação**:
```csharp
// CartController.cs - Checkout()
if (response != null && response.GetType() == typeof(string))
{
    TempData["Error"] = response;
    return RedirectToAction(nameof(Checkout));
}
```

**Mensagem**: "Coupon Price has changed. Please confirm!"

**Validação**: Alterar valor do cupom no banco antes do checkout.

### RN-010: Limpeza do Carrinho Após Checkout
**Regra**: Após checkout bem-sucedido, o carrinho do usuário é limpo.

**Implementação**:
```csharp
// CartController.cs - Checkout()
if (response != null)
{
    _ = _cartService.ClearCart(userId, token);
    return RedirectToAction(nameof(Confirmation));
}
```

**Validação**: Verificar que `/find-cart/{userId}` retorna carrinho vazio após checkout.

### RN-011: Processamento Assíncrono
**Regra**: Checkout publica mensagem em fila RabbitMQ (`checkoutqueue`), não processa sincronamente.

**Fluxo**:
```
Cart API → RabbitMQ (checkout) → Order API → RabbitMQ (payment) → Payment API → Email API
```

---

## 5. Produtos

### RN-012: Truncamento de Texto
**Regra**: Nome do produto exibido na listagem é limitado a 24 caracteres.

**Implementação**:
```csharp
// ProductViewModel.cs
public string SubstringName()
{
    if (Name.Length < 24) 
        return Name;
    return $"{Name.Substring(0, 21)} ...";
}
```

**Validação**: Verificar view de listagem (`Home/Index`).

### RN-013: Descrição Resumida
**Regra**: Descrição limitada a 355 caracteres na listagem.

**Implementação**: Similar a `SubstringName()`.

---

##