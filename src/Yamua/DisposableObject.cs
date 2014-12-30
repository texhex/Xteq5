using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    //MTH: An implementation of a disposable pattern as base class based on DisposableObject.
    //See [DisposableObject base class for C#](http://codereview.stackexchange.com/questions/2720/disposableobject-base-class-for-c) by [Nathanael](http://codereview.stackexchange.com/users/4699/nathanael)
    public abstract class DisposableObject : IDisposable
    {
        public bool __disposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //Code Analysis does not like that we have an extra IF and Debug.Assert in this finalizer
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]        
        ~DisposableObject()
        {
            //MTH: This IF looks a little bit stupid but allows you to set a breakpoint more easily. You're welcome. 
            if (__disposed == false)
            {
                Debug.Assert(__disposed, "WARNING: Object finalized without being disposed!");
            }
            Dispose(false);
        }

        private void Dispose(bool Disposing)
        {
            if (!__disposed)
            {
                if (Disposing)
                {
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
                __disposed = true;
            }
        }

        protected virtual void DisposeManagedResources() { }
        protected virtual void DisposeUnmanagedResources() { }

    }

    //MTH: [TryDispose](http://extensionmethod.net/csharp/object/trydispose) by [matteosp](http://extensionmethod.net/Author/matteosp)
    public static class DisposableObjectExtensions
    {
        public static void TryDispose(this DisposableObject Target)
        {
            IDisposable disposable = Target as IDisposable;
            
            if (disposable != null)
            {
                disposable.Dispose();
                
                //MTH: Setting the value to NULL only alters the parameter, not the variable of the caller since we can have an extension method and those do not allow REF parameters. 
                //disposable = null; 
            }
            
        }
    }

}
