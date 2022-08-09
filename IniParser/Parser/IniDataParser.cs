using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IniParser.Exceptions;
using IniParser.Model;
using IniParser.Model.Configuration;

namespace IniParser.Parser
{
	/// <summary>
	/// 	Responsible for parsing an string from an ini file, and creating
	/// 	an <see cref="T:IniParser.Model.IniData" /> structure.
	/// </summary>
	// Token: 0x02000006 RID: 6
	public class IniDataParser
	{
		/// <summary>
		///     Ctor
		/// </summary>
		/// <remarks>
		///     The parser uses a <see cref="T:IniParser.Model.Configuration.IniParserConfiguration" /> by default
		/// </remarks>
		// Token: 0x06000020 RID: 32 RVA: 0x00002373 File Offset: 0x00000573
		public IniDataParser() : this(new IniParserConfiguration())
		{
		}

		/// <summary>
		///     Ctor
		/// </summary>
		/// <param name="parserConfiguration">
		///     Parser's <see cref="T:IniParser.Model.Configuration.IniParserConfiguration" /> instance.
		/// </param>
		// Token: 0x06000021 RID: 33 RVA: 0x00002380 File Offset: 0x00000580
		public IniDataParser(IniParserConfiguration parserConfiguration)
		{
			if (parserConfiguration == null)
			{
				throw new ArgumentNullException("parserConfiguration");
			}
			this.Configuration = parserConfiguration;
			this._errorExceptions = new List<Exception>();
		}

		/// <summary>
		///     Configuration that defines the behaviour and constraints
		///     that the parser must follow.
		/// </summary>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000023B3 File Offset: 0x000005B3
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000023BB File Offset: 0x000005BB
		public virtual IniParserConfiguration Configuration { get; protected set; }

		/// <summary>
		/// True is the parsing operation encounter any problem
		/// </summary>
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000023C4 File Offset: 0x000005C4
		public bool HasError
		{
			get
			{
				return this._errorExceptions.Count > 0;
			}
		}

		/// <summary>
		/// Returns the list of errors found while parsing the ini file.
		/// </summary>
		/// <remarks>
		/// If the configuration option ThrowExceptionOnError is false it can contain one element
		/// for each problem found while parsing; otherwise it will only contain the very same 
		/// exception that was raised.
		/// </remarks>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000023D4 File Offset: 0x000005D4
		public ReadOnlyCollection<Exception> Errors
		{
			get
			{
				return this._errorExceptions.AsReadOnly();
			}
		}

		/// <summary>
		///     Parses a string containing valid ini data
		/// </summary>
		/// <param name="iniDataString">
		///     String with data
		/// </param>
		/// <returns>
		///     An <see cref="T:IniParser.Model.IniData" /> instance with the data contained in
		///     the <paramref name="iniDataString" /> correctly parsed an structured.
		/// </returns>
		/// <exception cref="T:IniParser.Exceptions.ParsingException">
		///     Thrown if the data could not be parsed
		/// </exception>
		// Token: 0x06000026 RID: 38 RVA: 0x000023E4 File Offset: 0x000005E4
		public IniData Parse(string iniDataString)
		{
			IniData iniData = this.Configuration.CaseInsensitive ? new IniDataCaseInsensitive() : new IniData();
			iniData.Configuration = this.Configuration.Clone();
			if (string.IsNullOrEmpty(iniDataString))
			{
				return iniData;
			}
			this._errorExceptions.Clear();
			this._currentCommentListTemp.Clear();
			this._currentSectionNameTemp = null;
			try
			{
				string[] array = iniDataString.Split(new string[]
				{
					"\n",
					"\r\n"
				}, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (!(text.Trim() == string.Empty))
					{
						try
						{
							this.ProcessLine(text, iniData);
						}
						catch (Exception ex)
						{
							ParsingException ex2 = new ParsingException(ex.Message, i + 1, text, ex);
							if (this.Configuration.ThrowExceptionsOnError)
							{
								throw ex2;
							}
							this._errorExceptions.Add(ex2);
						}
					}
				}
				if (this._currentCommentListTemp.Count > 0)
				{
					if (iniData.Sections.Count > 0)
					{
						iniData.Sections.GetSectionData(this._currentSectionNameTemp).TrailingComments.AddRange(this._currentCommentListTemp);
					}
					else if (iniData.Global.Count > 0)
					{
						iniData.Global.GetLast().Comments.AddRange(this._currentCommentListTemp);
					}
					this._currentCommentListTemp.Clear();
				}
			}
			catch (Exception item)
			{
				this._errorExceptions.Add(item);
				if (this.Configuration.ThrowExceptionsOnError)
				{
					throw;
				}
			}
			if (this.HasError)
			{
				return null;
			}
			return (IniData)iniData.Clone();
		}

