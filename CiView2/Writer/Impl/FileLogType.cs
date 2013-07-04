using System;

namespace CiView.Recorder
{
    [Flags]
    enum FileLogType
    {
        EndOfStream = 0,

        TypeMask                    = 3,

        TypeLog                     = 1,
        TypeOpenGroup               = 2,
        TypeOpenGroupWithException  = 3,
        TypeGroupClosed             = 4
    }
}
