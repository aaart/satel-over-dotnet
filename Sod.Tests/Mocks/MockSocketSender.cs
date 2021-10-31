using System;
using System.Threading.Tasks;
using Sod.Core.Socket;

namespace Sod.Tests.Mocks
{
    public class MockSocketSender : ISocketSender
    {
        private readonly Func<byte[], Task<int>> _impl;

        public MockSocketSender(Func<byte[], Task<int>> impl)
        {
            _impl = impl;
        }

        public Task<int> Send(byte[] data) => _impl(data);
    }
}