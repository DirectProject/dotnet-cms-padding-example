// // /* 
// //  Copyright (c) 2010, Direct Project
// //  All rights reserved.
// //
// //  Authors:
// //     John Theisen
// //   
// // Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// //
// // Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// // Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// // Neither the name of The Direct Project (directproject.org) nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// // THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// // */
//
// using Health.Direct.Common.Container;
// using Health.Direct.Common.Diagnostics;
// using Health.Direct.Diagnostics.NLog;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Linq;
// using System.Reflection;
// using Xunit;
// using Xunit.Abstractions;
// using static Org.BouncyCastle.Math.EC.ECCurve;
//
// namespace Health.Direct.Common.Tests.Container
// {
//     
//     public class SimpleContainerSectionFacts
//     {
//         private readonly ITestOutputHelper _testOutputHelper;
//         private System.Configuration.Configuration _config;
//         private SimpleContainerSection _simpleContainerSection;
//         private SimpleComponentElement _simpleComponentElement;
//
//         public SimpleContainerSectionFacts(ITestOutputHelper testOutputHelper)
//         {
//             var forceLoad = typeof(Health.Direct.Diagnostics.NLog.NLogFactory);
//             _testOutputHelper = testOutputHelper;
//             _config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
//             _simpleContainerSection = (SimpleContainerSection)_config.GetSection("container");
//             Assert.NotNull(_simpleContainerSection);
//
//             _simpleComponentElement = _simpleContainerSection.Components.Cast<SimpleComponentElement>().FirstOrDefault();
//             Assert.NotNull(_simpleComponentElement);
//             Assert.Equal("Health.Direct.Common.Tests.Container.IFoo, Health.Direct.Common.Tests", _simpleComponentElement.Service);
//             Assert.Equal("Health.Direct.Common.Tests.Container.Foo, Health.Direct.Common.Tests", _simpleComponentElement.Type);
//
//            
//             var joe = _config.GetSection("logger");
//         }
//                 
//         [Fact]
//         public void CreateInstance()
//         {
//             Assert.Equal(typeof(IFoo), _simpleComponentElement.ServiceType);
//
//             object instance = _simpleComponentElement.CreateInstance();
//             Assert.IsType<Foo>(instance);
//         }
//
//         [Fact]
//         public void RegisterContainerWithSection()
//         {
//             var foo = new SimpleDependencyResolver().RegisterFromConfig(_config).Resolve<IFoo>();
//             Assert.NotNull(foo);
//             Assert.IsType<Foo>(foo);
//         }
//
//         [Fact]
//         public void PrintLoggingSectionType()
//         {
//             try
//             {
//                 var section = ConfigurationManager.GetSection("logging");
//                 _testOutputHelper.WriteLine("Section: " + (section?.GetType().FullName ?? "null"));
//             }
//             catch (Exception ex)
//             {
//                 _testOutputHelper.WriteLine("Exception: " + ex);
//                 throw;
//             }
//         }
//
//         [Fact]
//         public void LifetimeSingleton()
//         {
//             NLogFactory joe;
//
//             var foo = IoC.Resolve<IFoo>(_config);
//             Assert.NotNull(foo);
//
//             var foo2 = IoC.Resolve<IFoo>(_config);
//             Assert.Same(foo, foo2);
//         }
//
//         [Fact]
//         public void LifetimeTransient()
//         {
//             var container = new SimpleDependencyResolver().RegisterFromConfig(_config);
//             var bar = container.Resolve<IBar>();
//             Assert.NotNull(bar);
//
//             var bar2 = container.Resolve<IBar>();
//             Assert.NotSame(bar, bar2);
//         }
//
//         [Fact]
//         public void RegisterMultipleComponentsWithSameServiceType()
//         {
//             var container = new SimpleDependencyResolver().RegisterFromConfig(_config);
//             container.Register<IFoo>(new Foo2());
//             var foo = container.Resolve<IFoo>();
//             Assert.IsType<Foo2>(foo);
//
//             var fooList = container.ResolveAll<IFoo>();
//             Assert.IsType<List<IFoo>>(fooList);
//         }
//     }
//
//     public interface IFoo
//     {
//     }
//
//     public class Foo : IFoo
//     {
//     }
//
//     public class Foo2 : IFoo
//     {
//     }
//
//     public interface IBar
//     {
//     }
//
//     public class Bar : IBar
//     {
//     }
// }