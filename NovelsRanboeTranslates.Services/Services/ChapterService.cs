﻿using NovelsRanboeTranslates.Domain.DTOs;
using NovelsRanboeTranslates.Domain.Models;
using NovelsRanboeTranslates.Repository.Interfaces;
using NovelsRanboeTranslates.Services.Interfraces;

namespace NovelsRanboeTranslates.Services.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;

        public ChapterService(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public Response<bool> AddChapter(int bookId, Chapter chapter)
        {
            var chapters = _chapterRepository.GetChaptersAsync(bookId).Result;
            if (chapters != null)
            {
                var chapterCount = chapters.Chapter.Count + 1;
                chapter.ChapterId = chapterCount;
                chapters.Chapter.Add(chapter);
                _chapterRepository.UpdateChaptersAsync(chapters);
                return new Response<bool>("Chapter correct added", true, System.Net.HttpStatusCode.OK);
            }
            var newChapters = new Chapters(bookId);
            chapter.ChapterId = 1;
            newChapters.Chapter.Add(chapter);
            _chapterRepository.CreateChaptersAsync(newChapters);
            return new Response<bool>("Chapters added, chapter added", true, System.Net.HttpStatusCode.OK);
        }

        public async Task<Response<ChaptersDTO>> GetChaptersDTOAsync(int bookId)
        {
            var chapters = await _chapterRepository.GetChaptersDTOAsync(bookId);

            if (chapters != null)
            {
                return new Response<ChaptersDTO>("Correct", chapters, System.Net.HttpStatusCode.OK);
            }
            else
            {
                return new Response<ChaptersDTO>("ChaptersNotFound", null, System.Net.HttpStatusCode.NotFound);
            }
        }

        public async Task<Response<Chapters>> GetChaptersAsync(int bookId)
        {
            var chapters = await _chapterRepository.GetChaptersAsync(bookId);

            if (chapters != null)
            {
                return new Response<Chapters>("Correct", chapters, System.Net.HttpStatusCode.OK);
            }
            else
            {
                return new Response<Chapters>("ChaptersNotFound", null, System.Net.HttpStatusCode.NotFound);
            }
        }

        public async Task<bool> UpdateOneChapter(int bookId, Chapter updateChapter)
        {
            return await _chapterRepository.UpdateOneChaptersAsync(bookId, updateChapter);
        }

        public async Task<Chapter> GetOneChapter(int bookId, int chapterId)
        {
            return _chapterRepository.GetOneChapter(bookId, chapterId);
        }
    }
}
