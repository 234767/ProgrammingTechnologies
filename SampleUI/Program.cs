using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Assembly assembly = Assembly.LoadFile(Path.GetFullPath(configuration.GetSection("DataAccess")["DllPath"]));

Type repositoryFactoryType = assembly.ExportedTypes
                                     .FirstOrDefault(t => typeof(IDataContextFactory).IsAssignableFrom(t))
                             ?? throw new
                                 RuntimeBinderException($"The assembly {assembly.GetName()} does not contain "
                                                        + $"any type implementing {nameof(IDataContextFactory)}");

IDataContextFactory dataContextFactory = (IDataContextFactory) Activator.CreateInstance(repositoryFactoryType)!;

IUserRepository repository = dataContextFactory.CreateDataContext().Users;

repository.Create(new User(
                           "234767",
                           "Jakub",
                           "Pawlak"
                          ));
Console.WriteLine("Got user: ");
Console.WriteLine(repository.Get("234767"));

public record User(string Id, string FirstName, string Surname) : IUser;