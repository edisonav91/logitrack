using LogiTrack.Application;
using LogiTrack.Domain;

namespace LogiTrack.ArchitectureTests;

public sealed class DependencyRulesTests
{
    [Fact]
    public void Domain_DoesNotReferenceOuterLayers()
    {
        var references = typeof(Delivery).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name)
            .ToHashSet(StringComparer.Ordinal);

        Assert.DoesNotContain("LogiTrack.Application", references);
        Assert.DoesNotContain("LogiTrack.Infrastructure", references);
        Assert.DoesNotContain("LogiTrack.Api", references);
    }

    [Fact]
    public void Application_DoesNotReferenceInfrastructureOrApi()
    {
        var references = typeof(AssignCourierUseCase).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name)
            .ToHashSet(StringComparer.Ordinal);

        Assert.Contains("LogiTrack.Domain", references);
        Assert.DoesNotContain("LogiTrack.Infrastructure", references);
        Assert.DoesNotContain("LogiTrack.Api", references);
    }
}

