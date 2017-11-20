﻿using System.Globalization;
using System.Windows.Data;

namespace Butler.Helper
{
	public class CultureAwareBinding : Binding
	{
		public CultureAwareBinding()
		{
			ConverterCulture = CultureInfo.CurrentCulture;
		}
	}
}