# Guia do AWS Cognito para o Nosso Projeto

Olá! Imagine que o **AWS Cognito** é como o **Grande Guardião de Chaves** do nosso clube secreto (que é o nosso aplicativo). Ele cuida de quem pode entrar, quem esqueceu a chave e quem é novo por aqui.

Aqui está como pedimos para ele fazer cada coisa:

---

## 🛠️ 0. Como Instalar (Preparar o Terreno)
**O que é?** Antes de tudo, precisamos trazer as ferramentas do Guardião para dentro do nosso projeto. É como se estivéssemos baixando o "manual de instruções" e os acessórios que o Guardião usa para trabalhar.

**Como fazemos?**
Abra o seu terminal (aquela telinha preta de comandos) dentro da pasta do projeto e digite esses dois comandos mágicos:

```bash
dotnet add package AWSSDK.CognitoIdentityProvider
dotnet add package Amazon.Extensions.CognitoAuthentication
```

Pronto! Agora o nosso projeto já sabe como conversar com o Guardião.

---

## 🔑 1. Login (Pedir para Entrar)
**O que é?** É quando você chega na porta do clube e diz seu nome e sua senha para o Guardião.
**Como fazemos no código?**
Usamos o comando `AdminInitiateAuthAsync`. É como mostrar sua identidade para o Guardião. Se a senha estiver certa, ele te entrega uma "pulseirinha mágica" (que chamamos de **Token**) para você poder entrar em qualquer lugar do clube.

**Exemplo em .NET:**
```csharp
var authRequest = new AdminInitiateAuthRequest
{
    UserPoolId = "Seu_Pool_Id",
    ClientId = "Seu_Client_Id",
    AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
    AuthParameters = new Dictionary<string, string>
    {
        { "USERNAME", "email@exemplo.com" },
        { "PASSWORD", "SuaSenha123!" }
    }
};

var response = await _cognitoClient.AdminInitiateAuthAsync(authRequest);

// O que o Guardião devolve (response):
// response.AuthenticationResult.IdToken      -> Seu crachá com seu nome
// response.AuthenticationResult.AccessToken -> Sua pulseirinha para entrar
// response.AuthenticationResult.RefreshToken -> Seu vale-troca para pulseiras novas
// response.AuthenticationResult.ExpiresIn    -> Quanto tempo a pulseirinha dura
```

**O JSON que o Guardião manda (Bruto):**
```json
{
  "AuthenticationResult": {
    "AccessToken": "eyJhbG...",
    "ExpiresIn": 3600,
    "IdToken": "eyJhbG...",
    "RefreshToken": "eyJhbG...",
    "TokenType": "Bearer"
  },
  "ChallengeParameters": {}
}
```


---

## ❓ 2. Esqueci Minha Senha (Perdi minha chave!)
**O que é?** Sabe quando você esquece onde deixou seu brinquedo? Aqui é quando você esquece sua senha.
**Como fazemos no código?**
Usamos o comando `ForgotPasswordAsync`. É como gritar: *"Guardião, me ajuda! Esqueci minha senha!"*. O Guardião então diz: *"Calma, vou te mandar um código secreto no seu e-mail para ter certeza que é você mesmo"*.

**Exemplo em .NET:**
```csharp
var forgotPasswordRequest = new ForgotPasswordRequest
{
    ClientId = "Seu_Client_Id",
    Username = "email@exemplo.com"
};

var response = await _cognitoClient.ForgotPasswordAsync(forgotPasswordRequest);

// O que o Guardião devolve (response):
// response.CodeDeliveryDetails.Destination -> Para onde o código foi enviado (ex: r***@g***.com)
// response.CodeDeliveryDetails.DeliveryMedium -> Se foi por EMAIL ou SMS
```

**O JSON que o Guardião manda (Bruto):**
```json
{
  "CodeDeliveryDetails": {
    "AttributeName": "email",
    "DeliveryMedium": "EMAIL",
    "Destination": "r***@g***.com"
  }
}
```


---

