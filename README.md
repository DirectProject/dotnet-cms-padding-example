# dotnet-cms-padding-example
Demonstrate a .NET example of createing a session key using the Optimal Asymmetric Encryption Padding (OAEP) encryption standard with a SHA256 hash algorithm

I have not found the exact docs, but interaction with Co-Pilot I have come to the following conclusion:

> *.NET Framework does not have support for specifying RSAEncryptionPadding (such as OAEP) in CmsRecipient or EnvelopedCms. The .NET Framework is feature-frozen; Microsoft has stated that all new cryptography features—including support for OAEP padding in CMS—are only available in .NET Core/.NET 5+ and later.*

Of course with .NET 5+ and later it is possible.  This repository pulled in the `common` library and `common.test` and dependencies to show how to implement the code.

Essentially the code is in the `SMIMECryptographer` within the `CreateEncryptedEnvelope` method.

Before:

```csharp
CmsRecipientCollection recipients = new CmsRecipientCollection(SubjectIdentifierType.IssuerAndSerialNumber, encryptingCertificates);
```

After:

```csharp
CmsRecipientCollection recipients = new CmsRecipientCollection();
foreach (X509Certificate2 cert in encryptingCertificates)
{
    recipients.Add(new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, cert, RSAEncryptionPadding.OaepSHA256)); // OID for RSAES-OAEP
}

```
