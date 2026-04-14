using FluentValidation;
using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Validators;

namespace AtlanticCity.Tests.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator;

    public LoginRequestValidatorTests()
    {
        _validator = new LoginRequestValidator();
    }

    [Fact]
    public void LoginRequestValidator_ValidRequest_Passes()
    {
        var request = new LoginRequestDto
        {
            Email = "user@email.com",
            Password = "123456"
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void LoginRequestValidator_EmptyEmail_Fails()
    {
        var request = new LoginRequestDto
        {
            Email = "",
            Password = "123456"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void LoginRequestValidator_InvalidEmail_Fails()
    {
        var request = new LoginRequestDto
        {
            Email = "not-an-email",
            Password = "123456"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void LoginRequestValidator_EmptyPassword_Fails()
    {
        var request = new LoginRequestDto
        {
            Email = "user@email.com",
            Password = ""
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public void LoginRequestValidator_ShortPassword_Fails()
    {
        var request = new LoginRequestDto
        {
            Email = "user@email.com",
            Password = "123"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }
}