## ✉️ 3. Enviar Código de Segurança (O Bilhete Secreto)
**O que é?** É o código que o Guardião manda para o seu e-mail ou celular.
**Como fazemos no código?**
Isso acontece automaticamente logo depois que você diz que esqueceu a senha. O Cognito envia um número (tipo um código de 6 dígitos) que é como um "segredo de amizade" para provar quem você é.

**No Código (.NET):**
Você encontra os detalhes de para onde o código foi no `response.CodeDeliveryDetails` do passo anterior!


---

## ✅ 4. Validar Código e Criar Nova Senha (Trocar a Chave)
**O que é?** É quando você mostra o código secreto para o Guardião e diz qual será sua nova senha.
**Como fazemos no código?**
Usamos o `ConfirmForgotPasswordAsync`. Você entrega o código que recebeu no e-mail e diz: *"Aqui está o código! Agora minha nova senha será 'Batata123'!"*. O Guardião verifica o código e, se estiver certo, troca sua chave antiga pela nova.

**Exemplo em .NET:**
```csharp
var confirmRequest = new ConfirmForgotPasswordRequest
{
    ClientId = "Seu_Client_Id",
    Username = "email@exemplo.com",
    ConfirmationCode = "123456", // O código que chegou por e-mail
    Password = "NovaSenhaSegura123!"
};

var response = await _cognitoClient.ConfirmForgotPasswordAsync(confirmRequest);

// O que o Guardião devolve (response):
// response.HttpStatusCode -> Se for 200 (OK), deu tudo certo e a senha foi trocada!
```

**O JSON que o Guardião manda (Bruto):**
```json
{} // Geralmente vem vazio, o importante é o Status ser 200!
```


---

## 🎫 5. Retornar Dados de Autenticação (A Pulseirinha Mágica / A Ficha do Clube)
**O que é?** É quando você precisa ver a "ficha completa" do usuário no clube, para saber quem ele é, o e-mail ou o nome que está salvo lá.
**Como fazemos no código?**
Usamos o comando `AdminGetUserAsync`. É como dizer: *"Guardião, me mostra a ficha dessa pessoa aqui!"*. Ele vai olhar nos arquivos e te devolver todas as informações daquele membro.

**Exemplo em .NET:**
```csharp
var getUserRequest = new AdminGetUserRequest
{
    UserPoolId = "Seu_Pool_Id",
    Username = "email@exemplo.com" // O e-mail de quem você quer ver a ficha
};

var response = await _cognitoClient.AdminGetUserAsync(getUserRequest);

// O que o Guardião devolve (response):
// response.UserAttributes -> Traz uma listinha com dados como "name" (nome) e "email".
// response.UserStatus -> Mostra se ele está "CONFIRMED" (tudo ok) ou se precisa de algo.
// response.Enabled -> Mostra se a pessoa está permitida a entrar no clube (true).
```

**O JSON que o Guardião manda (Bruto):**
```json
{
  "Enabled": true,
  "UserAttributes": [
    { "Name": "sub", "Value": "codigo-unico-do-usuario" },
    { "Name": "email", "Value": "rafael@exemplo.com" },
    { "Name": "name", "Value": "Rafael Silva" }
  ],
  "UserCreateDate": 1679860000,
  "UserLastModifiedDate": 1679865000,
  "UserStatus": "CONFIRMED",
  "Username": "rafael@exemplo.com"
}
```

## 👤 6. Criar Usuários (Convidar um Amigo)
**O que é?** É colocar o nome de um novo amigo na lista oficial do clube.
**Como fazemos no código?**
Usamos o `AdminCreateUserAsync`. Nós dizemos para o Guardião: *"Guardião, adicione o Rafael na nossa lista! O e-mail dele é rafael@exemplo.com"*. O Guardião anota tudo e já prepara um lugar para o novo amigo.

**Exemplo em .NET:**
```csharp
var createUserRequest = new AdminCreateUserRequest
{
    UserPoolId = "Seu_Pool_Id",
    Username = "novo_usuario@exemplo.com",
    UserAttributes = new List<AttributeType>
    {
        new AttributeType { Name = "email", Value = "novo_usuario@exemplo.com" },
        new AttributeType { Name = "email_verified", Value = "true" }
    },
    MessageAction = MessageActionType.SUPPRESS // Para não mandar e-mail automático agora
};

var response = await _cognitoClient.AdminCreateUserAsync(createUserRequest);

// O que o Guardião devolve (response):
// response.User.Username   -> O nome do usuário que foi criado
// response.User.UserStatus -> O status (ex: FORCE_CHANGE_PASSWORD se ele precisar trocar a senha)
// response.User.Enabled    -> Se o usuário já pode usar o clube (true/false)
```

