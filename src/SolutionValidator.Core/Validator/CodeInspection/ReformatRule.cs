#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ReformatRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using Catel.Logging;
	using Common;
	using Configuration;
	using FolderStructure;
	using ICSharpCode.NRefactory.CSharp;
	using ICSharpCode.SharpZipLib.Zip;
	using Properties;

	#endregion

	public class ReformatRule : Rule
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		private readonly IFileSystemHelper _fileSystemHelper;
		private readonly CSharpFormattingOptions _options;
		private readonly string _pattern;
		private IncludeExcludeCollection _sourceFileFilters;

		public ReformatRule(string optionsFilePath, IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string pattern = "*.cs")
		{
			_pattern = pattern;
			_fileSystemHelper = fileSystemHelper;
			_options = CSharpFormattingProperties.GetOptions(optionsFilePath);
			_sourceFileFilters = sourceFileFilters;
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			var formatter = new CSharpFormatter(_options);
			var fileNames = _fileSystemHelper.GetFiles(repositoryInfo.RepositoryRootPath, _pattern, _sourceFileFilters).ToList();

			string message;

			string backupFileName;
			try
			{
				backupFileName = Backup(repositoryInfo.RepositoryRootPath, fileNames, notify);
			}
			catch (Exception e)
			{
				message = Resources.ReformatRule_Validate_Formatter_can_not_create_backup_file;
				result.AddResult(ResultLevel.Error, message, notify);
				Logger.Error(message, e);
				return result;
			}
			foreach (var fileName in fileNames)
			{
				string code;
				try
				{
					code = File.ReadAllText(fileName);
				}
				catch (Exception e)
				{
					message = string.Format(Resources.ReformatRule_Validate_Can_not_read_file_for_formatting, fileName);
					result.AddResult(ResultLevel.Error, message, notify);
					Logger.Error(message, e);
					continue;
				}

				if (!BackupExists(backupFileName, fileName, code))
				{
					message = string.Format(Resources.ReformatRule_Validate_Not_formatting_file_because_can_not_verify_backup_exists, fileName);
					result.AddResult(ResultLevel.Error, message, notify);
					Logger.Error(message);
					continue;
				}

				string formattedCode;
				try
				{
					formattedCode = formatter.Format(code);
				}
				catch (Exception e)
				{
					message = string.Format(Resources.ReformatRule_Validate_Formatter_error, e.Message, fileName);
					result.AddResult(ResultLevel.Error, message, notify);
					Logger.Error(message, e);
					continue;
				}
				try
				{
					File.WriteAllText(fileName, formattedCode, Encoding.UTF8);	
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}

				message = string.Format(Resources.ReformatRule_Validate_File_successfully_formatted, fileName);
				result.AddResult(ResultLevel.Passed, message, notify);
			}
			return result;
		}

		private bool BackupExists(string backupFileName, string fileName, string code)
		{
			try
			{
				fileName = fileName.ToLower();
				using (var zipStream = new ZipInputStream(File.OpenRead(backupFileName)))
				{
					ZipEntry zipEntry;
					while ((zipEntry = zipStream.GetNextEntry()) != null)
					{
						if (!fileName.Contains(zipEntry.Name.ToLower()))
						{
							continue;
						}

						using (var zipReader = new StreamReader(zipStream))
						{
							var zippedCode = zipReader.ReadToEnd();
							return Equals(code, zippedCode);
						}
					}
				}
				return false;
			}
			catch (Exception e)
			{
				Logger.Error(Resources.ReformatRule_BackupExists_Can_not_verify_zipped_backup, e);
				return false;
			}
		}

		private string Backup(string root, IEnumerable<string> fileNames, Action<ValidationResult> notify = null)
		{
			var result = GetBackupName(root);
			using (var zipOutputStream = new ZipOutputStream(File.Create(result)))
			{
				zipOutputStream.SetLevel(6); // 6 is the default, see source. 0 - store only, 9 - ultra

				var buffer = new byte[4096];
				root = root.ToLower().Trim('\\') + "\\";

				foreach (var fileName in fileNames)
				{
					var name = fileName.Replace(root, "", StringComparison.InvariantCultureIgnoreCase);
					var entry = new ZipEntry(name) {DateTime = DateTime.Now};
					zipOutputStream.PutNextEntry(entry);

					using (var fs = File.OpenRead(fileName))
					{
						int sourceBytes;
						do
						{
							sourceBytes = fs.Read(buffer, 0, buffer.Length);
							zipOutputStream.Write(buffer, 0, sourceBytes);
						}
						while (sourceBytes > 0);
					}
				}

				zipOutputStream.Finish();
				zipOutputStream.Close();
			}
			return result;
		}

		private string GetBackupName(string root)
		{
			var fileName = string.Format(".reformat_backup_{0:yyyyMMdd_HH_mm_ss}.zip", DateTime.Now);
			return Path.Combine(root, fileName);
		}
	}
}