		/// <summary>
		///     Checks if a given string contains a comment.
		/// </summary>
		/// <param name="line">
		///     String with a line to be checked.
		/// </param>
		/// <returns>
		///     <c>true</c> if any substring from s is a comment, <c>false</c> otherwise.
		/// </returns>
		// Token: 0x06000027 RID: 39 RVA: 0x0000258C File Offset: 0x0000078C
		protected virtual bool LineContainsAComment(string line)
		{
			return !string.IsNullOrEmpty(line) && this.Configuration.CommentRegex.Match(line).Success;
		}

		/// <summary>
		///     Checks if a given string represents a section delimiter.
		/// </summary>
		/// <param name="line">
		///     The string to be checked.
		/// </param>
		/// <returns>
		///     <c>true</c> if the string represents a section, <c>false</c> otherwise.
		/// </returns>
		// Token: 0x06000028 RID: 40 RVA: 0x000025AE File Offset: 0x000007AE
		protected virtual bool LineMatchesASection(string line)
		{
			return !string.IsNullOrEmpty(line) && this.Configuration.SectionRegex.Match(line).Success;
		}

		/// <summary>
		///     Checks if a given string represents a key / value pair.
		/// </summary>
		/// <param name="line">
		///     The string to be checked.
		/// </param>
		/// <returns>
		///     <c>true</c> if the string represents a key / value pair, <c>false</c> otherwise.
		/// </returns>
		// Token: 0x06000029 RID: 41 RVA: 0x000025D0 File Offset: 0x000007D0
		protected virtual bool LineMatchesAKeyValuePair(string line)
		{
			return !string.IsNullOrEmpty(line) && line.Contains(this.Configuration.KeyValueAssigmentChar.ToString());
		}

		/// <summary>
		///     Removes a comment from a string if exist, and returns the string without
		///     the comment substring.
		/// </summary>
		/// <param name="line">
		///     The string we want to remove the comments from.
		/// </param>
		/// <returns>
		///     The string s without comments.
		/// </returns>
		// Token: 0x0600002A RID: 42 RVA: 0x00002600 File Offset: 0x00000800
		protected virtual string ExtractComment(string line)
		{
			string text = this.Configuration.CommentRegex.Match(line).Value.Trim();
			this._currentCommentListTemp.Add(text.Substring(1, text.Length - 1));
			return line.Replace(text, "").Trim();
		}

		/// <summary>
		///     Processes one line and parses the data found in that line
		///     (section or key/value pair who may or may not have comments)
		/// </summary>
		/// <param name="currentLine">The string with the line to process</param>
		// Token: 0x0600002B RID: 43 RVA: 0x00002654 File Offset: 0x00000854
		protected virtual void ProcessLine(string currentLine, IniData currentIniData)
		{
			currentLine = currentLine.Trim();
			if (this.LineContainsAComment(currentLine))
			{
				currentLine = this.ExtractComment(currentLine);
			}
			if (currentLine == string.Empty)
			{
				return;
			}
			if (this.LineMatchesASection(currentLine))
			{
				this.ProcessSection(currentLine, currentIniData);
				return;
			}
			if (this.LineMatchesAKeyValuePair(currentLine))
			{
				this.ProcessKeyValuePair(currentLine, currentIniData);
				return;
			}
			if (this.Configuration.SkipInvalidLines)
			{
				return;
			}
			throw new ParsingException("Unknown file format. Couldn't parse the line: '" + currentLine + "'.");
		}

		/// <summary>
		///     Proccess a string which contains an ini section.
		/// </summary>
		/// <param name="line">
		///     The string to be processed
		/// </param>
		// Token: 0x0600002C RID: 44 RVA: 0x000026D0 File Offset: 0x000008D0
		protected virtual void ProcessSection(string line, IniData currentIniData)
		{
			string text = this.Configuration.SectionRegex.Match(line).Value.Trim();
			text = text.Substring(1, text.Length - 2).Trim();
			if (text == string.Empty)
			{
				throw new ParsingException("Section name is empty");
			}
			this._currentSectionNameTemp = text;
			if (!currentIniData.Sections.ContainsSection(text))
			{
				currentIniData.Sections.AddSection(text);
				currentIniData.Sections.GetSectionData(text).LeadingComments = this._currentCommentListTemp;
				this._currentCommentListTemp.Clear();
				return;
			}
			if (this.Configuration.AllowDuplicateSections)
			{
				return;
			}
			throw new ParsingException(string.Format("Duplicate section with name '{0}' on line '{1}'", text, line));
		}

