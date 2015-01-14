using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    /// <summary>
    /// This class allows thread save event reporting by passing in the event handler as a parameter to this function which creates a local copy and hence thread-safe invokation.
    /// This class is taken from [Extension method for safe event invocations](http://softwareblog.alcedo.com/post/2010/03/12/Extension-method-for-safe-event-invocations.aspx) by [Fredrik Mörk](http://softwareblog.alcedo.com/page/About-me.aspx)
    /// </summary>
    public static class EventHandlerExtensions
    {
        //Use like this:
        /*
        protected void OnSomeEvent(EventArgs e)
        {
            SomeEvent.SafeInvoke(this, e);
        }
        */
        public static void SafeInvoke<T>(this EventHandler<T> evt, object sender, T e) where T : EventArgs
        {
            if (evt != null)
            {
                evt(sender, e);
            }
        }
    }  
}
