﻿using System;
using System.Activities;
using System.Diagnostics;

namespace Plugins.Shared.Library
{
    public abstract class ContinuableAsyncNativeActivity : AsyncTaskNativeActivity
    {
        public InArgument<bool> ContinueOnError { get; set; } = false;

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                base.Execute(context);
            }
            catch (Exception e)
            {
                if (ContinueOnError.Get(context))
                {
                    Trace.TraceError(e.ToString());
                }
                else
                {
                    throw;
                }
            }
        }

        protected override void BookmarkResumptionCallback(NativeActivityContext context, Bookmark bookmark, object value)
        {
            try
            {
                base.BookmarkResumptionCallback(context, bookmark, value);
            }
            catch (Exception e)
            {
                if (ContinueOnError.Get(context))
                {
                    Trace.TraceError(e.ToString());
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public abstract class AsyncTaskNativeActivityContinue<T> : AsyncTaskNativeActivity<T>
    {
        public InArgument<bool> ContinueOnError { get; set; } = false;

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                base.Execute(context);
            }
            catch (Exception e)
            {
                if (ContinueOnError.Get(context))
                {
                    Trace.TraceError(e.ToString());
                }
                else
                {
                    throw;
                }
            }
        }

        protected override void BookmarkResumptionCallback(NativeActivityContext context, Bookmark bookmark, object value)
        {
            try
            {
                base.BookmarkResumptionCallback(context, bookmark, value);
            }
            catch (Exception e)
            {
                if (ContinueOnError.Get(context))
                {
                    Trace.TraceError(e.ToString());
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
