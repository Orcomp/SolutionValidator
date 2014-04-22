// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemRuleParser.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.FolderStructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Rules;

    public class FileSystemRuleParser
    {
        private const string RecursionTokenReplacement = "T_o@k@e_n";
        private const string FileWildCardTokenReplacement = "W_T_o@k@e_n";

        private readonly IFileSystemHelper _fileSystemHelper;

        public FileSystemRuleParser(IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper;
        }

        public IEnumerable<FileSystemRule> Parse(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                return Parse(stream);
            }
        }

        public IEnumerable<FileSystemRule> Parse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return Parse(reader);
            }
        }

        public IEnumerable<FileSystemRule> Parse(StreamReader reader)
        {
            var result = new List<FileSystemRule>();
            var lineNumber = 1;

            try
            {
                string line;
                while (null != (line = reader.ReadLine()))
                {
                    FileSystemRule rule = ParseLine(line);
                    if (rule != null)
                    {
                        result.Add(rule);
                    }
                    lineNumber++;
                }
            }
            catch (ParseException e)
            {
                throw new ParseException(e.Message, lineNumber, 0);
            }

            return result;
        }

        public FileSystemRule ParseLine(string line)
        {
            line = line.Trim();

            // Empty line:
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }
            // Comment 
            if (line.StartsWith("#"))
            {
                return null;
            }

            line = line.Replace("/", "\\");

            var checkType = CheckType.MustExist;

            if (line.StartsWith("!"))
            {
                checkType = CheckType.MustNotExist;
                line = line.TrimStart('!');
            }

            // Check if path is valid (valid does not mean exist...)

            if (!IsPathValid(line))
            {
                throw new ParseException("Invalid path", 0, 0);
            }

            if (Path.IsPathRooted(line.Replace(FileSystemRule.RecursionToken, RecursionTokenReplacement)))
            {
                throw new ParseException("RelativePath must be relative (and not rooted)", 0, 0);
            }

            string[] parts = (line + "dummy ending").Split('\\');
            if (parts.Any(p => p.Contains(FileSystemRule.RecursionToken) && p.Length != 2))
            {
                throw new ParseException(string.Format("Invalid use of '{0}' token", FileSystemRule.RecursionToken), 0, 0);
            }

            if (parts.Any(p => p.Trim().Length == 0))
            {
                throw new ParseException(string.Format("RelativePath can not contain empty parts like 'folder1\\ \\folder2."), 0, 0);
            }

            if (line.EndsWith("\\"))
            {
                return new FolderRule(line, checkType, _fileSystemHelper);
            }
            return new FileRule(line, checkType, _fileSystemHelper);
        }

        private bool IsPathValid(string path)
        {
            try
            {
                string pathToCheck = path.Replace(FileSystemRule.RecursionToken, RecursionTokenReplacement);

                if (!path.EndsWith(@"\"))
                {
                    string[] split = pathToCheck.Split('\\');

                    if (split.Length > 0)
                    {
                        split[split.Length - 1] = split[split.Length - 1].Replace(FileSystemRule.FileWildCardToken, FileWildCardTokenReplacement);
                        pathToCheck = String.Join(@"\", split);
                    }
                }

                Path.GetFullPath(pathToCheck);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}