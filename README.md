# E-Commerce Unit Testing Project

## 📖 About The Project

This project demonstrates comprehensive unit testing practices in .NET using **xUnit** and **Moq** frameworks. Built around an e-commerce domain, it showcases how to properly test repository patterns, CRUD operations, and business logic with Entity Framework Core integration.

### 🎯 Learning Objectives

- Master unit testing fundamentals in .NET
- Understand xUnit framework features and best practices
- Learn dependency mocking with Moq
- Implement repository pattern testing strategies
- Work with Entity Framework Core in tests
- Practice Test-Driven Development (TDD)

## 🏗️ Project Architecture
```
case-1/
├── ECommerce.API/              # Web API layer
├── ECommerce.Domain/           # Domain entities and business logic
├── ECommerce.Repository/       # Data access layer with EF Core
└── ECommerce.Tests/            # Unit tests with xUnit and Moq
```

## 🛠️ Tech Stack

- **.NET 8.0** - Latest .NET framework
- **xUnit** - Modern testing framework for .NET
- **Moq** - Most popular mocking library for .NET
- **Entity Framework Core** - Object-Relational Mapper (ORM)
- **SQL Server** - Database (LocalDB for development)
- **AutoMapper** - Object-to-object mapping

## 📦 NuGet Packages

### Main Project
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### Test Project
```bash
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.NET.Test.Sdk
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

### Installation

1. Clone the repository
```bash
git clone https://github.com/yilmazfatmanur/case-1.git
cd case-1
```

2. Update database connection string in `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true;"
}
```

3. Apply migrations
```bash
cd ECommerce.API
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

5. Run tests
```bash
cd ../ECommerce.Tests
dotnet test
```

## 📚 Key Concepts Covered

### 1. xUnit Fundamentals

**Fact Attribute** - Used for tests without parameters:
```csharp
[Fact]
public void AddProduct_ShouldReturnProduct()
{
    // Arrange, Act, Assert
}
```

**Theory Attribute** - Used for parameterized tests:
```csharp
[Theory]
[InlineData(1, "Product1")]
[InlineData(2, "Product2")]
public void GetProduct_WithValidId_ShouldReturnProduct(int id, string name)
{
    // Test implementation
}
```

### 2. Moq - Mocking Framework

**Creating Mock Objects:**
```csharp
var mockRepository = new Mock<IProductRepository>();
mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
              .ReturnsAsync(new Product { Id = 1, Name = "Test" });
```

**Verify Method Calls:**
```csharp
mockRepository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
```

### 3. Assert Methods

- `Assert.Equal()` - Compare expected and actual values
- `Assert.NotNull()` - Verify object is not null
- `Assert.True()` / `Assert.False()` - Boolean assertions
- `Assert.Throws<T>()` - Exception verification
- `Assert.Contains()` - Collection assertions

### 4. Repository Pattern Testing

**Testing CRUD Operations:**

- **Create**: Test entity creation and database insertion
- **Read**: Test retrieval by ID and list operations
- **Update**: Test entity modification
- **Delete**: Test entity removal

**Mocking DbContext:**
```csharp
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDb")
    .Options;

using var context = new AppDbContext(options);
```

### 5. Testing Best Practices (AAA Pattern)
```csharp
[Fact]
public async Task AddProduct_ValidProduct_ReturnsSuccess()
{
    // Arrange
    var mockRepo = new Mock<IProductRepository>();
    var product = new Product { Name = "Test Product" };
    
    // Act
    var result = await mockRepo.Object.AddAsync(product);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test Product", result.Name);
}
```

## 🧪 Test Scenarios

This project includes comprehensive tests for:

- ✅ Product CRUD operations
- ✅ Inventory management
- ✅ Payment processing
- ✅ Shipping service integration
- ✅ Exception handling and edge cases
- ✅ Async method testing
- ✅ DbContext and AutoMapper mocking
- ✅ Integration tests with InMemory database

## 🔍 Using Visual Studio SQL Server Object Explorer

1. Open **View** → **SQL Server Object Explorer**
2. Expand **(localdb)\MSSQLLocalDB**
3. Navigate to **Databases** → **ECommerceDb**
4. Right-click on tables and select **View Data**

## 📖 Additional Resources

- [xUnit Documentation](https://xunit.net/)
- [Moq Quickstart](https://github.com/moq/moq4/wiki/Quickstart)
- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Fatmanur Yılmaz**

- GitHub: [@yilmazfatmanur](https://github.com/yilmazfatmanur)

## ⭐ Show your support

Give a ⭐️ if this project helped you learn unit testing!

---

**Ps:** This project is created for educational purposes to demonstrate unit testing concepts in .NET.
