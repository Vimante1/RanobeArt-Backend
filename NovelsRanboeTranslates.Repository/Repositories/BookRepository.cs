﻿using MongoDB.Driver;
using NovelsRanboeTranslates.Domain.Models;
using NovelsRanboeTranslates.Repository.Interfaces;

namespace NovelsRanboeTranslates.Repository.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(IMongoDbSettings settings) : base(settings, "Book") { }

        public Response<bool> Create(Book entity)
        {
            try
            {
                _collection.InsertOne(entity);
                return new Response<bool>("Correct", true, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message, false, System.Net.HttpStatusCode.BadRequest);
            };
        }

        public async Task<List<Book>> GetBestBooksByGenreAsync(List<string> genres)
        {
            var filter = Builders<Book>.Filter.In("Genre", genres);
            var sort = Builders<Book>.Sort.Descending("LikedPercent");

            var bestBooks = await _collection.Find(filter)
                                             .Sort(sort)
                                             .ToListAsync();

            return bestBooks;
        }

        public async Task<List<Book>> GetLatestBooksAsync()
        {
            var sort = Builders<Book>.Sort.Descending("Created");
            var latestBooks = await _collection.Find(_ => true)
                                                .Sort(sort)
                                                .Limit(20)
                                                .ToListAsync();
            return latestBooks;
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", bookId);
                var book = await _collection.Find(filter).FirstOrDefaultAsync();
                return book;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateLikedPercentBookAsync(Book book, int likedPercent)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq(b => b._id, book._id);
                var update = Builders<Book>.Update.Set(u => u.LikedPercent, likedPercent);
                await _collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch { return false; }
        }


        public bool ReplaceBookById(int bookId, Book newBook)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", bookId);
                var result = _collection.ReplaceOneAsync(filter, newBook);
                if (result != null) { return true; }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public Response<bool> Delete(Book entity)
        {
            throw new NotImplementedException();
        }
    }
}
