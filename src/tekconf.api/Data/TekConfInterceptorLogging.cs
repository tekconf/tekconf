﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TekConf.Api.Infrastructure.Logging;

namespace TekConf.Api.Data
{
    public class TekConfInterceptorLogging : DbCommandInterceptor
    {
        private readonly ILogger _logger = new Logger();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "SchoolInterceptor.ScalarExecuted", _stopwatch.Elapsed, "Command: {0}: ", command.CommandText);
            }
            base.ScalarExecuted(command, interceptionContext);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "SchoolInterceptor.NonQueryExecuted", _stopwatch.Elapsed, "Command: {0}: ", command.CommandText);
            }
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }
        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "SchoolInterceptor.ReaderExecuted", _stopwatch.Elapsed, "Command: {0}: ", command.CommandText);
            }
            base.ReaderExecuted(command, interceptionContext);
        }
    }
}