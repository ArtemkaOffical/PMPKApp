using System;
using System.Collections.Generic;
using System.Text;
using IniParser.Model.Configuration;

namespace IniParser.Model.Formatting
{
	// Token: 0x02000011 RID: 17
	public class DefaultIniDataFormatter : IIniDataFormatter
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00003A5D File Offset: 0x00001C5D
		public DefaultIniDataFormatter() : this(new IniParserConfiguration())
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003A6A File Offset: 0x00001C6A
		public DefaultIniDataFormatter(IniParserConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this.Configuration = configuration;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003A88 File Offset: 0x00001C88
		public virtual string IniDataToString(IniData iniData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Configuration.AllowKeysWithoutSection)
			{
				this.WriteKeyValueData(iniData.Global, stringBuilder);
			}
			foreach (SectionData section in iniData.Sections)
			{
				this.WriteSection(section, stringBuilder);
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		///     Configuration used to write an ini file with the proper
		///     delimiter characters and data.
		/// </summary>
		/// <remarks>
		///     If the <see cref="T:IniParser.Model.IniData" /> instance was created by a parser,
		///     this instance is a copy of the <see cref="T:IniParser.Model.Configuration.IniParserConfiguration" /> used
		///     by the parser (i.e. different objects instances)
		///     If this instance is created programatically without using a parser, this
		///     property returns an instance of <see cref="T:IniParser.Model.Configuration.IniParserConfiguration" />
		/// </remarks>
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003AFC File Offset: 0x00001CFC
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003B04 File Offset: 0x00001D04
		public IniParserConfiguration Configuration
		{
			get
			{
				return this._configuration;
			}
			set
			{
				this._configuration = value.Clone();
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003B14 File Offset: 0x00001D14
		private void WriteSection(SectionData section, StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.Append(this.Configuration.NewLineStr);
			}
			this.WriteComments(section.LeadingComments, sb);
			sb.Append(string.Format("{0}{1}{2}{3}", new object[]
			{
				this.Configuration.SectionStartChar,
				section.SectionName,
				this.Configuration.SectionEndChar,
				this.Configuration.NewLineStr
			}));
			this.WriteKeyValueData(section.Keys, sb);
			this.WriteComments(section.TrailingComments, sb);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003BB8 File Offset: 0x00001DB8
		private void WriteKeyValueData(KeyDataCollection keyDataCollection, StringBuilder sb)
		{
			foreach (KeyData keyData in keyDataCollection)
			{
				if (keyData.Comments.Count > 0)
				{
					sb.Append(this.Configuration.NewLineStr);
				}
				this.WriteComments(keyData.Comments, sb);
				sb.Append(string.Format("{0}{3}{1}{3}{2}{4}", new object[]
				{
					keyData.KeyName,
					this.Configuration.KeyValueAssigmentChar,
					keyData.Value,
					this.Configuration.AssigmentSpacer,
					this.Configuration.NewLineStr
				}));
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003C84 File Offset: 0x00001E84
		private void WriteComments(List<string> comments, StringBuilder sb)
		{
			foreach (string arg in comments)
			{
				sb.Append(string.Format("{0}{1}{2}", this.Configuration.CommentString, arg, this.Configuration.NewLineStr));
			}
		}

		// Token: 0x04000033 RID: 51
		private IniParserConfiguration _configuration;
	}
}
