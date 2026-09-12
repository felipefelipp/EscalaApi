using System.Net;
using EscalaApi.Services.Results;
using Flunt.Notifications;
using Xunit;

namespace EscalaApi.Tests;

public class ResultTests
{
    [Fact]
    public void Ok_DeveRetornarObjetoEStatus200()
    {
        var dado = "teste";
        var result = Result<string>.Ok(dado);

        Assert.True(result.Sucess);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("teste", result.Object);
        Assert.Empty(result.Notifications);
    }

    [Fact]
    public void Created_DeveRetornarObjetoEStatus201()
    {
        var dado = 42;
        var result = Result<int>.Created(dado);

        Assert.True(result.Sucess);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal(42, result.Object);
        Assert.Empty(result.Notifications);
    }

    [Fact]
    public void NoContent_DeveRetornarSucessoEStatus204()
    {
        var result = Result<object>.NoContent();

        Assert.True(result.Sucess);
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.Null(result.Object);
        Assert.Empty(result.Notifications);
    }

    [Fact]
    public void NotFound_DeveRetornarFalhaEStatus404()
    {
        var notifications = new List<Notification> { new("Id", "Registro não encontrado") };
        var result = Result<string>.NotFound(notifications);

        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Null(result.Object);
        Assert.Single(result.Notifications);
        Assert.Equal("Registro não encontrado", result.Notifications.First().Message);
    }

    [Fact]
    public void BadRequest_DeveRetornarFalhaEStatus400()
    {
        var notifications = new List<Notification> { new("Campo", "Campo inválido") };
        var result = Result<string>.BadRequest(notifications);

        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Null(result.Object);
        Assert.Single(result.Notifications);
    }

    [Fact]
    public void UnprocessableEntity_DeveRetornarFalhaEStatus422()
    {
        var notifications = new List<Notification> { new("Regra", "Não pode ser processado") };
        var result = Result<string>.UnprocessableEntity(notifications);

        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, result.StatusCode);
        Assert.Null(result.Object);
        Assert.Single(result.Notifications);
    }

    [Fact]
    public void Conflict_DeveRetornarFalhaEStatus409()
    {
        var notifications = new List<Notification> { new("Chave", "Conflito de registro") };
        var result = Result<string>.Conflict(notifications);

        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        Assert.Null(result.Object);
        Assert.Single(result.Notifications);
    }

    [Fact]
    public void InternalServerError_DeveRetornarFalhaEStatus500()
    {
        var notifications = new List<Notification> { new("Erro", "Erro interno") };
        var result = Result<string>.InternalServerError(notifications);

        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Null(result.Object);
        Assert.Single(result.Notifications);
    }
}
