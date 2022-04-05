using System;
using System.Linq;
using System.Reflection;
using DataAccess.API.Abstractions;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Assembly assembly = Assembly.LoadFile(configuration.GetSection("DataAccess")["DllPath"]);

Type? repositoryFactoryType = assembly.ExportedTypes
                                      .First(t => typeof(IDataContextFactory).IsAssignableFrom(t));

IDataContextFactory dataContextFactory = Activator.CreateInstance(repositoryFactoryType) as IDataContextFactory
                                       ?? throw new
                                           RuntimeBinderException($"The assembly {assembly.GetName()} does not contain "
                                                                  + $"any type implementing {nameof(IDataContextFactory)}");

IUserRepository repository = dataContextFactory.CreateDataContext().Users;

Console.WriteLine("Got user: ");
Console.WriteLine(repository.Get(""));