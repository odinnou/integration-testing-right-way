using Xunit;

namespace Tests.Configuration;

[CollectionDefinition("INTEGRATION_TEST_COLLECTION")]
public class IntegrationTestCollection : ICollectionFixture<TestContainerConfiguration>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}