using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Yamua
{
    public static class SecurityNeutralMutex
    {
        //Original Code: http://rdn-consulting.com/blog/2007/08/20/kernel-object-namespace-and-vista/
        public static Mutex Create(string name)
        {
            bool bTrash;
            return Create(name, out bTrash);
        }

        public static Mutex Create(string Name, out bool mutexWasCreated)
        {
            //Always use global scop
            string name = @"Global\" + Name;

            MutexSecurity sec = new MutexSecurity();

            MutexAccessRule secRule = new MutexAccessRule(
                                         new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                                         MutexRights.FullControl, AccessControlType.Allow);

            sec.AddAccessRule(secRule);

            bool mutexWasCreatedOut;
            Mutex m = new Mutex(false, name, out mutexWasCreatedOut, sec);

            mutexWasCreated = mutexWasCreatedOut;
            return m;
        }
    }
}
