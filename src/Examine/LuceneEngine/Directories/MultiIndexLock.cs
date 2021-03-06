using System.Security;
using Lucene.Net.Store;

namespace Examine.LuceneEngine.Directories
{
    /// <summary>
    /// Lock that wraps multiple locks
    /// </summary>
    [SecurityCritical]
    internal class MultiIndexLock : Lock
    {
        private readonly Lock _dirMaster;
        private readonly Lock _dirChild;
        
        public MultiIndexLock(Lock dirMaster, Lock dirChild)
        {
            _dirMaster = dirMaster;
            _dirChild = dirChild;
        }

        /// <summary>
        /// Attempts to obtain exclusive access and immediately return
        ///             upon success or failure.
        /// </summary>
        /// <returns>
        /// true iff exclusive access is obtained
        /// </returns>
        [SecurityCritical]
        public override bool Obtain()
        {
            return _dirMaster.Obtain() && _dirChild.Obtain();
        }

        [SecurityCritical]
        public override bool Obtain(long lockWaitTimeout)
        {
            return _dirMaster.Obtain(lockWaitTimeout) && _dirChild.Obtain(lockWaitTimeout);
        }

        /// <summary>
        /// Releases exclusive access. 
        /// </summary>
        [SecurityCritical]
        public override void Release()
        {
            _dirMaster.Release();
            _dirChild.Release();
        }

        /// <summary>
        /// Returns true if the resource is currently locked.  Note that one must
        ///             still call <see cref="M:Lucene.Net.Store.Lock.Obtain"/> before using the resource. 
        /// </summary>
        [SecurityCritical]
        public override bool IsLocked()
        {
            return _dirMaster.IsLocked() || _dirChild.IsLocked();
        }
    }
}