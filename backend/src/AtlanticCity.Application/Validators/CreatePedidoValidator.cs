using FluentValidation;

namespace AtlanticCity.Application.Validators;

public class CreatePedidoValidator : AbstractValidator<DTOs.CreatePedidoDto>
{
    public CreatePedidoValidator()
    {
        RuleFor(x => x.NumeroPedido)
            .NotEmpty().WithMessage("El número de pedido es requerido")
            .MaximumLength(50).WithMessage("El número de pedido no puede exceder 50 caracteres");

        RuleFor(x => x.Cliente)
            .NotEmpty().WithMessage("El cliente es requerido")
            .MaximumLength(150).WithMessage("El cliente no puede exceder 150 caracteres");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha es requerida");

        RuleFor(x => x.Total)
            .GreaterThan(0).WithMessage("El total debe ser mayor a 0");
    }
}