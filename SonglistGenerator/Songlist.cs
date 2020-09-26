﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SonglistGenerator
{
    class Songlist
    {
        private List<Chapter> chapters = new List<Chapter>();
        private Logger logger;

        public Songlist(Logger logger)
        {
            this.logger = logger;
        }

        public int NumberOfChapters => chapters.Count;

        internal void Add(Chapter chapter)
        {
            chapters.Add(chapter);
        }

        internal void CreateListOfSongs()
        {
            foreach (var chapter in chapters)
            {
                var latexFilesInsideChapter = Directory.GetFiles(chapter.Path, Program.LatexFileFilter);

                foreach (var latexFilePath in latexFilesInsideChapter)
                {
                    if (Path.GetFileName(latexFilePath) == Program.ChapterMasterFile)
                    {
                        // Ignore chapter master file
                        continue;
                    }

                    var song = new Song(latexFilePath);
                    chapter.Songs.Add(song);
                }

                logger.WriteLine($"Found {chapter.Songs.Count} songs in chapter {chapter.FolderName}");
            }
        }

        internal void Initialize()
        {
            foreach (var chapter in chapters)
            {
                chapter.Initialize();
                logger.WriteLine($"   Chapter \"{chapter.ChapterName}\" is located in folder \"{chapter.FolderName}\", UseArtists: {chapter.UseArtists}");
                foreach (var song in chapter.Songs)
                {
                    song.Initialize();
                    logger.WriteLine($"      Song \"{song.Title}\", author \"{song.Author}\", artist \"{song.Artist}\"");
                }
            }
        }

        private string NewMainFile()
        {
            var listOfChapters = new List<string>();
            var orderedChapters = chapters.OrderBy(x => x.ChapterName);
            foreach (var chapter in orderedChapters)
            {
                listOfChapters.Add($"\\include{{{chapter.FolderName}/master}}");
            }

            return string.Join(Environment.NewLine, listOfChapters);
        }

        internal void CreateOutputFile(string songRepositoryFolder, string outputPath)
        {
            var fileCreator = new OutputFileCreator(songRepositoryFolder);
            fileCreator.ReplaceMainFile(this.NewMainFile());
            foreach (var chapter in this.chapters)
            {
                fileCreator.ReplaceMasterFile(chapter.FolderName, chapter.NewMasterFile());
            }

            fileCreator.SaveZipArchive(outputPath);
        }
    }
}
