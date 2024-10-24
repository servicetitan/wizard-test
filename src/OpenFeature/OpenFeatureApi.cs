using OpenFeature;
using OpenFeature.Constant;
using OpenFeature.Model;
using ServiceTitan.Platform.Diagnostics;

namespace WizardTest.OpenFeature;

internal interface IOpenFeatureApi
{
    Api Api { get; }
    Task InitializeAsync(CancellationToken cancellationToken = default);
    Task ShutdownAsync();
}

internal class OpenFeatureApi(
    FeatureProvider featureProvider,
    ProviderEventsHandler providerEventsHandler,
    IEnumerable<Hook> hooks,
    ILogger<OpenFeatureApi> logger)
    : IOpenFeatureApi
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _initialized;

    public Api Api {
        get {
            if (!_initialized) {
                throw new InvalidOperationException("The OpenFeatureApi has not been initialized.");
            }

            return Api.Instance;
        }
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_initialized) {
            return;
        }

        await _semaphore.WaitAsync(cancellationToken);
        try {
            if (_initialized) {
                return;
            }

            var api = Api.Instance;
            api.AddHooks(hooks.Concat([new LogErrorHook(logger)]));
            api.AddHandler(ProviderEventTypes.ProviderError, providerEventsHandler.OnProviderError);
            api.AddHandler(ProviderEventTypes.ProviderReady, providerEventsHandler.OnProviderReady);
            api.AddHandler(ProviderEventTypes.ProviderStale, providerEventsHandler.OnProviderStale);
            api.AddHandler(ProviderEventTypes.ProviderConfigurationChanged,
                providerEventsHandler.OnProviderConfigurationChanged);
            await api.SetProviderAsync(featureProvider);
            _initialized = true;
        }
        finally {
            _semaphore.Release();
        }
    }

    public Task ShutdownAsync() => Api.Instance.ShutdownAsync();

    private class LogErrorHook(ILogger<OpenFeatureApi> logger) : Hook
    {
        public override ValueTask AfterAsync<T>(HookContext<T> context, FlagEvaluationDetails<T> details,
            IReadOnlyDictionary<string, object>? hints = null, CancellationToken cancellationToken = new())
        {
            if (details.ErrorType != ErrorType.None) {
                logger.Error(
                    $"Error during flag evaluation. Flag: {context.FlagKey}, ErrorType: {details.ErrorType}, ErrorMessage: {details.ErrorMessage}");
            }

            return base.AfterAsync(context, details, hints, cancellationToken);
        }

        public override ValueTask ErrorAsync<T>(HookContext<T> context, Exception error,
            IReadOnlyDictionary<string, object>? hints = null, CancellationToken cancellationToken = new())
        {
            logger.Error($"Exception thrown during flag evaluation. Flag: {context.FlagKey}", error);
            return base.ErrorAsync(context, error, hints, cancellationToken);
        }
    }
}
