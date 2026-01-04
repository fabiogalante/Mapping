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
    "items