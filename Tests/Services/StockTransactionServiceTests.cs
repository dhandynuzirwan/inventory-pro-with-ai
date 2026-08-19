using FluentAssertions;
using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InventorySystem.Tests.Services;

public class StockTransactionServiceTests
{
    private readonly Mock<IStockTransactionRepository> _transactionRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly StockTransactionService _sut; // System Under Test

    public StockTransactionServiceTests()
    {
        _transactionRepoMock = new Mock<IStockTransactionRepository>();
        _productRepoMock = new Mock<IProductRepository>();
        
        // Injecting Mocks instead of real repositories
        _sut = new StockTransactionService(_transactionRepoMock.Object, _productRepoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_SetStatusToPending_And_NotChangeStock()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Laptop", Stock = 10 };
        var transaction = new StockTransaction 
        { 
            ProductId = 1, 
            Quantity = 5, 
            Type = TransactionType.Out 
        };

        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _transactionRepoMock.Setup(r => r.AddAsync(It.IsAny<StockTransaction>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateAsync(transaction);

        // Assert
        result.Status.Should().Be(TransactionStatus.Pending);
        result.TransactionDate.Date.Should().Be(DateTime.Now.Date);
        product.Stock.Should().Be(10); // Stok tidak boleh berkurang
        
        // Memastikan metode AddAsync pada repositori dipanggil 1 kali
        _transactionRepoMock.Verify(r => r.AddAsync(It.Is<StockTransaction>(t => t == result)), Times.Once);
    }

    [Fact]
    public async Task ApproveAsync_WhenStockIn_Should_IncreaseStock_And_SetApproved()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Laptop", Stock = 10 };
        var transaction = new StockTransaction 
        { 
            Id = 100,
            ProductId = 1, 
            Quantity = 5, 
            Type = TransactionType.In,
            Status = TransactionStatus.Pending,
            Product = product
        };

        _transactionRepoMock.Setup(r => r.GetByIdWithProductAsync(100)).ReturnsAsync(transaction);
        _transactionRepoMock.Setup(r => r.UpdateAsync(It.IsAny<StockTransaction>())).Returns(Task.CompletedTask);

        // Act
        await _sut.ApproveAsync(100);

        // Assert
        transaction.Status.Should().Be(TransactionStatus.Approved);
        product.Stock.Should().Be(15); // Stok harus bertambah (10 + 5)
        _transactionRepoMock.Verify(r => r.UpdateAsync(transaction), Times.Once);
    }

    [Fact]
    public async Task ApproveAsync_WhenStockOutAndInsufficient_Should_ThrowException()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Laptop", Stock = 3 }; // Stok hanya 3
        var transaction = new StockTransaction 
        { 
            Id = 101,
            ProductId = 1, 
            Quantity = 5, // Minta 5
            Type = TransactionType.Out,
            Status = TransactionStatus.Pending,
            Product = product
        };

        _transactionRepoMock.Setup(r => r.GetByIdWithProductAsync(101)).ReturnsAsync(transaction);

        // Act
        Func<Task> act = async () => await _sut.ApproveAsync(101);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Stok tidak mencukupi*");
        
        transaction.Status.Should().Be(TransactionStatus.Pending); // Status tidak boleh berubah
        product.Stock.Should().Be(3); // Stok tetap 3
    }

    [Fact]
    public async Task RejectAsync_Should_SetStatusToRejected_And_NotChangeStock()
    {
        // Arrange
        var transaction = new StockTransaction 
        { 
            Id = 102,
            ProductId = 1, 
            Quantity = 5, 
            Type = TransactionType.Out,
            Status = TransactionStatus.Pending
        };

        _transactionRepoMock.Setup(r => r.GetByIdAsync(102)).ReturnsAsync(transaction);
        _transactionRepoMock.Setup(r => r.UpdateAsync(It.IsAny<StockTransaction>())).Returns(Task.CompletedTask);

        // Act
        await _sut.RejectAsync(102);

        // Assert
        transaction.Status.Should().Be(TransactionStatus.Rejected);
        _transactionRepoMock.Verify(r => r.UpdateAsync(transaction), Times.Once);
    }
}
