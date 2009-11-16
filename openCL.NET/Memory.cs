// 
// Copyright (c) 2009, Kazuki Oikawa
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace openCL
{
	public class Memory : HandleBase
	{
		internal Memory (IntPtr handle, bool incrementRef) : base (handle)
		{
			if (incrementRef)
				Native.clRetainMemObject (handle);
		}

		protected override void Dispose (bool disposing)
		{
			Native.clReleaseMemObject (_handle);
		}

		public MemObjectType Type {
			get { return (MemObjectType)Native.QueryInfoUInt32 (QueryType.Memory, _handle, MemInfo.Type); }
		}

		public MemoryFlags Flags {
			get { return (MemoryFlags)Native.QueryInfoInt64 (QueryType.Memory, _handle, MemInfo.Flags); }
		}

		public long Size {
			get { return Native.QueryInfoSize (QueryType.Memory, _handle, MemInfo.Size).ToInt64 (); }
		}

		public IntPtr HostPtr {
			get { return Native.QueryInfoSize (QueryType.Memory, _handle, MemInfo.HostPtr); }
		}

		public uint MapCount {
			get { return Native.QueryInfoUInt32 (QueryType.Memory, _handle, MemInfo.MapCount); }
		}

		public uint ReferenceCount {
			get { return Native.QueryInfoUInt32 (QueryType.Memory, _handle, MemInfo.ReferenceCount); }
		}

		public Context Context {
			get { return new Context (Native.QueryInfoSize (QueryType.Memory, _handle, MemInfo.Context), true); }
		}
	}
}
