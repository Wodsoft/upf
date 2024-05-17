using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    [TypeConverter(typeof(CursorConverter))]
    public sealed class Cursor : IDisposable
    {
        private ICursorContext? _context;
        private readonly string? _fileName;
        private readonly CursorType _cursorType = CursorType.None;
        private readonly bool _scaleWithDpi = false;

        /// <summary>
        /// Constructor for Standard Cursors, needn't be public as Stock Cursors
        /// are exposed in Cursors clas.
        /// </summary>
        /// <param name="cursorType"></param>
        internal Cursor(CursorType cursorType)
        {
            if (IsValidCursorType(cursorType))
            {
                _cursorType = cursorType;
            }
            else
            {
                throw new ArgumentException("Invalid cursor type.");
            }
        }

        /// <summary>
        /// Cursor from .ani or .cur file
        /// </summary>
        /// <param name="cursorFile"></param>
        public Cursor(string cursorFile) : this(cursorFile, false)
        {
        }

        /// <summary>
        /// Cursor from .ani or .cur file
        /// </summary>
        /// <param name="cursorFile"></param>
        /// <param name="scaleWithDpi"></param>
        public Cursor(string cursorFile, bool scaleWithDpi)
        {
            if (cursorFile == null)
                throw new ArgumentNullException("cursorFile");
            _scaleWithDpi = scaleWithDpi;
            _context = FrameworkCoreProvider.GetInputProvider().CreateCursorContext(cursorFile);
            _fileName = cursorFile;
            _cursorType = CursorType.Custom;
        }

        /// <summary>
        /// Cursor from Stream
        /// </summary>
        /// <param name="cursorStream"></param>
        public Cursor(Stream cursorStream) : this(cursorStream, false)
        {
        }

        /// <summary>
        /// Cursor from Stream
        /// </summary>
        /// <param name="cursorStream"></param>
        /// <param name="scaleWithDpi"></param>
        public Cursor(Stream cursorStream, bool scaleWithDpi)
        {
            _scaleWithDpi = scaleWithDpi;
            _context = FrameworkCoreProvider.GetInputProvider().CreateCursorContext(cursorStream);
            _cursorType = CursorType.Custom;
        }

        ///  <summary>
        ///     Cleans up the resources allocated by this object.  Once called, the cursor
        ///     object is no longer useful.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        /// <summary>
        /// CursorType - Cursor Type Enumeration
        /// </summary>
        /// <value></value>
        public CursorType CursorType
        {
            get
            {
                return _cursorType;
            }
        }

        public ICursorContext? Context => _context;

        /// <summary>
        /// String Output
        /// </summary>
        /// <remarks>
        /// Remove this and let users use CursorConverter.ConvertToInvariantString() method
        /// </remarks>
        public override string ToString()
        {
            if (_fileName != null)
                return _fileName;
            else
            {
                // Get the string representation fo the cursor type enumeration.
                return Enum.GetName(typeof(CursorType), _cursorType)!;
            }
        }

        private bool IsValidCursorType(CursorType cursorType)
        {
            return ((int)cursorType >= (int)CursorType.None && (int)cursorType <= (int)CursorType.ArrowCD);
        }
    }
}
