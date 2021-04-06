using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrredComplNumFinder02
{
    public interface iProgressIndicator
    {
        // Interface iProgressIndicator
        // can be used by processes running for an extended period of time
        // to indicate their progress.
        // Values  progress<0: Progress interrupted by some failure
        // Values 0<=progress<=1: progress underway, 0=just started, 1=finished
        // Values progress >1: Process not running anymore or not started
        void UpdateProgress(double progress);
    }
}
