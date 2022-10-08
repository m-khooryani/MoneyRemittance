using MediatR;

namespace MoneyRemittance.BuildingBlocks.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>, IQueryBase
{
}

public interface IQueryBase { }
