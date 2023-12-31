﻿using NovelsRanboeTranslates.Domain.DTOs;
using NovelsRanboeTranslates.Domain.Models;

namespace NovelsRanboeTranslates.Repository.Interfaces
{
    public interface IChapterRepository : IBaseRepository<Chapters>
    {
        public Task<bool> CreateChaptersAsync(Chapters chapters);
        public Task<Chapters> GetChaptersAsync(int id);
        public Task<ChaptersDTO> GetChaptersDTOAsync(int id);
        public Task<bool> UpdateChaptersAsync(Chapters chapters);
        public Task<bool> UpdateOneChaptersAsync(int bookId, Chapter updateChapter);
        public Chapter GetOneChapter(int bookId, int chapterId);
    }
}
