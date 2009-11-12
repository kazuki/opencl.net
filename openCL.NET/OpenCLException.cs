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
	public class OpenCLException : Exception
	{
		int _errcode;
		string _msg;

		public OpenCLException (int errcode)
		{
			if (errcode == 0)
				throw new ArgumentOutOfRangeException ();

			_errcode = errcode;
			switch (errcode) {
				case -1: _msg = "DEVICE_NOT_FOUND"; break;
				case -2: _msg = "DEVICE_NOT_AVAILABLE"; break;
				case -3: _msg = "COMPILER_NOT_AVAILABLE"; break;
				case -4: _msg = "MEM_OBJECT_ALLOCATION_FAILURE"; break;
				case -5: _msg = "OUT_OF_RESOURCES"; break;
				case -6: _msg = "OUT_OF_HOST_MEMORY"; break;
				case -7: _msg = "PROFILING_INFO_NOT_AVAILABLE"; break;
				case -8: _msg = "MEM_COPY_OVERLAP"; break;
				case -9: _msg = "IMAGE_FORMAT_MISMATCH"; break;
				case -10: _msg = "IMAGE_FORMAT_NOT_SUPPORTED"; break;
				case -11: _msg = "BUILD_PROGRAM_FAILURE"; break;
				case -12: _msg = "MAP_FAILURE"; break;
				case -30: _msg = "INVALID_VALUE"; break;
				case -31: _msg = "INVALID_DEVICE_TYPE"; break;
				case -32: _msg = "INVALID_PLATFORM"; break;
				case -33: _msg = "INVALID_DEVICE"; break;
				case -34: _msg = "INVALID_CONTEXT"; break;
				case -35: _msg = "INVALID_QUEUE_PROPERTIES"; break;
				case -36: _msg = "INVALID_COMMAND_QUEUE"; break;
				case -37: _msg = "INVALID_HOST_PTR"; break;
				case -38: _msg = "INVALID_MEM_OBJECT"; break;
				case -39: _msg = "INVALID_IMAGE_FORMAT_DESCRIPTOR"; break;
				case -40: _msg = "INVALID_IMAGE_SIZE"; break;
				case -41: _msg = "INVALID_SAMPLER"; break;
				case -42: _msg = "INVALID_BINARY"; break;
				case -43: _msg = "INVALID_BUILD_OPTIONS"; break;
				case -44: _msg = "INVALID_PROGRAM"; break;
				case -45: _msg = "INVALID_PROGRAM_EXECUTABLE"; break;
				case -46: _msg = "INVALID_KERNEL_NAME"; break;
				case -47: _msg = "INVALID_KERNEL_DEFINITION"; break;
				case -48: _msg = "INVALID_KERNEL"; break;
				case -49: _msg = "INVALID_ARG_INDEX"; break;
				case -50: _msg = "INVALID_ARG_VALUE"; break;
				case -51: _msg = "INVALID_ARG_SIZE"; break;
				case -52: _msg = "INVALID_KERNEL_ARGS"; break;
				case -53: _msg = "INVALID_WORK_DIMENSION"; break;
				case -54: _msg = "INVALID_WORK_GROUP_SIZE"; break;
				case -55: _msg = "INVALID_WORK_ITEM_SIZE"; break;
				case -56: _msg = "INVALID_GLOBAL_OFFSET"; break;
				case -57: _msg = "INVALID_EVENT_WAIT_LIST"; break;
				case -58: _msg = "INVALID_EVENT"; break;
				case -59: _msg = "INVALID_OPERATION"; break;
				case -60: _msg = "INVALID_GL_OBJECT"; break;
				case -61: _msg = "INVALID_BUFFER_SIZE"; break;
				case -62: _msg = "INVALID_MIP_LEVEL"; break;
				case -63: _msg = "INVALID_GLOBAL_WORK_SIZE"; break;
				default: _msg = "UNKNOWN CODE (" + errcode.ToString () + ")"; break;
			}
		}

		public int ErrorCode {
			get { return _errcode; }
		}

		public override string Message {
			get { return _msg; }
		}

		public static void Check (int errcode)
		{
			if (errcode == 0)
				return;
			throw new OpenCLException (errcode);
		}
	}
}