**O JSON que o Guardião manda (Bruto):**
```json
{
  "User": {
    "Attributes": [
      { "Name": "sub", "Value": "codigo-unico-do-usuario" },
      { "Name": "email", "Value": "rafael@exemplo.com" }
    ],
    "Enabled": true,
    "UserCreateDate": 1679860000,
    "UserStatus": "FORCE_CHANGE_PASSWORD",
    "Username": "rafael@exemplo.com"
  }
}
```


---

## 🔄 7. Renovar a Pulseirinha (Refresh Token) 
**O que é?** Imagine que a sua pulseirinha mágica (que deixa você entrar no clube) tem um prazo de validade. Quando ela fica "veia", você não precisa dar sua senha de novo! Você só mostra um "Cupom de Troca" (que chamamos de **RefreshToken**) para o Guardião e ele te dá uma pulseirinha novinha em folha.

**Exemplo em .NET:**
```csharp
var refreshRequest = new AdminInitiateAuthRequest
{
    UserPoolId = "Seu_Pool_Id",
    ClientId = "Seu_Client_Id",
    AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
    AuthParameters = new Dictionary<string, string>
    {
        { "REFRESH_TOKEN", "Seu_Cupom_De_Troca_Aqui" } // Aquele RefreshToken do login
    }
};

var response = await _cognitoClient.AdminInitiateAuthAsync(refreshRequest);

// O que o Guardião devolve (response):
// response.AuthenticationResult.IdToken      -> Seu crachá renovado!
// response.AuthenticationResult.AccessToken -> Sua pulseirinha novinha!
```

**O JSON que o Guardião manda (Bruto):**
```json
{
  "AuthenticationResult": {
    "AccessToken": "eyJhbG... (Novo AccessToken)",
    "ExpiresIn": 3600,
    "IdToken": "eyJhbG... (Novo IdToken)",
    "TokenType": "Bearer"
  },
  "ChallengeParameters": {}
}
```


---

## 🆕 8. Primeiro Acesso (Troca de Senha Obrigatória)
**O que é?** Quando você é convidado para o clube pela primeira vez, o Guardião te dá uma senha provisória só para você conseguir chegar até ele. Mas na hora de entrar, ele te para e diz: *"Ei! Você precisa escolher sua PRÓPRIA senha agora e me dizer seu nome para eu completar seu cadastro!"*.

**Exemplo em .NET:**
```csharp
var challengeRequest = new AdminRespondToAuthChallengeRequest
{
    UserPoolId = "Seu_Pool_Id",
    ClientId = "Seu_Client_Id",
    ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
    Session = "Sessao_Do_Login_Anterior", // Você ganha isso ao tentar logar com a senha provisória
    ChallengeResponses = new Dictionary<string, string>
    {
        { "USERNAME", "email@exemplo.com" },
        { "NEW_PASSWORD", "MinhaSenhaReal123!" },
        { "userAttributes.name", "Rafael Silva" } // Aqui o usuário informa o nome!
    }
};

var response = await _cognitoClient.AdminRespondToAuthChallengeAsync(challengeRequest);

// O que o Guardião devolve (response):
// Se der certo, ele já te entrega as pulseirinhas (tokens) no response.AuthenticationResult!
```

**O JSON que o Guardião manda (Bruto):**
```json
{
  "AuthenticationResult": {
    "AccessToken": "eyJhbG...",
    "IdToken": "eyJhbG...",
    "RefreshToken": "eyJhbG...",
    "ExpiresIn": 3600
  },
  "ChallengeParameters": {}
}
```


---

