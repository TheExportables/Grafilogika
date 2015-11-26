using Grafilogika.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Grafilogika
{
    public static class DBManager
    {
        public static void AddGame(string name, string uploader, string game)
        {
            Games addThis = new Games();

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                try
                {
                    addThis.Name = name;
                    addThis.Uploader = uploader;
                    addThis.Wins = 0;
                    addThis.Rating = 0;
                    addThis.Mistakes = 0;
                    addThis.Game = game;
                    dbCtx.Games.Add(addThis);

                    dbCtx.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public static void AddUser(string name, string pw, int isadmin)
        {
            Users addThis = new Users();

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                addThis.Name = name;
                addThis.Password = pw;
                addThis.Wins = 0;
                addThis.Mistakes = 0;
                addThis.Isadmin = isadmin;
                dbCtx.Users.Add(addThis);

                dbCtx.SaveChanges();
            }
        }

        #region GameQuery
        public static List<Games> GetGamesByUploader(string uploader)
        {
            try
            {
                using (var dbCtx = new GrafilogikaDBEntities())
                {
                    var result = (from g in dbCtx.Games
                                  where g.Uploader == uploader
                                  select g).ToList();

                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static Games GetGameByName(string name)
        {
            try
            {
                using (var dbCtx = new GrafilogikaDBEntities())
                {
                    var result = (from g in dbCtx.Games
                                  where g.Name == name
                                  select g).First();

                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Games> GetAllGames()
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                return dbCtx.Games.ToList();
            }
        }

        public static void UpdateGameRating(Games game, int rating)
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                try
                {
                    if (game.Rating == null || game.Rating == 0)
                    {
                        game.Rating = rating;
                    }
                    else
                    {
                        game.Rating += rating;
                    }
                    dbCtx.Entry(game).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static void UpdateGameMistakes(Games game)
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                try
                {
                    if(game.Mistakes == null)
                    {
                        game.Mistakes = 0;
                    }
                    game.Mistakes++;
                    dbCtx.Entry(game).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static void UpdateGameWins(Games game)
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                try
                {
                    if (game.Wins == null)
                    {
                        game.Wins = 0;
                    }
                    game.Wins++;
                    dbCtx.Entry(game).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region UserQuery
        public static List<Users> GetAllUsers()
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                return dbCtx.Users.ToList();
            }
        }

        public static Users GetUserByName(string name)
        {
            try
            {
                using (var dbCtx = new GrafilogikaDBEntities())
                {
                    var result = (from u in dbCtx.Users
                                  where u.Name == name
                                  select u).First();

                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Users GetUserByNameAndPassword(string name, string pswd)
        {
            try
            {
                using (var dbCtx = new GrafilogikaDBEntities())
                {
                    var result = (from u in dbCtx.Users
                                  where u.Name == name && u.Password == pswd
                                  select u).First();

                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void UpdateUserWins(Users usr)
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                try
                {
                    if (usr.Wins == null)
                    {
                        usr.Wins = 0;
                    }
                    usr.Wins++;
                    dbCtx.Entry(usr).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static void UpdateUserMistakes(Users usr)
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                try
                {
                    if (usr.Mistakes == null)
                    {
                        usr.Mistakes = 0;
                    }
                    usr.Mistakes++;
                    dbCtx.Entry(usr).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static void DeleteGameByName(string gameName)
        {
            using (var dbCtx = new GrafilogikaDBEntities())
            {
                try
                {
                    var result = (from g in dbCtx.Games
                                  where g.Name == gameName
                                  select g).First();
                    dbCtx.Games.Remove(result);
                    dbCtx.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

    }
}