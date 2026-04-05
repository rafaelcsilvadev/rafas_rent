# Guia de CORS (Cross-Origin Resource Sharing)

Imagina que a sua API é como uma lanchonete com uma porta mágica. Por padrão, ela só deixa pessoas do mesmo bairro (localhost) entrarem. O CORS é a sua "Lista VIP" para deixar amigos de outros bairros (outras portas ou domínios) fazerem pedidos para você.

## Opção 1: Liberar Tudo (Desenvolvimento)
Isso é como uma festa aberta no parque. Todo mundo é bem-vindo e pode fazer qualquer coisa!

```csharp
options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});
```

## Opção 2: Política Restrita (Produção)
Isso é como uma festa em casa. Você só convida amigos específicos (origens) e só deixa eles fazerem certas coisas (métodos).

```csharp
options.AddPolicy("StrictPolicy", policy =>
{
    policy.WithOrigins("https://seu-dominio-frontend.com", "http://localhost:3000")
          .WithMethods("GET", "POST")
          .WithHeaders("Content-Type", "Authorization")
          .AllowCredentials(); // Permite "cartões de identificação" especiais (cookies/auth)
});
```

## Regras Importantes
- **WithOrigins**: Diz quem pode entrar na festa.
- **WithMethods**: Diz o que eles podem fazer (GET, POST, DELETE, etc.).
- **WithHeaders**: Diz que informações eles precisam trazer.
- **AllowCredentials**: Deixa usar cookies e tokens de autenticação especiais.

## Como usar no Program.cs

1. Adicione o serviço:
```csharp
builder.Services.AddCors(options => { /* sua política aqui */ });
```

2. Use o middleware (antes de MapControllers):
```csharp
app.UseCors("NomeDaSuaPolitica");
```

---

## CORS em AWS Lambda

Diferente da nossa lanchonete (API .NET), as **AWS Lambdas** são como entregas rápidas. Geralmente, o **API Gateway** resolve o CORS por você no painel da AWS. Mas, se você precisar fazer na mão dentro da Lambda, você deve incluir as informações de segurança no papel de entrega (o cabeçalho da resposta).

### Exemplo de Resposta JSON da Lambda (Lambda Response)

Sempre que a sua Lambda responder, ela precisa mandar esses "papéis" (headers) colados no lanche:

```json
{
  "statusCode": 200,
  "headers": {
    "Access-Control-Allow-Origin": "*", // Para produção, use o endereço do seu site (Ex: https://meu-site.com.br)
    "Access-Control-Allow-Methods": "OPTIONS,GET,POST", // Quais tipos de pedido a Lambda aceita
    "Access-Control-Allow-Headers": "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token" // Quais informações extras são permitidas
  },
  "body": "{\"message\": \"Hello from Lambda!\"}"
}
```

### Regras nas Lambdas:
- **API Gateway**: Você pode configurar o CORS diretamente no site da AWS (no console do API Gateway).
- **Tratamento Manual**: Se o API Gateway não estiver cuidando disso, sua Lambda **tem que devolver** os headers acima, senão o navegador do cliente vai bloquear a resposta por segurança (mesmo que a Lambda tenha funcionado!).
