using Xunit;

namespace IbdTracker.Tests
{
    [CollectionDefinition("SharedFixture")]
    public sealed class SharedFixtureCollection : ICollectionFixture<SharedFixture>
    {
    }
}