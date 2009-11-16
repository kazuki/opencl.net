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
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace openCL
{
	public class Device
	{
		IntPtr _handle;

		internal Device (IntPtr handle)
		{
			_handle = handle;
		}

		internal IntPtr Handle {
			get { return _handle; }
		}

		public DeviceType Type {
			get { return (DeviceType)Native.QueryInfoInt64 (QueryType.Device, _handle, DeviceInfo.Type); }
		}

		public uint VendorID {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.VendorID); }
		}

		public uint MaxComputeUnits {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MaxComputeUnits); }
		}

		public uint MaxWorkItemDimensions {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MaxWorkItemDimensions); }
		}

		public long[] MaxWorkItemSizes {
			get {
				IntPtr[] tmp = Native.QueryInfoIntPtrArray (QueryType.Device, _handle, DeviceInfo.MaxWorkItemSizes);
				long[] array = new long[tmp.Length];
				for (int i = 0; i < array.Length; i ++)
					array[i] = tmp[i].ToInt64 ();
				return array;
			}
		}

		public long MaxWorkGroupSize {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.MaxWorkGroupSize).ToInt64 (); }
		}

		public uint PreferredVectorWidthChar {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.PreferredVectorWidthChar); }
		}

		public uint PreferredVectorWidthShort {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.PreferredVectorWidthShort); }
		}

		public uint PreferredVectorWidthInt {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.PreferredVectorWidthInt); }
		}

		public uint PreferredVectorWidthLong {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.PreferredVectorWidthLong); }
		}

		public uint PreferredVectorWidthFloat {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.PreferredVectorWidthFloat); }
		}

		public uint PreferredVectorWidthDouble {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.PreferredVectorWidthDouble); }
		}

		public uint MaxClockFrequency {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MaxClockFrequency); }
		}

		public uint AddressBits {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.AddressBits); }
		}

		public ulong MaxMemAllocSize {
			get { return Native.QueryInfoUInt64 (QueryType.Device, _handle, DeviceInfo.MaxMemAllocSize); }
		}

		public bool ImageSupport {
			get { return Native.QueryInfoBoolean (QueryType.Device, _handle, DeviceInfo.ImageSupport); }
		}

		public uint MaxReadImageArgs {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MaxReadImageArgs); }
		}

		public uint MaxWriteImageArgs {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MaxWriteImageArgs); }
		}

		public long Image2dMaxWidth {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.Image2dMaxWidth).ToInt64 (); }
		}

		public long Image2dMaxHeight {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.Image2dMaxHeight).ToInt64 (); }
		}

		public long Image3dMaxWidth {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.Image3dMaxWidth).ToInt64 (); }
		}

		public long Image3dMaxHeight {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.Image3dMaxHeight).ToInt64 (); }
		}

		public long Image3dMaxDepth {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.Image3dMaxDepth).ToInt64 (); }
		}

		public uint MaxSamplers {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MaxSamplers); }
		}

		public long MaxParameterSize {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.MaxParameterSize).ToInt64 (); }
		}

		public uint MemBaseAddrAlign {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MemBaseAddrAlign); }
		}

		public uint MinDataTypeAlignSize {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MinDataTypeAlignSize); }
		}

		public DeviceFpConfig SingleFpConfig {
			get { return (DeviceFpConfig)Native.QueryInfoInt64 (QueryType.Device, _handle, DeviceInfo.SingleFpConfig); }
		}

		public DeviceMemCacheType GlobalMemCacheType {
			get { return (DeviceMemCacheType)Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.GlobalMemCacheType); }
		}

		public uint GlobalMemCacheLineSize {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.GlobalMemCacheLineSize); }
		}

		public ulong GlobalMemCacheSize {
			get { return Native.QueryInfoUInt64 (QueryType.Device, _handle, DeviceInfo.GlobalMemCacheSize); }
		}

		public ulong GlobalMemSize {
			get { return Native.QueryInfoUInt64 (QueryType.Device, _handle, DeviceInfo.GlobalMemSize); }
		}

		public ulong MaxConstantBufferSize {
			get { return Native.QueryInfoUInt64 (QueryType.Device, _handle, DeviceInfo.MaxConstantBufferSize); }
		}

		public uint MaxConstantArgs {
			get { return Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.MaxConstantArgs); }
		}

		public DeviceLocalMemType LocalMemType {
			get { return (DeviceLocalMemType)Native.QueryInfoUInt32 (QueryType.Device, _handle, DeviceInfo.LocalMemType); }
		}

		public ulong LocalMemSize {
			get { return Native.QueryInfoUInt64 (QueryType.Device, _handle, DeviceInfo.LocalMemSize); }
		}

		public bool ErrorCorrectionSupport {
			get { return Native.QueryInfoBoolean (QueryType.Device, _handle, DeviceInfo.ErrorCorrectionSupport); }
		}

		public long ProfilingTimerResolution {
			get { return Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.ProfilingTimerResolution).ToInt64 (); }
		}

		public bool EndianLittle {
			get { return Native.QueryInfoBoolean (QueryType.Device, _handle, DeviceInfo.EndianLittle); }
		}

		public bool Available {
			get { return Native.QueryInfoBoolean (QueryType.Device, _handle, DeviceInfo.Available); }
		}

		public bool CompilerAvailable {
			get { return Native.QueryInfoBoolean (QueryType.Device, _handle, DeviceInfo.CompilerAvailable); }
		}

		public DeviceExecCapabilities ExecutionCapabilities {
			get { return (DeviceExecCapabilities)Native.QueryInfoInt64 (QueryType.Device, _handle, DeviceInfo.ExecutionCapabilities); }
		}

		public CommandQueueProperties QueueProperties {
			get { return (CommandQueueProperties)Native.QueryInfoInt64 (QueryType.Device, _handle, DeviceInfo.QueueProperties); }
		}

		public Platform Platform {
			get { return new Platform (Native.QueryInfoSize (QueryType.Device, _handle, DeviceInfo.Platform)); }
		}

		public string Name {
			get { return Native.QueryInfoString (QueryType.Device, _handle, DeviceInfo.Name); }
		}

		public string Vendor {
			get { return Native.QueryInfoString (QueryType.Device, _handle, DeviceInfo.Vendor); }
		}

		public string DriverVersion {
			get { return Native.QueryInfoString (QueryType.Device, _handle, DeviceInfo.DriverVersion); }
		}

		public string Profile {
			get { return Native.QueryInfoString (QueryType.Device, _handle, DeviceInfo.Profile); }
		}

		public string Version {
			get { return Native.QueryInfoString (QueryType.Device, _handle, DeviceInfo.Version); }
		}

		public string Extentions {
			get { return Native.QueryInfoString (QueryType.Device, _handle, DeviceInfo.Extentions); }
		}
	}
}
