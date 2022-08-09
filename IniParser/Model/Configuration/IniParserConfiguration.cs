using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IniParser.Model.Configuration
{
	/// <summary>
	///     Defines data for a Parser configuration object.
	/// </summary>
	///     With a configuration object you can redefine how the parser
	///     will detect special items in the ini file by defining new regex
	///     (e.g. you can redefine the comment regex so it just treat text as
	///     a comment iff the comment caracter is the first in the line)
	///     or changing the set of characters used to define elements in
	///     the ini file (e.g. change the 'comment' caracter from ';' to '#')
	///     You can also define how the parser should treat errors, or how liberal
	///     or conservative should it be when parsing files with "strange" formats.
	// Token: 0x0200000F RID: 15
	public class IniParserConfiguration : ICloneable
	{
		/// <summary>
		///     Default values used if an instance of <see cref="T:IniParser.Parser.IniDataParser" />
		///     is created without specifying a configuration.
		/// </summary>
		/// <remarks>
		///     By default the various delimiters for the data are setted:
		///     <para>';' for one-line comments</para>
		///     <para>'[' ']' for delimiting a section</para>
		///     <para>'=' for linking key / value pairs</para>
		///     <example>
		///         An example of well formed data with the default values:
		///         <para>
		///         ;section comment<br />
		///         [section] ; section comment<br />
		///         <br />
		///         ; key comment<br />
		///         key = value ;key comment<br />
		///         <br />
		///         ;key2 comment<br />
		///         key2 = value<br />
		///         </para>
		///     </example>
		/// </remarks>
		// Token: 0x0600008E RID: 142 RVA: 0x0000358C File Offset: 0x0000178C
		public IniParserConfiguration()
		{
			this.CommentString = ";";
			this.SectionStartChar = '[';
			this.SectionEndChar = ']';
			this.KeyValueAssigmentChar = '=';
			this.AssigmentSpacer = " ";
			this.NewLineStr = Environment.NewLine;
			this.ConcatenateDuplicateKeys = false;
			this.AllowKeysWithoutSection = true;
			this.AllowDuplicateKeys = false;
			this.AllowDuplicateSections = false;
			this.AllowCreateSectionsOnFly = true;
			this.ThrowExceptionsOnError = true;
			this.SkipInvalidLines = false;
		}

		/// <summary>
		///     Copy ctor.
		/// </summary>
		/// <param name="ori">
		///     Original instance to be copied.
		/// </param>
		// Token: 0x0600008F RID: 143 RVA: 0x0000360C File Offset: 0x0000180C
		public IniParserConfiguration(IniParserConfiguration ori)
		{
			this.AllowDuplicateKeys = ori.AllowDuplicateKeys;
			this.OverrideDuplicateKeys = ori.OverrideDuplicateKeys;
			this.AllowDuplicateSections = ori.AllowDuplicateSections;
			this.AllowKeysWithoutSection = ori.AllowKeysWithoutSection;
			this.AllowCreateSectionsOnFly = ori.AllowCreateSectionsOnFly;
			this.SectionStartChar = ori.SectionStartChar;
			this.SectionEndChar = ori.SectionEndChar;
			this.CommentString = ori.CommentString;
			this.ThrowExceptionsOnError = ori.ThrowExceptionsOnError;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000368B File Offset: 0x0000188B
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003693 File Offset: 0x00001893
		public Regex CommentRegex { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000369C File Offset: 0x0000189C
		// (set) Token: 0x06000093 RID: 147 RVA: 0x000036A4 File Offset: 0x000018A4
		public Regex SectionRegex { get; set; }

		/// <summary>
		///     Sets the char that defines the start of a section name.
		/// </summary>
		/// <remarks>
		///     Defaults to character '['
		/// </remarks>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000036AD File Offset: 0x000018AD
		// (set) Token: 0x06000095 RID: 149 RVA: 0x000036B5 File Offset: 0x000018B5
		public char SectionStartChar
		{
			get
			{
				return this._sectionStartChar;
			}
			set
			{
				this._sectionStartChar = value;
				this.RecreateSectionRegex(this._sectionStartChar);
			}
		}

		/// <summary>
		///     Sets the char that defines the end of a section name.
		/// </summary>
		/// <remarks>
		///     Defaults to character ']'
		/// </remarks>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000036CA File Offset: 0x000018CA
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000036D2 File Offset: 0x000018D2
		public char SectionEndChar
		{
			get
			{
				return this._sectionEndChar;
			}
			set
			{
				this._sectionEndChar = value;
				this.RecreateSectionRegex(this._sectionEndChar);
			}
		}

		/// <summary>
		///     Retrieving section / keys by name is done with a case-insensitive
		///     search.
		/// </summary>
		/// <remarks>
		///     Defaults to false (case sensitive search)
		/// </remarks>
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000036E7 File Offset: 0x000018E7
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000036EF File Offset: 0x000018EF
		public bool CaseInsensitive { get; set; }

		/// <summary>
		///     Sets the char that defines the start of a comment.
		///     A comment spans from the comment character to the end of the line.
		/// </summary>
		/// <remarks>
		///     Defaults to character ';'
		/// </remarks>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000036F8 File Offset: 0x000018F8
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003706 File Offset: 0x00001906
		[Obsolete("Please use the CommentString property")]
		public char CommentChar
		{
			get
			{
				return this.CommentString[0];
			}
			set
			{
				this.CommentString = value.ToString();
			}
		}

		/// <summary>
		///     Sets the string that defines the start of a comment.
		///     A comment spans from the mirst matching comment string
		///     to the end of the line.
		/// </summary>
		/// <remarks>
		///     Defaults to string ";"
		/// </remarks>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003715 File Offset: 0x00001915
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003728 File Offset: 0x00001928
		public string CommentString
		{
			get
			{
				return this._commentString ?? string.Empty;
			}
			set
			{
				foreach (char c in "[]\\^$.|?*+()")
				{
					value = value.Replace(new string(c, 1), "\\" + c.ToString());
				}
				this.CommentRegex = new Regex(string.Format("^{0}(.*)", value));
				this._commentString = value;
			}
		}

		/// <summary>
		///     Gets or sets the string to use as new line string when formating an IniData structure using a
		///     IIniDataFormatter. Parsing an ini-file accepts any new line character (Unix/windows)
		/// </summary>
		/// <remarks>
		///     This allows to write a file with unix new line characters on windows (and vice versa)
		/// </remarks>
		/// <value>Defaults to value Environment.NewLine</value>
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003791 File Offset: 0x00001991
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003799 File Offset: 0x00001999
		public string NewLineStr { get; set; }

		/// <summary>
		///     Sets the char that defines a value assigned to a key
		/// </summary>
		/// <remarks>
		///     Defaults to character '='
		/// </remarks>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000037A2 File Offset: 0x000019A2
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x000037AA File Offset: 0x000019AA
		public char KeyValueAssigmentChar { get; set; }

		/// <summary>
		///     Sets the string around KeyValuesAssignmentChar
		/// </summary>
		/// <remarks>
		///     Defaults to string ' '
		/// </remarks>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000037B3 File Offset: 0x000019B3
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x000037BB File Offset: 0x000019BB
		public string AssigmentSpacer { get; set; }

		/// <summary>
		///     Allows having keys in the file that don't belong to any section.
		///     i.e. allows defining keys before defining a section.
		///     If set to <c>false</c> and keys without a section are defined,
		///     the <see cref="T:IniParser.Parser.IniDataParser" /> will stop with an error.
		/// </summary>
		/// <remarks>
		///     Defaults to <c>true</c>.
		/// </remarks>
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000037C4 File Offset: 0x000019C4
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x000037CC File Offset: 0x000019CC
		public bool AllowKeysWithoutSection { get; set; }

		/// <summary>
		///     If set to <c>false</c> and the <see cref="T:IniParser.Parser.IniDataParser" /> finds duplicate keys in a
		///     section the parser will stop with an error.
		///     If set to <c>true</c>, duplicated keys are allowed in the file. The value
		///     of the duplicate key will be the last value asigned to the key in the file.
		/// </summary>
		/// <remarks>
		///     Defaults to <c>false</c>.
		/// </remarks>
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000037D5 File Offset: 0x000019D5
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000037DD File Offset: 0x000019DD
		public bool AllowDuplicateKeys { get; set; }

		/// <summary>
		///     Only used if <see cref="P:IniParser.Model.Configuration.IniParserConfiguration.AllowDuplicateKeys" /> is also <c>true</c>
		///     If set to <c>true</c> when the parser finds a duplicate key, it overrites
		///     the previous value, so the key will always contain the value of the
		///     last key readed in the file
		///     If set to <c>false</c> the first readed value is preserved, so the key will
		///     always contain the value of the first key readed in the file
		/// </summary>
		/// <remarks>
		///     Defaults to <c>false</c>.
		/// </remarks>
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000037E6 File Offset: 0x000019E6
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000037EE File Offset: 0x000019EE
		public bool OverrideDuplicateKeys { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether duplicate keys are concatenate
		///     together by <see cref="!:ConcatenateSeparator" />.
		/// </summary>
		/// <value>
		///     Defaults to <c>false</c>.
		/// </value>
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000037F7 File Offset: 0x000019F7
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000037FF File Offset: 0x000019FF
		public bool ConcatenateDuplicateKeys { get; set; }

		/// <summary>
		///     If <c>true</c> the <see cref="T:IniParser.Parser.IniDataParser" /> instance will thrown an exception
		///     if an error is found.
		///     If <c>false</c> the parser will just stop execution and return a null value.
		/// </summary>
		/// <remarks>
		///     Defaults to <c>true</c>.
		/// </remarks>
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003808 File Offset: 0x00001A08
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003810 File Offset: 0x00001A10
		public bool ThrowExceptionsOnError { get; set; }

		/// <summary>
		///     If set to <c>false</c> and the <see cref="T:IniParser.Parser.IniDataParser" /> finds a duplicate section
		///     the parser will stop with an error.
		///     If set to <c>true</c>, duplicated sections are allowed in the file, but only a
		///     <see cref="T:IniParser.Model.SectionData" /> element will be created in the <see cref="P:IniParser.Model.IniData.Sections" />
		///     collection.
		/// </summary>
		/// <remarks>
		///     Defaults to <c>false</c>.
		/// </remarks>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003819 File Offset: 0x00001A19
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00003821 File Offset: 0x00001A21
		public bool AllowDuplicateSections { get; set; }

		/// <summary>
		///     If set to <c>false</c>, the <see cref="T:IniParser.Parser.IniDataParser" /> stop with a error if you try
		///     to access a section that was not created previously and the parser will stop with an error.
		///     If set to <c>true</c>, inexistents sections are created, always returning a valid
		///     <see cref="T:IniParser.Model.SectionData" /> element.
		/// </summary>
		/// <remarks>
		///     Defaults to <c>false</c>.
		/// </remarks>
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000382A File Offset: 0x00001A2A
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00003832 File Offset: 0x00001A32
		public bool AllowCreateSectionsOnFly { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x0000383B File Offset: 0x00001A3B
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003843 File Offset: 0x00001A43
		public bool SkipInvalidLines { get; set; }

		// Token: 0x060000B4 RID: 180 RVA: 0x0000384C File Offset: 0x00001A4C
		private void RecreateSectionRegex(char value)
		{
			if (char.IsControl(value) || char.IsWhiteSpace(value) || this.CommentString.Contains(new string(new char[]
			{
				value
			})) || value == this.KeyValueAssigmentChar)
			{
				throw new Exception(string.Format("Invalid character for section delimiter: '{0}", value));
			}
			string text = "^(\\s*?)";
			if ("[]\\^$.|?*+()".Contains(new string(this._sectionStartChar, 1)))
			{
				text = text + "\\" + this._sectionStartChar.ToString();
			}
			else
			{
				text += this._sectionStartChar.ToString();
			}
			text += "{1}\\s*[\\p{L}\\p{P}\\p{M}_\\\"\\'\\{\\}\\#\\+\\;\\*\\%\\(\\)\\=\\?\\&\\$\\,\\:\\/\\.\\-\\w\\d\\s\\\\\\~]+\\s*";
			if ("[]\\^$.|?*+()".Contains(new string(this._sectionEndChar, 1)))
			{
				text = text + "\\" + this._sectionEndChar.ToString();
			}
			else
			{
				text += this._sectionEndChar.ToString();
			}
			text += "(\\s*?)$";
			this.SectionRegex = new Regex(text);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003954 File Offset: 0x00001B54
		public override int GetHashCode()
		{
			int num = 27;
			foreach (PropertyInfo propertyInfo in base.GetType().GetProperties())
			{
				num = num * 7 + propertyInfo.GetValue(this, null).GetHashCode();
			}
			return num;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003998 File Offset: 0x00001B98
		public override bool Equals(object obj)
		{
			IniParserConfiguration iniParserConfiguration = obj as IniParserConfiguration;
			if (iniParserConfiguration == null)
			{
				return false;
			}
			Type type = base.GetType();
			try
			{
				foreach (PropertyInfo propertyInfo in type.GetProperties())
				{
					if (propertyInfo.GetValue(iniParserConfiguration, null).Equals(propertyInfo.GetValue(this, null)))
					{
						return false;
					}
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		// Token: 0x060000B7 RID: 183 RVA: 0x00003A0C File Offset: 0x00001C0C
		public IniParserConfiguration Clone()
		{
			return base.MemberwiseClone() as IniParserConfiguration;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003A19 File Offset: 0x00001C19
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x04000028 RID: 40
		private char _sectionStartChar;

		// Token: 0x04000029 RID: 41
		private char _sectionEndChar;

		// Token: 0x0400002A RID: 42
		private string _commentString;

		// Token: 0x0400002B RID: 43
		protected const string _strCommentRegex = "^{0}(.*)";

		// Token: 0x0400002C RID: 44
		protected const string _strSectionRegexStart = "^(\\s*?)";

		// Token: 0x0400002D RID: 45
		protected const string _strSectionRegexMiddle = "{1}\\s*[\\p{L}\\p{P}\\p{M}_\\\"\\'\\{\\}\\#\\+\\;\\*\\%\\(\\)\\=\\?\\&\\$\\,\\:\\/\\.\\-\\w\\d\\s\\\\\\~]+\\s*";

		// Token: 0x0400002E RID: 46
		protected const string _strSectionRegexEnd = "(\\s*?)$";

		// Token: 0x0400002F RID: 47
		protected const string _strKeyRegex = "^(\\s*[_\\.\\d\\w]*\\s*)";

		// Token: 0x04000030 RID: 48
		protected const string _strValueRegex = "([\\s\\d\\w\\W\\.]*)$";

		// Token: 0x04000031 RID: 49
		protected const string _strSpecialRegexChars = "[]\\^$.|?*+()";
	}
}
