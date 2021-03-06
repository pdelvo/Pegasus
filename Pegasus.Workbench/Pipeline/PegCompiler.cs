﻿// Copyright © 2015 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Pegasus.Workbench.Pipeline
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using Pegasus.Expressions;

    internal sealed class PegCompiler : IDisposable
    {
        private readonly IDisposable disposable;

        public PegCompiler(IObservable<Grammar> grammars)
        {
            var compileResults = grammars
                .Select(r => r == null ? new Compiler.CompileResult(r) : Compiler.PegCompiler.Compile(r))
                .Publish();

            this.Codes = compileResults.Select(r => r.Code);
            this.Errors = compileResults.Select(r => r.Errors.ToList().AsReadOnly());

            this.disposable = compileResults.Connect();
        }

        public IObservable<string> Codes { get; }

        public IObservable<IList<CompilerError>> Errors { get; }

        public void Dispose() => this.disposable.Dispose();
    }
}