		/// <summary>
		///     Processes a string containing an ini key/value pair.
		/// </summary>
		/// <param name="line">
		///     The string to be processed
		/// </param>
		// Token: 0x0600002D RID: 45 RVA: 0x0000278C File Offset: 0x0000098C
		protected virtual void ProcessKeyValuePair(string line, IniData currentIniData)
		{
			string text = this.ExtractKey(line);
			if (string.IsNullOrEmpty(text) && this.Configuration.SkipInvalidLines)
			{
				return;
			}
			string value = this.ExtractValue(line);
			if (!string.IsNullOrEmpty(this._currentSectionNameTemp))
			{
				SectionData sectionData = currentIniData.Sections.GetSectionData(this._currentSectionNameTemp);
				this.AddKeyToKeyValueCollection(text, value, sectionData.Keys, this._currentSectionNameTemp);
				return;
			}
			if (!this.Configuration.AllowKeysWithoutSection)
			{
				throw new ParsingException("key value pairs must be enclosed in a section");
			}
			this.AddKeyToKeyValueCollection(text, value, currentIniData.Global, "global");
		}

		/// <summary>
		///     Extracts the key portion of a string containing a key/value pair..
		/// </summary>
		/// <param name="s">    
		///     The string to be processed, which contains a key/value pair
		/// </param>
		/// <returns>
		///     The name of the extracted key.
		/// </returns>
		// Token: 0x0600002E RID: 46 RVA: 0x00002820 File Offset: 0x00000A20
		protected virtual string ExtractKey(string s)
		{
			int length = s.IndexOf(this.Configuration.KeyValueAssigmentChar, 0);
			return s.Substring(0, length).Trim();
		}

		/// <summary>
		///     Extracts the value portion of a string containing a key/value pair..
		/// </summary>
		/// <param name="s">
		///     The string to be processed, which contains a key/value pair
		/// </param>
		/// <returns>
		///     The name of the extracted value.
		/// </returns>
		// Token: 0x0600002F RID: 47 RVA: 0x00002850 File Offset: 0x00000A50
		protected virtual string ExtractValue(string s)
		{
			int num = s.IndexOf(this.Configuration.KeyValueAssigmentChar, 0);
			return s.Substring(num + 1, s.Length - num - 1).Trim();
		}

		/// <summary>
		///     Abstract Method that decides what to do in case we are trying to add a duplicated key to a section
		/// </summary>
		// Token: 0x06000030 RID: 48 RVA: 0x00002888 File Offset: 0x00000A88
		protected virtual void HandleDuplicatedKeyInCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
		{
			if (!this.Configuration.AllowDuplicateKeys)
			{
				throw new ParsingException(string.Format("Duplicated key '{0}' found in section '{1}", key, sectionName));
			}
			if (this.Configuration.OverrideDuplicateKeys)
			{
				keyDataCollection[key] = value;
			}
		}

		/// <summary>
		///     Adds a key to a concrete <see cref="T:IniParser.Model.KeyDataCollection" /> instance, checking
		///     if duplicate keys are allowed in the configuration
		/// </summary>
		/// <param name="key">
		///     Key name
		/// </param>
		/// <param name="value">
		///     Key's value
		/// </param>
		/// <param name="keyDataCollection">
		///     <see cref="T:IniParser.Model.KeyData" /> collection where the key should be inserted
		/// </param>
		/// <param name="sectionName">
		///     Name of the section where the <see cref="T:IniParser.Model.KeyDataCollection" /> is contained. 
		///     Used only for logging purposes.
		/// </param>
		// Token: 0x06000031 RID: 49 RVA: 0x000028BF File Offset: 0x00000ABF
		private void AddKeyToKeyValueCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
		{
			if (keyDataCollection.ContainsKey(key))
			{
				this.HandleDuplicatedKeyInCollection(key, value, keyDataCollection, sectionName);
			}
			else
			{
				keyDataCollection.AddKey(key, value);
			}
			keyDataCollection.GetKeyData(key).Comments = this._currentCommentListTemp;
			this._currentCommentListTemp.Clear();
		}

		// Token: 0x04000006 RID: 6
		private List<Exception> _errorExceptions;

		/// <summary>
		///     Temp list of comments
		/// </summary>
		// Token: 0x04000008 RID: 8
		private readonly List<string> _currentCommentListTemp = new List<string>();

		/// <summary>
		///     Tmp var with the name of the seccion which is being process
		/// </summary>
		// Token: 0x04000009 RID: 9
		private string _currentSectionNameTemp;
	}
}
