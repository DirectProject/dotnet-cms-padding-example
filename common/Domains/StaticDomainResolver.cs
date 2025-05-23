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

using System;
using System.Collections.Generic;
using Health.Direct.Common.Extensions;
using Health.Direct.Common.Mail;

namespace Health.Direct.Common.Domains
{
    /// <summary>
    /// Supports a static list of domains.
    /// </summary>
    public class StaticDomainResolver : IDomainResolver
    {
        
        //
        // Currently, we use this dictionary as a fast lookup table
        // In the future, we may maintain additional state for each domain
        //
        Dictionary<string, string> m_managedDomains;

        /// <summary>
        /// Creates a IDomainResolver instance to support static domain configuration.
        /// The Domain elements of the SmtpAgentConfig represent the static domain list.
        /// </summary>
        /// <param name="domains">Static list of domains</param>
        public StaticDomainResolver(string[] domains)
        {
            m_managedDomains = new Dictionary<string, string>(MailStandard.Comparer); // Case-IN-sensitive
            if(domains.IsNullOrEmpty())
            {
                return;
            }

            for (int i = 0; i < domains.Length; ++i)
            {
                string domain = domains[i];
                m_managedDomains[domain] = domain;
            }
        }

        /// <summary>
        /// Single domain support
        /// </summary>
        /// <param name="domain"></param>
        public StaticDomainResolver(string domain):this(new [] {domain})
        {
        }

        /// <summary>
        /// List of domains
        /// </summary>
        public IEnumerable<string> Domains
        {
            get
            {
                return m_managedDomains.Keys;
            }
        }

        /// <summary>
        /// Tests if an address is managed.
        /// </summary>
        /// <param name="domain">The domain in <c>string</c> form to test</param>
        /// <returns><c>true</c> if the address's domain is managed by the agent,
        /// <c>false</c> otherwise.</returns>
        public bool IsManaged(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentException("value was null or empty", "domain");
            }

            return m_managedDomains.ContainsKey(domain);
        }

        /// <inheritdoc />
        public bool HsmEnabled(string address)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domains"></param>
        /// <returns></returns>
        public bool Validate(string[] domains)
        {
            if (domains == null || domains.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < domains.Length; ++i)
            {
                string domain = domains[i];
                if (string.IsNullOrEmpty(domain))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
