namespace Documents.Application.Utility;

public class Orchestration
{
    private List<Transaction> _transactions;
 
	public Orchestration()
    {
        _transactions = new List<Transaction>();
    }

    public Orchestration(List<Transaction> transactions)
    {
        _transactions = transactions;
    }

    public void AddTransaction(Transaction operation)
    {
        _transactions.Add(operation);
    }

    public async Task ApplyAsync()
    {
        try
        {
            await CommitAsync();
        }
        catch (Exception ex)
        {
            await RollbackAsync();
            throw ex;
        }
    }

    private async Task CommitAsync()
    {
        foreach (var operation in _transactions)
        {
            await operation.CommitAsync();
        }
	}

    private async Task RollbackAsync()
    {
        foreach(var operation in _transactions)
        {
            try
            {
				await operation.RollbackAsync();
			}
			catch
            {

            }
        }
    }
}
