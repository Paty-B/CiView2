using System;

namespace CiView.Recorder
{
    [Flags]
    enum FileLogType
    {
        EndOfStream = 0,

        TypeMask                    = 3,

        TypeLog                     = 0,
        TypeOpenGroup               = 1,
        TypeOpenGroupWithException  = 2,
        TypeGroupClosed             = 3,


    }
}
