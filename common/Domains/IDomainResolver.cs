﻿/* 
 Copyright (c) 2010, Direct Project
 All rights reserved.

 Authors:
    Joe Shook     jshook@kryptiq.com
  
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of The Direct Project (directproject.org) nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
*/


using System.Collections.Generic;

namespace Health.Direct.Common.Domains
{
    /// <summary>
    /// Supports resolution of domain tenancy.
    /// If no domains exist and empty string array is returned.
    /// Throw exceptions if there was an error during retrieval, such as network issues
    /// Implementations may use implementation specific caching policies.
    /// </summary>
    public interface IDomainResolver
    {
        /// <summary>
        /// List of domains
        /// </summary>
        IEnumerable<string> Domains { get; }

        /// <summary>
        /// Tests if an address is managed.
        /// </summary>
        /// <param name="domain">The domain in <c>string</c> form to test</param>
        /// <returns><c>true</c> if the address's domain is managed by the agent,
        /// <c>false</c> otherwise.</returns>
        bool IsManaged(string domain);

        /// <summary>
        /// Test if an address should be highly secured.
        /// The consumer will most likely use this indicator more secure procedures such as FIPS level 140-2 for example.
        /// This method came into existence when the the HSMCryptographer plugin was created for FHA interoperability. 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        bool HsmEnabled(string address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domains"></param>
        /// <returns></returns>
        bool Validate(string[] domains);
    };
}