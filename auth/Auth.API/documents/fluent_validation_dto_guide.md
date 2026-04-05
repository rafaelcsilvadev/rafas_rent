# Guia Limpo: DTOs e FluentValidation (Classes Separadas)

Se você não acha legal colocar uma classe dentro da outra (e muitos programadores concordam com você!), o jeito oficial e mais comum no mundo .NET é separar as coisas: uma classe só para a "caixa" (DTO) e outra classe separada para o "segurança" (Validador).

## 1. Como instalar? 🚀

Abra o terminal na pasta do projeto e rode:

```bash
dotnet add package FluentValidation.AspNetCore
```

## 2. O Jeito Separado e Organizado 📦👮‍♂️

Neste formato, você deixa cada um no seu quadrado. Fica super fácil de achar os arquivos depois!

### Parte 1: O DTO
A classe fica super limpa, apenas guardando os dados:

```csharp
namespace Auth.API.Dtos
{
    public class CreateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
```

### Parte 2: O Validador
Crie uma classe separada só para cuidar das regras (você pode até colocar em uma pastinha chamada `Validators` se quiser):

```csharp
using FluentValidation;
using Auth.API.Dtos;

namespace Auth.API.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Must be a valid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}

---

## 3. O Jeito Moderno (Usando Record) ✨

Se você quiser deixar o seu código ainda mais moderno e limpo, você pode usar um `record` no lugar de uma `class` para o seu DTO.

Como os DTOs servem apenas para "transportar" os dados sem alterá-los, o `record` é simplesmente perfeito para isso!

### Parte 1: O DTO (Como Record)
Olha como o arquivo fica absurdamente pequeno e direto ao ponto:

```csharp
namespace Auth.API.Dtos
{
    public record CreateUserDto(string Name, string Email, string Password);
}
```

### Parte 2: O Validador
O Validador continua em uma classe separada, exatamente do mesmo jeito! O `FluentValidation` funciona perfeitamente com os `records`.

```csharp
using FluentValidation;
using Auth.API.Dtos;

namespace Auth.API.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters");

            // As regras para Email e Password continuam iguais...
        }
    }
}
```
