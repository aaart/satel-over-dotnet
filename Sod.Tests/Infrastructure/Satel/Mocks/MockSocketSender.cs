﻿using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Socket;

namespace Sod.Tests.Infrastructure.Satel.Mocks;

public class MockSocketSender : ISocketSender
{
    private readonly Func<byte[], Task<int>> _impl;

    public MockSocketSender(Func<byte[], Task<int>> impl)
    {
        _impl = impl;
    }

    public Task<int> Send(byte[] data)
    {
        return _impl(data);
    }
}