# 🛠️ Tutorial Atualizado: Middlewares com AWS Cognito e Rotas Privadas 🔐

Oi de novo! Agora que já sabemos quem é o **Guarda (Middleware)**, vamos ensinar a ele como reconhecer o **Cartão Especial do Cognito** e como ele deve decidir quais portas proteger.

---

## 1. Ensinando o Guarda a ler o Cartão do Cognito (JWT) 🔑

O **AWS Cognito** dá um cartão mágico chamado **Token JWT**. Para o guarda saber que esse cartão é de verdade, precisamos mostrar para ele de qual "clube" (User Pool) os cartões vêm.

### Passo 1: O que o Guarda precisa saber (no Program.cs)

Você vai precisar dizer ao guarda duas coisas:
-   **Pool ID:** O nome do seu clube Cognito.
-   **Região:** Onde o clube fica (ex: `us-east-1`).

```csharp
// Passo A: Configurando o manual do Guarda (Cuidado: Precisa do pacote JwtBearer)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        // Diga ao guarda onde conferir se o cartão é real
        options.Authority = "https://cognito-idp.us-east-1.amazonaws.com/your-pool-id";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false // Para APIs simples, podemos ignorar isso por enquanto
        };
    });

builder.Services.AddAuthorization();
```

---

## 2. Escolhendo quais Portas proteger (Rotas Privadas) 🚪

Se o guarda ficar na porta principal do castelo, ninguém entra em lugar nenhum sem o cartão. Mas talvez você queira que a recepção (Login) seja livre para todos!

### Passo A: Coloque a Placa "Aberto para Todos" (Public)
Nas rotas que **não** precisam de Token (ex: Login e Registro), usamos uma placa chamada `[AllowAnonymous]`.

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous] // Todo mundo pode entrar aqui para pedir sua chave!
    public IActionResult Login() { /* ... */ }
}
```

### Passo B: Coloque a Placa "Apenas Membros" (Private)
Nas rotas que **precisam** do Token (ex: Perfil, Pagamentos), usamos a placa `[Authorize]`.

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // Todos os caminhos deste corredor precisam da chave mágica!
public class ProfileController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMyData() { /* ... */ }
}
```

---

## 3. O que acontece na porta? 🛡️

Quando alguém tenta abrir uma porta com a placa `[Authorize]`:
1.  **O Guarda para a pessoa:** "Pare! Onde está seu cartão?"
2.  **O Guarda confere:** "Ah, esse é um cartão do Cognito? Deixa eu ver se o clube `us-east-1` realmente deu ele para você..."
3.  **Decisão final:**
    *   Se o cartão for bom: "Pode entrar!"
    *   Se não tiver cartão ou for falso: "Fora daqui! Erro 401 (Não Autorizado)!" 🚫

---

## 💡 Lembrete Importante 🛠️
Você precisa instalar este pacote primeiro para o código funcionar:
`dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`

---

Pronto! Agora o seu guarda é um especialista em Cognito e fala o seu idioma! 🕵️‍♂️✨
