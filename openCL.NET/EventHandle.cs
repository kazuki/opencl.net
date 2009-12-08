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
	public class EventHandle : HandleBase
	{
		internal EventHandle (IntPtr handle) : base (handle)
		{
		}

		protected override void Dispose (bool disposing)
		{
			Native.clReleaseEvent (_handle);
		}

		public void WaitOne ()
		{
			OpenCLException.Check (Native.clWaitForEvents (1, new IntPtr[] {_handle}));
		}

		public static void WaitAll (EventHandle[] waits)
		{
			IntPtr[] array = ToIntPtrArray (waits);
			if (array == null)
				return;
			OpenCLException.Check (Native.clWaitForEvents ((uint)array.Length, array));
		}

		internal static IntPtr[] ToIntPtrArray (EventHandle[] waits)
		{
			if (waits == null || waits.Length == 0)
				return null;
			IntPtr[] array = new IntPtr[waits.Length];
			for (int i = 0; i < array.Length; i++)
				array[i] = waits[i]._handle;
			return array;
		}

		public TimeSpan ProfilingCommandQueued {
			get { return Native.QueryInfoTimeSpanFromNanoseconds (QueryType.Profiling, _handle, ProfilingInfo.CommandQueued); }
		}

		public TimeSpan ProfilingCommandSubmit {
			get { return Native.QueryInfoTimeSpanFromNanoseconds (QueryType.Profiling, _handle, ProfilingInfo.CommandSubmit); }
		}

		public TimeSpan ProfilingCommandStart {
			get { return Native.QueryInfoTimeSpanFromNanoseconds (QueryType.Profiling, _handle, ProfilingInfo.CommandStart); }
		}

		public TimeSpan ProfilingCommandEnd {
			get { return Native.QueryInfoTimeSpanFromNanoseconds (QueryType.Profiling, _handle, ProfilingInfo.CommandEnd); }
		}
	}
}
