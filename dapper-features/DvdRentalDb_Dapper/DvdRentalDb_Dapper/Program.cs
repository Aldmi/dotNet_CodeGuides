using DvdRentalDb_Dapper.Domain;
using DvdRentalDb_Dapper.Persistance.Dapper.PG.ReaderDal.FilmWithActorsReader;
using DvdRentalDb_Dapper.Persistance.Interfaces;

Console.WriteLine("Hello, World!");

var connectionString = "User ID = postgres; Password = dmitr; Server = localhost; Port = 5433; Integrated Security = true; Pooling = true; Database = dvdrental_test";
//var connectionString = "User ID = postgres; Password = dmitr; Server = localhost; Port = 5432; Integrated Security = true; Pooling = true; Database = dvdrental";


// try
// {
//     using IDbConnection db = new NpgsqlConnection(connectionString);
//     db.Open();
//     var listActors= (await db.QueryAsync<ActorTable>($"SELECT * FROM {ActorTable.TableName}")).ToList();
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }
//


// //Показать все фильмы выбранного актера
// try
// {
//     using IDbConnection db = new NpgsqlConnection(connectionString);
//     db.Open();
//
//     var query = 
//         @"SELECT a.first_name as Actor_first_name, f.title as film_title, f.rating as film_rating
//             FROM actor as a
//             JOIN film_actor fa on a.actor_id = fa.actor_id
//             JOIN film f on fa.film_id = f.film_id
//             WHERE a.actor_id = @actor_id;";
//     
//     
//     var data = await db.QueryAsync<FilmActorDto>(query, new {actor_id = 1});
//     var allFilmsByActor = data.ToList();
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }


// // добавить нового актера и вернуть его Id
// try
// {
//     using IDbConnection db = new NpgsqlConnection(connectionString);
//     db.Open();
//     
//     const string query = @"INSERT INTO actor(first_name, last_name) VALUES (@first_name, @last_name) RETURNING actor_id";
//     var actor = new ActorDal {first_name = "Lana", last_name = "Watchovski"};
//     var actorId = (await db.QueryAsync<int>(query, actor)).FirstOrDefault();
//     actor.actor_id = actorId;
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }


// добавить список актеров и вернуть их Id
// try
// {
//     using IDbConnection db = new NpgsqlConnection(connectionString);
//     db.Open();
//     
//     const string query = @"INSERT INTO actor(first_name, last_name) VALUES (@first_name, @last_name) RETURNING actor_id;";
//     var actorList = new List<ActorDal>
//     {
//         new ActorDal {FirstName = "Bob", LastName = "Torton"},
//         new ActorDal {FirstName = "Nikita", LastName = "Mickhalkov"}
//     };
//     //var actorId = await db.QueryAsync<int>(query, actorList);
//    // var actorId = await db.QueryAsync<int>(query, actorList);
//     var actorId = await db.ExecuteAsync(query, actorList);
//     //actorList.actor_id = actorId;
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }


// // добавить новый фильм
// try
// {
//     //SqlMapper.AddTypeHandler(typeof(MpaaRating), new RatingTypeHandler());
//     //SqlMapper.AddTypeMap(typeof(MpaaRating), DbType.String);
//     
//     using IDbConnection db = new NpgsqlConnection(connectionString);
//     db.Open();
//     
//     const string query = @"INSERT INTO film(title, description, release_year, rental_duration, rental_rate, length, replacement_cost, rating, spacial_features, language_id)
//                            VALUES (@title, @description, @release_year, @rental_duration, @rental_rate, @length, @replacement_cost, @rating, @spacial_features,  @language_id) RETURNING film_id";
//     
//      var film = new FilmDal
//      {
//          title = "Chamber Italian",
//          description = "A Fateful Reflection of a Moose And a Husband who must Overcome a Monkey in Nigeria,",
//          release_year = 2006,
//          rental_duration = 7,
//          rental_rate = 4.99,
//          length = 117,
//          replacement_cost = 14.99,
//          rating = MpaaRating.G,
//          spacial_features = new []{"Deleted Scenes", "Behind the Scenes"},
//          language_id = 1
//      };
//       var filmId = (await db.QueryAsync<int>(query, film)).FirstOrDefault();
//       film.film_id = filmId;
//       
//       const string queryFilmActor = @"INSERT INTO film_actor(film_id, actor_id)
//                            VALUES (@film_id, @actor_id )";
//       
//       var affected = await db.ExecuteAsync(queryFilmActor, new {film.film_id, actor_id = 1}); //TODO: как добавить много актеров к фильму за 1 запрос,
//       
//       //TODO: посмотреть транзакции
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }


// // Получить все фильмы
// try
// {
//     SqlMapper.AddTypeHandler(typeof(MpaaRating), new RatingTypeHandler());
//     //SqlMapper.AddTypeMap(typeof(MpaaRating), DbType.String);
//     
//     using IDbConnection db = new NpgsqlConnection(connectionString);
//     db.Open();
//     
//     const string query = @"SELECT * FROM film";
//     var films = (await db.QueryAsync <FilmDal>(query)).ToList();
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }




//******************************************************************************
 // IActorReadService actorReadService = new ActorReadServiceDapper();
 // var findRes=actorReadService.GetActorsByCriteria(new ActorSearchCriteria{FirstName = "Penelope"});
 // Console.ReadKey();

//******************************************************************************
IFilmWithActorsReader filmWithActorsReader = new FilmWithActorsReaderDapper();
var findRes = filmWithActorsReader.GetActorsByCriteria(new FilmSearchCriteria
{
   Offset = 2,
   Limit = 4
});

Console.ReadKey();




