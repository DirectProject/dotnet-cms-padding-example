using Health.Direct.Common.Cryptography;
using Health.Direct.Common.Mime;
using System;
using System.Collections.Generic;
using NSubstitute;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Health.Direct.Common.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace Health.Direct.Common.Tests;
public class EncryptionTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public EncryptionTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Encrypt_Then_Decrypt_LogsDebugMessages()
    {
        // Arrange
        var logger = Substitute.For<ILogger>();

        // Capture all Debug(string) calls
        var debugMessages = new List<string>();
        logger
            .When(x => x.Debug(Arg.Any<string>()))
            .Do(callInfo => debugMessages.Add(callInfo.Arg<string>()));

        var cryptographer = new SMIMECryptographer(logger);

        using var cert = CreateSelfSignedCertificate();
        var originalEntity = new MimeEntity(new Body("Hello, world!"), "text/plain");

        // Act
        MimeEntity encrypted = cryptographer.Encrypt(originalEntity, cert);
        byte[] encryptedBytes = cryptographer.GetEncryptedBytes(encrypted);
        MimeEntity decrypted = cryptographer.DecryptEntity(encryptedBytes, cert);
                

        // (Re-run the operation if you want to capture logs during the operation)
        // Optionally, you can use logger.Received().Debug(Arg.Any<string>()) to assert

        // Output what was logged
        foreach (var msg in debugMessages)
        {
            // This will show in test output if using xUnit ITestOutputHelper, or Console otherwise
            _testOutputHelper.WriteLine("LOGGED: " + msg);
        }

        // Assert: at least one debug message was logged
        Assert.NotEmpty(debugMessages);

        Assert.Equivalent("KeyEncryptionAlgorithm OID: 1.2.840.113549.1.1.7 (RSAES_OAEP)", debugMessages.Single());
    }

    private static X509Certificate2 CreateSelfSignedCertificate()
    {
        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest(
            "CN=Test",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        var cert = request.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(1));
        return new X509Certificate2(cert.Export(X509ContentType.Pfx));
    }
}