## 📧 9. Verificar E-mail (Confirmar Cadastro)
**O que é?** Quando você se cadastra sozinho no clube (sem um convite do Admin), o Guardião quer ter certeza de que o e-mail que você deu é mesmo seu. Ele te manda um código e diz: *"Opa! Mostra pra mim o código que eu mandei por e-mail para eu ter certeza que é você!"*.

**Exemplo em .NET:**
```csharp
var confirmSignUpRequest = new ConfirmSignUpRequest
{
    ClientId = "Seu_Client_Id",
    Username = "email@exemplo.com", // Seu e-mail ou nome de usuário
    ConfirmationCode = "123456"    // O código de 6 dígitos que chegou por e-mail
};

var response = await _cognitoClient.ConfirmSignUpAsync(confirmSignUpRequest);

// O que o Guardião devolve (response):
// Se o response.HttpStatusCode for 200 (OK), o e-mail foi verificado!
```

**O JSON que o Guardião manda (Bruto):**
```json
{} // Resposta vazia, apenas confirmando que deu tudo certo!
```


---

## ✏️ 10. Atualizar Dados (Mudar o Nome no Cadastro)
**O que é?** É quando você quer mudar alguma informação sua no clube, como o seu nome.
**Como fazemos no código?**
Usamos o comando `AdminUpdateUserAttributesAsync`. A gente fala para o Guardião: *"Guardião, atualiza o nome desse e-mail para esse novo nome aqui!"*.

**Exemplo em .NET:**
```csharp
var updateRequest = new AdminUpdateUserAttributesRequest
{
    UserPoolId = "Seu_Pool_Id",
    Username = "email@exemplo.com", // O e-mail de quem vamos mudar
    UserAttributes = new List<AttributeType>
    {
        new AttributeType { Name = "name", Value = "Novo Nome Legal" }
    }
};

var response = await _cognitoClient.AdminUpdateUserAttributesAsync(updateRequest);

// O que o Guardião devolve (response):
// Se der tudo certo, ele só devolve o Status 200 (OK), avisando que mudou!
```

**O JSON que o Guardião manda (Bruto):**
```json
{} // Resposta vazia, só confirmando que deu certo!
```


---

## 🗑️ 11. Excluir Usuário (Sair do Clube)
**O que é?** É quando você não quer mais fazer parte do clube e pede para o Guardião apagar todos os seus dados.
**Como fazemos no código?**
Usamos o comando `AdminDeleteUserAsync`. A gente avisa: *"Guardião, pode apagar esse usuário do sistema!"*.

**Exemplo em .NET:**
```csharp
var deleteRequest = new AdminDeleteUserRequest
{
    UserPoolId = "Seu_Pool_Id",
    Username = "email@exemplo.com" // O e-mail de quem vamos excluir
};

var response = await _cognitoClient.AdminDeleteUserAsync(deleteRequest);

// O que o Guardião devolve (response):
// Devolve Status 200 (OK) se excluiu com sucesso!
```

**O JSON que o Guardião manda (Bruto):**
```json
{} // Resposta vazia, confirmando que apagou!
```


---

## 🛠️ Seção Técnica (Para o Desenvolvedor)

Se você for o "Mestre do Código", aqui estão os nomes que o Guardião entende:

| Ação | Comando do AWS SDK (.NET) |
| :--- | :--- |
| **Login** | `AdminInitiateAuthAsync` ou `InitiateAuthAsync` |
| **Esqueci Senha** | `ForgotPasswordAsync` |
| **Validar Código** | `ConfirmForgotPasswordAsync` |
| Novo Usuário | `AdminCreateUserAsync` ou `SignUpAsync` |
| **Pegar Dados** | `AdminGetUserAsync` |
| **Renovar Token** | `AdminInitiateAuthAsync (REFRESH_TOKEN_AUTH)` |
| **Primeiro Acesso** | `AdminRespondToAuthChallengeAsync` |
| **Confirmar Cadastro** | `ConfirmSignUpAsync` |
| **Atualizar Dados** | `AdminUpdateUserAttributesAsync` |
| **Excluir Usuário** | `AdminDeleteUserAsync` |

> [!TIP]
> **Lembre-se:** O Guardião é muito rigoroso! Se você errar a senha ou o código, ele não vai deixar você passar. Segurança em primeiro lugar! 🛡️
