using Polly;

namespace Gateway.DelegateHandlers;

public class RetryHandler : DelegatingHandler
{
	private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy = Policy
		.HandleResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500)
		.WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(3));

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return await _retryPolicy.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
	}
}
