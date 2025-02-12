using MediatR;

namespace Services.Application.CQRS.ServicesCategories.Commands.DeleteServiceCategory;

public record DeleteServiceCategoryCommand(Guid Id) : IRequest;
