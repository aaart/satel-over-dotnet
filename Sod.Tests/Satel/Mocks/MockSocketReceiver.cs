﻿using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Socket;

namespace Sod.Tests.Satel.Mocks
{
    public class MockSocketReceiver : ISocketReceiver
    {
        private readonly Func<Task<(int byteCount, ArraySegment<byte> receivedBinaryData)>> _impl;

        public MockSocketReceiver(Func<Task<(int byteCount, ArraySegment<byte> receivedBinaryData)>> impl)
        {
            _impl = impl;
        }

        public async Task<(int byteCount, ArraySegment<byte> receivedBinaryData)> ReceiveAsync() => await _impl();
    }
}