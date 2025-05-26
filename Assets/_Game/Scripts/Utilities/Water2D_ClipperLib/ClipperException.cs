using System;

namespace Water2D_ClipperLib
{
	internal class ClipperException : Exception
	{
		public ClipperException(string description) : base(description)
		{
		}
	}
}
