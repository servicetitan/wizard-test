# OpenFeature module

This module provides concrete implementations for the abstraction of acquiring feature flag values. `OpenFeature` can
work with various "feature providers," but as of now this specific implementation works only with the LaunchDarkly
provider.

## External dependencies

- `OpenFeature`
- `LaunchDarkly.OpenFeature.ServerProvider`

## Notes

- Feature flags are evaluated based on a specific context (see `EvaluationContext`).
- To acquire a feature flag value, the following are required:
    - Client (see `OpenFeatureApi`)
    - Context (see `UserEvaluationContext`)
- The evaluation context can be set within one of two scopes:
    - **Application scope**: The evaluation context is set once per application run (e.g., environment-based features:
      prod, stage, dev).
    - **Request scope**: The evaluation context is set once per request (e.g., features based on tenant ID or user ID).
- Scope-Specific Lifetime Registration. The registration of the `*Context` implementation should follow these guidelines
  based on the scope:
    - **Application scope**: Register `*Context` with a singleton lifetime. The `*Feature` implementation can be
      registered with any lifetime, but it is recommended to match the lifetime of `*Context`.
    - **Request scope**: Register `*Context` with a scoped lifetime. The `*Feature` implementation can be registered
      with either a scoped or transient lifetime.
- OpenFeatureSettings for LaunchDarkly. The `OpenFeatureSettings:LaunchDarkly` section should contain only settings
  related to the LaunchDarkly integration.

## Implementation Details

**Goal**:

- The app module should not be aware of specific feature flag implementations.
- Business logic should be resilient to changes in feature flag evaluation logic.

### Provided Implementation

The `App` defines two driven ports:

- `IShouldCapitalizeLetters` port: This port is responsible for providing the value of a specific feature flag.
- `IUserContext` port: This port handles the user-related feature context.

The `OpenFeature` module implements:

- `UserEvaluationContext`: This class uses the provided `IUserContext` to construct an EvaluationContext.
- `IsLoginEnabledFeature` adapter: This adapter evaluates the feature flag using the client and context.

The `Web` host implements:

- `HttpContextUserFeatureContext` adapter: This adapter retrieves context parameters from the HttpRequest to implement
  `IUserContext`.

### Variants

The provided implementation may not fit every service. Developers should choose the approach that best suits their
goals.

#### Variant 1: Encapsulate All Feature Flags in a Single Class

The app defines an `IFeatures` driven port instead of `IShouldCapitalizeLetters`. This port provides values for multiple
feature flags.

`Features` adapter: This adapter uses the client and context to evaluate all feature flags.

```csharp
public interface IFeatures  
{  
    Task<bool> GetIsLoginEnabledValue();  
    ...  
    Task<string> GetDefaultEmailSubject();  
}
```

Pros:

- Aggregates knowledge of all feature flags in a single class.
- Reduces the number of `*Feature` classes.

Cons:

- The class may become large and harder to maintain.

#### Variant 2: Generic Feature Flag Evaluation

The app defines an `IFeaturesAccessor` driven port instead of `IIsLoginEnabledFeature`. This port evaluates generic
values like string, bool, and int using the feature flag name.

```csharp
public interface IFeaturesAccessor  
{  
    Task<bool> GetBooleanValue(string name, bool defaultValue);  
    ...  
    Task<string> GetStringValue(string name, string defaultValue);  
}
```

Pros:

- Introducing new feature flags is fast and convenient.

Cons:

- This variant is not resilient to future changes, such as implementation of flag evaluation logic that doesn't fit
  generic scenario.
- Creates coupling between business logic and feature flag evaluation.
- Using free-form parameters like name and defaultValue may introduce bugs (e.g., different parts of the business logic
  using different default values).
- Harder to mock in tests.

## Tests

### 1. `OpenFeature.IntegrationTests`

- The test fixture substitutes the LaunchDarkly provider with an InMemoryProvider.

### 2. `OpenFeature.ContractTests`

These tests require LaunchDarkly SDK key.

Key should be set either

- in `Testing\appsettings.Test.json` file
- in `OpenFeature.ContractTests\OpenFeatureModuleFixture.cs` class
- provided via environment variable: `OpenFeatureSettings__LaunchDarkly__SdkKey`

It is strongly discouraged to commit secret keys into a repository. During CI, this setting should be configured as an
environment variable

`ShouldCapitalizeLettersFeatureTests` checks whether there where any errors during flag resolution. In order for this
test work developers have to:

1. Provide correct SDK key as mentioned above
2. Configure feature flag in LaunchDarkly