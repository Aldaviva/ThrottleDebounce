using System;

namespace Tests;

public abstract class BaseTest {

    protected static readonly TimeSpan WAIT_TIME = TimeSpan.FromMilliseconds(500);

    protected int executionCount     = 0;
    protected int mostRecentArgument = -1;

}