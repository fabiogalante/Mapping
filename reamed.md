# 🏗️ Order Management API - Domain-Driven Design com EF Core

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4?style=flat&logo=.net)
![License](https://img.shields.io/badge/license-MIT-green)
![DDD](https://img.shields.io/badge/pattern-DDD-blue)

Implementação completa de uma API RESTful seguindo princípios de **Domain-Driven Design (DDD)** com **Entity Framework Core** usando banco de dados **In-Memory**.

Este projeto é baseado no artigo [**"Mapping Domain-Driven Design Concepts To The Database With EF Core"**](https://medium.com/@mariammaurice/mapping-domain-driven-design-concepts-to-the-database-with-ef-core-c92b3cc3cc85) por Mori.

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Conceitos DDD Implementados](#-conceitos-ddd-implementados)
- [Arquitetura](#-arquitetura)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
  - [Setup Automático](#setup-automático)
  - [Setup Manual](#setup-manual)
- [Executando o Projeto](#-executando-o-projeto)
- [Endpoints da API](#-endpoints-da-api)
- [Exemplos de Uso](#-exemplos-de-uso)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Conceitos Técnicos](#-conceitos-técnicos)
- [Testes](#-testes)
- [Boas Práticas Implementadas](#-boas-práticas-implementadas)
- [Roadmap](#-roadmap)
- [Contribuindo](#-contribuindo)
- [Referências](#-referências)
- [Licença](#-licença)

---

## 🎯 Sobre o Projeto

Este projeto demonstra como implementar uma API de gerenciamento de pedidos seguindo os princípios de **Domain-Driven Design**, onde:

> **"O modelo de domínio expressa o negócio — não o banco de dados."**

### Objetivos

✅ Criar um domínio rico e expressivo  
✅ Isolar completamente a lógica de negócio da infraestrutura  
✅ Usar EF Core sem comprometer o design do domínio  
✅ Implementar agregados, entidades e value objects corretamente  
✅ Garantir invariantes de negócio em todos os momentos

---

## 🧩 Conceitos DDD Implementados

### 1. **Aggregate Root**
- `Order` controla todo o ciclo de vida dos itens do pedido
- Mantém consistência através de invariantes
- Encapsulamento total com backing fields

### 2. **Entities**
- `OrderItem` - parte do agregado Order
- Possui identidade contextual
- Construtor interno (apenas Order pode criar)

### 3. **Value Objects**
- `Money` - valor monetário com moeda
- `Address` - endereço completo
- Imutáveis (C# records)
- Igualdade baseada em valor

### 4. **Strongly Typed IDs**
- `OrderId`, `CustomerId`, `ProductId`
- Evita passar IDs incorretos (type safety)
- Elimina primitive obsession

### 5. **Domain Exceptions**
- `DomainException` - exceções específicas de negócio
- Mensagens claras e descritivas

### 6. **Repository Pattern**
- Interface no domínio (`IOrderRepository`)
- Implementação na infraestrutura
- Abstrai persistência

---

## 🏛️ Arquitetura

```
┌─────────────────────────────────────────────┐
│           Presentation Layer                │
│         (API Endpoints / DTOs)              │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│          Application Layer                  │
│         (Casos de Uso / Services)           │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│            Domain Layer                     │
│  ┌──────────────────────────────────────┐  │
│  │  Aggregates: Order                   │  │
│  │  Entities: OrderItem                 │  │
│  │  Value Objects: Money, Address       │  │
│  │  Domain Exceptions                   │  │
│  │  Repository Interfaces               │  │
│  └──────────────────────────────────────┘  │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│        Infrastructure Layer                 │
│  ┌──────────────────────────────────────┐  │
│  │  EF Core DbContext                   │  │
│  │  Entity Configurations               │  │
│  │  Repository Implementations          │  │
│  │  Database (In-Memory)                │  │
│  └──────────────────────────────────────┘  │
└─────────────────────────────────────────────┘
```

### Princípio da Dependência

```
Domain ← Infrastructure
   ↑
   │
Application
   ↑
   │
Presentation
```

**O domínio não depende de nada. Tudo depende do domínio.**

---

## 🔧 Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior
- IDE/Editor de código:
  - [Visual Studio 2022+](https://visualstudio.microsoft.com/)
  - [Visual Studio Code](https://code.visualstudio.com/)
  - [JetBrains Rider](https://www.jetbrains.com/rider/)
- Terminal/PowerShell/Bash

---

## 📥 Instalação

### Setup Automático

#### **Linux/Mac (Bash)**

```bash
# Baixar e executar o script de setup
curl -O https://raw.githubusercontent.com/seu-repo/setup.sh
chmod +x setup.sh
./setup.sh
```

#### **Windows (PowerShell)**

```powershell
# Executar script de setup
.\setup.ps1
```

### Setup Manual

#### 1. Criar o projeto

```bash
dotnet new webapi -n OrderManagement.Api -f net10.0
cd OrderManagement.Api
```

#### 2. Adicionar pacotes NuGet

```bash
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 10.0.0
dotnet add package Microsoft.AspNetCore.OpenApi --version 10.0.0
dotnet add package Swashbuckle.AspNetCore --version 7.2.0
```

#### 3. Criar estrutura de pastas

```bash
# Linux/Mac
mkdir -p Domain/{ValueObjects,Entities,Aggregates,Exceptions,Repositories}
mkdir -p Infrastructure/{Persistence,Repositories}

# Windows (PowerShell)
New-Item -ItemType Directory -Path "Domain\ValueObjects" -Force
New-Item -ItemType Directory -Path "Domain\Entities" -Force
New-Item -ItemType Directory -Path "Domain\Aggregates" -Force
New-Item -ItemType Directory -Path "Domain\Exceptions" -Force
New-Item -ItemType Directory -Path "Domain\Repositories" -Force
New-Item -ItemType Directory -Path "Infrastructure\Persistence" -Force
New-Item -ItemType Directory -Path "Infrastructure\Repositories" -Force
```

#### 4. Copiar os arquivos fornecidos

Copie os arquivos dos artifacts anteriores para as respectivas pastas.

#### 5. Restaurar e compilar

```bash
dotnet restore
dotnet build
```

---

## 🚀 Executando o Projeto

```bash
dotnet run
```

A API estará disponível em:
- **HTTPS**: `https://localhost:7xxx`
- **HTTP**: `http://localhost:5xxx`
- **Swagger UI**: `https://localhost:7xxx/swagger`

---

## 📡 Endpoints da API

### Base URL
```
https://localhost:7xxx/api
```

### 1. **Criar Pedido**

```http
POST /api/orders
Content-Type: application/json

{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
      "quantity": 2,
      "unitPrice": 50.00,
      "currency": "USD"
    },
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "quantity": 1,
      "unitPrice": 30.00,
      "currency": "USD"
    }
  ]
}
```

**Resposta (201 Created):**
```json
{
  "orderId": "9b3c4e5f-6789-4012-abcd-ef0123456789",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "totalAmount": 130.00,
  "currency": "USD",
  "itemCount": 2
}
```

### 2. **Buscar Pedido por ID**

```http
GET /api/orders/{id}
```

**Resposta (200 OK):**
```json
{
  "orderId": "9b3c4e5f-6789-4012-abcd-ef0123456789",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "totalAmount": 130.00,
  "currency": "USD",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
      "quantity": 2,
      "unitPrice": 50.00,
      "currency": "USD",
      "subTotal": 100.00
    },
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "quantity": 1,
      "unitPrice": 30.00,
      "currency": "USD",
      "subTotal": 30.00
    }
  ]
}
```

### 3. **Listar Todos os Pedidos**

```http
GET /api/orders
```

**Resposta (200 OK):**
```json
[
  {
    "orderId": "9b3c4e5f-6789-4012-abcd-ef0123456789",
    "totalAmount": 130.00,
    "itemsCount": 2
  },
  {
    "orderId": "8a2b3c4d-5678-9012-3456-7890abcdef01",
    "totalAmount": 75.00,
    "itemsCount": 1
  }
]
```

### 4. **Adicionar Item ao Pedido**

```http
POST /api/orders/{id}/items
Content-Type: application/json

{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
  "quantity": 3,
  "unitPrice": 15.00,
  "currency": "USD"
}
```

**Resposta (200 OK):**
```json
{
  "orderId": "9b3c4e5f-6789-4012-abcd-ef0123456789",
  "totalAmount": 175.00,
  "itemCount": 3
}
```

---

## 💡 Exemplos de Uso

### Usando cURL

#### Criar um pedido
```bash
curl -X POST https://localhost:7xxx/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "items": [
      {
        "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
        "quantity": 2,
        "unitPrice": 50.00,
        "currency": "USD"
      }
    ]
  }'
```

#### Buscar pedido
```bash
curl https://localhost:7xxx/api/orders/{orderId}
```

### Usando C# (HttpClient)

```csharp
using System.Net.Http.Json;

var client = new HttpClient { BaseAddress = new Uri("https://localhost:7xxx") };

// Criar pedido
var request = new CreateOrderRequest(
    CustomerId: Guid.NewGuid(),
    Items: new List<OrderItemRequest>
    {
        new(Guid.NewGuid(), 2, 50.00m, "USD")
    }
);

var response = await client.PostAsJsonAsync("/api/orders", request);
var order = await response.Content.ReadFromJsonAsync<OrderResponse>();

Console.WriteLine($"Order created: {order.OrderId}");
```

---

## 📂 Estrutura do Projeto

```
OrderManagement.Api/
│
├── 📁 Domain/                          # Camada de Domínio (Pura)
│   │
│   ├── 📁 Aggregates/
│   │   └── Order.cs                    # Aggregate Root
│   │
│   ├── 📁 Entities/
│   │   └── OrderItem.cs                # Entity (parte do agregado)
│   │
│   ├── 📁 ValueObjects/
│   │   ├── Money.cs                    # Value Object - Dinheiro
│   │   ├── Address.cs                  # Value Object - Endereço
│   │   ├── OrderId.cs                  # Strongly Typed ID
│   │   ├── CustomerId.cs               # Strongly Typed ID
│   │   └── ProductId.cs                # Strongly Typed ID
│   │
│   ├── 📁 Exceptions/
│   │   └── DomainException.cs          # Exceções de domínio
│   │
│   └── 📁 Repositories/
│       └── IOrderRepository.cs         # Interface do repositório
│
├── 📁 Infrastructure/                  # Camada de Infraestrutura
│   │
│   ├── 📁 Persistence/
│   │   ├── AppDbContext.cs             # EF Core DbContext
│   │   └── OrderConfiguration.cs       # Configuração EF Core
│   │
│   └── 📁 Repositories/
│       └── OrderRepository.cs          # Implementação do repositório
│
├── Program.cs                          # Configuração da API + Endpoints
├── OrderManagement.Api.csproj          # Arquivo do projeto
└── README.md                           # Este arquivo
```

---

## 🎓 Conceitos Técnicos

### 1. **Aggregate Root (Order)**

```csharp
public sealed class Order
{
    private readonly List<OrderItem> _items = new();
    
    // Encapsulamento total
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    // Invariante: quantidade deve ser positiva
    public void AddItem(ProductId productId, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive.");
            
        _items.Add(new OrderItem(productId, quantity, unitPrice));
        RecalculateTotal(); // Mantém consistência
    }
}
```

**Características:**
- ✅ Sem setters públicos
- ✅ Backing fields privados
- ✅ Validações no método de negócio
- ✅ Total calculado automaticamente

### 2. **Value Objects**

```csharp
public sealed record Money(decimal Amount, string Currency)
{
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new DomainException("Currencies must match.");
        
        return new Money(a.Amount + b.Amount, a.Currency);
    }
}
```

**Características:**
- ✅ Imutável (record)
- ✅ Igualdade por valor
- ✅ Sem identidade
- ✅ Operadores personalizados

### 3. **Strongly Typed IDs**

```csharp
public sealed record OrderId(Guid Value);
public sealed record CustomerId(Guid Value);
public sealed record ProductId(Guid Value);

// Uso:
var order = new Order(
    new OrderId(Guid.NewGuid()),
    new CustomerId(customerId)  // Type-safe!
);
```

**Benefícios:**
- ❌ Impossível passar ID errado
- ✅ Compilador valida tipos
- ✅ Código mais legível

### 4. **EF Core Mapping (Sem poluir o domínio)**

```csharp
public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Conversão de Strongly Typed ID
        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => new OrderId(value));
        
        // Value Object como Owned Entity
        builder.OwnsOne(o => o.TotalPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount");
            money.Property(m => m.Currency).HasColumnName("Currency");
        });
        
        // Backing field para encapsulamento
        builder.Navigation(o => o.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
```

### 5. **Check Constraints (Invariantes no BD)**

```csharp
builder.ToTable("Orders", table =>
{
    table.HasCheckConstraint(
        "CK_Orders_TotalAmount_Positive",
        "[TotalAmount] >= 0");
});
```

**Dupla proteção:**
- ✅ Domínio valida na aplicação
- ✅ Banco valida na persistência

---

## 🧪 Testes

### Teste de Domínio (Sem banco de dados)

```csharp
[Fact]
public void Order_Should_Calculate_Total_Correctly()
{
    // Arrange
    var order = new Order(
        new OrderId(Guid.NewGuid()),
        new CustomerId(Guid.NewGuid())
    );
    
    // Act
    order.AddItem(
        new ProductId(Guid.NewGuid()),
        2,
        new Money(50, "USD")
    );
    
    order.AddItem(
        new ProductId(Guid.NewGuid()),
        1,
        new Money(30, "USD")
    );
    
    // Assert
    Assert.Equal(130, order.TotalPrice.Amount);
    Assert.Equal("USD", order.TotalPrice.Currency);
}

[Fact]
public void Order_Should_Not_Accept_Negative_Quantity()
{
    // Arrange
    var order = new Order(
        new OrderId(Guid.NewGuid()),
        new CustomerId(Guid.NewGuid())
    );
    
    // Act & Assert
    Assert.Throws<DomainException>(() =>
        order.AddItem(
            new ProductId(Guid.NewGuid()),
            -1,  // Quantidade inválida
            new Money(50, "USD")
        )
    );
}

[Fact]
public void Money_Should_Not_Add_Different_Currencies()
{
    // Arrange
    var usd = new Money(100, "USD");
    var eur = new Money(100, "EUR");
    
    // Act & Assert
    Assert.Throws<DomainException>(() => usd + eur);
}
```

### Executar Testes

```bash
dotnet test
```

---

## ✨ Boas Práticas Implementadas

### ✅ Domain Layer (Domínio)
- [x] Sem dependências externas (EF Core, ASP.NET, etc)
- [x] Apenas C# puro
- [x] Regras de negócio encapsuladas
- [x] Invariantes sempre respeitadas
- [x] Testável sem infraestrutura

### ✅ Infrastructure Layer (Infraestrutura)
- [x] EF Core isolado
- [x] Configurações separadas (Fluent API)
- [x] Repository Pattern
- [x] Conversões de Value Objects
- [x] Backing fields respeitados

### ✅ API Layer (Apresentação)
- [x] DTOs separados do domínio
- [x] Minimal APIs (endpoints limpos)
- [x] Validações de entrada
- [x] Tratamento de exceções
- [x] Swagger configurado

### ✅ Separação de Responsabilidades
```
Domain      → O QUE o sistema faz (lógica de negócio)
Application → COMO usar o domínio (casos de uso)
Infrastructure → ONDE persistir (banco de dados)
API         → COMO acessar (HTTP endpoints)
```

---

## 🗺️ Roadmap

### ✅ Fase 1 - Fundamentos (Completo)
- [x] Aggregate Root
- [x] Value Objects
- [x] Strongly Typed IDs
- [x] Repository Pattern
- [x] EF Core In-Memory

### 🚧 Fase 2 - Melhorias
- [ ] Domain Events
- [ ] CQRS (Command Query Responsibility Segregation)
- [ ] Validation com FluentValidation
- [ ] Logs estruturados
- [ ] Health Checks

### 📋 Fase 3 - Produção
- [ ] Trocar para SQL Server
- [ ] Migrations
- [ ] Docker support
- [ ] CI/CD pipelines
- [ ] Monitoring e observability

### 🎯 Fase 4 - Avançado
- [ ] Event Sourcing
- [ ] Outbox Pattern
- [ ] Distributed Transactions
- [ ] Rate Limiting
- [ ] API Versioning

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### Diretrizes

- Siga os princípios de DDD
- Mantenha o domínio puro (sem dependências)
- Adicione testes para novas funcionalidades
- Documente mudanças significativas

---

## 📚 Referências

### Artigos
- [Mapping Domain-Driven Design Concepts To The Database With EF Core](https://medium.com/@mariammaurice/mapping-domain-driven-design-concepts-to-the-database-with-ef-core-c92b3cc3cc85) - Mori (2026)

### Livros
- **Domain-Driven Design** - Eric Evans
- **Implementing Domain-Driven Design** - Vaughn Vernon
- **Clean Architecture** - Robert C. Martin

### Documentação
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👨‍💻 Autor

**Projeto de Demonstração**  
Baseado no artigo de Mori sobre DDD e EF Core

---

## 🙏 Agradecimentos

- **Mori** - Pelo excelente artigo que inspirou este projeto
- **Eric Evans** - Pelos conceitos fundamentais de DDD
- **Microsoft** - Pelo Entity Framework Core

---

## 📞 Suporte

Se você tiver dúvidas ou problemas:

1. Verifique a [documentação](#-índice)
2. Abra uma [issue](https://github.com/seu-repo/issues)
3. Consulte as [referências](#-referências)

---

<div align="center">

**⭐ Se este projeto foi útil, considere dar uma estrela!**

Made with ❤️ following Domain-Driven Design principles

</div>