#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using Catel.Logging;
	using Common;
	using Configuration;
	using FolderStructure;
	using ICSharpCode.SharpZipLib.Zip;
	using Properties;

	#endregion

	public abstract class TransformRule : Rule
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		private readonly string _fileNamePattern;
		private readonly IFileSystemHelper _fileSystemHelper;
		private readonly bool _isBackupEnabled;
		private readonly IncludeExcludeCollection _sourceFileFilters;

		protected TransformRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
		{
			_isBackupEnabled = isBackupEnabled;
			_fileNamePattern = fileNamePattern;
			_fileSystemHelper = fileSystemHelper;
			_sourceFileFilters = sourceFileFilters;
		}

		protected abstract string TransformedMessage { get; }
		protected abstract string TransformingMessage { get; }
		protected abstract string TransformerMessage { get; }
		protected abstract string TransformMessage { get; }
		protected abstract string Transform(string code, string fileName, Action<ValidationResult> notify);

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			var fileNames = _fileSystemHelper.GetFiles(repositoryInfo.RepositoryRootPath, _fileNamePattern, _sourceFileFilters).ToList();

			string message;

			var backupFileName = string.Empty;
			if (_isBackupEnabled)
			{
				try
				{
					backupFileName = Backup(repositoryInfo.RepositoryRootPath, fileNames);
				}
				catch (Exception e)
				{
					message = string.Format(Resources.TransformRule_Validate_rule_can_not_create_backup_file, TransformMessage.ToLower());
					result.AddResult(ResultLevel.Error, message, notify);
					Logger.Error(message, e);
					return result;
				}
			}

			foreach (var fileName in fileNames)
			{
				string code;
				try
				{
					code = _fileSystemHelper.ReadAllText(fileName);
				}
				catch (Exception e)
				{
					message = string.Format(Resources.TransformRule_Validate_Can_not_read_file, TransformingMessage.ToLower(), fileName);
					result.AddResult(ResultLevel.Error, message, notify);
					Logger.Error(message, e);
					continue;
				}

				if (_isBackupEnabled)
				{
					if (!BackupExists(backupFileName, fileName, code))
					{
						message = string.Format(Resources.TransformRule_Validate_Not_file_because_can_not_verify_backup, TransformingMessage.ToLower(), fileName);
						result.AddResult(ResultLevel.Error, message, notify);
						Logger.Error(message);
						continue;
					}
				}

				string transformedCode;
				try
				{
					transformedCode = Transform(code, fileName, notify);
				}
				catch (Exception e)
				{
					message = string.Format(Resources.TransformRule_Validate_error_in_file, TransformerMessage, e.Message, fileName);
					result.AddResult(ResultLevel.Error, message, notify);
					Logger.Error(message, e);
					continue;
				}
				try
				{
					_fileSystemHelper.WriteAllText(fileName, transformedCode, Encoding.UTF8);
				}
				catch (Exception e)
				{
					message = string.Format(Resources.TransformRule_Validate_Code_error_in_file, TransformingMessage.ToLower(), fileName);
					result.AddResult(ResultLevel.Error, message, notify);
					Logger.Error(message, e);
					continue;
				}

				message = string.Format(Resources.TransformRule_Validate_File_successfully, TransformedMessage.ToLower(), fileName);
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
				Logger.Error(string.Format(Resources.TransformRule_BackupExists_can_not_verify, TransformerMessage), e);
				return false;
			}
		}

		private string Backup(string root, IEnumerable<string> fileNames)
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