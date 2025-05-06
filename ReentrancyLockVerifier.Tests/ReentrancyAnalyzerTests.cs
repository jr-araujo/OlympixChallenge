using ReentrancyLockVerifier.Core;

namespace ReentrancyLockVerifier.Tests;

public class ReentrancyAnalyzerTests
{
    [Fact]
    public void ShouldNotDetectVulnerabilitiesInSafeBank()
    {
        var bytecode = File.ReadAllBytes("./Contracts/SafeBank.bin");
        var analyzer = new ContractAnalyzer();
        var vulnerabilities = analyzer.Analyze(bytecode);
    
        Assert.Empty(vulnerabilities);
    }
    
    [Fact]
    public void ShouldDetectMissingBalanceCheck()
    {
        var bytecode = File.ReadAllBytes("./Contracts/NoBalanceCheck.bin");
        var analyzer = new ContractAnalyzer();
        var vulnerabilities = analyzer.Analyze(bytecode);
    
        Assert.Contains(vulnerabilities, v => v.Type == VulnerabilityType.MissingBalanceCheck);
    }
    
    [Fact]
    public void ShouldDetectSharedStateRaceCondition()
    {
        var bytecode = File.ReadAllBytes("./Contracts/SharedState.bin");
        var analyzer = new ContractAnalyzer();
        var vulnerabilities = analyzer.Analyze(bytecode);
    
        Assert.Contains(vulnerabilities, v => v.Type == VulnerabilityType.SharedStateRaceCondition);
    }
}