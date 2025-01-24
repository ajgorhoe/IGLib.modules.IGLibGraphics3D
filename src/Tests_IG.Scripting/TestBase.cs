using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using System;

namespace IG.SandboxTests
{
    public class TestBase<TestClass>
    {

        /// <summaryConstructor of the common test classes' base class.</summary>
        /// <param name="output">Object that provides access to tests' output by the XUnit testing test famework.</param>
        public TestBase(ITestOutputHelper output)
        {
            Output = output;
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new XUnitLoggerProvider(output));
        }

        public ITestOutputHelper Output { get; init; }

        private LoggerFactory LoggerFactory{ get; set; }

        // ToDo: provide a logger!

        //public ILogger GetLogger()
        //{
        //    throw new NotImplementedException();
        //    // return LoggerFactory.CreateLogger();
        //}

    }

    // For logging, see:
    // https://www.meziantou.net/how-to-get-asp-net-core-logs-in-the-output-of-xunit-tests.htm#how-to-create-an-ins
    // https://stackoverflow.com/questions/46169169/net-core-2-0-configurelogging-xunit-test


    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
            => new XUnitLogger(_testOutputHelper, categoryName);

        public ILogger CreateLoger<T>() => new XUnitLogger(_testOutputHelper, typeof(T).Name);

        public void Dispose()
        { }
    }

    public class XUnitLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _categoryName;

        public XUnitLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception)}");
            if (exception != null)
                _testOutputHelper.WriteLine(exception.ToString());
        }

        private class NoopDisposable : IDisposable
        {
            public static readonly NoopDisposable Instance = new NoopDisposable();
            public void Dispose()
            { }
        }

    }

}
