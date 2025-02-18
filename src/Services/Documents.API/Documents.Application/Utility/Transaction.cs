namespace Documents.Application.Utility;

public class Transaction
{
	private bool _isCompleted;
	private Func<Task> _commitFunc;
	private Func<Task> _rollbackFunc;

    public Transaction(Func<Task> commitFunc, Func<Task> rollbackFunc)
    {
		_isCompleted = false;
		_commitFunc = commitFunc;
		_rollbackFunc = rollbackFunc;
	}
    public async Task CommitAsync()
	{
		await _commitFunc.Invoke();
		_isCompleted = true;
	}

	public async Task RollbackAsync()
	{
		if (_isCompleted)
		{
			try
			{
				await _rollbackFunc.Invoke();
				_isCompleted = false;
			}
			catch
			{ }
		}
	}
}
