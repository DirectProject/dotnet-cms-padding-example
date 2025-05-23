﻿/* 
 Copyright (c) 2010, Direct Project
 All rights reserved.

 Authors:
    Umesh Madan     umeshma@microsoft.com
  
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of The Direct Project (directproject.org) nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Health.Direct.Common.Extensions;
using Health.Direct.Common.Mail;
using Xunit;

namespace Health.Direct.Common.Tests.Mail
{
    public class RFC822DateFacts
    {
        static string[] DayNames = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        static string[] MonthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static IEnumerable<object[]> Dates
        {
            get
            {
                DateTime now = DateTime.Now;
                for (int i = 0; i < 7; ++i)
                {
                    yield return new object[] { now.AddDays(i) };
                }
            }
        }

        public static IEnumerable<object[]> Months
        {
            get
            {
                DateTime now = DateTime.Now;
                for (int i = 0; i < 12; ++i)
                {
                    yield return new object[] { now.AddMonths(i) };
                }
            }
        }

        [Theory]
        [MemberData("Dates", DisableDiscoveryEnumeration = true)]
        public void TestDateSerialization(DateTime now)
        {
            string dateString = null;
            Assert.Null(Record.Exception(() => dateString = now.ToRFC822String()));
            Assert.True(!string.IsNullOrEmpty(dateString));
        }

        [Theory]
        [MemberData("Dates", DisableDiscoveryEnumeration = true)]
        public void TestDayName(DateTime now)
        {
            string dateString = null;
            string dayName = null;

            Assert.Null(Record.Exception(() => dateString = now.ToRFC822String()));

            Assert.Null(Record.Exception(() => dayName = dateString.Substring(0, dateString.IndexOf(','))));
            Assert.True(RFC822DateFacts.DayNames.Contains(dayName));
        }

        [Theory]
        [MemberData("Months", DisableDiscoveryEnumeration = true)]
        public void TestMonth(DateTime now)
        {
            string dateString = null;

            Assert.Null(Record.Exception(() => dateString = now.ToRFC822String()));
            Assert.True(this.FindMonth(dateString));
        }

        [Theory]
        [MemberData("Dates", DisableDiscoveryEnumeration = true)]
        public void TestZone(DateTime now)
        {
            string dateString = null;

            Assert.Null(Record.Exception(() => dateString = now.ToRFC822String()));
            Console.WriteLine(dateString);
            int indexOfDash = -1;
            Assert.Null(Record.Exception(() => indexOfDash = dateString.LastIndexOf('-')));
            if (indexOfDash == -1)
            {
                indexOfDash = dateString.LastIndexOf('+');
            }
            Assert.True(indexOfDash >= 0);

            // Verify there is no colon after the dash or plus
            Assert.True(dateString.IndexOf(':', indexOfDash) < 0);
        }

        [Theory]
        [MemberData("Dates", DisableDiscoveryEnumeration = true)]
        public void TestZoneUtc(DateTime now)
        {
            string dateString = null;

            Assert.Null(Record.Exception(() => dateString = now.ToUniversalTime().ToRFC822String()));
            Console.WriteLine(dateString);
            int indexOfDash = -1;
            Assert.Null(Record.Exception(() => indexOfDash = dateString.LastIndexOf('-')));
            if (indexOfDash == -1)
            {
                indexOfDash = dateString.LastIndexOf('+');
            }
            Assert.True(indexOfDash >= 0);

            // Verify there is no colon after the dash or plus
            Assert.True(dateString.IndexOf(':', indexOfDash) < 0);
        }

        [Fact]
        public void TestMessage()
        {
            Message message = new Message();
            Assert.Null(Record.Exception(() => message.Timestamp()));

            string dateValue = null;
            Assert.Null(Record.Exception(() => dateValue = message.DateValue));
            Assert.True(!string.IsNullOrEmpty(dateValue));
            Assert.True(this.FindMonth(dateValue));
        }

        bool FindMonth(string dateString)
        {
            for (int i = 0; i < MonthNames.Length; ++i)
            {
                int startAt = dateString.IndexOf(MonthNames[i], StringComparison.Ordinal);
                if (startAt < 0)
                {
                    continue;
                }
                int endAt = dateString.IndexOf(' ', startAt + 1);
                string month = dateString.Substring(startAt, endAt - startAt);
                if (month == MonthNames[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
