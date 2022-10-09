using Dapper;
using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.BuildingBlocks.Dapper;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.Application.Transactions.Commands.ProjectReadModel;

internal class ProjectTransactionReadModelCommandHandler : CommandHandler<ProjectTransactionReadModelCommand>
{
    private readonly DapperContext _dapperContext;
    private readonly IAggregateRepository _aggregateRepository;

    public ProjectTransactionReadModelCommandHandler(
        DapperContext dapperContext,
        IAggregateRepository aggregateRepository)
    {
        _dapperContext = dapperContext;
        _aggregateRepository = aggregateRepository;
    }

    public override async Task HandleAsync(ProjectTransactionReadModelCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _aggregateRepository
            .LoadAsync<Transaction, TransactionId>(command.TransactionId);

        using var connection = _dapperContext.OpenConnection();
        await connection.ExecuteScalarAsync(
            "INSERT INTO [Transactions] ([TransactionId], [SenderFirstName]) " +
            "VALUES (@TransactionId, @SenderFirstName) ",
            new
            {
                TransactionId = transaction.Id.ToString(),
                transaction.SenderFirstName
            });
    }
}
