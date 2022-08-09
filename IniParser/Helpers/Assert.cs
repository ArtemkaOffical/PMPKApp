using System;

namespace IniParser.Helpers
{
	// Token: 0x02000013 RID: 19
	internal static class Assert
	{
		/// <summary>
		/// Asserts that a strings has no blank spaces.
		/// </summary>
		/// <param name="s">The string to be checked.</param>
		/// <returns></returns>
		// Token: 0x060000C9 RID: 201 RVA: 0x00003CF4 File Offset: 0x00001EF4
		internal static bool StringHasNoBlankSpaces(string s)
		{
			return !s.Contains(" ");
		}
	}
}
