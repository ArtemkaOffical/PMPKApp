using System;
using System.IO;
using System.Text;
using IniParser.Exceptions;
using IniParser.Model;
using IniParser.Parser;

namespace IniParser
{
	/// <summary>
	///     Represents an INI data parser for files.
	/// </summary>
	// Token: 0x02000002 RID: 2
	public class FileIniDataParser : StreamIniDataParser
	{
		/// <summary>
		///     Ctor
		/// </summary>
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public FileIniDataParser()
		{
		}

		/// <summary>
		///     Ctor
		/// </summary>
		/// <param name="parser"></param>
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public FileIniDataParser(IniDataParser parser) : base(parser)
		{
			base.Parser = parser;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002068 File Offset: 0x00000268
		[Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
		public IniData LoadFile(string filePath)
		{
			return this.ReadFile(filePath);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002071 File Offset: 0x00000271
		[Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
		public IniData LoadFile(string filePath, Encoding fileEncoding)
		{
			return this.ReadFile(filePath, fileEncoding);
		}

		/// <summary>
		///     Implements reading ini data from a file.
		/// </summary>
		/// <remarks>
		///     Uses <see cref="P:System.Text.Encoding.Default" /> codification for the file.
		/// </remarks>
		/// <param name="filePath">
		///     Path to the file
		/// </param>
		// Token: 0x06000005 RID: 5 RVA: 0x0000207B File Offset: 0x0000027B
		public IniData ReadFile(string filePath)
		{
			return this.ReadFile(filePath, Encoding.ASCII);
		}

		/// <summary>
		///     Implements reading ini data from a file.
		/// </summary>
		/// <param name="filePath">
		///     Path to the file
		/// </param>
		/// <param name="fileEncoding">
		///     File's encoding.
		/// </param>
		// Token: 0x06000006 RID: 6 RVA: 0x0000208C File Offset: 0x0000028C
		public IniData ReadFile(string filePath, Encoding fileEncoding)
		{
			if (filePath == string.Empty)
			{
				throw new ArgumentException("Bad filename.");
			}
			IniData result;
			try
			{
				using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					using (StreamReader streamReader = new StreamReader(fileStream, fileEncoding))
					{
						result = base.ReadData(streamReader);
					}
				}
			}
			catch (IOException innerException)
			{
				throw new ParsingException(string.Format("Could not parse file {0}", filePath), innerException);
			}
			return result;
		}

		/// <summary>
		///     Saves INI data to a file.
		/// </summary>
		/// <remarks>
		///     Creats an ASCII encoded file by default.
		/// </remarks>
		/// <param name="filePath">
		///     Path to the file.
		/// </param>
		/// <param name="parsedData">
		///     IniData to be saved as an INI file.
		/// </param>
		// Token: 0x06000007 RID: 7 RVA: 0x00002120 File Offset: 0x00000320
		[Obsolete("Please use WriteFile method instead of this one as is more semantically accurate")]
		public void SaveFile(string filePath, IniData parsedData)
		{
			this.WriteFile(filePath, parsedData, Encoding.UTF8);
		}

		/// <summary>
		///     Writes INI data to a text file.
		/// </summary>
		/// <param name="filePath">
		///     Path to the file.
		/// </param>
		/// <param name="parsedData">
		///     IniData to be saved as an INI file.
		/// </param>
		/// <param name="fileEncoding">
		///     Specifies the encoding used to create the file.
		/// </param>
		// Token: 0x06000008 RID: 8 RVA: 0x00002130 File Offset: 0x00000330
		public void WriteFile(string filePath, IniData parsedData, Encoding fileEncoding = null)
		{
			if (fileEncoding == null)
			{
				fileEncoding = Encoding.UTF8;
			}
			if (string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentException("Bad filename.");
			}
			if (parsedData == null)
			{
				throw new ArgumentNullException("parsedData");
			}
			using (FileStream fileStream = File.Open(filePath, FileMode.Create, FileAccess.Write))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream, fileEncoding))
				{
					base.WriteData(streamWriter, parsedData);
				}
			}
		}
	}
}
