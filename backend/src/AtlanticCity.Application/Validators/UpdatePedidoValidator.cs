using FluentValidation;

namespace AtlanticCity.Application.Validators;

public class UpdatePedidoValidator : AbstractValidator<DTOs.UpdatePedidoDto>
{
    public UpdatePedidoValidator()
    {
        RuleFor(x => x.Cliente)
            .NotEmpty().WithMessage("El cliente es requerido")
            .MaximumLength(150).WithMessage("El cliente no puede exceder 150 caracteres");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha es requerida");

        RuleFor(x => x.Total)
            .GreaterThan(0).WithMessage("El total debe ser mayor a 0");

        RuleFor(x => x.Estado)
            .IsInEnum().WithMessage("El estado es inválido");
    }
}
