using ReentrancyLockVerifier.Core;

namespace ReentrancyLockVerifier.Tests;

public class ReentrancyAnalyzerTests
{
    [Theory]
    [InlineData("./Contracts/SafeBank.bin")]
    public void ShouldNotDetectVulnerabilitiesInSafeBank(string contractPath)
    {
        var bytecode = File.ReadAllBytes(contractPath);
        var analyzer = new ContractAnalyzer();
        var vulnerabilities = analyzer.Analyze(bytecode);
    
        Assert.Empty(vulnerabilities);
    }
    
    [Theory]
    [InlineData("./Contracts/NoBalanceCheck.bin")]
    [InlineData("./Contracts/SharedState.bin")]
    public void ShouldDetectMissingBalanceCheck(string contractPath)
    {
        var bytecode = File.ReadAllBytes(contractPath);
        var analyzer = new ContractAnalyzer();
        var vulnerabilities = analyzer.Analyze(bytecode);
    
        Assert.Contains(vulnerabilities, v => 
            v.Type is VulnerabilityType.MissingBalanceCheck or VulnerabilityType.SharedStateRaceCondition);
    }
}