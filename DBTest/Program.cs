using DataAccess.Database;
using DataAccess.Database.Records;

var leases = new DatabaseDataLayerFactory().CreateReturnRepository();

//await books.CreateAsync( new Book( "B1.1", new BookInfo( "B1", "Nedznicy", "Wiadomo kto", null ) ) );

var results = await leases.GetAsync("1");

Console.Write("");
